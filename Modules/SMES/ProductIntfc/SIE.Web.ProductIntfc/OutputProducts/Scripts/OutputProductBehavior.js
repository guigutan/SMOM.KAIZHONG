Ext.define('SIE.Web.ProductIntfc.OutputProducts.OutputProductBehavior',
    {
        onViewReady: function (view) {
            var me = this;
            var entity = view.getCurrent();
            alert(222);
            view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
        },
        /**
         * 属性变更处理
         * @param {any} 
         */
        onEntityPropertyChanged: function (e, opt) {
            var me = this;
            var entity = e.entity;

            if (e.property === "ItemId") {
                SIE.invokeDataQuery({
                    type: "SIE.Web.ProductIntfc.OutputProducts.DataQuery.OutputProductsDataQueryer",
                    method: "GetBillWh",
                    params: [entity.data.OutPutType],
                    token: me.view.getToken(),
                    async: false,
                    callback: function callback(res) {
                        if (res.Success) {
                            entity.setWarehouseId(res.Result.WarehouseId);
                            entity.setWarehouseId_Display(res.Result.Code);
                        }
                    }
                });
            }
        }

    });