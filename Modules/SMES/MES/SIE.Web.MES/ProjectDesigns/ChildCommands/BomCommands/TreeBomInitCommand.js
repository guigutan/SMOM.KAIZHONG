SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.BomCommands.TreeBomInitCommand", {
    meta: { text: "引用标准Bom", group: "edit", iconCls: "icon-SectionExpandAll icon-blue" },
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
        SIE.Msg.askQuestion("引用标准Bom会覆盖当前数据明细，是否继续？".t(), function () {
            me.initBom(view);
        });
        
    },
    initBom: function (view) {
        var data = {};
        SIE.Msg.wait("正在引用标准Bom...".t());
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