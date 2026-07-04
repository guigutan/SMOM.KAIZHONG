Ext.define('SIE.Web.EMS.AssetScraps.Behaviors.AssetScrapEquipmentDetailsBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view
         */
        onDataLoaded: function (view) {
            var me = this;
            var equipChildStore = view.getData();
            var formEntity = view.getParent().getCurrent();

            for (var i = 0; i < equipChildStore.getCount(); i++) {
                var equipRecord = equipChildStore.getAt(i);
                equipRecord.setFactoryId(formEntity.data.FactoryId);
                equipRecord.setManageDeptId(formEntity.data.ManageDeptId);
                equipRecord.setUseDeptId(formEntity.data.UseDeptId);
                equipRecord.setWarehouseId(formEntity.data.WarehouseId);
                equipRecord.setIsFixAsset(formEntity.data.IsFixAsset);
             }

            view.mon(equipChildStore, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {

            var view = this;
            if (e.property == 'EquipAccountId') {

                if (e.entity.data.EquipAccountId != null) {

                    SIE.invokeDataQuery({
                        method: 'GetAssetScrapEquipmentsById',
                        params: [e.entity.data.EquipAccountId],
                        async: false,
                        action: 'queryer',
                        type: 'SIE.Web.EMS.AssetScraps.DataQueryer.AssetScrapDataQueryer',
                        token: view.token,
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

                var equipChildStore = e.entity.store;
                var amountList = equipChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getScrapNetValue()); })
                    .select(function (p) { return parseFloat(p.getScrapNetValue()); });
                var amount = amountList.sum();

                e.entity._AssetScrap.setAmount(amount);
            }
        }
    });