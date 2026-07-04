SIE.defineCommand('SIE.Web.CSM.Suppliers.Commands.SupplierItemDeleteCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        return true;
    },
    execute: function (listView, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定删除选中的{0}笔数据吗？确定后直接删除'.t(), this.selectedItems.length),
            function () {
                var me = this;
                var selectIds = listView.getSelectionIds(this.selectedItems);
                listView.execute({
                    withIds: true,
                    selectIds: selectIds,
                    success: function (res) { //回调
                        listView.loadData();
                    }
                });
            });
    },
});