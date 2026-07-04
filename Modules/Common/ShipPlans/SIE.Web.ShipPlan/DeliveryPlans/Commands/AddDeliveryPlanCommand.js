SIE.defineCommand('SIE.Web.ShipPlan.Commands.AddDeliveryPlanCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setOrderType(data.OrderType);
                    entity.setState(data.State);
                    entity.setSourceType(data.SourceType);
                    entity.setDeliveryDate(data.DeliveryDate);
                    entity.setNoCreateQty(data.NoCreateQty);
                    entity.setDeliveryQty(data.DeliveryQty);
                    entity.setCancelQty(data.CancelQty);
                }
            }, me.view);
        }
    },
});