SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageAttachmentCommand', {
    extend:"SIE.Web.Common.Attachments.Commands.FtpDownloadCommand",
    meta: { text: "查看附件", group: "edit", iconCls: "icon-OpenFile icon-blue" },
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current == null || current.getAttachment() == '' || current.getAttachment() == null) {
            return false;
        }
        return true;
    },
    execute: function (listView, source) {
        var item = listView.getCurrent();
        var fileName = item.getAttachment().split('/')[item.getAttachment().split('/').length - 1];
        var filePath = item.getAttachment();
        var me = this;
        SIE.Signature.otherCheckIsNeedToSign("下载", listView, function () {
            me.doSubmit({
                Name: listView.getSourceCmd().command,
                Token: listView.getToken(),
                Data: SIE.data.Utils.seriaizeRequest({
                    Data: SIE.data.Utils.seriaizeRequest({
                        FileName: fileName,
                        FilePath: filePath,
                    })
                })
            });
        });
    }
});
