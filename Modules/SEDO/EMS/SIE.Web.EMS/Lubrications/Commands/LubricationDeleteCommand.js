SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.LubricationDeleteCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selectedItems = view.getSelection();
        if (selectedItems.length === 0) {
            return false;
        }
        var res = true;
        SIE.each(selectedItems, function (model) {
            //只有单据来源是手工创建且未审批的数据可以删除
            if ((model.data.BillSourceType != SIE.Equipments.Enums.BillSourceType.Manual.value || model.data.ApprovalStatus != 10)
            ) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (listView, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定删除选中的数据吗？确定后直接删除'.t()),
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
    }
});