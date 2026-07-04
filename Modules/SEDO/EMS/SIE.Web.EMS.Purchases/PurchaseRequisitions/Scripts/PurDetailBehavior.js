Ext.define('SIE.Web.EMS.Purchases.PurchaseRequisitions.PurDetailBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        SIE.each(store, function (entity) {
            entity.setProjectId(entity._PurchaseRequisition.getProjectId());
            entity.setPurchaseType(entity._PurchaseRequisition.getPurchaseType());
            entity.setPurchaseObjectType(entity._PurchaseRequisition.getPurchaseObjectType());
            entity.setObjectCodeInfoId(entity.getObjectCode());
            entity.setObjectCodeInfoId_Display(entity.getObjectCode());
        });
        view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
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