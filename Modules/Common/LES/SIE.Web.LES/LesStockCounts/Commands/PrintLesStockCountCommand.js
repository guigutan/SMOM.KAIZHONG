SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.PrintLesStockCountCommand', {
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var billIds = view.getSelectionIds();
        var orderType = view.getCurrent().getOrderType();
        if (view.getSelection().any(function (f) { return f.data.OrderType != orderType })) {
            me.showWin(view, null, billIds);
        }
        else {
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
                method: 'GetStockCountNumberRule',
                token: view.token,
                params: [orderType],
                success: function (res) {
                    me.showWin(view, res.Result, billIds);
                },
                error: function (res) {
                    SIE.Msg.showError(res.Message);
                }
            });
        }
    },
    showWin: function (view, entityData, billIds) {
        SIE.AutoUI.getMeta({
            model: 'SIE.LES.LesStockCounts.ViewModels.LesStockCountPrintViewModel',
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
                SIE.Web.Core.CommonFuns.getBillPrintCache(entity, view.model);
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
                            SIE.Web.Core.CommonFuns.setBillPrintCache(entity.data, view.model);
                            view.execute({
                                data: { BillIdList: billIds, BillTemplateId: indata.BillTemplateId },
                                success: function (r) { //回调                                      
                                    SIE.Web.Core.CommonFuns.ShowPrintPreview(r);
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