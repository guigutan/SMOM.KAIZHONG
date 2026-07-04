SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.DeleteTurnOverRuleDetailCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }

        var rule = view.getParent();
        if (rule == null) return false;
        var ruleCur = view.getParent().getCurrent();
        if (ruleCur == null) return false;
        if (ruleCur != null)
            if (ruleCur.data.IsDefault === true) return false;

        return true;
    }
});