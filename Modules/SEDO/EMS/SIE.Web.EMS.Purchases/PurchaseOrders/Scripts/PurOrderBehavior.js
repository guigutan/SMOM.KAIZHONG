Ext.define('SIE.Web.EMS.Purchases.PurchaseOrders.PurOrderBehavior', {
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
                    method: 'GetNewPurchaseOrder',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Purchases.PurchaseOrders.PurchaseOrderDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setOrderNo(info.OrderNo);
                            entity.setPurchaseOrderStatus(info.PurchaseOrderStatus);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setCurrency(info.Currency);
                            entity.setAmountUnit(info.Currency);
                            entity.setBuyerId(info.BuyerId);
                            entity.setPurchaseObjectType(info.PurchaseObjectType);
                            var userInfo = CRT.Context.GlobalContext.getContext('userInfo');
                            entity.setBuyerId_Display(userInfo.Name);
                        }
                    }
                });
            }
        }
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onDataLoaded: function (view) {
        var qty = view._children.length;
        var tabPanel = view._children[0].getControl().ownerCt.ownerCt;
        for (var i = 1; i < qty; i++) {
            tabPanel.setActiveTab(i);
        }
        tabPanel.setActiveTab(0);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'Currency') {
            e.entity.setAmountUnit(e.value);
            let payView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.PurchaseOrders.PaymentTerms"; });
            if (payView) {
                let payStore = payView.getData();
                SIE.each(payStore, function (entity) {
                    entity.setCurrency(e.value);
                    entity.setAmountUnit(e.value);
                });
                payView.setData(payStore);
            }
        }
        if (e.property === 'TotalAmount') {
            let payView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.PurchaseOrders.PaymentTerms"; });
            if (payView) {
                let payStore = payView.getData();
                SIE.each(payStore, function (entity) {
                    let oldPercent = entity.getPercent();
                    let amount = oldPercent * e.value / 100;
                    amount = Math.floor(amount * 100) / 100;
                    entity.setAmount(amount);
                });
                payView.setData(payStore);
            }
        }
        if (e.property === 'FactoryId' || e.property === 'DepartmentId' || e.property === 'PurchaseObjectType') {
            e.entity.setVarietyQuantity(0);
            e.entity.setTotalAmount(0);
            var childView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.PurchaseOrders.PurchaseOrderItem"; });
            if (childView) {
                var store = childView.getData();
                store.removeAll();
                childView.setData(store);
            }
        }
    }
});