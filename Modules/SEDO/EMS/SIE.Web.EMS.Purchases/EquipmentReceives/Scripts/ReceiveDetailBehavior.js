Ext.define('SIE.Web.EMS.Purchases.EquipmentReceives.ReceiveDetailBehavior', {
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
        if (e.property === 'PurchaseOrderItemId' && e.value !== null && e.entity.getReceiveType() === 10) {
            SIE.invokeDataQuery({
                method: 'GetEquipModelInfo',
                params: [e.value],
                action: 'queryer',
                type: 'SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer',
                token: me.token,
                success: function (res) {
                    if (res.Result != null) {
                        var info = res.Result.data.items[0].data;
                        e.entity.setEquipModelId_Display(info.Code);
                        e.entity.setEquipModelId(info.Id);
                        e.entity.setEquipModelName(info.Name);
                        e.entity.setOrderEquipModelId(info.Id);
                    }
                }
            });
        }
        if (e.property === 'EquipModelId') {
            let old = e.entity.getOrderEquipModelId();
            if (old !== null && e.value !== null && old !== e.value) {
                e.entity.setGiveaway(true);
            }
        }
        if (e.property === 'Price' && e.value > 0) {
            if (e.entity.getGiveaway() === true)
                e.entity.setPrice(0);
        }
        if (e.property === 'Giveaway' && e.value === true) {
            e.entity.setPrice(0);
            e.entity.setWarehouseId(null);
        }
    }
});