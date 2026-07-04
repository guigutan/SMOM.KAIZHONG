SIE.defineCommand('SIE.Web.LES.Distributions.Commands.PrintBillDistributionCommand', {
    meta: { text: "打印单据", group: "edit", /*hierarchy: "打印",*/ iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var billIds = view.getSelectionIds();       
        me.showWin(view, null, billIds);              
    },
    showWin: function (view, entityData, billIds) {
        SIE.AutoUI.getMeta({
            model: 'SIE.LES.Distributions.Printables.DistributionPrintViewModel',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                SIE.Web.WMS.CommmonAction.getBillPrintCache(entity, view.model);
                if (!(entity.getBillTemplateId() > 0) && entityData && entityData != null) {
                    entity.setBillTemplateId(entityData.TemplateId);
                    entity.setBillTemplateId_Display(entityData.TemplateName);
                }
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "打印".t(),
                    width: 400,
                    height: 240,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.BillTemplateId == null) {
                                SIE.Msg.showError("模板不能为空!".t());
                                return false;
                            }
                            SIE.Web.WMS.CommmonAction.setBillPrintCache(entity.data, view.model);
                            view.execute({
                                data: { BillIdList: billIds, BillTemplateId: indata.BillTemplateId },
                                success: function (r) { //回调                                      
                                    SIE.Web.WMS.CommmonAction.ShowPrintPreview(r);
                                    win.close();
                                }
                            });
                        }
                    }
                });
            }
        });
    }
});