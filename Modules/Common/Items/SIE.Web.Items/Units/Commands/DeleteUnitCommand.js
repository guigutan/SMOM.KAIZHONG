SIE.defineCommand('SIE.Web.Items.Units.Commands.DeleteUnitCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (!item.isNew() && item.getUnitSource() == 1)
                    flag = false;
            });
            return flag;
        }
    }
});