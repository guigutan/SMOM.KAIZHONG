SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.SaveFixedAccountDetailsCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit" },
    onSavedMsg: function (view, res) {
        Ext.Msg.show({
            title: '提示'.t(),
            message: '保存成功'.t(),
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO,
            callback: function () {
                CRT.Workbench.closeCurrentTab();
            }
        });
    }
});