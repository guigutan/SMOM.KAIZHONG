Ext.define('SIE.Web.Items.Items.Behaviors.BaseItemBehavior', {
    /**
     * 数据加载后
     * @param {*} view 
     */
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getData();
        view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = e.entity;
        var data = e.entity.data;
        if (e.property.length > 0) {
            if (e.property == 'IsLabel' && e.value == false) {
                var itemStock = this._parent._children.first(function (f) { return f.model == 'SIE.WMS.Common.ItemStockData' })
                if (itemStock && itemStock.getData() && itemStock.getData().getIsSerialNumber()) {
                    SIE.Msg.showMessage('物料是序列号管理此项不能取消勾选！'.t());
                    entity.setIsLabel(true);
                    var check = this.getControl().items.items.first(function (f) { return f.name = 'IsLabel' });
                    check.setValue(true);
                }
                else {
                    //没有激活库存资料
                    SIE.invokeDataQuery({
                        async: false,
                        type: "SIE.Web.Warehouses.ItemStockDatas.ItemStockDataQueryer",
                        method: 'CheckIsSer',
                        token: me.token,
                        params: [data.Id],
                        success: function (res) {
                            if (res.Result) {
                                SIE.Msg.showMessage('物料是序列号管理此项不能取消勾选！'.t());
                                entity.setIsLabel(true);
                                var check = me.getControl().items.items.first(function (f) { return f.name = 'IsLabel' });
                                check.setValue(true);
                            }
                        }
                    })
                }
            }
        }
    },
});