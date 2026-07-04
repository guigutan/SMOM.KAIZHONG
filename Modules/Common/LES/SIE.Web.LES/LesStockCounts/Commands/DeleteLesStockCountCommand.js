SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.DeleteLesStockCountCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            for (i = 0; i < view.getSelectedEntities().length; i++) {
                var cur = view.getSelectedEntities()[i].data;
                //SIE.Warehouses.CountState.Create.value(创建)=0
                if (cur.State != 0) {
                    return false;
                }
            }
        }
        return true;
    }
});