SIE.defineCommand('SIE.Web.MES.BatchGeneration.Commands.BatchWoGenerateCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "批次生成并过站", group: "edit", iconCls: "icon-TextRelease icon-blue" },
    canExecute: function (listView) {
        var entity = listView.getCurrent();
        return entity != null && (entity.data.GeneratedQty == null || entity.data.GeneratedQty < (entity.data.PlanQty + entity.data.ScrapQty));
    },
    execute: function (listView, source) {
        var model = this.getEditEntity(listView);
        this.showView(model);
    },
    validateTemp: function (listView) {
        var curEntity = listView.getCurrent();
        var curData = curEntity.getData();
    },
    getEditEntity: function (listView) {
        var curEntity = listView.getCurrent();
        var curData = curEntity.getData();
        var model = SIE.getModel('SIE.Web.MES.BatchGeneration.BatchGeneratingViewModel');
        var newModel = new model();
        newModel.setBatchWoId(curData.Id);
        newModel.setBatchWoNo(curData.No);
        newModel.setPlanQty(curData.PlanQty);
        newModel.setPlanBeginDate(curData.PlanBeginDate);
        if (curData.GeneratedQty == null) curData.GeneratedQty = 0;
        newModel.setGeneratedQty(curData.GeneratedQty);
        newModel.setGenerateingQty(curData.PlanQty + curData.ScrapQty - curData.GeneratedQty);
        newModel.setNotGenerateQty(curData.PlanQty + curData.ScrapQty - curData.GeneratedQty);
        newModel.setProductId(curData.ProductId);
        newModel.setPageCount(1);
        newModel.ownerView = listView;
        newModel.token = this.view.token;
        this.getQuryData(newModel);
        this.mon(newModel, 'propertyChanged', this.onEditPropertyChanged, this);
        return newModel;
    },
    getQuryData: function (newModel) {
        var woId = newModel.getBatchWoId();
        SIE.invokeDataQuery({
            method: 'GetBatchGeneratingViewModel',
            params: [woId],
            action: 'queryer',
            type: 'SIE.Web.MES.BatchGeneration.WipBatchsDataQueryer',
            token: this.view.token,
            success: function (res) {
                if (res.Result["ErrorMsg"] != "") {
                    SIE.Msg.showError(res.Result["ErrorMsg"]);
                }
                else {
                    if (res.Result["WarnMsg"] != "") {
                        SIE.Msg.showWarning(res.Result["WarnMsg"]);
                    }
                    newModel.setBatchRule(res.Result["BatchRule"]);
                    newModel.setBatchQty(res.Result["BatchQty"]);
                    newModel.setNumberRuleId(res.Result["NumberRuleId"]);
                    newModel.setNumberRuleId_Display(res.Result["NumberRuleName"]);

                    newModel.setResourceId(res.Result["ResourceId"]);
                    newModel.setResourceId_Display(res.Result["ResourceName"]);
                    newModel.setProcessId(res.Result["ProcessId"]);
                    newModel.setProcessId_Display(res.Result["ProcessName"]);

                    newModel.setTemplateId(res.Result["TemplateId"]);
                    newModel.setTemplateId_Display(res.Result["TemplateName"]);
                }
            }
        });
    },
    showView: function (editEntity) {
        var me = this;
        var mainView = me.view;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: false,
                isDetail: true,
                ignoreQuery: true,
                viewGroup: "DetailsView",
                token: this.view.token,
                module: mainView.module,
                model: "SIE.Web.MES.BatchGeneration.BatchGeneratingViewModel",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    detailView._setDefaultValue(editEntity);
                    detailView.setData(editEntity);
                    detailView.mainView = mainView;
                    var ui = detailView.getControl();
                    var win = SIE.Window.show({
                        title: "批次生成并过站".L10N(),
                        width: 930,
                        height: 500,
                        buttons: [
                            { xtype: "button", text: "确定".t(), hidden: true },
                            { xtype: "button", text: "取消".t(), hidden: true }
                        ],
                        items: ui,
                        id: "BatchGeneratingViewModel001",
                    });
                }
            });
        }
    },
    getBarcodeRuleDtl: function (newModel) {
        var barcodeRuleDtl = "";
        var numberRuleId = newModel.getNumberRuleId();
        if (newModel.data.NumberRuleId == null || newModel.data.NumberRuleId <= 0) {
            return;
        }
        else {
            SIE.invokeDataQuery({
                method: 'GetRuleDetail',
                params: [numberRuleId],
                action: 'queryer',
                type: 'SIE.Web.MES.BatchGeneration.WipBatchsDataQueryer',
                token: this.view.token,
                success: function (res) {
                    var rtnData = res.Result;
                    newModel.setBarcodeRuleDtl(rtnData);
                }
            });
        }
    },
    getChildBarcodeRuleDtl: function (newModel) {
        var barcodeRuleDtl = "";
        var childNumberRuleId = newModel.getChildNumberRuleId();
        if (newModel.data.ChildNumberRuleId == null || newModel.data.ChildNumberRuleId < 0) {
            return;
        }
        else {
            SIE.invokeDataQuery({
                method: 'GetRuleDetail',
                params: [childNumberRuleId],
                action: 'queryer',
                type: 'SIE.Web.MES.BatchGeneration.WipBatchsDataQueryer',
                token: this.view.token,
                success: function (res) {
                    var rtnData = res.Result;
                    newModel.setChildBarcodeRuleDtl(rtnData);
                }
            });
        }
    },
    onEditPropertyChanged: function (e) {
        if (e.property.length > 0) {
            if (e.property == "ResourceId" || e.property == "ProcessId") {
                this.getStationResourceId(e.entity);
            }
            //BatchRule改变在自定义编辑器中实现
        }
    },

    getStationResourceId: function (newModel) {

        var resourceId = newModel.getResourceId();
        var processId = newModel.getProcessId();
        SIE.invokeDataQuery({
            method: 'getStationResourceId',
            params: [processId,resourceId],
            action: 'queryer',
            type: 'SIE.Web.MES.BatchGeneration.WipBatchsDataQueryer',
            token: this.view.token,
            success: function (res) {
                var rtnData = res.Result;
                if (rtnData != null && rtnData != "") {
                    newModel.setStationId(rtnData["StationId"]);
                    newModel.setStationId_Display(rtnData["StationName"]);
                }
            }
        });
    },

    getBeginSnEndSn: function (newModel) {
        if (!this.getBeginSnEndSnValidata(newModel))
            return;
        var numberRuleId = newModel.getNumberRuleId();
        var generateingQty = newModel.getGenerateingQty();
        var batchQty = newModel.getBatchQty();
        SIE.invokeDataQuery({
            method: 'GetBeginSnEndSn',
            params: [generateingQty, batchQty, numberRuleId],
            action: 'queryer',
            type: 'SIE.Web.MES.BatchGeneration.WipBatchsDataQueryer',
            token: this.view.token,
            success: function (res) {
                var rtnData = res.Result;
                newModel.setBeginSn(rtnData.BeginSn);
                newModel.setEndSn(rtnData.EndSn);
            }
        });
    },
    getChildBeginSnEndSn: function (newModel) {
        if (newModel.data.ChildNumberRuleId == null || newModel.data.ChildNumberRuleId < 0 || newModel.data.ChildBatchQty <= 0 || newModel.data.GenerateingQty <= 0 || newModel.getBatchQty() <= 0) {
            return;
        }
        else {
            var numberRuleId = newModel.getNumberRuleId();
            var childNumberRuleId = newModel.getChildNumberRuleId();
            var generateingQty = newModel.getGenerateingQty();
            var batchQty = newModel.getBatchQty();
            var childBatchQty = newModel.getChildBatchQty();
            SIE.invokeDataQuery({
                method: 'GetChildBeginSnEndSn',
                params: [generateingQty, batchQty, numberRuleId, childNumberRuleId, childBatchQty],
                action: 'queryer',
                type: 'SIE.Web.MES.BatchGeneration.WipBatchsDataQueryer',
                token: this.view.token,
                success: function (res) {
                    var rtnData = res.Result;
                    newModel.setChildBeginSn(rtnData.ChildBeginSn);
                    newModel.setChildEndSn(rtnData.ChildEndSn);
                }
            });
        }
    },
    getPrintControl: function (newModel) {
        if (!this.getBeginSnEndSnValidata(newModel)) {
            return;
        }
        else {
            var numberRuleId = newModel.getNumberRuleId();
            var childNumberRuleId = newModel.getChildNumberRuleId();
            var generateingQty = newModel.getGenerateingQty();
            var batchQty = newModel.getBatchQty();
            var childBatchQty = newModel.getChildBatchQty();
            var printControl = newModel.getPrintControl();
            SIE.invokeDataQuery({
                method: 'PrintControlChange',
                params: [generateingQty, batchQty, numberRuleId, printControl, childBatchQty, childNumberRuleId],
                action: 'queryer',
                type: 'SIE.Web.MES.BatchGeneration.WipBatchsDataQueryer',
                token: this.view.token,
                success: function (res) {
                    var rtnData = res.Result;
                    newModel.setBeginSn(rtnData.BeginSn);
                    newModel.setEndSn(rtnData.EndSn);
                    newModel.setChildBeginSn(rtnData.ChildBeginSn);
                    newModel.setChildEndSn(rtnData.ChildEndSn);
                }
            });
        }
    },
    getBeginSnEndSnValidata: function (newModel) {
        var validateFlag = true;
        if (newModel.data.NumberRuleId == null || newModel.data.NumberRuleId <= 0) {
            validateFlag = false;
            // SIE.Msg.showError("条码规则未设置!".t());
        }
        else if (newModel.data.BatchWoId == null || newModel.data.BatchWoId <= 0) {
            validateFlag = false;
            //SIE.Msg.showError("工单信息不正确!".t());
        }
        else if (newModel.data.GenerateingQty == null || newModel.data.GenerateingQty <= 0) {
            validateFlag = false;
            // SIE.Msg.showError("生成数量不正确!".t());
        }
        else if (newModel.data.BatchQty == null || newModel.data.BatchQty <= 0) {
            validateFlag = false;
            // SIE.Msg.showError("批次数量不正确!".t());
        }
        return validateFlag;
    },
});