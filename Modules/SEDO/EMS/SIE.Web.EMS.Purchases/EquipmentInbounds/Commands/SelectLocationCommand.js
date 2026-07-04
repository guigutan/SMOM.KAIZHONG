SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentInbounds.Commands.SelectLocationCommand', {
    meta: { text: "批量选择库位", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        if (view._parent == null)
            return false;
        var entity = view._parent.getCurrent();
        if (entity == null)
            return false;
        if (entity.getInboundStatus() !== 10)
            return false;
        if (entity.getWarehouse() == null)
            return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;

        var warehouseId = view._parent.getCurrent().getWarehouseId();
        var inboundId = view._parent.getCurrent().getId();

        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Purchases.EquipmentInbounds.EquipmentInboundDetail",
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.createDetailView(res);
                var entity = new detailView._model();
                entity.setWarehouseId(warehouseId);
                detailView.setData(entity);
                var win = SIE.Window.show({
                    title: "批量选择库位".t(),
                    width: 480,
                    height: 200,
                    items: detailView.getControl(),
                    id: "SelectLocationCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var detailInfo = detailView.getData().data;

                            //电子签名信息
                            var signdata = {
                                command: me.meta.command,
                                entityType: me.view.model,
                                parentType: me.view.getParent() ? me.view.getParent().model : ""
                            }

                            SIE.invokeDataQuery({
                                type: "SIE.Web.EMS.Purchases.EquipmentInbounds.EquipmentInboundDataQueryer",
                                method: "SelectLocation",
                                params: [detailInfo.StorageLocationId, inboundId],
                                async: false,
                                token: view.token,
                                logInfo: signdata,
                                success: function (resa) {
                                    win.close();
                                    view.loadData();
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