SIE.defineCommand('SIE.Web.Tech.Routings.Commands.PublishCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "发布", group: "edit", tooltip:"发布工艺路线，发布后无法更改".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        var version = view.CurRoutingVersion;
        if (version.data.Id === null && version.data.isCopy) {
            SIE.Msg.showInstantMessage('不允许发布，工艺路线版本未保存'.t());
            return;
        }
        var drawCtrl = Ext.getCmp(view.routingDrawControlId);
        var layout = drawCtrl.getXml();
        if (view.layout.designControl.designCanvas.validateDesignData() === false)
            return;
        var iptdata = {
            Id: version.data.Id,
            Layout: layout
        };
        //客制化界面命令注册签名（注册在context，没有后台请求）
        SIE.Signature.createCmdContext({ command: Ext.getClassName(this), commandName: "发布", Type: view.model });
        view.execute({
            command: Ext.getClassName(this),
            data: iptdata,
            success: function (res) {
                var routingVersion = res.Result;
                var routingNode = version.parentNode;
                view.CurRoutingVersion.set('state', 1);
                view.CurRoutingVersion.set('isDefault', routingVersion.isDefault);
                if (routingVersion.isDefault === 0)
                    view.layout.routingControl.setDefaultCommandState(true);
                else {
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

                view.layout.designControl.setMainBlockCommandDisabled('SaveCommand', true);
                view.layout.designControl.setMainBlockCommandDisabled('PublishCommand', true);
                view.layout.designControl.setMainBlockCommandDisabled('LeftRightCommand', true);
                view.layout.designControl.designCanvas.setLock(true);
                SIE.Msg.showInstantMessage('发布成功'.t());
            }
        });
    }
});