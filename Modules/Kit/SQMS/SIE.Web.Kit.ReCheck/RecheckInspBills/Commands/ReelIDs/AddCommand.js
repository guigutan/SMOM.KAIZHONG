SIE.defineCommand('SIE.Web.Kit.Recheck.RecheckInspBills.Commands.ReelIDs.AddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },

    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (!parent || parent.getInspectionStatus() === SIE.Enum.QMS.Common.InspectionStatus.Inspectioned)
            return false;
        return this.callParent(arguments);
    }
});