SIE.defineCommand('SIE.Web.MES.Checker.Commands.CheckerUpholdImportEffectiveDateCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "批改（不改工厂和检具类型）", hierarchy: "导入".t(), group: "business", iconCls: "icon-Upload icon-green" }
});

//嵌入的资源失效，重新嵌入再签入。