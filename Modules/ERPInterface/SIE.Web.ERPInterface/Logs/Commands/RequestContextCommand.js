SIE.defineCommand('SIE.Web.ERPInterface.Logs.Commands.RequestContextCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看请求报文", group: "edit", iconCls: "icon-PageSearch icon-blue" },
    canExecute: function (listView) {
        return listView != null && listView.getSelection().length == 1;
    },
    execute: function (view, source) {
        var me = this;
        var downloadJobTimeDetail = me.view.getCurrent().data;

        var win = SIE.Window.show({
            title: "查看请求报文".t(),
            width: 472,
            height: 500,
            items: [{
                id: 'ErpRequestStrtextareaId',
                xtype: 'textareafield',
                name: 'ErpRequestStrtextarea1',
                value: downloadJobTimeDetail.RequestStr,
                LogId: downloadJobTimeDetail.Id,
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