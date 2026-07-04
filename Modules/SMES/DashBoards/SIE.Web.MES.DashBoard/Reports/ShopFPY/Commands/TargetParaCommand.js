SIE.defineCommand('SIE.Web.MES.DashBoard.Reports.ShopFPY.Commands.TargetParaCommand', {
    meta: { text: "目标参数设置", group: "edit", iconCls: "icon-ConfigItem icon-blue" },
    /**
     * 执行方法
     * @param view 查询逻辑视图
     * @param source 数据源
     */
    execute: function (view, source) {
        CRT.Workbench.addPage({
            tabId: 'menu_' + 'SIE.MES.DashBoard.Reports.FpySettings.ShopFpySetting'.replace(/[.|,]/g, ''),
            entityType: 'SIE.MES.DashBoard.Reports.FpySettings.ShopFpySetting',
            module: view.module,
            isAggt: true,
            title: '直通率设置'.L10N()
        });
    },
});