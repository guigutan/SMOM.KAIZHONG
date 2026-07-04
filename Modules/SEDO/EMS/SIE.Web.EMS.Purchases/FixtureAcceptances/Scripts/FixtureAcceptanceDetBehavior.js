Ext.define('SIE.Web.EMS.Purchases.FixtureAcceptances.FixtureAcceptanceDetBehavior', {
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
        if (e.property === 'PassQty') {
            let receiveQty = e.entity.getReceiveQty();
            if (e.value > receiveQty) {
                SIE.Msg.showError("合格数量不能大于接收数量".t());
            }
            let unqualifiedQty = e.entity.getUnqualifiedQty();
            if (receiveQty != unqualifiedQty + e.value) {
                e.entity.setUnqualifiedQty(receiveQty - e.value);
            }
        }
        if (e.property === 'UnqualifiedQty') {
            let receiveQty = e.entity.getReceiveQty();
            if (e.value > receiveQty) {
                SIE.Msg.showError("不合格数量不能大于接收数量".t());
            }
            let passQtyQty = e.entity.getPassQty();
            if (receiveQty != passQtyQty + e.value) {
                e.entity.setPassQty(receiveQty - e.value);
            }
        }
    }
});