Ext.define('SIE.Web.Items.ProductBoms.Behaviors.ProductBomListBehavior',
    {

        /**
        * view生命周期函数--数据加载后
        * @param {any} view 逻辑视图
        */
        onDataLoaded: function (view) {
            var me = this;
            this.view = view;
            var entity = view.getData();
            view.mun(entity, 'propertyChanged');
            view.mon(entity, "propertyChanged", me.onPropertyChanged, me);
        },

        /**
       * 属性变更处理
       * @param {any} 
       */
        onPropertyChanged: function (e) {
            if (e.property.length > 0) {
                var entity = e.entity;
                var data = entity.data;
                var token = this.view.token;
                if (e.property == "ProductId") {
                    if (e.value == null) {
                        SIE.invokeDataQuery({
                            type: "SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery",
                            method: "ValidateApsRef",
                            params: [data],
                            async: false,
                            token: token,
                            callback: function (res) {

                            },
                        });
                    }
                }
            }
        }
    });