SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.FlowCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "审批流", group: "edit", iconCls: "icon-ArrowLeftRight icon-blue" },
    canExecute: function (view) {
        //未选择行
        var gridControl = Ext.getCmp("fileManage-id");
        if (!gridControl) return false;
        var items=gridControl.getSelectionModel().getSelection();
        if (items.length !== 1) return false;
        if (!items[0].data.IsFile) return false;
        var FileStateEnum = new SIE.Enum.FMS();
        if (items[0].data.FileState !== FileStateEnum.FileState.Created && items[0].data.FileState !== FileStateEnum.FileState.Edit) return false;
        return true;
    },
    execute: function (view) {
        SIE.Web.FMS.FileManages.CommonFunctions.StartFlow();
    }
});