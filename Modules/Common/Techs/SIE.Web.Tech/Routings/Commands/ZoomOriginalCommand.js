SIE.defineCommand('SIE.Web.Tech.Routings.Commands.ZoomOriginalCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "1:1还原", group: "edit", tooltip: "1:1还原".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        view.layout.designControl.designCanvas.zoomOriginal();
    }
});