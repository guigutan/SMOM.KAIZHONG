SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.UploadCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "上传", group: "edit", iconCls: "icon-ExportData icon-green" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view) {  
        SIE.Web.FMS.FileManages.CommonFunctions.UploadFiles();
    }
});