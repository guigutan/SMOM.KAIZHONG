SIE.defineCommand('SIE.Web.Kit.Recheck.RecheckInspBills.Commands.ReelIDs.EditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (!parent || parent.getInspectionStatus() === SIE.Enum.QMS.Common.InspectionStatus.Inspectioned)
            return false;
        return this.callParent(arguments);
    }
});