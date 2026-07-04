SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.EditProjectCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.ApprovalStatus !== 10 && p.data.ApprovalStatus !== 50) return false;
        return true;
    }
});