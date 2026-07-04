Ext.define('SIE.Web.EMS.AssetScraps.Behaviors.AssetScrapFixtureDetailsBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view
         */
        onDataLoaded: function (view) {
            var me = this;
            var fixtureChildStore = view.getData();
            var formEntity = view.getParent().getCurrent();
            view.mon(fixtureChildStore, 'propertyChanged', me.onEntityPropertyChanged, view);

            for (var i = 0; i < fixtureChildStore.getCount(); i++) {

                var fixtureRecord = fixtureChildStore.getAt(i);
                fixtureRecord.setWarehouseId(formEntity.data.WarehouseId);
                fixtureRecord.setIsFixAsset(formEntity.data.IsFixAsset);
            }
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {

            var view = this;

            if (e.property == 'ScrapNetValue') {

                var fixtureChildStore = e.entity.store;
                var amountList = fixtureChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getScrapNetValue()); })
                    .select(function (p) { return parseFloat(p.getScrapNetValue()); });
                var amount = amountList.sum();

                e.entity._AssetScrap.setAmount(amount);
            }

            if (e.property == 'FixtureEncodeId' || e.property == 'StorageLocationId' || e.property == 'FixtureQualityState') {

                var warehouseId = view.getParent().getCurrent().data.WarehouseId;
                if (e.entity.data.FixtureEncodeId != null && warehouseId != null) {
                    SIE.invokeDataQuery({
                        method: 'GetCanUseNumByWarehouseId',
                        params: [warehouseId, e.entity.data.FixtureEncodeId, e.entity.data.StorageLocationId, e.entity.data.FixtureQualityState],
                        async: false,
                        action: 'queryer',
                        type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
                        token: view.token,
                        success: function (res) {
                            if (res.Success) {
                                e.entity.setStoreUsableQty(res.Result);
                            }
                        }
                    });
                }
                else {
                    e.entity.setStoreUsableQty(0);
                }
            }
        }
    });