SIE.defineCommand('SIE.Web.EMS.InventoryPlans.Commands.ImportSpareListCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (parent == null ) return false;
        if (parent.isDirty() || parent.getApprovalStatus() != 10) {
            return false;
        }
        return true;
    }
});