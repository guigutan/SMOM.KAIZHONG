SIE.defineCommand('SIE.Web.EMS.Equipments.Models.Commands.EquipModelProjectImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "点检保养项目导入", group: "business", iconCls: "icon-Download icon-blue" },
    /**
* 下载模板-成功
*/
    downloadTemplateSuccess: function (res) {
        var filePath = res.Result.FilePath;
        var url = window.location.origin + "/" + filePath;
        window.open(url);
    }
});