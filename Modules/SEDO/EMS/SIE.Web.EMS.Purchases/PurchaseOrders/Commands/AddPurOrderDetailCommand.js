SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.AddPurOrderDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var lineNo = me.view.getData().count();
        var poData = me.view.getData().getData();
        if (lineNo > 1) {
            var tempLineNoList = poData.items.where(function (p) { return p.getLineNo() != null; })
                .select(function (p) { return parseInt(p.getLineNo()); });
            lineNo = tempLineNoList.max() + 1;
        }
        entity.setLineNo(lineNo);
        entity.setStatus(10);
        entity.setFactoryId(entity._PurchaseOrder.getFactoryId());
        entity.setDepartmentId(entity._PurchaseOrder.getDepartmentId());
        entity.setPurchaseObjectType(entity._PurchaseOrder.getPurchaseObjectType());
        entity.setDeliveryDate(new Date());
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
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