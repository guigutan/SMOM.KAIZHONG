SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.DeleteWorkHourCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) {
            return false;
        }
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
        return true;
    }
});