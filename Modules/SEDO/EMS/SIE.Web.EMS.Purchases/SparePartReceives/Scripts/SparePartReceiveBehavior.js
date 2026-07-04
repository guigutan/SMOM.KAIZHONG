Ext.define('SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveBehavior', {
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
                    method: 'GetNewSparePartReceive',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer',
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
            let childView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveDetail"; });
            if (childView) {
                let store = childView.getData();
                SIE.each(store, function (entity) {
                    entity.setFactoryId(e.entity.data.FactoryId);
                    entity.setDepartmentId(e.entity.data.DepartmentId);
                    entity.setReceiveType(e.entity.data.ReceiveType);
                    entity.setPurchaseOrderId(null);
                    entity.setPurchaseOrderItemId(null);
                    if (e.property === 'ReceiveType' && e.value === 20) {
                        entity.setPrice(0);
                        entity.setTaxRate(0);
                        entity.setPriceNoTax(0);
                    }
                });
                childView.setData(store);
            }
        }
    }
});