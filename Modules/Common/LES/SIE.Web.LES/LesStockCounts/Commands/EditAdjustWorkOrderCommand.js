SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.EditAdjustWorkOrderCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        return !p.data.IsAuto;
    },
});