SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.BomCommands.TreeBomUpdateCommand", {
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
        me.updateBom(view, design);
    },
    updateBom: function (view, design) {
        var data = design.getProjectMaintainId();
        SIE.Msg.wait("正在更新版本...".t());
        view.execute({
            data: data,
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) {
                if (res.Success) {
                    view.reloadData();
                    SIE.Msg.close();
                }
            }
        });
    }
})