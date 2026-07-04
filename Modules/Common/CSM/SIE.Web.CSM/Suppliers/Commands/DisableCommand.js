SIE.defineCommand('SIE.Web.CSM.Suppliers.Commands.DisableCommand', {
    meta: { text: "禁用", group: "edit", iconCls: "icon-Cancel icon-blue" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (item.data.State === 0) {
                return false;
            }
        }
        return true;
    },
    execute: function (listview, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定禁用选中资料？'.t(), this.selectedItems.length),
            function () {
                //减少网络传输，此自定义命令只需要传选中的ID
                var selectIds = listview.getSelectionIds(this.selectedItems);
                listview.execute({
                    withIds: true,
                    selectIds: selectIds,
                    success: function (res) { //回调
                        listview.reloadData();
                    }
                });
            });
    },
});