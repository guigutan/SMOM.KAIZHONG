Ext.define('SIE.Web.EMS.AssetReturns.Behaviors.AssetReturnEquipmentBehavior', {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view
         */
        onDataLoaded: function (view) {
            var me = this;
            var store = view.getData();
            for (var i = 0; i < store.getCount(); i++) {
                var record = store.getAt(i);

                if (record.data.ReturnType != null && record.data.ReturnType != 0) {
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
            if (e.property == 'ReturnType') {

                if (e.entity.data.ReturnType == null || e.entity.data.ReturnType == 0) {
                    me.getControl().getSelectionModel().deselect(e.entity);
                }
                else {
                    me.getControl().getSelectionModel().select(e.entity, true);
                }
            }
        }
    });