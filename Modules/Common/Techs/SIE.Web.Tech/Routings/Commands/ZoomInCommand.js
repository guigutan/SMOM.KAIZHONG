SIE.defineCommand('SIE.Web.Tech.Routings.Commands.ZoomInCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "放大", group: "edit", tooltip: "放大(快捷键:shift+滚轮)".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        view.layout.designControl.designCanvas.zoomIn();
    }
});