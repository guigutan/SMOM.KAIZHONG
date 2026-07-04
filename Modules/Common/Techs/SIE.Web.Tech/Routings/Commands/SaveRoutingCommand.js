SIE.defineCommand('SIE.Web.Tech.Routings.Commands.SaveRoutingCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "保存", group: "edit", tooltip: "保存工艺路线".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        var version = view.CurRoutingVersion;
        var routingId = version.data.routingId;
        var routingVersionId = version.data.Id;

        
        var layout = Ext.getCmp(view.routingDrawControlId).getXml();

        var iptdata = {
            RoutingId: routingId,
            RoutingVersionId: routingVersionId,
            Layout: layout
        };
        //判断是否复制粘贴保存       
        if (version.data.isCopy || version.data.isNew) {
            iptdata.RoutingId = version.data.targetRoutingId;
            iptdata.VersionName = version.data.versionName;
            iptdata.RoutingVersionId = 0;
        }
        //客制化界面命令注册签名（注册在context，没有后台请求）
        SIE.Signature.createCmdContext({ command: Ext.getClassName(this), commandName: "保存", Type: view.model });
        view.execute({
            command: Ext.getClassName(this),
            data: iptdata,
            success: function (res) {
                SIE.Msg.showInstantMessage('保存成功'.t());
                if (res.Result !== true) {
                    //复制粘贴保存后，展开节点                  
                    var routingVersion = res.Result;
                    var routingNode = version.parentNode;
                    if (routingVersion.versionNum)
                        routingNode.data.MaxVersionNum = routingVersion.versionNum;
                    routingNode.removeChild(version);
                    if (!routingNode) {
                        var routingControl = view.layout.routingControl;
                        routingNode = routingControl.getRootNode().childNodes.selectMany(function (p) { return p.childNodes; }).first(function (p) { return p.data.Id === routingVersion.routingId; });
                    }
                    var node = routingNode.appendChild(routingVersion);
                    var treePanel = routingNode.getOwnerTree();
                    treePanel.getSelectionModel().select(node);
                    var treeview = treePanel.getView();
                    if (routingNode && routingNode.parentNode) {
                        routingNode.expand();
                        routingNode.parentNode.expand();
                    }
                    treeview.focusRow(node);
                    treeview.fireEvent('itemdblclick', treeview, node, treeview.getNodeByRecord(node), treeview.indexOfRow(node));
                }
            }
        });
    }
});
