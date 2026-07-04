Ext.define('SIE.Web.EMS.Purchases.PurchaseOrders.PaymentTermsBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        me.isMarkSaved = true;
        me.setPaymentStore(store);
        view.PayBehavior = me;
        view.amountUpdate = true;
        view.percentUpdate = true;
        view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    setPaymentStore: function (store) {
        var me = this;
        var amount = 0;
        var percent = 0;
        SIE.each(store, function (entity) {
            amount = amount + entity.getAmount();
            percent = percent + entity.getPercent();
            amount = Math.floor(amount * 100) / 100;
            percent = Math.floor(percent * 100) / 100;
            entity.setCumulativeAmount(amount);
            entity.setCumulativePercent(percent);
            if (me.isMarkSaved) {
                entity.markSaved();
            }
        });
        me.isMarkSaved = false;
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'Percent' && me.percentUpdate) {
            let totalAmount = me._parent.getCurrent().getTotalAmount();
            let amount = totalAmount * e.value / 100;
            amount = Math.floor(amount * 100) / 100;
            let oldAmount = e.entity.getAmount();
            if (oldAmount !== amount) {
                me.amountUpdate = false;
                e.entity.setAmount(amount);
                me.amountUpdate = true;
            }
            let store = me.getData();
            me.PayBehavior.setPaymentStore(store);
        }
        if (e.property === 'Amount' && me.amountUpdate) {
            let totalAmount = me._parent.getCurrent().getTotalAmount();
            let percent = e.value / totalAmount * 100;
            percent = Math.floor(percent * 100) / 100;
            let oldPercent = e.entity.getPercent();
            if (oldPercent !== percent) {
                me.percentUpdate = false;
                e.entity.setPercent(percent);
                me.percentUpdate = true;
            }
            let store = me.getData();
            me.PayBehavior.setPaymentStore(store);
        }
    }
});