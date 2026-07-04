SIE.defineCommand('SIE.Web.ProductIntfc.OutputProducts.Commands.AddOutPutProductCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-blue" },
    execute: function (view, source) {
        var me = this;
        var editEntity = me.getEditEntity();
        me.onEditting(editEntity);
        me.edit(editEntity);
        me.onEdited(editEntity);
    },

    onItemCreated: function (entity) {
        var me = this;
        var view = this.view;
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
        view.fireEvent("newentityadded", entity);
        me.callParent(arguments);

    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            var me = this;
            var entity = e.entity;

            if (e.property === "ItemId") {
                SIE.invokeDataQuery({
                    type: "SIE.Web.ProductIntfc.OutputProducts.DataQuery.OutputProductsDataQueryer",
                    method: "GetBillWh",
                    params: [entity.data.OutPutType],
                    token: me.view.getToken(),
                    async: false,
                    callback: function callback(res) {
                        if (res.Success && res.Result != null && res.Result.data && res.Result.data.items.length > 0) {
                            entity.setWarehouseId_Display(res.Result.data.items[0].data.Code);
                            entity.setWarehouseId(res.Result.data.items[0].data.Id);
                        }
                    }
                });
            }
        }
    }
});