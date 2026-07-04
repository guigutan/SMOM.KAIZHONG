SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.EditTaskCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (p.getTaskState() !== SIE.AbnormalInfo.Common.TaskStateEnum.ToDo) { return false; }
        if (p.getTaskType() !== SIE.AbnormalInfo.Common.TaskType.Manual) { return false; }
        return true;
    },

    execute: function (view, source) {
        var me = this;
        var indata = {};
        var current = view.getCurrent();

        CRT.Workbench.addPage({
            entityType: me.view.model,
            recordId: current.data.Id,
            title: me.getEditViewTitle(current),
            isDetail: true,
            params: {
                IsAdd: false,
            }
        });

    },
});