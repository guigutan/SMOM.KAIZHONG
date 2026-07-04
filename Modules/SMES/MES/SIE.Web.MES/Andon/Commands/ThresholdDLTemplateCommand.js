SIE.defineCommand('SIE.Web.MES.Andon.Commands.ThresholdDLTemplateCommand', {
    extend: 'SIE.Web.Common.Import.Commands.DownloadTemplateCommand',
    meta: { text: "下载模板", hierarchy: "导入".t(), group: "business", iconCls: "icon-Upload icon-green" },
    /**
     * @override 创建导出参数
     * @param {} view 视图
     * @param {} tmplId 模板ID
     * @param {} data 导出数据
     * @returns {}
    */
    createExportParam: function (view) {
        var me = this;
        var param = {
            Name: "SIE.Web.MES.Andon.Commands.ThresholdImportCommand",
            Token: view.getToken(),
            Data: SIE.data.Utils.seriaizeRequest({
                Data: SIE.data.Utils.seriaizeRequest({
                    BehaviorName: "Download",
                    Type: view.model
                })
            })
        };

        return param;
    },
});