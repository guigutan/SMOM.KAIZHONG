SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.DeletePrepareCommand", {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getPrepareStatus() != 0) {
            return false;
        }
        return true;
    }
})