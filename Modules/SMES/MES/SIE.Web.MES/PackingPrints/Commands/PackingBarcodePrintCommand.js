SIE.defineCommand('SIE.Web.MES.PackingPrints.Commands.PackingBarcodePrintCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        if (!view.getCurrent())
            return false;
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 1)
            return false;
        return true;
    },
    getEditEntity: function () {
        var numIni = 1;
        var curEntity = this.view.getCurrent();
        var curData = curEntity.getData();
        var model = SIE.getModel('SIE.MES.PackingPrints.ViewModels.PackingBarcodePrintViewModel');
        var entity = new model();
        entity.setWorkOrderId(curData.Id);
        entity.setWorkOrderNo(curData.No);
        entity.setWorkOrderPlanQty(curData.PlanQty);
        entity.setWorkOrderPlanBeginDate(curData.PlanBeginDate);
        entity.setPrintQty(numIni);
        entity.setPageCount(numIni);
        entity.token = this.view.token;
        this.mon(entity, 'propertyChanged', this.onEditPropertyChanged, this);
        return entity;
    },
    /*
     *弹出子窗体 
     */
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
                model: "SIE.MES.PackingPrints.ViewModels.PackingBarcodePrintViewModel",
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
                        title: "包装号打印".t(),
                        width: 900,
                        height: 520,
                        buttons: [
                            { xtype: "button", text: "确定".t(), hidden: true },
                            { xtype: "button", text: "取消".t(), hidden: true }
                        ],
                        items: ui,
                        id: "PackingBarcodePrintViewModel001",
                    });
                },
            });
        }
    },
    /*
     * 属性变更处理    
     */
    onEditPropertyChanged: function (e) {
        if (e.property.length > 0) {
            if (e.property == "PackageRuleDetailId" && e.entity.data.PackageRuleDetailId > 0 && e.value != e.oldvalue) {
                this.getChangedValue(e.entity,true);
            }
            else if ((e.property == "PrintQty" || e.property == "PrintControl") && e.entity.data.NumberRuleId > 0) {
                this.getChangedValue(e.entity,false);
            }
        }
    },
    /*
     * 获取开始结束条码
     * @newModel 当前实体  
     */
    getChangedValue: function (entity,flag) {
        if (!this.validateData(entity))
            return;
        var me = this.view;
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.PackingPrints.DataQueryers.PackingBarcodePrintDataQuery",
            method: "GetPrintData",
            params: [entity.data,flag],
            async: false,
            token: me.token,
            callback: function (res) {
                if (res.Success) {
                    var defaultInfo = res.Result;
                    entity.setProductQty(defaultInfo.ProductQty);
                    entity.setPrintedQty(defaultInfo.PrintedQty);
                    entity.setPrintQty(defaultInfo.PrintQty);
                    entity.setResidualQty(defaultInfo.ResidualQty);
                    entity.setNumberRuleId(defaultInfo.NumberRuleId);
                    entity.setNumberRuleId_Display(defaultInfo.NumberRuleName);
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
        if (entity.data.PackageRuleDetailId == null || entity.data.PackageRuleDetailId <= 0) {
            validateFlag = false;
            SIE.Msg.showMessage("工单包装规则不存在!".t());
        }
        else if (entity.data.WorkOrderId == null || entity.data.WorkOrderId <= 0) {
            validateFlag = false;
            SIE.Msg.showMessage("工单信息不正确!".t());
        }
        else if (entity.data.PrintQty == null || entity.data.PrintQty <= 0) {
            validateFlag = false;
            SIE.Msg.showMessage("打印数量不正确!".t());
        }
        return validateFlag;
    }
});