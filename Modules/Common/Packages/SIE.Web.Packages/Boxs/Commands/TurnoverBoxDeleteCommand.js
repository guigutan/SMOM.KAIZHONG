SIE.defineCommand('SIE.Web.Packages.Boxs.Commands.TurnoverBoxDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        var sel = view.getSelection();
        for (i = 0; i < sel.length; i++) {
            var item = sel[i].data;
            if (item.State == 0 || item.State == 2) {
                return false;
                break;
            }
        }

        return true;
    },
});