SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.SubmitSparePartStoreCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit" },
    onSaving: function (view, res) {

        var dtlChildView = view.findChild('SIE.EMS.SpareParts.StoreDetail');

        dtlChildView.getData().getData().items.forEach(function (detail) {
            detail.dirty = true;
        });

        return this.callParent(arguments);
    },
    onSavedMsg: function (view, res) {
        Ext.Msg.show({
            title: '提示'.t(),
            message: '提交成功'.t(),
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO,
            callback: function () {
                CRT.Workbench.closeCurrentTab();
            }
        });
    }
});