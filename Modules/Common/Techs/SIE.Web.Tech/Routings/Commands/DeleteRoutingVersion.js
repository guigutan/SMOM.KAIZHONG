SIE.defineCommand('SIE.Web.Tech.Routings.Commands.DeleteRoutingVersion', {
    extend: 'SIE.cmd.Command',
    meta: { text: "", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        var routingVersionId = 0;
        if (view.CurRoutingVersion) {
            var version = view.CurRoutingVersion;
            routingVersionId = version.get('Id');
        }
        var versionNode = source;
        var versionId = versionNode.get("Id");
        var command = Ext.getClassName(this);
        if (versionId !== 0) {
             //客制化界面命令注册签名（注册在context，没有后台请求）
            SIE.Signature.createCmdContext({ command: command, commandName: "删除", Type: view.model });
            SIE.Msg.confirm("确定要删除选择的工艺流程吗?".L10N(), function () {
                view.execute({
                    command: command,
                    data: versionId,
                    success: function (res) {
                        var treePanel = versionNode.parentNode.getOwnerTree();
                        var routingNode = versionNode.parentNode;
                        routingNode.data.MaxVersionNum = res.Result;
                        routingNode.removeChild(versionNode);
                        if (routingVersionId === versionId) {
                            if (routingNode.childNodes.length > 0) {
                                treePanel.getSelectionModel().select(routingNode.childNodes[0]);
                                var treeview = treePanel.getView();
                                if (routingNode && routingNode.parentNode) {
                                    routingNode.expand();
                                    routingNode.parentNode.expand();
                                }
                                treeview.focusRow(routingNode.childNodes[0]);
                                treeview.fireEvent('itemdblclick', treeview, routingNode.childNodes[0], treeview.getNodeByRecord(routingNode.childNodes[0]), treeview.indexOfRow(routingNode.childNodes[0]));
                            }                            
                        }
                    }
                });
            });
        }
        else {
            SIE.Msg.confirm("确定要删除选择的工艺流程吗?".L10N(), function () {
                var treePanel = versionNode.parentNode.getOwnerTree();
                var routingNode = versionNode.parentNode;
                versionNode.parentNode.removeChild(versionNode);
                if (routingNode.childNodes.length > 0) {
                    treePanel.getSelectionModel().select(routingNode.childNodes[0]);
                    var treeview = treePanel.getView();
                    if (routingNode && routingNode.parentNode) {
                        routingNode.expand();
                        routingNode.parentNode.expand();
                    }
                    treeview.focusRow(routingNode.childNodes[0]);
                    treeview.fireEvent('itemdblclick', treeview, routingNode.childNodes[0], treeview.getNodeByRecord(routingNode.childNodes[0]), treeview.indexOfRow(routingNode.childNodes[0]));
                }                
            });
        }
    }
});