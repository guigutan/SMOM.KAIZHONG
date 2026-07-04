SIE.defineCommand('SIE.Web.Tech.Routings.Commands.AddRoutingVersion', {
    extend: 'SIE.cmd.Command',
    meta: { text: "", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        var routingNode = source;
        var treePanel = routingNode.getOwnerTree();
        var routingId = routingNode.get("Id");

        SIE.invokeDataQuery({
            type: "SIE.Web.Tech.Routings.TechDataQueryer",
            method: "GetAddRoutingVersionInfo",
            params: [routingNode.data.MaxVersionNum],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Success) {
                    var routingVersion = res.Result;
                    routingVersion.Version.targetRoutingId = routingId;
                    routingVersion.Version.routingId = routingId;
                    var designControl = treePanel.mainView.layout.designControl;
                    designControl.resetMainBlock();
                    designControl.designCanvas.setLock(false);
                    designControl.setMainBlockCommandDisabled('SaveCommand', false);
                    designControl.setMainBlockCommandDisabled('PublishCommand', true);
                    designControl.setMainBlockCommandDisabled('LeftRightCommand', true);
                    designControl.setMainBlockCommandDisabled('UpDownCommand', true);
                    designControl.setMainBlockCommandDisabled('VerticalDistributionCommand', true);
                    designControl.setMainBlockCommandDisabled('HorizontalDistributionCommand', true);
                    designControl.designCanvas.drawRouting(routingVersion.Layout);
                    Ext.query('#spTitle')[0].innerHTML = "";

                    if (!routingNode) {
                        var routingControl = treePanel.mainView.layout.routingControl;
                        routingNode = routingControl.getRootNode().childNodes.selectMany(function (p) { return p.childNodes; }).first(function (p) { return p.data.Id === routingVersion.routingId; });
                    }
                    var node = routingNode.appendChild(routingVersion.Version);
                    treePanel.getSelectionModel().select(node);
                    var treeview = treePanel.getView();
                    if (routingNode && routingNode.parentNode) {
                        routingNode.expand();
                        routingNode.parentNode.expand();
                    }
                    treeview.focusRow(node);
                    treePanel.mainView.routingNode = node;
                    treePanel.mainView.CurRoutingVersion = node;
                }
            }
        });
    }
});