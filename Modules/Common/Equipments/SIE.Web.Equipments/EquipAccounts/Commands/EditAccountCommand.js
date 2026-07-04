SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.EditAccountCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            CRT.Workbench.addPage({
                entityType: me.view.model,
                title: me.getEditViewTitle(entity),
                recordId: entity.getId(),
                viewGroup: "EditViewGroup",
                isDetail: true
            });
        }
    }
});