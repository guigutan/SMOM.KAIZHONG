SIE.defineCommand('SIE.Web.EMS.IdleArchives.Commands.SaveIdleArchivesCommand', {
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.FormSave',
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('保存成功'.t());
    },
    onSaved: function (view, res) {
        var me = this;
        me.onSavedMsg();
        var current = view.getCurrent();
        current.markSaved();
        setTimeout(function () {
            CRT.Workbench.closeCurrentTab();
            CRT.Event.fire("SIE.EMS.IdleArchives.IdleArchive_refresh");
        }, 2000);

    }
});
