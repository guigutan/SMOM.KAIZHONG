SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands.EmsDownloadCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.FtpDownloadCommand',
    meta: { text: "下载", group: "edit", iconCls: "icon-Download icon-blue" },
    canExecute: function (view) {
        var item = view.getCurrent();
        if (item != null && item.data != null && item.data.FilePath != null && item.data.FilePath != "") {
            return true;
        }

        return false;
    },
});