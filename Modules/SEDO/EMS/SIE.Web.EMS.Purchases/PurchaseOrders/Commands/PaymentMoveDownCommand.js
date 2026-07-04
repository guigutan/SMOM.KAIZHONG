SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.PaymentMoveDownCommand', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveDownCommand',
    meta: { text: "下移", group: "business", iconCls: "icon-ArrowLongDown icon-blue" },
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
        //交换序列位置
        var dataindex = 0;
        dataindex = items[index].data.INDEX_;
        items[index].data.INDEX_ = items[index + 1].data.INDEX_;
        items[index + 1].data.INDEX_ = dataindex;

        items.splice(index, 1);
        items.splice(index + 1, 0, entity);
        this.setItems(data, items);

        //当前选中
        listView.setCurrent(null);
        listView.setCurrent(entity);

        if (!entity.isNew()) {
            var EntityMetadataList = [];
            EntityMetadataList.push(items[index].data);
            EntityMetadataList.push(items[index + 1].data);
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