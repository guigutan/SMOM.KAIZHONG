SIE.defineCommand('SIE.Web.Packages.QrCodeParseRules.Commands.EnableQrCodeParseRuleCommand', {
    extend: 'SIE.cmd.Enable',
    meta: { text: "启用", group: "edit", iconCls: "icon-Play icon-blue" },
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0 || this.selectedItems.length > 1) {
            return false;
        }

        if (listview.getData().isDirty())
            return false;

        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (item.data.State === 1) {
                return false;
            }
        }

        return true;
    }
});