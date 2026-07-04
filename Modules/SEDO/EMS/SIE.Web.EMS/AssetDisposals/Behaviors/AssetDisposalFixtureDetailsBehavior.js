Ext.define('SIE.Web.EMS.AssetDisposals.Behaviors.AssetDisposalFixtureDetailsBehavior',
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

            if (e.property == 'NetValue') {

                var fixtureChildStore = e.entity.store;
                var amountList = fixtureChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getNetValue()); })
                    .select(function (p) { return parseFloat(p.getNetValue()); });
                var amount = amountList.sum();

                e.entity._AssetDisposal.setAmount(amount);
            }
        }
    });