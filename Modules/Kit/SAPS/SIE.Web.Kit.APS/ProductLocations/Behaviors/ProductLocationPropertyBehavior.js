Ext.define('SIE.Web.Kit.APS.ProductLocations.Behaviors.ProductLocationPropertyBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            this.view = view;
            var entity = view.getData();
            view.mon(entity, "propertyChanged", me.onPropertyChanged, me);
        },

        /**
         * 属性变更处理
         * @param {any} 
         */
        onPropertyChanged: function (e) {
            var me = this;
            if (e.property.length > 0) {
                var entity = e.entity;
                if (e.property === "Classification" ) {
                    //分类修改分类值
                    entity.setTypeValue(null);
                    entity.setMinValue(null);
                    entity.setMaxValue(null);
                }
            }
        }
    });