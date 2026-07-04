SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageLookUpCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看", group: "edit", iconCls: "icon-Magnify icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() === null) { return false; }
        if (view.getSelection().length != 1) { return false; }
        return true;
    },
    showView: function (editEntity) {
        var me = this;
        var entityId = editEntity.entityName + '-' + "LookUpView" + '-' + editEntity.getId();
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            recordId: editEntity.data.Id,
            ignoreQuery: true,
            title: me.getEditViewTitle(editEntity),
            entityType: this.view.model,
            viewGroup: "LookUpViewGroup",
            isDetail: true
        });
    }
});