SIE.defineCommand("SIE.Web.ProductIntfc.OutputProducts.Commands.OutputProductReceiveCommand", {
    meta: { text: "副产品收货", group: "edit", iconCls: "icon-Checkmark icon-blue" },
    _operationView: null,//操作弹窗视图
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        sel.forEach(p => {
            if (p.getState() == 3)    //关闭状态工单不允许收货
                return false;
        });
        return true;
    },
    execute: function (view) {
        var me = this;
        me.showReceiveView(view);
    },
    //弹出框
    showReceiveView: function (view) {
        var me = this;
        var woIds = me.view.getSelectionIds();

        SIE.AutoUI.getMeta({
            model: 'SIE.ProductIntfc.OutputProducts.OutputProductRecordViewModel',
            viewGroup: 'ListView',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var listView = SIE.AutoUI.createListView(mainBlock);

                me._operationView = listView;
                var filter = {
                    Method: 'GetOutputProductRecordViewModels',
                    Parameters: [woIds]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: 'SIE.Web.ProductIntfc.OutputProducts.DataQuery.OutputProductsDataQueryer',
                });
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "副产品收货".t(),
                    width: 1200,
                    height: 600,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {

                            var indata = listView.getData().data.items.map(p => p.data);
                            SIE.Msg.askQuestion('确认要提交收货吗?'.t(), function () {
                                view.execute({
                                    data: indata,
                                    success: function (res) {
                                        win.close();
                                        SIE.Msg.showInstantMessage("提交成功".t());
                                        view.reloadData();
                                    },
                                });
                            });

                            return false;
                        }
                    }
                });
            },
        });
    }

})