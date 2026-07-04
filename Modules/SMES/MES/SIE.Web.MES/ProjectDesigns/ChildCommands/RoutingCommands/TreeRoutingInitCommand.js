SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands.TreeRoutingInitCommand", {
    meta: { text: "引用工艺路线设置", group: "edit", iconCls: "icon-SectionExpandAll icon-blue" },
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
        SIE.Msg.askQuestion("引用标准Bom会覆盖当前数据产品工艺路线明细，清空工序Bom明细、参数明细，是否继续？".t(), function () {
            me.initRouting(view);
        });
        
    },
    initRouting: function (view) {
        var data = {}
        SIE.Msg.wait("正在引用工艺路线设置...".t());
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
})