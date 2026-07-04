Ext.define('SIE.Web.Fixtures.Repairs.Script.FixtureRepairDetailBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            var store = view.getData();
            view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        /**
         * onEntityPropertyChanged 属性变更事件
         * @param {*} e 参数
         */
        onEntityPropertyChanged: function (e) {
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property.indexOf('InspectionResult') >= 0) {
                    if (e.value != null) {
                        entity.setRepairWhereaboutStatus(10);
                    } else {
                        entity.setRepairWhereaboutStatus(null);
                    }
                }
            }
        }
    });