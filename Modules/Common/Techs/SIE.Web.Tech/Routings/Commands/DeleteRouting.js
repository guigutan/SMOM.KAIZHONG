SIE.defineCommand('SIE.Web.Tech.Routings.Commands.DeleteRouting', {
    extend: 'SIE.cmd.Command',
    meta: { text: "", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        var routingNode = source;
        var treePanel = routingNode.getOwnerTree();
        var routingId = routingNode.get("Id");
        var command = Ext.getClassName(this);
        //客制化界面命令注册签名（注册在context，没有后台请求）
        SIE.Signature.createCmdContext({ command: command, commandName: "删除", Type: view.model });
        SIE.Msg.confirm("确定要删除选择的工艺路线吗?".L10N(), function () {
            view.execute({
                command: command,
                data: routingId,
                success: function (res) {
                    routingNode.parentNode.removeChild(routingNode);
                    if (view.routingId === routingId) {
                        view.layout.designControl.resetMainBlock();
                    }
                }
            });
        });
    }
});