SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.DeleteTaskCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {

        if (view.hasSelectedEntities() && view.getSelection().length > 0) {
            var flag = view.getSelection().all(function (c) { return c.getTaskState() == SIE.AbnormalInfo.Common.TaskStateEnum.ToDo && c.getTaskType() == SIE.AbnormalInfo.Common.TaskType.Manual });
            return flag;
        }
        return false;
    },
});