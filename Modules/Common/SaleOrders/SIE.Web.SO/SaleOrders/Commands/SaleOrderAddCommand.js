SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.SaleOrderAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                success: function (res) {
                    var data = res.Result;
                    entity.setCode(data.Code);
                    entity.setTotalQty(data.TotalQty);
                    entity.setOrderRowsQty(data.OrderRowsQty);
                    entity.setRegisterDateTime(data.RegisterDateTime)
                    entity.setOrderSourceType(data.OrderSourceType)
                }
            }, me.view);
            //this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    }
    /*
    ,
    _onEntityPropertyChanged: function (e) {
        if (e.property.length > 0) {
            var entity = e.entity;
            var preData = entity.previousValues;
            var data = entity.data;
            var token = this.view.token;
            if (e.property.indexOf('ItemId') >= 0) {
                if (data != preData) {
                    SIE.invokeDataQuery({
                        type: "SIE.Web.APS.ProductionOrders.DataQuery.ProductOrderDataQueryer",
                        method: "GetProcessTechRoute",
                        params: [data.ItemId],
                        async: false,
                        token: token,
                        callback: function (res) {
                            if (res.Success && res.Result != null) {
                                var routed = res.Result.data.items[0];
                                if (routed != null) {
                                    entity.setProcessTechRouteId_Display(routed.data.Name);
                                    entity.setProcessTechRouteId(routed.data.Id);
                                }
                            } else {
                                entity.setProcessTechRouteId_Display(null);
                                entity.setProcessTechRouteId(null);
                            }
                        },
                    });
                }
            }
        }
    }
    */
});