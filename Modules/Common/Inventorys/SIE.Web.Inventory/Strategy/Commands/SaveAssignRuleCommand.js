SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.SaveAssignRuleCommand', {
    meta: { text: "保存", group: "edit", iconCls: "iconfont icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.Save',
    canExecute: function (view) {
        return view.getData().isDirty();
    }
});