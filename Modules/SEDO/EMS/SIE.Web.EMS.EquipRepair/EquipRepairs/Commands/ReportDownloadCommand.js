SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ReportDownloadCommand', {
   // extend: 'SIE.cmd.ExportCommandBase',
    extend: "SIE.Web.Common.Attachments.Commands.FtpDownloadCommand",
    meta: { text: "下载维修报告", group: "edit", group: "edit", iconCls: "iconfont icon-Download icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity)
            return !Ext.isEmpty(entity.data.OutsourcedMaintenanceReport);
        else
            return false;
    },
    execute: function (listView, source) {
        var item = listView.getCurrent();
        var fileName = item.getOutsourcedMaintenanceReport().split('/')[item.getOutsourcedMaintenanceReport().split('/').length - 1];
        var filePath = item.getOutsourcedMaintenanceReport();
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
    /*execute: function (view, source) {
        var me = this;
        var item = view.getCurrent();
        console.log(item);
        this.doSubmit({
            Name: view.getSourceCmd().command,
            Token: view.getToken(),
            Data: SIE.data.Utils.seriaizeRequest({
                Data: SIE.data.Utils.seriaizeRequest({
                    FilePath: item.data.OutsourcedMaintenanceReport
                })
            })
        });
    }
});
*/
