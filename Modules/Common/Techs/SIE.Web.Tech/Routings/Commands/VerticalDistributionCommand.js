SIE.defineCommand('SIE.Web.Tech.Routings.Commands.VerticalDistributionCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "纵向分布", group: "edit", tooltip: "纵向分布".t(), iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        view.layout.designControl.designCanvas.verticalDistribution();
    }
});