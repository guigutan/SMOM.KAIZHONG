SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.EditDemandCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        var res = true;
        if (entity == null) return false;
        if ((entity.data.ApprovalStatus != 10 && entity.data.ApprovalStatus != 50 )|| entity.data.Close == 1) {
            res = false;
            return res;
        }
        return res;
    },
    execute: function (view, source) {
        var me = this;
        var editEntity = this.getEditEntity();
        var entityId = editEntity.entityName + '-' + editEntity.data.Id;
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            entityType: me.view.model,
            recordId: editEntity.data.Id,
            title: me.getEditViewTitle(editEntity),
            isDetail: true,
            params: {
                tabId: tabId,
                isEdit: true
            }
        });
    }
});