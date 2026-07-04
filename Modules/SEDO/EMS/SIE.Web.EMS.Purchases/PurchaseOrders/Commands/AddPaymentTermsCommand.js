SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.AddPaymentTermsCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        entity.setCurrency(entity._PurchaseOrder.getCurrency());
        entity.setAmountUnit(entity._PurchaseOrder.getAmountUnit());
        var store = me.view.getData();
        me.setPaymentStore(store);
        me.view.PayBehavior = me;
        me.view.amountUpdate = true;
        me.view.percentUpdate = true;
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    setPaymentStore: function (store) {
        var amount = 0;
        var percent = 0;
        SIE.each(store, function (entity) {
            amount = amount + entity.getAmount();
            percent = percent + entity.getPercent();
            amount = Math.floor(amount * 100) / 100;
            percent = Math.floor(percent * 100) / 100;
            entity.setCumulativeAmount(amount);
            entity.setCumulativePercent(percent);
        });
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