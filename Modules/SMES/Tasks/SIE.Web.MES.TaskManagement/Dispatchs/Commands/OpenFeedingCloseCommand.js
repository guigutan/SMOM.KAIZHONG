SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.OpenFeedingCloseCommand', {
    meta: { text: "打开上料", group: "edit", iconCls: "icon-ModifyPassword icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0 && selecteditems.all(p => p.getIsFeedingClose() == true)) {
            return true;
            //for (var i = 0; i < selecteditems.length; i++) {
            //    if (selecteditems[i].data.TaskStatus === 40 || selecteditems[i].data.TaskStatus === 50 || selecteditems[i].data.TaskStatus === 60) {
            //        return false;
            //    }
            //}
            //return true;
        }
        return false;
    },
    execute: function (view, source) {
        view.execute({
            data: view.getSelectionIds(),
            success: function (res) { //回调
                view.reloadData();
            }
        });
    },
});