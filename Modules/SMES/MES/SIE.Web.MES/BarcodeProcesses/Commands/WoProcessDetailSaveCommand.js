SIE.defineCommand("SIE.Web.MES.BarcodeProcesses.Commands.WoProcessDetailSaveCommand", {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaved: function (view, res) {
        var me = this;
        var operationView = view;
        if (view.associateCmd) {
            operationView = view.associateCmd.view;
        }
        view._parent.reloadData();
        me.onSavedMsg(view, res);
    },
})
