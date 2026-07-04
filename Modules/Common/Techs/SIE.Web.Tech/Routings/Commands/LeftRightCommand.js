SIE.defineCommand('SIE.Web.Tech.Routings.Commands.LeftRightCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "左右居中", group: "edit", tooltip: "左右居中".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        view.layout.designControl.designCanvas.leftRightAlignment();
    }
});