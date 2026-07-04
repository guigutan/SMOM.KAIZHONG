SIE.defineCommand('SIE.Web.EMS.InventoryPlans.Commands.DeleteEquipListCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selections = view.getSelection();
        var parent = view.getParent().getCurrent();
        if (selections.length == 0 || parent.getApprovalStatus() != 10) {//选中项
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var selectionIds = view.getSelectionIds();
        view.execute({
            data: selectionIds,
            success: function (res) {
                store.commitChanges();
                view.setCurrent(null, true);
                view.reloadData();
                SIE.Msg.showMessage('删除成功！'.t());
            },
            error: function (res) {
                store.rejectChanges();
            }
        });
    }
});