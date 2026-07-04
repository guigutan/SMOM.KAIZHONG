SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands.TreeRoutingUpDateCommand", {
    meta: { text: "更新版本", group: "edit", iconCls: "icon-Upgrade icon-blue" },
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current == null || current.isDirty()) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var me = this;
        var design = view.getParent().getCurrent();
        var curent = view.getCurrent();
        var data = {
            Id: curent.getId(),
            DesignDetailId: design.getId(),
            ProjectMaintainId: design.getProjectMaintainId(),
        }
        SIE.Msg.wait("正在更新版本...".t());
        view.execute({
            data: data,
            success: function (res) {
                if (res.Success) {
                    view.reloadData();
                    SIE.Msg.close();
                }
            }
        });
    }
})