SIE.defineCommand('SIE.Web.MES.Edge.Commands.ReSubmitMessageCommand', {
    meta: { text: "重新提交", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.IsError == 0) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    execute: function (view, source) {
        var selLength = view.getSelectionIds().length;
        console.log(selLength);
        SIE.Msg.askQuestion('是否提交数据?'.t(), function () {
            SIE.Msg.wait('正在提交，请稍候...'.t());
            view.execute({
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) {
                    SIE.Msg.hide();
                    SIE.Msg.showMessage(Ext.String.format('提交完成，成功【{0}】条，失败【{1}】条'.t(), res.Result, selLength - res.Result));
                    view.reloadData();
                }
            });
        });
    }
});