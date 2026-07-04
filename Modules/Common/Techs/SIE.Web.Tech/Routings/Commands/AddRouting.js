SIE.defineCommand('SIE.Web.Tech.Routings.Commands.AddRouting', {
    extend: 'SIE.cmd.Command',
    meta: { text: "", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source,win) { 
        var iptdata = {
            CategoryId: source.categoryId,
            Name: source.name,
            Description: source.desc
        };
        view.execute({
            command: Ext.getClassName(this),
            data: iptdata,
            success: function (res) {
                //在树中添加工艺路线节点
                var routing = res.Result;
                var categroyNode = source.Category;
                var node = categroyNode.appendChild(routing);
                var treePanel = categroyNode.getOwnerTree(); 
                treePanel.getSelectionModel().select(node);
                var treeview = treePanel.getView(); 
                categroyNode.expand();
                node.expand();
                treeview.focusRow(node); 
                treePanel.fireEvent('itemclick', treeview, node, treeview.getNodeByRecord(node), treeview.indexOfRow(node));
                if (win != null)
                    win.close();
            }
        });
    }
});