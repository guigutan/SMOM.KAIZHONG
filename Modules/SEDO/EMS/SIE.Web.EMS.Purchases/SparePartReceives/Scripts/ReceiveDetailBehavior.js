Ext.define('SIE.Web.EMS.Purchases.SparePartReceives.ReceiveDetailBehavior', {
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

        //ReceiveType 接收类型； 10 采购接收; 20 赠品接收;
        if (e.property === 'PurchaseOrderItemId' && e.value !== null
            && (e.entity.getReceiveType() === 10 || e.entity.getReceiveType() === 20)) {
            SIE.invokeDataQuery({
                method: 'GetSparePartInfo',
                params: [e.value],
                action: 'queryer',
                type: 'SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer',
                token: me.token,
                success: function (res) {
                    if (res.Result != null) {
                        var info = res.Result.data.items[0].data;
                        e.entity.setSparePartId_Display(info.SparePartCode);
                        e.entity.setSparePartId(info.Id);
                        e.entity.setSparePartName(info.SparePartName);
                        e.entity.setControlMethod(info.ControlMethod);
                        e.entity.setUnitName(info.UnitName);
                        e.entity.setExemptionInspect(info.ExemptionInspect);
                    }
                }
            });
        }
        if (e.property === 'Price' || e.property === 'TaxRate') {
            let price = e.entity.getPrice();
            let taxRate = e.entity.getTaxRate();
            let priceNoTax = price * (1 - (taxRate / 100)).toFixed(2);
            e.entity.setPriceNoTax(priceNoTax);
        }
    }
});