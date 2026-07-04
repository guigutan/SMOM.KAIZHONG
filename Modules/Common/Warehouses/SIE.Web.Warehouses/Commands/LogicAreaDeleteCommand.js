SIE.defineCommand('SIE.Web.Warehouses.Commands.LogicAreaDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (!item.isNew() && item.getState() != SIE.Domain.State.Disable.value)
                    flag = false;
            });
            return flag;
        }
    }
});