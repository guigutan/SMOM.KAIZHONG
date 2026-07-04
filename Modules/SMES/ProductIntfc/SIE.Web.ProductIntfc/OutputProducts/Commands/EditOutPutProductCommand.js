SIE.defineCommand('SIE.Web.ProductIntfc.OutputProducts.Commands.EditOutPutProductCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
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