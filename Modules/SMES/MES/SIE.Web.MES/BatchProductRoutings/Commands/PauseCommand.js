SIE.defineCommand('SIE.Web.MES.BatchProductRoutings.Commands.PauseCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "暂停", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        var command = Ext.getClassName(this);
        view.execute({
            command: command,
            data: routingId,
            success: function (res) {
                view.layout.refreshWipProductInfo(layout.barcode);  //重新加载工艺路线信息 
            }
        });
    }
});