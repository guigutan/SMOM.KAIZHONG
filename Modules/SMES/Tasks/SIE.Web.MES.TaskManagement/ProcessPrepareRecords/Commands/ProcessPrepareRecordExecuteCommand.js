SIE.defineCommand("SIE.Web.MES.TaskManagement.ProcessPrepareRecords.Commands.ProcessPrepareRecordExecuteCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "执行", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getCurrent() === null) {
            return false;
        }
        if (view.getSelection().length != 1) {
            return false;
        }
        if (view.getCurrent() && view.getCurrent().getData().PrepareState != 0) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        if (view && view.getCurrent()) {
            var entity = view.getCurrent();
            var me = this;
            CRT.Workbench.addPage({
                entityType: view.model,
                title: "执行".t(),
                recordId: entity.getId(),
                viewGroup: "ExecuteViewStr",
                params: {
                    WorkOrderId: entity.getWorkOrderId(),
                    Id: entity.getId()
                },
                isDetail: true,
            });
        }
    },
});
