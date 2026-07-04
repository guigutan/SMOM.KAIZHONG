SIE.defineCommand('SIE.Web.MES.TaskManagement.ProcessTaskLists.Commands.TaskDeleteCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        var result = true;
        this.selectedItems.forEach(item => {

            if ((item.getTaskStatus() != 10 && item.getTaskStatus() != 0) || item.getReportQty() != 0) {
                result = false;
                return;
            }
        });
        return result;
    },
    execute: function (listView, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定删除选中的{0}笔数据吗？确定后直接删除'.t(), this.selectedItems.length),
            function () {
                var me = this;
                var selectIds = [];
                listView.getSelection().forEach(it => { selectIds.push(it.getDispatchTaskId()); });
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