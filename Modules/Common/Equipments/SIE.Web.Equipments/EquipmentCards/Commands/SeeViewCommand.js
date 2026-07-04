SIE.defineCommand('SIE.Web.Equipments.EquipmentCards.Commands.SeeViewCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看", group: "edit", iconCls: "icon-FileEye icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() === null) { return false; }
        if (view.getSelection().length !== 1) { return false; }
        return true;
    },
    showView: function (editEntity) {
        var me = this;
        var entityId = editEntity.entityName + '-' + "SeeView" + '-' + editEntity.getId();
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            recordId: editEntity.data.Id,
            ignoreQuery: true,
            title: me.getEditViewTitle(editEntity),
            entityType: this.view.model,
            viewGroup: "SeeView",
            isDetail: true
        });
    }
});
