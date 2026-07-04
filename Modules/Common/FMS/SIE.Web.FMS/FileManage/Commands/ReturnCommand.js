SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.ReturnCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "撤回", group: "edit", iconCls: "icon-Reload icon-blue" },
    canExecute: function (view) {
        var gridControl = Ext.getCmp("fileManage-id");
        if (!gridControl) return false;
        var items = gridControl.getSelectionModel().getSelection();
        if (items.length !== 1) return false;
        if (!items[0].data.IsFile) return false;
        var FileStateEnum = new SIE.Enum.FMS();
        if (items[0].data.FileState !== FileStateEnum.FileState.Audit && items[0].data.FileState !== FileStateEnum.FileState.ToRelease) return false;
        return true;
    },
    execute: function (view) {
        var gridControl = Ext.getCmp("fileManage-id");
        if (gridControl) {
            var curfolderId = gridControl.SieView.CurFolderId;
            var items = gridControl.getSelectionModel().getSelection();
            SIE.Msg.askQuestion('确定撤回审批流程？'.t(), function () {
                var fileIds = items.select(function (p) { return p.data.FId; });
                view.execute({
                    data: fileIds,
                    success: function (res) {
                        if (res.Result) {
                            SIE.Msg.showInstantMessage('操作成功！'.t(), "提示".t(), 2);
                            SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(curfolderId);
                        }
                    },
                    error: function (res) {
                        SIE.Web.FMS.FileManages.CommonFunctions.SetGridStore(curfolderId);
                    }
                });
            });
        }
    }
});