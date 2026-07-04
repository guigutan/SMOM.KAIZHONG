SIE.defineCommand("SIE.Web.MES.Projects.Commands.ProcessStandardEditCommand", {
    extend: "SIE.cmd.Edit",
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel && sel.length > 1) {
            return false;
        }
        var cur = view.getCurrent();
        if (cur != null && cur.getProcessStStatus() != 2) {
            return true;
        }
        return false;
    }
})  