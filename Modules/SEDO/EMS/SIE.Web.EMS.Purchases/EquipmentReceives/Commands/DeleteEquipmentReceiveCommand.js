SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentReceives.Commands.DeleteEquipmentReceiveCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            //改成待接收时，才能删除
            if (model.data.ReceiveBillStatus !== 0) {
                res = false;
                return false;
            }
        });
        return res;
    }
});