SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageLocationRouteImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入库位巷道关系", hierarchy: "导入", group: "business", iconCls: "icon-Download icon-blue" },
    /**
     * 下载模板-成功
     * @param {any} res
     */
    downloadTemplateSuccess: function (res) {
        var filePath = res.Result.FilePath;
        var url = window.location.origin + "/" + filePath;
        window.open(url);
    }
});