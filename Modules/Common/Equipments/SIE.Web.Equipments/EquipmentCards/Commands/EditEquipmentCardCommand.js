SIE.defineCommand('SIE.Web.Equipments.EquipmentCards.Commands.EditEquipmentCardCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    /**
      * 显示界面绑定属性变更事件并设置默认数据
      * @param editEntity 当前实体      
      */
    canExecute: function (view) {
        var entity = view.getCurrent();
        var res = false;
        if (entity != null && entity.data) {
            if ((entity.data.ApprovalStatus === SIE.Equipments.Enums.ApprovalStatus.Draft.value
                || entity.data.ApprovalStatus === SIE.Equipments.Enums.ApprovalStatus.Reject.value
                || entity.data.ApprovalStatus === SIE.Equipments.Enums.ApprovalStatus.Audited.value)
                && view != null && view.getSelection().length == 1) {
                res = true;
            }
        }
        return res;
    },
    showView: function (entity) {
        if (entity) {
            var me = this;
            CRT.Workbench.addPage({
                entityType: me.view.model,
                title: me.getEditViewTitle(entity),
                recordId: entity.getId(),
                viewGroup: "EditView",
                isDetail: true
            });
        }
    }
});
