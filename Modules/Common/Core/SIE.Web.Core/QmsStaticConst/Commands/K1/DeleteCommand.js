SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.K1.DeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.hasSelectedEntities()) {
            var defaultRowArray = [];//2-3为默认行，不能删除
            for (var i = 2; i <= 3; i++) {
                defaultRowArray.push(i);
            }

            var rowArray = view.getSelection().select(function (c) { return c.data.TestQty; });
            for (var i = 0; i < rowArray.length; i++) {
                if (Ext.Array.contains(defaultRowArray, rowArray[i])) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
});