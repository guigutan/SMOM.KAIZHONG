SIE.defineCommand('SIE.Web.MES.ItemEquipAccount.Commands.EquipAccountItemEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.State == 1)
                res = false;
        });
        return res;
    }
});
