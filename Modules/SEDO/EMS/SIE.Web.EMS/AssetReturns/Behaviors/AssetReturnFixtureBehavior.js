Ext.define('SIE.Web.EMS.AssetReturns.Behaviors.AssetReturnFixtureBehavior', {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view
         */
        onDataLoaded: function (view) {
            var me = this;
            var store = view.getData();
            for (var i = 0; i < store.getCount(); i++) {
                var record = store.getAt(i);

                if (record.data.Qty > 0) {
                    view.getControl().getSelectionModel().select(record, true);
                }
            }
            view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {
            var me = this;

            if (e.property == 'ReturnType' && e.entity.data.ManageMode == 5) {

                e.entity.setQty((e.entity.data.ReturnType == null || e.entity.data.ReturnType == 0) ? 0 : 1);
            }

            if (e.property == 'Qty') {

                if (e.entity.data.Qty > 0) {

                    me.getControl().getSelectionModel().select(e.entity, true);
                }
                else {
                    me.getControl().getSelectionModel().deselect(e.entity);
                }
            }
        }
    });