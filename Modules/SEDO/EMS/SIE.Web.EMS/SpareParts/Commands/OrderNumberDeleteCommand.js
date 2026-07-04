SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.OrderNumberDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            var flag = true;

            var storeStatus = view._parent._parent._selection[0].data.StoreStatus
            Ext.each(view.getSelection(), function (item) {
                if (storeStatus != 0)
                    flag = false;
            });
            return flag;
        }
    }
});