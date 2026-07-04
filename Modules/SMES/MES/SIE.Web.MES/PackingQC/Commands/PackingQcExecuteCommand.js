SIE.defineCommand("SIE.Web.MES.PackingQC.Commands.PackingQcExecuteCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "执行", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getCurrent() === null)
        {
            return false;
        }
        if (view.getSelection().length != 1)
        {
            return false;
        }
        if (view.getCurrent() && view.getCurrent().getData().Confirm == 0)
        {
            return false;
        }
        return true;
    },
    showView: function(entity) {
        if (entity)
        {
            var model = entity.data;
            var me = this;
            CRT.Workbench.addPage({
                entityType: me.view.model,
                title: "执行".t(),
                recordId: entity.getId(),
                viewGroup: "ExecuteViewStr",
                params: {
                    WorkOrderId: entity.getId(),
                },
                isDetail: true,
            });
        }
    },
});
