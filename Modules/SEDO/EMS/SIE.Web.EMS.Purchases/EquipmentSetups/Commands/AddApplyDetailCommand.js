SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddApplyDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'SparePartId' || e.property === 'WarehouseId') {
            let sparePartId = e.entity.getSparePartId();
            let warehouseId = e.entity.getWarehouseId();
            if (sparePartId > 0 && warehouseId > 0) {
                SIE.invokeDataQuery({
                    method: 'GetWarehouseQty',
                    params: [sparePartId, warehouseId],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Purchases.EquipmentSetups.EquipmentSetupDataQueryer',
                    token: me.token,
                    success: function (res) {
                        if (res.Result != null) {
                            e.entity.setWarehouseQty(res.Result);
                        }
                    }
                });
            } else {
                e.entity.setWarehouseQty(null);
            }
        }
    }
});