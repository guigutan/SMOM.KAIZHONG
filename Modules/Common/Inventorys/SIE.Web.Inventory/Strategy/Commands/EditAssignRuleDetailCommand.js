SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.EditAssignRuleDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        var rule = view.getParent();
        if (rule == null) return false;
        var ruleCur = view.getParent().getCurrent();
        if (ruleCur == null) return false;
        if (ruleCur != null) {
            if (ruleCur.data.IsDefault === true) return false;
        }

        return true;
    }
});