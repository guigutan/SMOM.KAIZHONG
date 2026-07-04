SIE.defineCommand('SIE.Web.Inventory.Transactions.Commands.EditTransactionDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        var transaction = view.getParent().getCurrent();
        if (transaction == null && transaction.data.SourceType == SIE.Common.SourceType.Internal.value)
            return false;
        return true
    }
});