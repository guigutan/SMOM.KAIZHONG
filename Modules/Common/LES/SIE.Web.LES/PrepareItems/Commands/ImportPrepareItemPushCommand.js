SIE.defineCommand('SIE.Web.LES.PrepareItems.Commands.ImportPrepareItemPushCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-blue" },
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