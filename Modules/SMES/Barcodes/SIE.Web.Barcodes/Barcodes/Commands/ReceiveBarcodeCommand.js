SIE.defineCommand('SIE.Web.Barcodes.ReceiveBarcodeCommand', {
    meta: { text: "领用", group: "edit", iconCls: "icon-Receive icon-blue" },
    _scrapVM: null,
    canExecute: function (listView) {
        var entity = listView.getCurrent();
        return entity != null && entity.data.State == 0;
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
                model: "SIE.Web.Barcodes.ViewModels.ReceiveBarcodeViewModel",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    var ui = detailView.getControl();
                    detailView.listView = listView;
                    detailView.view = me.view;
                    var win = SIE.Window.show({
                        title: "条码领用".t(),
                        width: 300,
                        height: 200,
                        items: ui,
                        id: "ReceiveViewModel001",
                        callback: function (btn) {
                            if (btn == "确定".t()) {
                                var useName = ui.viewModel.data.p.UserName;
                                var passWord = ui.viewModel.data.p.Password;
                                var entity = detailView.listView.getCurrent();
                                var barcodeId = entity.data.Id;
                                var view = detailView.view;
                                if (Ext.String.trim(useName) == "") {
                                    SIE.Msg.showError("请输入用户名和密码!".t());
                                }
                                else {
                                    view.execute({
                                        data: { BarCodeId: barcodeId, UseName: useName, Password: passWord },
                                        success: function (res) {
                                            if (res.Result == true) {
                                                listView.reloadData();
                                                SIE.Msg.showMessage('领用成功!'.t());
                                                win.close();
                                            }
                                        }
                                    });
                                }
                                return false;
                            }
                        }
                    });
                }
            });
        }
    }
});