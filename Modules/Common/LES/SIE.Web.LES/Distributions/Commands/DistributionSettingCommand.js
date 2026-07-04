SIE.defineCommand('SIE.Web.LES.Distributions.Commands.DistributionSettingCommand', {
    meta: { text: "配送设置", group: "edit", iconCls: "icon-ConfigItem icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.LES.Distributions.DistributionSetting',
            title: '配送设置'.L10N(),
            module: listView.module,
            isAggt: true
        });
    }
});