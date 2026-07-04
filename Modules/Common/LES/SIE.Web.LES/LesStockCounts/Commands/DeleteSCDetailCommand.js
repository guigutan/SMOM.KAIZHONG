SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.DeleteSCDetailCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            var curPar = view.getParent().getCurrent();
            if (curPar == null) return false;
            //SIE.Warehouses.CountState.Finished.value(完工)=50
            if (curPar.getState() == 50) {
                return false;
            }

            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getIsNewInventory() != true) {
                    flag = false;
                }
            });
            return flag;
        }
        return true;
    }
});