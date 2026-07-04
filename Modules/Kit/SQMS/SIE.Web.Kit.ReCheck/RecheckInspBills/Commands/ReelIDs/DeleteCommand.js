SIE.defineCommand('SIE.Web.Kit.Recheck.RecheckInspBills.Commands.ReelIDs.DeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (!parent || parent.getInspectionStatus() === SIE.Enum.QMS.Common.InspectionStatus.Inspectioned)
            return false;
        return this.callParent(arguments);
    }
});