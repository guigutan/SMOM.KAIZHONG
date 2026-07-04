Ext.define('SIE.Web.EMS.Lubrications.Behaviors.LubricationDetailAddBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            var entity = view.getData();
            view.mon(entity, "propertyChanged", me.onPropertyChanged, me);
        },

        /**
         * 属性变更处理
         * @param {any} 
         */
        onPropertyChanged: function (e) {
            if (e.property.length > 0) {
                var entity = e.entity;
                if (e.property === "ActualValue") {
                    var minValue = entity.getMinValue();
                    var maxValue = entity.getMaxValue();
                    if (minValue != null && maxValue != null) {
                        if (e.value < minValue || e.value > maxValue) {
                            entity.setActualValue(null);
                            SIE.Msg.showInstantMessage('实际加油量只能在加油量下限与加油量上限之间!'.t());
                        }
                    }
                }
            }
        }
    });