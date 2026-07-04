SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.ViewTaskCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看", group: "edit", iconCls: "icon-FileEye icon-blue" },

    //edit: function (entity) {
    //    this.editInForm(entity);
    //},

    showView: function (editEntity) {

        var me = this;
        var entityId = editEntity.entityName + '_' + "ReadonlyView" + '_' + editEntity.getId();
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            recordId: editEntity.data.Id,
            title: me.getEditViewTitle(editEntity),
            entityType: me.view.model,
            viewGroup: "ReadonlyView",
            pageClass: 'SIE.Web.AbnormalInfo.AnomalyMonitors.AbnormalMonitorTasks.WritingAbnormalProcessViewPage',
            isDetail: true,
        });

    },
});