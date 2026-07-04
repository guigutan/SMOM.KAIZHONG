SIE.defineCommand("SIE.Web.MES.Projects.Commands.ProcessStandardDtlDeleteCommand", {
    extend: "SIE.cmd.Delete",
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (parent != null && parent.getProcessStStatus() != 2) return true;
        return false;
    }
})  