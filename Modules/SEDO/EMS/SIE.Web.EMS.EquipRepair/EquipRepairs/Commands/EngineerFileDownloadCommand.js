SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.EngineerFileDownloadCommand', {
    extend: 'SIE.cmd.ExportCommandBase',
    meta: { text: "下载", group: "edit", group: "edit", iconCls: "iconfont icon-Download icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        if (view.getCurrent() == null || view.getCurrent().getEngineerAttachment() == "") return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var item = view.getCurrent();

        this.doSubmit({
            Name: view.getSourceCmd().command,
            Token: view.getToken(),
            Data: SIE.data.Utils.seriaizeRequest({
                Data: SIE.data.Utils.seriaizeRequest({
                    FilePath: item.data.EngineerAttachment
                })
            })
        });
    }
});

