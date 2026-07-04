SIE.defineCommand('SIE.Web.MES.TeamManagement.SikllAuthentications.Commands.OperationRecordDelCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

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