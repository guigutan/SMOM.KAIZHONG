Ext.define('SIE.Web.EMS.AssetIssues.Behaviors.AssetIssueFixtureBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view
         */
        onDataLoaded: function (view) {
            var me = this;
            var fixtureChildStore = view.getData();
            var formEntity = view.getParent().getCurrent();

            for (var i = 0; i < fixtureChildStore.getCount(); i++) {
                var fixtureRecord = fixtureChildStore.getAt(i);
                fixtureRecord.setWarehouseId(formEntity.data.WarehouseId);

                if (fixtureRecord.data.Qty > 0) {
                    view.getControl().getSelectionModel().select(fixtureRecord, true);
                }
            }
            view.mon(fixtureChildStore, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {
            var view = this;

            if (e.property == 'FixtureAccountId') {

                e.entity.setQty(e.entity.data.FixtureAccountId == null ? 0 : 1);
            }

            if (e.property == 'Qty') {

                if (e.entity.data.Qty > 0) {

                    view.getControl().getSelectionModel().select(e.entity, true);
                }
                else {
                    view.getControl().getSelectionModel().deselect(e.entity);
                }
            }

            if (e.property == 'FixtureEncodeId' || e.property == 'StorageLocationId' || e.property == 'QualityStatus') {

                var warehouseId = view.getParent().getCurrent().data.WarehouseId;
                if (e.entity.data.FixtureEncodeId != null && warehouseId != null) {
                    SIE.invokeDataQuery({
                        method: 'GetCanUseNumByWarehouseId',
                        params: [warehouseId, e.entity.data.FixtureEncodeId, e.entity.data.StorageLocationId, e.entity.data.QualityStatus],
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