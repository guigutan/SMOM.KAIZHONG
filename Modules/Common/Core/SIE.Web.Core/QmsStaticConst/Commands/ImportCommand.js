SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.ImportCommand', {
    extend: 'SIE.Web.Core.Common.Commands.Import.ImportMasterSubordinateCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Download icon-blue", model: "SIE.Web.Core.QmsStaticConst.ViewModels.StaticConstImportViewModel" },
});