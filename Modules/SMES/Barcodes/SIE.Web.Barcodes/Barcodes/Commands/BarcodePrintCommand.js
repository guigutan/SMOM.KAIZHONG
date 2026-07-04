SIE.defineCommand('SIE.Web.Barcodes.BarcodePrintCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "单体打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 1)
            return false;
        if (entity != null && entity.data) {
            if (entity.data.UseOldSn == 1) return false;
            return entity.data.State != 4 && entity.data.PrintedQty < entity.data.PlanQty;
        }
        return false;
    },
    /*
     * @override 执行打印
     * @returns{}
     * 弹出打印明细视图
     */
    /*execute: function (view, source) {
        showView(view);
    },*/

    getEditEntity: function () {
        var numIni = 1;
        var curEntity = this.view.getCurrent();
        var curData = curEntity.getData();
        var printingQty = curData.PlanQty - curData.PrintedQty;
        var model = SIE.getModel('SIE.Web.Barcodes.BarcodePrintViewModel');
        var entity = new model();

        entity.setWorkOrderId(curData.Id);
        entity.setWorkOrderNo(curData.No);
        entity.setWorkOrderPlanQty(curData.PlanQty);
        entity.setWorkOrderPlanBeginDate(curData.PlanBeginDate);
        entity.setPrintedQty(curData.PrintedQty);
        entity.setResidualQty(printingQty);
        entity.setPrintQty(printingQty);
        entity.setSingleQty(numIni);
        entity.setPageCount(numIni);
        entity.token = this.view.token;
        this.getQureyQty(entity);
        this.mon(entity, 'propertyChanged', this.onEditPropertyChanged, this); 
        if (entity.data.TemplateId != null && entity.data.TemplateEntityType !== "SIE.Barcodes.Printables.BarcodePrintable,SIE.Barcodes") {
            SIE.Msg.showMessage("工单打印设置的默认标签模板不是条码类型的，请重新选择打印模板!".t()); 
            return;
        }
        if (entity.data.ResidualQty == 0) {
            SIE.Msg.showMessage("剩余数量为0，不可打印!".t());
            return;
        }
        return entity;
    },
    /*
     *弹出子窗体 
     */
    showView: function (editEntity) {
        if (!editEntity)
            return;
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
                model: "SIE.Web.Barcodes.BarcodePrintViewModel",
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
                        title: "单体打印 条码打印".t(),
                        width: 900,
                        height: 520,
                        buttons: [
                            { xtype: "button", text: "确定".t(), hidden: true },
                            { xtype: "button", text: "取消".t(), hidden: true }
                        ],
                        items: ui,
                        id: "BarcodePrintViewModel001",
                    });
                },
            });
        }
    },
    getQureyQty: function (entity) {
        var me = this.view;
        var workOrderId = entity.getWorkOrderId();
        SIE.invokeDataQuery({
            type: "SIE.Web.Barcodes.Barcodes.DataQuery.BarcodeDataQuery",
            method: "GetBarcodePrintData",
            params: [workOrderId],
            async: false,
            token: me.token,
            callback: function (res) {
                if (res.Success) {
                    var defaultInfo = res.Result;
                    entity.setDumpingQty(defaultInfo.DumpingQty);
                    entity.setNumberRuleId(defaultInfo.NumberRuleId);
                    entity.setNumberRuleId_Display(defaultInfo.NumberRuleName);
                    if (defaultInfo.TemplateId != null && defaultInfo.TemplateEntityType == "SIE.Barcodes.Printables.BarcodePrintable,SIE.Barcodes") {
                        entity.setTemplateId(defaultInfo.TemplateId);
                        entity.setTemplateId_Display(defaultInfo.TemplateName);
                    }
                    
                    entity.setBeginSn(defaultInfo.BeginSn);
                    entity.setEndSn(defaultInfo.EndSn);
                    entity.setBarcodeRuleDtl(defaultInfo.BarcodeRuleDtl);
                    entity.setTemplateEntityType(defaultInfo.TemplateEntityType);
                }
            }
        });
    },

    /*
     * 属性变更处理    
     */
    onEditPropertyChanged: function (e) {
        if (e.property.length > 0) {
            if (e.property == "NumberRuleId" && e.entity.data.NumberRuleId > 0 && e.value != e.oldvalue) {
                this.getChangedValue(e.entity);
            }
            else if ((e.property == "PrintQty" || e.property == "SingleQty" || e.property == "PrintControl") && e.entity.data.NumberRuleId > 0) {
                this.getChangedValue(e.entity);
            }
        }
    },
    /*
     * 获取开始结束条码
     * @newModel 当前实体  
     */
    getChangedValue: function (entity) {
        if (!this.validateData(entity))
            return;
        var me = this.view;
        SIE.invokeDataQuery({
            type: "SIE.Web.Barcodes.Barcodes.DataQuery.BarcodeDataQuery",
            method: "GetPrintData",
            params: [entity.data],
            async: false,
            token: me.token,
            callback: function (res) {
                if (res.Success) {
                    var defaultInfo = res.Result;
                    entity.setBeginSn(defaultInfo.BeginSn);
                    entity.setEndSn(defaultInfo.EndSn);
                    entity.setBarcodeRuleDtl(defaultInfo.BarcodeRuleDtl);
                    entity.setTemplateEntityType(defaultInfo.TemplateEntityType);
                }
            }
        });
    },
    /*
     * 获取开始结束条码-验证
     * @newModel 当前实体  
     */
    validateData: function (entity) {
        var validateFlag = true;
        if (entity.data.NumberRuleId == null || entity.data.NumberRuleId <= 0) {
            validateFlag = false;
            SIE.Msg.showMessage("条码规则未设置!".t());
        }
        else if (entity.data.WorkOrderId == null || entity.data.WorkOrderId <= 0) {
            validateFlag = false;
            SIE.Msg.showMessage("工单信息不正确!".t());
        }
        else if (entity.data.PrintQty == null || entity.data.PrintQty <= 0) {
            validateFlag = false;
            SIE.Msg.showMessage("打印数量不正确!".t());
        }
        else if (entity.data.SingleQty == null && entity.data.SingleQty <= 0) {
            validateFlag = false;
            SIE.Msg.showMessage("单张数量不正确!".t());
        }

        return validateFlag;
    },
});