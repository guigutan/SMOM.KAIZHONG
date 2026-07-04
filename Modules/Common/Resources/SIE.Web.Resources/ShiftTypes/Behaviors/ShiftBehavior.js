Ext.define('SIE.Web.Resources.ShiftTypes.Behaviors.ShiftBehavior',
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
            if (e.property.length > 0) {
                if (e.property.indexOf('BeginTime') >= 0 || e.property.indexOf('EndTime') >= 0) {
                    if (e.entity.data.BeginTime != null && e.entity.data.EndTime != null &&
                        e.entity.data.BeginTime.toTimeString() > e.entity.data.EndTime.toTimeString()) {
                        e.entity.setIsOverDay(true);
                    }
                    else {
                        e.entity.setIsOverDay(false);
                    }
                }
            }
        }
    });