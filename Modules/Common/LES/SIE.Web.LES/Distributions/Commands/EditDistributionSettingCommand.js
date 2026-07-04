SIE.defineCommand('SIE.Web.LES.Distributions.Commands.EditDistributionSettingCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var p = view.getCurrent();
        if (view.getSelection() == null || view.getSelection().length != 1 || p == null || p.getState() == 1) {
            return false;
        }
        return true;
    },
});