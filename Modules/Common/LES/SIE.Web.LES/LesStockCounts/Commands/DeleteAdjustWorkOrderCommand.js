SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.DeleteAdjustWorkOrderCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getIsAuto() == true) {
                    flag = false;
                }
            });
            return flag;
        }
        return true;
    },
    execute: function (view) {
        Ext.each(view.getSelection(), function (item) {
            view.getParent().childDatas.remove(item.data);            
        });
        view.removeSelection();
    }
});