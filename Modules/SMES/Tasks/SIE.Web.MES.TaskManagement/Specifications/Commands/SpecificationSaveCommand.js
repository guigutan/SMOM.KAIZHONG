SIE.defineCommand('SIE.Web.MES.TaskManagement.Specifications.Commands.SpecificationSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaved: function (view, res) {
        var me = this;
        CRT.Event.fire(view.model + '_refresh');
        var data = view.getData();
        data.commitChanges();
        view.syncCmdState();
        me.onSavedMsg(view, res);
    },
});