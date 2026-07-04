Ext.define('SIE.Web.EMS.Checks.Plans.Scripts.CheckPlanSparePartAplBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {
        },
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {

        },

        onViewReady: function (view) {
        },

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
                            type: 'SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer',
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
