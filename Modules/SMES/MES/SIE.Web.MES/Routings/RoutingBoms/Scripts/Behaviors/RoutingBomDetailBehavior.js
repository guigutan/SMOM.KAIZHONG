Ext.define('SIE.Web.MES.Routings.RoutingBoms.Scripts.Behaviors.RoutingBomDetailBehavior',
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
            var parentData = me._parent.getCurrent().data;
            if (parentData == null || parentData == undefined)
                return;
            if (e.property.length > 0) {
                var entity = e.entity;
                var mainCode = null;
                if (e.property === 'MaterialId') {
                    entity.setWorkStepId(null);
                    entity.setWorkStepId_Display("");
                    entity.setRoutingProcessId(null);
                    entity.setRoutingProcessId_Display("");
                    entity.setMainMaterialId(null);
                    entity.setMainMaterialId_Display("");
                    SIE.invokeDataQuery({
                        type: "SIE.Web.MES.Routings.RoutingBoms.DataQueryers.RoutingBomDetailDataQuery",
                        method: "GetProductBomDetailViewModel",
                        params: [parentData.ProductId, entity.getMaterialId(), parentData.ProcessSegmentId],
                        async: false,
                        token: me.token,
                        callback: function (res) {
                            if (res.Success) {
                                var defaultInfo = res.Result;
                                if (defaultInfo != null && res.Result.data.items.length > 0) {
                                    mainCode = res.Result.data.items[0].getMainMaterialCode();
                                    entity.setAmount(res.Result.data.items[0].getUnitQty())
                                }
                            }
                        }
                    });
                    if (mainCode) {
                        SIE.invokeDataQuery({
                            type: "SIE.Web.MES.Routings.RoutingBoms.DataQueryers.RoutingBomDetailDataQuery",
                            method: "GetItemByCode",
                            params: [mainCode],
                            async: false,
                            token: me.token,
                            callback: function (res) {
                                if (res.Success) {
                                    var defaultInfo = res.Result;
                                    if (defaultInfo != null && res.Result.data.items.length > 0) {
                                        entity.setMainMaterialId(res.Result.data.items[0].getId());
                                        entity.setMainMaterialId_Display(res.Result.data.items[0].getCode());
                                    } else {
                                        entity.setMainMaterialId(null);
                                        entity.setMainMaterialId_Display("");
                                    }
                                }
                            }
                        });
                    }
                }
                else if (e.property === 'RoutingProcessId') {
                    entity.setWorkStepId(null);
                    entity.setWorkStepId_Display("");
                }
            }
        }
    });