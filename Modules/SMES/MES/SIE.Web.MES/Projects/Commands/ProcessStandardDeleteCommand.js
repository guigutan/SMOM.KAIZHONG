SIE.defineCommand("SIE.Web.MES.Projects.Commands.ProcessStandardDeleteCommand", {
    extend: "SIE.cmd.Delete",
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var sel = view.getSelection();
        var flag = true;
        if (sel == null || sel.length <= 0) {
            return false;
        }
        Ext.each(sel, function (item) {
            if (item.getProcessStStatus() == 2) {
                flag = false;
                return;
            }
        });
        return flag;
    }
})  