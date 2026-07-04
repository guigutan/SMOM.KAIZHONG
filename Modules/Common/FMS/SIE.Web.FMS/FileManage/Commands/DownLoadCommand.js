SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.DownLoadCommand', {
    extend: 'SIE.cmd.ExportCommandBase',
    meta: { text: "下载", group: "edit", iconCls: "icon-FileTree icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (ListView) {   
        SIE.Web.FMS.FileManages.CommonFunctions.DownloadFile();
    }
});