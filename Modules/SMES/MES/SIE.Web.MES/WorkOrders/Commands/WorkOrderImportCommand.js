SIE.defineCommand('SIE.Web.MES.WorkOrders.WorkOrderImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入工单", hierarchy: "工单生成", group: "business", iconCls: "icon-Upload icon-blue" },
    downloadTemplateSuccess: function (res) {
        var filePath = res.Result.FilePath;
        var url = window.location.origin + "/" + filePath;
        window.open(url);
    }
});