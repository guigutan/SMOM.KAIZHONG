SIE.defineCommand('SIE.Web.EMS.AssetScraps.Commands.AddAssetScrapEquipmentCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var formEntity = me.view.getParent().getCurrent();

        entity.setFactoryId(formEntity.getFactoryId());
        entity.setManageDeptId(formEntity.getManageDeptId());
        entity.setUseDeptId(formEntity.getUseDeptId());
        entity.setWarehouseId(formEntity.getWarehouseId());
        entity.setIsFixAsset(formEntity.getIsFixAsset());

        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;

        if (e.property == 'EquipAccountId') {

            if (e.entity.data.EquipAccountId != null) {

                SIE.invokeDataQuery({
                    method: 'GetAssetScrapEquipmentsById',
                    params: [e.entity.data.EquipAccountId],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetScraps.DataQueryer.AssetScrapDataQueryer',
                    token: me.view.token,
                    success: function (res) {

                        if (res.Success) {

                            if (res.Result.data.items.length > 0) {

                                var record = res.Result.data.items[0];
                                e.entity.setRepairHours(record.data.RepairHours);
                                e.entity.setMaintenanceHours(record.data.MaintenanceHours);
                                e.entity.setSparePartCost(record.data.SparePartCost);
                                e.entity.setOutRepairCost(record.data.OutRepairCost);
                                e.entity.setTotalRepairHours(record.data.TotalRepairHours);
                                e.entity.setTotalSparePartCost(record.data.TotalSparePartCost);
                            }

                        }
                    }
                });

            }
            else {
                e.entity.setRepairHours(0);
                e.entity.setMaintenanceHours(0);
                e.entity.setSparePartCost(0);
                e.entity.setOutRepairCost(0);
                e.entity.setTotalRepairHours(0);
                e.entity.setTotalSparePartCost(0);
            }
        }

        if (e.property == 'ScrapNetValue') {

            var equipChildStore = me.view.getData();
            var amountList = equipChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getScrapNetValue()); })
                .select(function (p) { return parseFloat(p.getScrapNetValue()); });
            var amount = amountList.sum();

            e.entity._AssetScrap.setAmount(amount);
        }
    }

});