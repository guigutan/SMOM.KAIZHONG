SIE.defineCommand('SIE.Web.Barcodes.Panels.Commands.PanelPrintCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 1)
            return false;
        if (entity != null && entity.data) {
            if (entity.data.UseOldSn == 1) return false;
            if (entity.data.PanelQty <= 0) return false;
            return entity.data.State != 4;
        }
        return false;
    },
    getEditEntity: function () {
        var numIni = 1;
        var curEntity = this.view.getCurrent();
        var curData = curEntity.getData();
        var model = SIE.getModel('SIE.Web.Barcodes.Panels.ViewModels.PanelPrintViewModel');
        var entity = new model();
        entity.setWorkOrderId(curData.Id);
        entity.setWorkOrderNo(curData.No);
        entity.setWorkOrderPlanQty(curData.PlanQty);
        entity.setWorkOrderPlanBeginDate(curData.PlanBeginDate);
        entity.setPanelQty(curData.PanelQty);
        entity.setPrintedQty(curData.PanelPrintQty);
        if (curData.PanelQty > 0) {

            var printingQty = curData.PlanQty / curData.PanelQty;
            if (curData.PlanQty % curData.PanelQty == 0)
                entity.setPrintQty(printingQty - curData.PanelPrintQty);
            else
                entity.setPrintQty(printingQty + 1 - curData.PanelPrintQty);

            if (entity.getPrintQty() < 0)
                entity.setPrintQty(0);
        }
        entity.setPageCount(numIni);
        entity.token = this.view.token;
        this.getQureyQty(entity);
        this.mon(entity, 'propertyChanged', this.onEditPropertyChanged, this);
        return entity;
    },
    /*
     *弹出子窗体 
     */
    showView: function (editEntity) {
        if (editEntity.getNumberRuleId() == null) {
            SIE.Msg.showMessage("未配置拼板码打印规则和模板,请检查规则配置！".t());
            return;
        }
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
                model: "SIE.Web.Barcodes.Panels.ViewModels.PanelPrintViewModel",
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
                        title: "拼板码打印".t(),
                        width: 900,
                        height: 520,
                        buttons: [
                            { xtype: "button", text: "确定".t(), hidden: true },
                            { xtype: "button", text: "取消".t(), hidden: true }
                        ],
                        items: ui,
                        id: "PanelPrintViewModel001",
                    });
                },
            });
        }
    },
    getQureyQty: function (entity) {
        var me = this.view;
        var workOrderId = entity.getWorkOrderId();
        SIE.invokeDataQuery({
            type: "SIE.Web.Barcodes.Panels.DataQueryers.PanelPrintDataQuery",
            method: "GetBarcodePrintData",
            params: [workOrderId],
            async: false,
            token: me.token,
            callback: function (res) {
                if (res.Success && res.Result != null) {
                    var defaultInfo = res.Result;
                    entity.setDumpingQty(defaultInfo.DumpingQty);
                    entity.setNumberRuleId(defaultInfo.NumberRuleId);
                    entity.setNumberRuleId_Display(defaultInfo.NumberRuleName);
                    entity.setTemplateId(defaultInfo.TemplateId);
                    entity.setTemplateId_Display(defaultInfo.TemplateName);
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
            else if ((e.property == "PrintQty" || e.property == "PanelQty" || e.property == "PrintControl") && e.entity.data.NumberRuleId > 0) {
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
            type: "SIE.Web.Barcodes.Panels.DataQueryers.PanelPrintDataQuery",
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
        return validateFlag;
    }
});