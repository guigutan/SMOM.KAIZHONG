SIE.defineCommand('SIE.Web.Tech.Routings.Commands.HorizontalDistributionCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "横向分布", group: "edit", tooltip: "横向分布".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        view.layout.designControl.designCanvas.horizontalDistribution();
    }
});