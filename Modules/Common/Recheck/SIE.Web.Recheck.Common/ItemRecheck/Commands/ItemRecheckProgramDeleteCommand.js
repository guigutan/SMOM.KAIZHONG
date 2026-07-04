SIE.defineCommand('SIE.Web.Recheck.Common.ItemRecheck.Commands.ItemRecheckProgramDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel.length == 0) return false;
        for (var i = 0; i < sel.length; i++) {
            var item = sel[i].data;
            if (item.State != 0)
                return false;
        }
        return true;
    }
});