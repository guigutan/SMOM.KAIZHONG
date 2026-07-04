SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.SynModelCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "同步型号数据", group: "edit", iconCls: "icon-SyncCode icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                //if (selecteditems[i].dirty) {
                //    return false;
                //}
            }
            return true;
        }
        return false;
    },
    execute: function (view) {
        var selection = view.getSelection();
        for (var i = 0; i < selection.length; i++) {
            if (selection[i].dirty) {
                SIE.Msg.showWarning('数据未保存，请先保存后再同步型号数据'.t());
                return;
            }
            SIE.Msg.askQuestion(Ext.String.format('是否同步型号数据？'.t()),
                function () {
                    SIE.Msg.wait("正在同步型号数据，请稍等...".t());
                    view.execute({
                        data: view.getCurrent().data,
                        withIds: true,
                        selectIds: view.getSelectionIds(),
                        success: function (res) { //回调
                            SIE.Msg.hide();
                            if (res.Success) {
                                var errMsg = res.Result;
                                if (errMsg == '同步型号数据成功'.t())
                                     view.reloadData();              
                                SIE.Msg.showMessage(errMsg.t());
                            }
                        }
                    });
                });
        }
    }
});