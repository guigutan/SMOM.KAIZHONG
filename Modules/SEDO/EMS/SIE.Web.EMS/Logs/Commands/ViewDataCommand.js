SIE.defineCommand('SIE.Web.EMS.Logs.Commands.ViewDataCommand', {
    meta: { text: "查看详细数据", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }

        var data = view.getCurrent();
        

        return data != null;
    },

    showView: function (editEntity) {
        var me = this;
        var view = me.view;
        var win = SIE.Window.show({
            title: "查看详细数据".t(),
            width: 472,
            height: 500,
            items: [{
                xtype: 'textareafield',
                value: editEntity.data.DetailMsg,
                LogId: editEntity.Id,
                readOnly: true,//ebsUploadLog.IsSuccess 
                hideLabel: true,
                anchor: '100%',
                grow: true,
                height: 500,
                width: 450,
                autoScroll: true
            }],
            buttons: ['取消'.t()],
            callback: function (btn) {
                if (btn == "取消".t()) {
                    win.close();
                }
            }
        });
    }
});