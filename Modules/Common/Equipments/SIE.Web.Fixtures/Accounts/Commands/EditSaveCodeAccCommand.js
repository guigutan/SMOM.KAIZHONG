SIE.defineCommand('SIE.Web.Fixtures.Accounts.Commands.EditSaveCodeAccCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
    * 保存
    * @param {} view
    */
    doSave: function (view) {
        var me = this;
        me.view = view;
        var data = view.getCurrent().data;
        var indata = {};
        indata.Data = Ext.encode(data);
        view.execute({
            data: indata,
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg !== '') {
                    SIE.Msg.showError(errMsg);
                    return;
                } else {
                    SIE.Msg.showInstantMessage('保存成功！', '提示', 3);
                    entity.markSaved();
                    me.view.syncCmdState();
                    window.setTimeout(function () {
                        CRT.Event.fire("SIE.Fixtures.Fixtures.Accounts.FixtureCodeAccount_refresh");
                        CRT.Workbench.closeCurrentTab();
                    }, 3000);
                }
            }
        });
    }
});