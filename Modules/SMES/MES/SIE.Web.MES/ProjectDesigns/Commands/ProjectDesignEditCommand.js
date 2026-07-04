SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignEditCommand", {
    extend: "SIE.cmd.Edit",
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getExamineStatus() == 1) {
            return false;
        }
        return true;
    }
})