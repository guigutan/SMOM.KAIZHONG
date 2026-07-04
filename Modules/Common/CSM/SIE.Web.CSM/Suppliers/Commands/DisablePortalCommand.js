SIE.defineCommand('SIE.Web.CSM.Suppliers.Commands.DisablePortalCommand', {
    meta: { text: "禁用门户", group: "edit", iconCls: "icon-Cancel icon-blue" },
    selectedItems: [],
    canExecute: function (listview) {
        if (listview.getData().isDirty()) return false;
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (item.data.IsPortal === false) {
                return false;
            }
        }
        return true;
    },
    execute: function (listview, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定禁用选中的{0}条资料？'.t(), this.selectedItems.length),
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