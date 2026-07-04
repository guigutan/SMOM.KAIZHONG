SIE.defineCommand('SIE.Web.Fixtures.InboundOrders.Commands.InboundCommand', {
    meta: { text: "入库", group: "edit", iconCls: "icon-WarehouseImport icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.InboundStatus !== 10 && model.data.InboundStatus != 20) {//待入库、入库中、所选入库单的保养状态是否为空或者保养完成
                res = false;
                return false;
            }
            if (!model.data.MaintainStatus || model.data.MaintainStatus == 15) {
                res = true;
                return true;
            } else {
                res = false;
                return false;
            }

        });
        return res;
    },
    execute: function (view, source) {
        var datas = [];

        view.getSelection().forEach(function (p) { datas.push(p.getId()) });
        var entity = view.getSelection()[0].getData();
        var ch1 = view._children[0].getData();
        SIE.AutoUI.getMeta({
            model: "SIE.Fixtures.InboundOrders.InboundOrder",
            ignoreCommands: false,
            isDetail: true,
            viewGroup: entity.ManageMode == 5 ? 'DetailsView' :"CodeDetailsView",
            ignoreQuery: true,
            callback: function (res) {
                var mainBolck;
                mainBolck = res;
                var detailView = SIE.AutoUI.generateAggtControl(mainBolck);
                detailView.token = view.getToken();
                detailView._view._setDefaultValue(view.getCurrent());
                detailView._view.setData(view.getCurrent());
                var ui = detailView.getControl();
                //设置默认的库位
                SIE.invokeDataQuery({
                    type: "SIE.Web.Fixtures.InboundOrders.DataQuery.InboundOrdersDataQueryer",
                    method: "GetDefaultLocation",
                    params: [view.getCurrent().getId()],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        if (res.Success) {
                            var data = res.Result;
                            if (data) {
                                var dataitems = detailView._view._children[0].getData().getData().items;
                                Ext.each(dataitems, function (item) {
                                    if (item.data.StorageLocationId == null) {
                                        item.data.StorageLocationId_Display = data.data.items[0].getName();
                                        item.data.StorageLocationId = data.data.items[0].getId();
                                        item.data.StorageLocation = data.data.items[0].data;
                                        detailView._view.getCurrent().dirty = true;
                                        detailView._view.getCurrent().setScanedNum(detailView._view.getCurrent().getScanedNum()+1);
                                        item.dirty = true;
                                    }
                                });
                            }
                        }
                    }
                });


                warehouseImport_win = SIE.Window.show({
                    title: "入库".t(),
                    width: 800,
                    height: 600,
                    buttons: [],
                    items: ui,
                    callback: function (btn) {
                        return false;
                    }
                });
                warehouseImport_win.ParentView = view;
            }
        });
    }
});