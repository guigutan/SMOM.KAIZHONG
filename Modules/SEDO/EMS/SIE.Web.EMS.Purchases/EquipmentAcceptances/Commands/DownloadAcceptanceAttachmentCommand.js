SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.DownloadAcceptanceAttachmentCommand', {
    extend: 'SIE.cmd.ExportCommandBase',
    meta: { text: "下载", group: "edit", iconCls: "iconfont icon-Download icon-blue" },
    selectedItems: [],
    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        if (view.getCurrent() == null) {
            return false;
        }

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
                    FilePath: item.data.FilePath,
                    SelectedIds: view.getSelectionIds(me.selectedItems)[0]
                })
            })
        });

        view.reloadData();
    }
});