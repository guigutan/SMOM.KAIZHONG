SIE.defineCommand('SIE.Web.Tech.Routings.Commands.ZoomOutCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "缩小", group: "edit", tooltip: "缩小(快捷键:shift+滚轮)".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        view.layout.designControl.designCanvas.zoomOut();
    }
});