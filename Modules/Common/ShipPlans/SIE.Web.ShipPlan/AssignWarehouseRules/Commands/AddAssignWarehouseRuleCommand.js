SIE.defineCommand('SIE.Web.ShipPlan.Commands.AddAssignWarehouseRuleCommand', {
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
                    entity.setItemType(data.ItemType);
                    entity.setPriority(data.Priority);
                }
            }, me.view);
        }
    },
});