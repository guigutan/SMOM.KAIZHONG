SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.DeleteCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "删除", group: "edit", iconCls: "icon-PageDelete icon-red" },
    canExecute: function (view) {
        var gridControl = Ext.getCmp("fileManage-id");
        if (!gridControl) return false;
        var items = gridControl.getSelectionModel().getSelection();
        if (items.length <= 0) return false;
        var FileStateEnum = new SIE.Enum.FMS();
        if (items.any(function (p) { return p.data.IsFile && p.data.FileState !== FileStateEnum.FileState.Created }))
            return false;
        return true;
    },
    execute: function (view) {
        SIE.Web.FMS.FileManages.CommonFunctions.DeleteFoldersAndFiles();
    }
});