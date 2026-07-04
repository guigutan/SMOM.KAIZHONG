SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.DisableTurnOverRuleCommand', {
    extend: 'SIE.cmd.Disable',
    meta: { text: "禁用", group: "business", iconCls: "icon-Cancel icon-red" },

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