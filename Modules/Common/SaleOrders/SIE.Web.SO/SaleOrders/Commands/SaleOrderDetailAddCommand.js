SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.SaleOrderDetailAddCommand', {
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
                    entity.setQty(data.Qty);
                    entity.setLineState(data.LineState);
                    entity.setMiDateTime(data.MiDateTime);
                    entity.setMaterialPnl(data.MaterialPnl);
                    entity.setSetPnl(data.SetPnl);
                    entity.setPcsPnl(data.PcsPnl);
                    entity.setRequireDelivery(data.RequireDelivery);
                    entity.setIsHangUp(data.IsHangUp);
                    //entity.setUnitId(data.UnitId);
                    //entity.setUnit(data.Unit);
                    entity.setUnitId_Display(data.ExtValues["UnitId_Display"]);
                    entity.setUnitId(data.UnitId)
                    //entity.setProcessTechRouteId_Display(routed.data.Name);
                    //entity.setProcessTechRouteId(routed.data.Id);
                    entity.setArea(data.Area)
                    entity.setSingleArea(data.SingleArea)
                }
            }, me.view);
        }
    }
});