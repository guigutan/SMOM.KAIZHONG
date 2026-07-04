SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands.TreeRoutingEditCommand", {
    extend: "SIE.cmd.Edit",
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var cur = view.getCurrent();
        if (cur != null && !cur.getHasUp()) return true;
        return false;
    }
})