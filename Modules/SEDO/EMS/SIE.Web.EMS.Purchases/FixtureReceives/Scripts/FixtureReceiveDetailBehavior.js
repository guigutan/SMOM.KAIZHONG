Ext.define('SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDetailBehavior', {
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
        if (e.property === 'PurchaseOrderItemId') {
            if (e.entity.getReceiveType() === 10) {
                if (e.value !== null) {
                    SIE.invokeDataQuery({
                        method: 'GetFixtureInfo',
                        params: [e.value],
                        action: 'queryer',
                        type: 'SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer',
                        token: me.token,
                        success: function (res) {
                            if (res.Result != null) {
                                var info = res.Result;
                                e.entity.setFixtureEncodeId_Display(info.Code);
                                e.entity.setFixtureEncodeId(info.Id);
                                e.entity.setModelCode(info.ModelCode);
                                e.entity.setModelName(info.ModelName);
                                e.entity.setManageMode(info.ManageMode);
                                e.entity.setUnitId_Display(info.UnitName);
                                e.entity.setUnitId(info.UnitId);
                                if (info.Code != "")//能带出工治具编码 同时又是采购类型则设置FixtureEncodeId为禁止编辑
                                {
                                    e.entity.setFixtureEncodeIdReadOnly(true);
                                } else
                                    e.entity.setFixtureEncodeIdReadOnly(false);
                            }
                        }
                    });
                } else {
                    e.entity.setFixtureEncodeIdReadOnly(false);
                }
            }
        }
        if ((e.property === 'Price' || e.property === 'TaxRate' || e.property === 'PriceNoTax') && e.entity.getReceiveType() === 20) {
            e.entity.setPrice("");
            e.entity.setTaxRate("");
            e.entity.setPriceNoTax("");
        }
    }
});