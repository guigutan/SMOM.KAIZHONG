SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageExportXlsCommand', {
    extend:'SIE.cmd.ExportXls',
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    myview: {}, // 当前视图对象
    fieldNames: [],//导出的数据
});