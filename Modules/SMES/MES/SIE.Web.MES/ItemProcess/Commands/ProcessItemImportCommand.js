SIE.defineCommand('SIE.Web.MES.ItemProcess.Commands.ProcessItemImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", hierarchy: "导入".t(), group: "business", iconCls: "icon-Upload icon-green" }
});