SIE.defineCommand('SIE.Web.MES.DashBoard.Reports.ProductFPY.Commands.TargetParaCommand', {
    meta: { text: "目标参数设置", group: "edit" },
    /**
     * 执行方法
     * @param view 查询逻辑视图
     * @param source 数据源
     */
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.MES.DashBoard.Reports.FpySettings.ProductModelFpySetting',
            module: view.module,
            title: '直通率设置'.L10N(),
            isAggt: true
        });
    },
});