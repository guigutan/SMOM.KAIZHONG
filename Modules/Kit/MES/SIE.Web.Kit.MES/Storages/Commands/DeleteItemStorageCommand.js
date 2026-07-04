SIE.defineCommand('SIE.Web.Kit.MES.Storages.Commands.DeleteItemStorageCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    extend: 'SIE.cmd.Delete',
    canExecute: function (view) {
        var models = view.getSelection();
        if (models == null || models.length == 0)
            return false;
        for (var i = 0; i < models.length; i++) {
            if (models[i].getQty() > 0)
                return false;
        }
        return true;
    }
});