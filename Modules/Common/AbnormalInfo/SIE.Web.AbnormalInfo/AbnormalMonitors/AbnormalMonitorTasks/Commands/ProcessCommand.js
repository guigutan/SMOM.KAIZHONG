SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.ProcessCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "异常处理", group: "edit", iconCls: "icon-Runtime icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式
    isTabExist: false,

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (p.getTaskState() == SIE.AbnormalInfo.Common.TaskStateEnum.ToDo || p.getTaskState() == SIE.AbnormalInfo.Common.TaskStateEnum.Doing) { return true; }
        return false;
    },

    edit: function (entity) {
        this.editInForm(entity);
    },

    showView: function (editEntity) {

        var me = this;
        var entityId = editEntity.entityName + '_' + "DetailView" + '_' + editEntity.getId();
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            recordId: editEntity.data.Id,
            title: me.getEditViewTitle(editEntity),
            entityType: me.view.model,
            viewGroup: "ProcessView",
            pageClass: 'SIE.Web.AbnormalInfo.AnomalyMonitors.AbnormalMonitorTasks.WritingAbnormalProcessViewPage',
            isDetail: true,
        });

    }

});