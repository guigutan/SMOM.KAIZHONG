SIE.defineCommand('SIE.Web.MES.TeamManagement.SikllAuthentications.Commands.TrainingRecordEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        var entity = view.getCurrent();
        if (entity.getIsHistory() == true)
            return false;
        return entity != null;
    },
});