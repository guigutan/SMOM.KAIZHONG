SIE.defineCommand('SIE.Web.Tech.Routings.Commands.EditRouting', {
    extend: 'SIE.cmd.Command',
    meta: { text: "", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source, win) {

        var iptdata = {
            Id: source.id,
            Name: source.name,
            Description: source.desc
        };
        view.execute({
            command: Ext.getClassName(this),
            data: iptdata,
            success: function (res) {
                source.routingNode.set('Name', source.name);
                source.routingNode.set('Description', source.desc);
                source.routingNode.set('text', source.name);
                if (win != null)
                    win.close();
            }
        });
    }
});
