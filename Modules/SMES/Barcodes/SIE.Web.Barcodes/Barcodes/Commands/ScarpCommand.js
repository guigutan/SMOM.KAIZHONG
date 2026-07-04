SIE.defineCommand('SIE.Web.Barcodes.ScarpCommand', {
    meta: { text: "报废", group: "edit", iconCls: "icon-Delete icon-blue" },
    _scrapVM: null,
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.IsScraped === true) {
                res = false;
            }
        });
        return res;
    },
    execute: function (listView, source) {
        var me = this;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: false,
                isDetail: true,
                ignoreQuery: true,
                viewGroup: "DetailsView",
                token: this.view.token,
                model: "SIE.Web.Barcodes.ViewModels.ScrapBarcodeViewModel",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    var ui = detailView.getControl(); 
                    detailView.view = me.view;
                    var win = SIE.Window.show({
                        title: "条码报废".t(),
                        width: 480,
                        height: 250,
                        items: ui,
                        id: "ScarpViewModel001",
                        callback: function (btn) {
                            if (btn == "确定".t()) {
                                var reason = ui.viewModel.data.p.Reason;
                                var sModels = listView.getSelectionIds();
                                var view = detailView.view;
                                view.execute({
                                    data: { BarCodeIds: sModels, Reason: reason },
                                    success: function (res) {
                                        if (res.Success == true) {
                                            SIE.Msg.showInstantMessage('保存成功'.t());
                                            var selectedEntities= listView.getSelectedEntities();
                                            listView.unSelectEntities(selectedEntities);
                                            listView.reloadData();
                                            win.close();
                                        }
                                    }
                                });
                                return false;
                            }
                        }
                    });
                }
            });
        }
    }
});