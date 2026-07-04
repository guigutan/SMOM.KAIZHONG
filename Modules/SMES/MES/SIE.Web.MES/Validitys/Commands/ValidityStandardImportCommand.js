SIE.defineCommand('SIE.Web.MES.Validitys.Commands.ValidityStandardImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        return true;
    },
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
