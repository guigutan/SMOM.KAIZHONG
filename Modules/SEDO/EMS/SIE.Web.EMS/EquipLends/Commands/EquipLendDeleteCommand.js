SIE.defineCommand("SIE.Web.EMS.EquipLends.Commands.EquipLendDeleteCommand", {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        var flag = true;
        var sel = view.getSelection();
        for (i = 0; i < sel.length; i++) {
            var item = sel[i].data;
            if (item.LendState != 0) {
                flag = false;
                break;
            }
        }

        return flag;
    },
})
