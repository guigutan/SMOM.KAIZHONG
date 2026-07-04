SIE.defineCommand('SIE.Web.LES.StockPlans.Commands.SaveStockPlanCommand', {
    meta: { text: "保存", group: "edit", iconCls: "iconfont icon-SaveEntity icon-blue" },
    extend: 'SIE.Web.ShipPlan.Commands.SaveDeliveryPlanCommand',
    canExecute: function (view) {
        return view.getData().isDirty();
        //this.callParent(arguments);
    }
});