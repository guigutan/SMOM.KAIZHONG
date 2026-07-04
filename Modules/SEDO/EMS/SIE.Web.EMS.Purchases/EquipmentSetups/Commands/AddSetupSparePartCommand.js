SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddSetupSparePartCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        entity.setEquipmentSetupId(this.view._parent.getCurrent().getId());
    }
});