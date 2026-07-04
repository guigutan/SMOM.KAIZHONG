SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.SparePartStoreDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getStoreStatus() != 0)
                    flag = false;
            });
            return flag;
        }
    }
});