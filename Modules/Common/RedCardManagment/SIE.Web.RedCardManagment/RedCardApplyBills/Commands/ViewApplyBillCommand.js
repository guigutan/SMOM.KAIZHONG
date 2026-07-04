SIE.defineCommand('SIE.Web.RedCardManagment.RedCardApplyBills.Commands.ViewApplyBillCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看", group: "edit", iconCls: "icon-FileEye icon-blue" },


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
            isDetail: true,
        });

    },
});