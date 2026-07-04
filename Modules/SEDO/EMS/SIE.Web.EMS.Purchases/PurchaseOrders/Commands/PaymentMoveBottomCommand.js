SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.PaymentMoveBottomCommand', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveBottomCommand',
    meta: { text: "置底", splitTo: "下移", group: "business", iconCls: "icon-AlignBottom icon-blue" },
    execute: function (listView, source) {
        var me = this;
        var data = listView.getData();
        var check = true;
        SIE.each(data.data.items, function (model) {
            if (model.phantom === true) {
                check = false;
                return false;
            }
        });
        if (!check) {
            SIE.Msg.showError("数据未保存无法移动".t());
            return;
        }
        var items = data.data.items;
        var entity = listView.getCurrent();
        var index = this.getItemsIndex(items, entity);
        if (!entity.isNew()) {
            items[index].data.INDEX_ = items[items.length - 1].data.INDEX_;
            var EntityMetadataList = [];
            //重新排序
            for (var i = index + 1; i < items.length; i++) {
                items[i].data.INDEX_ = items[i].data.INDEX_ - 1;
                EntityMetadataList.push(items[i].data);
            }
            EntityMetadataList.push(items[index].data);
        }
        items.splice(index, 1);
        items.push(entity);
        this.setItems(data, items);

        listView.setCurrent(null);
        listView.setCurrent(entity);

        if (!entity.isNew()) {
            var indata = {};
            indata.u = EntityMetadataList;
            listView.execute({
                data: indata,
                success: function (res) {
                    me.setPaymentStore(data);
                    return true;
                }
            });
        } else {
            me.setPaymentStore(data);
        }
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
    }
});