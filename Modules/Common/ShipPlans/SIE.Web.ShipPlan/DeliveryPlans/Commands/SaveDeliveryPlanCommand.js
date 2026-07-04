SIE.defineCommand('SIE.Web.ShipPlan.Commands.SaveDeliveryPlanCommand', {
    meta: { text: "保存", group: "edit", iconCls: "iconfont icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.Save',
    canExecute: function (view) {
        return view.getData().isDirty();
    }
});