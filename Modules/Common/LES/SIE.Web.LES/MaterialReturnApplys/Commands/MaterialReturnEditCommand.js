SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.MaterialReturnEditCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getReStatus() != 0) {
            return false;
        }
        return true;
    },
})