//SIE.defineCommand('SIE.Web.MES.DataBarcode.Commands.DataBarcodeImportCommand', {
//    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
//    meta: { text: "导入", hierarchy: "导入".t(), group: "business", iconCls: "icon-Upload icon-green" }
//});

SIE.defineCommand('SIE.Web.MES.DataBarcode.Commands.DataBarcodeImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", hierarchy: "导入".t(), group: "business", iconCls: "icon-Upload icon-blue" },
});