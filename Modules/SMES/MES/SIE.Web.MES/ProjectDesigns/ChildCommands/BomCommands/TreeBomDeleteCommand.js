SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.BomCommands.TreeBomDeleteCommand", {
    extend: "SIE.cmd.Delete",
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var cur = view.getCurrent();
        if (cur != null && !cur.getHasUp()) return true;
        return false;
    }
})