SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.AddPurDetailCommand', {
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
        entity.setProjectId(entity._PurchaseRequisition.getProjectId());
        entity.setPurchaseType(entity._PurchaseRequisition.getPurchaseType());
        entity.setPurchaseObjectType(entity._PurchaseRequisition.getPurchaseObjectType());
        entity.setDemandDate(new Date());
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'Qty' || e.property === 'PurchaseRequisitionId' || e.property === 'ObjectCode' || e.property === 'Description') {
            var totalQty = 0;
            var count = [];
            me.getData().data.items.forEach(function (p) {
                totalQty += p.data.Qty;
                let str = p.data.ObjectCode + p.data.Description;
                if (count.indexOf(str) === -1 && str.length > 0) {
                    count.push(str);
                }
            });
            var pur = me._parent.getCurrent();
            pur.setTotalAmount(totalQty);
            pur.setVarietyQuantity(count.length);
        }
        if (e.property === 'Qty' || e.property === 'Price') {
            var price = e.entity.getPrice();
            var qty = e.entity.getQty();
            if (price != null && qty != null) {
                var num = Math.floor(price * qty * 100) / 100;
                e.entity.setTotalAmount(num);
            }
        }
    }
});