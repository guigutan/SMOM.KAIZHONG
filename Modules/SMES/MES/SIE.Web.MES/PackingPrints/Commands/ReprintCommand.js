SIE.defineCommand('SIE.Web.MES.PackingPrints.Commands.ReprintCommand', {
    meta: { text: "补打", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        return true;
    },
    execute: function (view, source) {
        SIE.AutoUI.getMeta({
            model: "SIE.MES.PackingPrints.ViewModels.ReprintInfoViewModel",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var mainBolck;
                if (res.mainBolck)
                    mainBolck = res.mainBolck;
                else
                    mainBolck = res;
                var detailView = SIE.AutoUI.createDetailView(mainBolck);
                detailView.token = view.getToken();
                var entity = new detailView._model();
                var ui = detailView.getControl();
                entity.setTimes(1);
                detailView.setData(entity);
                var win = SIE.Window.show({
                    title: "包装号补打".t(),
                    width: 480,
                    height: 250,
                    items: ui,
                    id: "PackingReprintInfo001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var reprintInfo = detailView.getData().data;
                            if (!reprintInfo.TemplateId || reprintInfo.TemplateId <= 0) {
                                SIE.Msg.showMessage("打印模板不能为空".L10N());
                                return false;
                            }
                            if (!reprintInfo.Reason) {
                                SIE.Msg.showMessage("补打原因不能为空".L10N());
                                return false;
                            }
                            var selModels = view.getSelectionModel();
                            view.execute({
                                data: { BarCodeIds: view.getSelectionIds(), Reason: reprintInfo.Reason, Times: reprintInfo.Times, TemplateId: reprintInfo.TemplateId },
                                withIds: true,
                                selectIds: view.getSelectionIds(),
                                success: function (res) {
                                    win.close();
                                    var param = { content: res.Result };
                                    CRT.Workbench.showPageDialog({
                                        id: 'PackingReprint_rpt',
                                        text: "包装号补打".t(),
                                        url: '/Modules/PrintTemplate/DevPrint',
                                        params: param,
                                        method: 'POST'
                                    });
                                    selModels.deselectAll();
                                    view.reloadData();
                                },
                                error: function (res) {
                                    SIE.Msg.showMessage(res.Result);
                                    return false;
                                }
                            });
                            return false;
                        }
                    }
                });
            }
        });
    }
});