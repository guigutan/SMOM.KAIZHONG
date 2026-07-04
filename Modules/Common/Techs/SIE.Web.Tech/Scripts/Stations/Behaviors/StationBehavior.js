Ext.define('SIE.Web.Tech.Scripts.Behaviors.StationBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        //onViewReady: function (view) {
        //    var me = this;
        //    var entity = view.getCurrent();
        //    view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
        //},
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
            var me = this;
            if (e.property.length > 0) {
                var entity = e.entity;
                if (e.property === 'ProcessId') {
                    entity.setWorkStepId(null);
                    entity.setWorkStepId_Display("");
                }
            }
        }
    });