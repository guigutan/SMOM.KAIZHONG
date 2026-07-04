SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.DisableSparePartCommand', {
    extend: 'SIE.cmd.ListEditableBase',
    meta: { text: "禁用", group: "edit", iconCls: "icon-Cancel icon-red" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (item.data.CreateDate != null) {
                if (item.data.State === 0) {
                    return false;
                }
            }
            else
                return false;
        }
        return true;
    },
    execute: function (listview, source) {
        SIE.Msg.askQuestion('是否禁用这些备件?'.t(),
            function () {
                //减少网络传输，此自定义命令只需要传选中的ID                
                listview.execute({
                    withIds: true,
                    selectIds: listview.getSelectionIds(),
                    success: function (res) {
                        //回调
                        listview.reloadData();
                    }
                });
            });
    }
});