SIE.defineCommand('SIE.Web.ProductIntfc.OutputProducts.Commands.DeleteOutPutProductCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {

        if (view.hasSelectedEntities() && view.getSelection().length > 0) {
            var flag = view.getSelection().all(function (c) { return c.getInStorageState() != 20;});
            return flag;
        }
        return false;
    },
});