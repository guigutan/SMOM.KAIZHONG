SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonGroupImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", hierarchy: "导入".t(), group: "business", iconCls: "icon-Upload icon-green" },
    parentId: null,
});