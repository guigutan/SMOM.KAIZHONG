SIE.defineCommand('SIE.Web.Tech.Routings.Commands.UpDownCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "上下居中", group: "edit", tooltip: "上下居中".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        view.layout.designControl.designCanvas.verticalAlignment();
    }
});