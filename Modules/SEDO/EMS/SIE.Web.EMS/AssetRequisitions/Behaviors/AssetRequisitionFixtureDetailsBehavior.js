Ext.define('SIE.Web.EMS.AssetRequisitions.Behaviors.AssetRequisitionFixtureDetailsBehavior',
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
            }
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {

            var view = this;
            if (e.property == 'EstimatedAmount') {

                var fixtureChildStore = e.entity.store;
                var amountList = fixtureChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getEstimatedAmount()); })
                    .select(function (p) { return parseFloat(p.getEstimatedAmount()); });
                var amount = amountList.sum();

                view.getParent().getData().setAmount(amount);
            }

            if (e.property == 'FixtureEncodeId') {

                var warehouseId = view.getParent().getCurrent().data.WarehouseId;
                if (e.entity.data.FixtureEncodeId != null && warehouseId != null) {
                    SIE.invokeDataQuery({
                        method: 'GetCanUseNumByWarehouseId',
                        params: [warehouseId, e.entity.data.FixtureEncodeId],
                        async: false,
                        action: 'queryer',
                        type: 'SIE.Web.EMS.AssetRequisitions.DataQueryer.AssetRequisitionDataQueryer',
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