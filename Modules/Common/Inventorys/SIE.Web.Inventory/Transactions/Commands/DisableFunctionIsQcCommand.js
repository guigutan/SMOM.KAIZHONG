SIE.defineCommand('SIE.Web.Inventory.Transactions.Commands.DisableFunctionIsQcCommand', {
    meta: { text: "禁用IQC报检", group: "business", hierarchy: "禁用", iconCls: "icon-Cancel icon-red" },
    selectedItems: [],
    canExecute: function (view) {
        if (view.getData().isDirty()) return false;
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (item.data.IsQc === false) {
                return false;
            }
        }
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定禁用IQC报检选中的{0}条资料？'.t(), this.selectedItems.length),
            function () {
                //减少网络传输，此自定义命令只需要传选中的ID
                var selectIds = view.getSelectionIds(this.selectedItems);
                view.execute({
                    withIds: true,
                    selectIds: selectIds,
                    success: function (res) { //回调
                        view.reloadData();
                    }
                });
            });
    },
});