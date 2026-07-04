SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignViewCommand", {
    meta: { text: "查看", group: "edit", iconCls: "icon-Magnify icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length != 1) {
            return false;
        }
        if (sel[0].isDirty() || sel[0].getExamineStatus() == 1) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var entity = view.getCurrent();
        CRT.Workbench.addPage({
            entityType: "SIE.MES.ProjectDesigns.ProjectDesignDetail",
            title: "查看-项目号需求设计".t(),
            viewGroup: "LookUpViewGroup",
            module: view.module,
            recordId: entity.getId(),
            isDetail: true,
            ignoreQuery: true,
        })
    }
})