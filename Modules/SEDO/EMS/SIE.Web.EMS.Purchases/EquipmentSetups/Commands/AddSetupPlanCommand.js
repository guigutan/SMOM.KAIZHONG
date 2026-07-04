SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddSetupPlanCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        entity.setPlanStartDateTime(new Date());
        entity.setPlanEndDateTime(new Date());
        entity.setOutSource(entity._EquipmentSetup.getOutSource());
    }
});