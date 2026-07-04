SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands.TreeRoutingBomInitCommand", {
    meta: { text: "工艺BOM生成", group: "edit", iconCls: "icon-SectionExpandAll icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel.length <= 0) {
            return false;
        }
        var flag = true;
        sel.forEach(i => {
            if (i.dirty) {
                flag = false;
                return;
            }
        });
        return flag;
    },
    execute: function (view) {

        var me = this;
        var me = this;
        SIE.Msg.askQuestion("工艺BOM生成会覆盖当前数据，是否继续？".t(), function () {
            me.initProcessBom(view);
        });
    },
    initProcessBom: function (view) {
        var data = {}
        SIE.Msg.wait("正在生成工艺BOM...".t());
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
        })
    }
});