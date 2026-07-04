Ext.define('SIE.Web.EMS.Purchases.EquipmentAcceptances.EquipAcceptDetailBehavior', {
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
        if (e.property === 'AcceptanceStatus') {
            let passQty = 0;
            var unqualifiedQty = 0;
            me.getData().data.items.forEach(function (p) {
                if (p.data.AcceptanceStatus === 1) {
                    passQty++;
                }
                if (p.data.AcceptanceStatus === 2) {
                    unqualifiedQty++;
                }
            });
            let order = me._parent.getCurrent();
            order.setPassQty(passQty);
            order.setUnqualifiedQty(unqualifiedQty);
        }
    }
});