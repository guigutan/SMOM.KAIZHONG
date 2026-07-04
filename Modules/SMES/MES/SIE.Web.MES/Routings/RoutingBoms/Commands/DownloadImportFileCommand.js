SIE.defineCommand('SIE.Web.MES.Routings.RoutingBoms.Commands.DownloadImportFileCommand', {
    extend: 'SIE.cmd.ExportCommandBase', 
    meta: { text: "下载原文件", group: "edit", iconCls: "icon-Download icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        if (view.getCurrent() == null) return false;
        return true;
    },
    execute: function (view, source) {
        if (view.getCurrent().getAttachmentId() == null) {
            SIE.Msg.showInstantMessage('此导入日志未保存文件附件!'.t());
            return;
        }
        this.doSubmit({
            Name: view.getSourceCmd().command,
            Token: view.getToken(),
            Data: SIE.data.Utils.seriaizeRequest({
                Data: view.getCurrent().getAttachmentId()
            })
        });
    }
});