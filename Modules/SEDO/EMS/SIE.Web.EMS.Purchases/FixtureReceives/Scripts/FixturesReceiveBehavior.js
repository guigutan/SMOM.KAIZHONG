Ext.define('SIE.Web.EMS.Purchases.FixtureReceives.FixturesReceiveBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.action = params.action;
            if (params.action === 0) {
                SIE.invokeDataQuery({
                    method: 'GetNewFixtureReceive',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setReceiveNo(info.ReceiveNo);
                            entity.setReceiveBillStatus(info.ReceiveBillStatus);
                            entity.setReceiveType(info.ReceiveType);
                        }
                    }
                });
            }
        }
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'FactoryId' || e.property === 'DepartmentId' || e.property === 'ReceiveType') {
            let childView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
            if (childView) {
                let store = childView.getData();
                SIE.each(store, function (entity) {
                    entity.setFactoryId(e.entity.data.FactoryId);
                    entity.setDepartmentId(e.entity.data.DepartmentId);
                    entity.setReceiveType(e.entity.data.ReceiveType);
                    entity.setPurchaseOrderId(null);
                    entity.setPurchaseOrderItemId(null);
                    entity.setFixtureEncodeId(null);

                    entity.setModelCode("");
                    entity.setModelName("");
                    entity.setManageMode("");
                });
                childView.setData(store);
            }
        }
        if (e.property === 'ReceiveType') {
            let childView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
            if (childView) {
                let store = childView.getData();
                SIE.each(store, function (entity) {
                    if (e.value === 30) {
                        entity.setSupplierId(null);
                        entity.setSupplierName("");
                    } else {
                        entity.setCustomerId(null);
                        entity.setCustomerName("");
                    }
                    if (e.value === 20) {
                        entity.setGiveaway(true);
                        entity.setPrice(0);
                    } else if (e.value > 20) {
                        entity.setGiveaway(false);
                    }
                });
                childView.setData(store);
            }
        }
    }
});