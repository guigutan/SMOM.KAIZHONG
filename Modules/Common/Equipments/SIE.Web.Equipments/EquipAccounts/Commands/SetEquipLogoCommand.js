SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.SetEquipLogoCommand', {
    meta: { text: "设为LOGO", group: "edit", iconCls: "icon-CalendarCheck icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        return  (selectModels.length != 0);
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        if (selectIds.length > 1) {
            SIE.Msg.showMessage("只允许设置一条记录为LOGO!".t());
            return false;
        }
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("设置成功!".t());
                view.reloadData();
                
            }
        });
    }
});