SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignDeleteCommand", {
    extend: "SIE.cmd.Delete",
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getExamineStatus() == 1 || (entity.getBaseInfo() == 1 || entity.getRoutingInfo() == 1 || entity.getBomInfo() == 1 || entity.getAttachInfo() == 1)) {
            return false;
        }
        return true;
    }
})