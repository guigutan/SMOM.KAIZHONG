SIE.defineCommand('SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands.DetailRedrawReturnCommand', {
    meta: { text: "撤销", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
         
         
        var selectedItem = view.getCurrent();
        if (selectedItem == null) {
            return false;
        }
        var res = true;
        if (selectedItem.data.ReturnState != 10) {
            res = false;
        }
        return res;
    },
    execute: function (listView, source) {
        SIE.Msg.askQuestion(Ext.String.format('撤销当前的数据吗？'.t()),
            function () {
                var me = this;
                var selectIds = listView.getCurrent().getId();
                listView.execute({
                    withIds: true,
                    selectIds: [selectIds],
                    success: function (res) { //回调
                        var current = listView.getCurrent();
                        current.markSaved();
                        window.setTimeout(function () {
                            SIE.Msg.showToast('撤销成功'.t(), '完成');
                            CRT.Event.fire("SIE.LES.RetreatItemManage.MaterialReturns.MaterialReturn_refresh");
                            CRT.Workbench.closeCurrentTab();
                        }, 1000);
                    }
                });
            });
    }
    
}); 