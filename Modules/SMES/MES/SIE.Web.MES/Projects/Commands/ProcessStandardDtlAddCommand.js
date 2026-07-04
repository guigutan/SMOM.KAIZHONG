SIE.defineCommand("SIE.Web.MES.Projects.Commands.ProcessStandardDtlAddCommand", {
    extend: "SIE.cmd.Add",
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (parent != null && parent.getProcessStStatus() != 2) return true;
        return false;
    }
})