SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.ScarpCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "作废", group: "edit", iconCls: "icon-CalendarRemove icon-red" },
    canExecute: function (view) {
        var gridControl = Ext.getCmp("fileManage-id");
        if (!gridControl) return false;
        var items = gridControl.getSelectionModel().getSelection();
        if (items.length !== 1) return false;
        if (!items[0].data.IsFile) return false;
        var FileStateEnum = new SIE.Enum.FMS();
        if (items[0].data.FileState !== FileStateEnum.FileState.Release && items[0].data.FileState !== FileStateEnum.FileState.Edit) return false;
        return true;
    },
    execute: function (view) {
        SIE.Web.FMS.FileManages.CommonFunctions.ScarpFiles();
    }
});