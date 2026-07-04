SIE.defineCommand('SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands.SubmitReturnCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-Check icon-green" },
    canExecute: function (view) {
        var selectedItems = view.getSelection();
        if (selectedItems.length === 0) {
            return false;
        }
        var res = true;
        SIE.each(selectedItems, function (model) {
            //只有单据来源待提交可以
            if ((model.data.ReturnState !=10)
            ) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (listView, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定提交选中的数据吗？'.t()),
            function () {
                var me = this;
                var selectIds = listView.getSelectionIds(this.selectedItems);
                listView.execute({
                    withIds: true,
                    selectIds: selectIds,
                    success: function (res) { //回调
                        SIE.Msg.showToast('提交成功'.t(), '完成');
                        listView.loadData();
                    }
                });
            });
    }
    
}); 