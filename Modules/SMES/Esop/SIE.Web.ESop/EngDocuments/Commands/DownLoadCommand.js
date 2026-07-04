SIE.defineCommand('SIE.Web.ESop.EngDocuments.Commands.DownLoadCommand', {
    extend: 'SIE.cmd.ExportCommandBase',
    meta: { text: "下载", group: "edit", iconCls: "icon-FileTree icon-blue" },
    canExecute: function (view) {
        var current = view.getSelection();
        if (current === null || current.length === 0) {
            return false;
        }
        var hasFid = current.every(item => {
            return item.getFId() !== null;
        });
        if (!hasFid) {
            return false;
        }
        return true;
    },
    execute: function (ListView) {
        var me = this;
        var current = ListView.getSelection();
        if (current.length > 0) {
            var fileIds = current.select(function (p) { return p.getFId(); });
            SIE.invokeDataQuery({
                method: 'DownLoadFiles',
                params: [fileIds],
                action: 'queryer',
                type: 'SIE.Web.FMS.FileManageDataQueryer',
                token: ListView.token,
                success: function (res) {
                    var data = res.Result;
                    if (data == "") {
                        current.forEach(function (p) {
                            SIE.Web.FMS.FileManages.CommonFunctions.DownLoadSubmit({
                                Name: 'SIE.Web.Common.Attachments.Commands.FtpDownloadCommand',
                                Token: ListView.getToken(),
                                Data: SIE.data.Utils.seriaizeRequest({
                                    Data: SIE.data.Utils.seriaizeRequest({
                                        FileName: p.data.ServerFileName,
                                        FilePath: p.data.SavePath,
                                    })
                                })
                            });
                        });
                    }
                    else {
                        SIE.Msg.showError(data);
                    }
                }
            });

        }
        else
            SIE.Msg.showWarning('请选择文件！'.t());
        
    }
});
