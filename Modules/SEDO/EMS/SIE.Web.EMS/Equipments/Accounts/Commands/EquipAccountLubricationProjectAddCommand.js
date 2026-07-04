SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.EquipAccountLubricationProjectAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var me = this;
            var EquipAccountId = me.view.getParent()._current.getId()
            entity.setEquipAccountId(EquipAccountId);
        }
    },
});
