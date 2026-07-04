SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.EditAssignRuleCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }

        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (SIE.Domain.State.Enable.value !== item.getState()) {
                return false;
            }

            if (item.data.IsDefault === true) {
                return false;
            }
        }

        return true;
    }
});