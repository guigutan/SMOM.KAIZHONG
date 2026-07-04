SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.DeletePurOrderDetailCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    execute: function (view) {
        var msg = Ext.String.format('你确定删除选择的{0}条数据吗？删除后，需要再次点击保存！'.t(), view.getSelection().length);
        SIE.Msg.askQuestion(msg, function () {
            view.removeSelection();
            var data = view.getData().data;
            if (data.length > 0) {
                view.getControl().setSelection(data.items[0]);
                view.setCurrent(data.items[0], true);
            } else {
                view.setCurrent(null, true);
            }
            let totalQty = 0;
            let count = 0;
            data.items.forEach(function (p) { totalQty += p.data.Amount; count++; });
            let order = view._parent.getCurrent();
            order.setVarietyQuantity(count);
            order.setTotalAmount(totalQty);
        });
    }
});