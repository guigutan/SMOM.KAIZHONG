SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.SaveAcceptAttachmentCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "上传", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return this.callParent(arguments);
    },
    execute: function (view, source) {
        var data = view.getCurrent().data;
        view.execute({
            withIds: true,
            data: data,
            success: function (rst) {
                var win = Ext.getCmp('UploadSpareAcceptCommand_Window');
                win.close();
                CRT.Event.fire("SIE.EMS.Purchases.SparePartAcceptances.SparePartAcceptance_refresh");
                Ext.MessageBox.alert("提示".t(), "上传成功".t());
            }
        });
    }
});