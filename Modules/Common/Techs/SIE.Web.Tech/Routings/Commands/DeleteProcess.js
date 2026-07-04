SIE.defineCommand('SIE.Web.Tech.Routings.Commands.DeleteProcess', {
    extend: 'SIE.cmd.Command',
    meta: { text: "", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        var processNode = source;
        var processId = processNode.get("Id");
        var command = Ext.getClassName(this);
        SIE.Msg.confirm("确定删除工序".L10N(), function () {
            view.execute({
                command: command,
                data: processId,
                success: function (res) {
                    if (res.Result)
                        processNode.parentNode.removeChild(processNode);
                    else
                        SIE.Msg.showError(Ext.String.format('删除失败,工序[Id={0}]不存在'.L10N(), processId));
                }
            });
        });
    }
});