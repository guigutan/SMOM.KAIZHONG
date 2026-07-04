Ext.define('SIE.Web.EMS.Purchases.PurchaseOrders.PurOrderItemBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'Qty' || e.property === 'Price') {
            let qty = e.entity.getQty();
            let price = e.entity.getPrice();
            if (qty && price)
                e.entity.setAmount(qty * price);
            let totalQty = 0;
            var count = [];
            me.getData().data.items.forEach(function (p) {
                totalQty += p.data.Amount;
                let str = p.data.ObjectCode + p.data.Description;
                if (count.indexOf(str) === -1 && str.length > 0) {
                    count.push(str);
                }
            });
            let order = me._parent.getCurrent();
            order.setVarietyQuantity(count.length);
            order.setTotalAmount(totalQty);
        }
        if (e.property === 'Price' || e.property === 'TaxRate') {
            let price = e.entity.getPrice();
            let taxRate = e.entity.getTaxRate();
            let priceNoTax = price * (1 - (taxRate / 100)).toFixed(2);
            e.entity.setPriceNoTax(priceNoTax);
        }
    }
});