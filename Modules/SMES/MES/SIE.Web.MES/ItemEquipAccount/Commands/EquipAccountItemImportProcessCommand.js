SIE.defineCommand('SIE.Web.MES.ItemEquipAccount.Commands.EquipAccountItemImportProcessCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "工序修改", hierarchy: "导入".t(), group: "business", iconCls: "icon-Upload icon-green" }
});