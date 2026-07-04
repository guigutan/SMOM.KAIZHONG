SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddWorkHourCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        var parent = view._parent.getCurrent();
        if (parent == null) {
            return false;
        }
        if (parent.data.ApprovalStatus !== 40) {
            return false;
        }
        if (parent.data.SetupStatus == 30 || parent.data.SetupStatus == 40) {
            return false;
        }
        return view.canAddItem();
    },
    onItemCreated: function (entity) {
        entity.setStartDateTime(null);
        entity.setEndDateTime(null);
        entity.setHours(null);
        entity.setApprovalStatus(entity._EquipmentSetup.getApprovalStatus());
        entity.setSetupStatus(entity._EquipmentSetup.getSetupStatus());
    }
});