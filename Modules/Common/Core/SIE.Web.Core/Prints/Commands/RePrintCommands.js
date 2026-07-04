SIE.defineCommand('SIE.Web.Core.Prints.Commands.RePrintCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "补打", group: "edit", iconCls: "icon-PrintData icon-blue" },
    execute: function (view, source) {
        var entity = view && view.getCurrent() && view.getCurrent().data;
        if (entity == null) return;
        var id = entity.Id;
        var dataKey = entity.DataKey;
        var command = Ext.getClassName(this);
        SIE.Msg.confirm("确定要补打吗".L10N(), function () {
            view.execute({
                command: command,
                data: id,
                success: function (res) {
                    if (res.Result)
                        SIE.Msg.showInstantMessage('打印请求已发送完成!'.t(), "标签补打", 3);
                    else
                        SIE.Msg.showError(Ext.String.format('打印失败'.L10N(), dataKey));
                }
            });
        });
    }
});