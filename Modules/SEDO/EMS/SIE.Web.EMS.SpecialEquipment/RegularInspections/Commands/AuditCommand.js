SIE.defineCommand('SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.AuditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "审核", group: "edit", iconCls: "icon-FileEye icon-blue" },
    /**
    * @override 命令可执行判断
    * @param {} view 逻辑视图
    * @returns {} 
    */
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current) {
            var state = current.getApprovalStatus();
            return state === SIE.Equipments.Enums.ApprovalStatus.PendingReview.value || state === SIE.Equipments.Enums.ApprovalStatus.UnderReview.value;
        }
        return this.callParent(arguments);
    },

    showView: function (editEntity) {
        var me = this;
        var entityId = editEntity.entityName + '-' + "AuditView" + '-' + editEntity.getId();
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            recordId: editEntity.data.Id,
            ignoreQuery: true,
            title: me.getEditViewTitle(editEntity),
            entityType: this.view.model,
            viewGroup: "AuditView",
            isDetail: true
        });
    }
});