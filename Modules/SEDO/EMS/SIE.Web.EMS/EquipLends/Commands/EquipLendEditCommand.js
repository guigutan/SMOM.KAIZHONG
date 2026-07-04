SIE.defineCommand("SIE.Web.EMS.EquipLends.Commands.EquipLendEditCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改".t(), group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function(view) {
        var current = view.getCurrent();
        if (current == null) {
            return false;
        }
        if (current.getLendState() != 0) {
            return false;
        }
        return true;
    },
})
