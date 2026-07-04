Ext.define('SIE.Web.EMS.Lubrications.Behaviors.LubricationSparePartAplBehavior',
    {
        
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            if (view) {
                var records = view.getData();
                if (records) {
                    view.mon(records, 'propertyChanged', me.onPropertyChanged, view);
                }
            };
        },

        /**
        * 属性变更处理*/
        onPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property === 'OutStockWarehouseId') {
                    var warehouseId = entity.getOutStockWarehouseId();
                    if (warehouseId != null) {
                        SIE.invokeDataQuery({
                            method: 'GetSparePartStoreQty',
                            params: [entity.getSparePartId(), entity.getOutStockWarehouseId()],
                            action: 'queryer',
                            type: 'SIE.Web.EMS.Lubrications.DataQuery.LubricationDataQuery',
                            token: me.token,
                            success: function (res) {
                                if (res.Success) {
                                    entity.setStoreQty(res.Result);
                                }
                            }
                        });
                    }
                    else {
                        entity.setStoreQty(0);
                    }
                };
            }
        },
    });
