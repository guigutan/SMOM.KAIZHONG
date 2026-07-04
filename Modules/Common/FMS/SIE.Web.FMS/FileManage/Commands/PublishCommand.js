SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.PublishCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "发布", group: "edit", iconCls: "icon-Submit icon-blue" },
    canExecute: function (view) {
        var gridControl = Ext.getCmp("fileManage-id");
        if (!gridControl) return false;
        var items = gridControl.getSelectionModel().getSelection();
        if (items.length <1) return false;
        return true;
    },
    execute: function (view) { 
        SIE.Web.FMS.FileManages.CommonFunctions.PublishFiles();
    }
});