SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.UpgradeAccountCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "升级", group: "business", iconCls: "icon-Upload icon-green" },

    canExecute: function (view) {
        var account = view.getCurrent();
        if (account == null || account.data.TreePId == null) {
            return false;
        }

        var accountList = view.getSelection();
        if (accountList == null || accountList.length > 1) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {
        var data = view.getCurrent().data;
        SIE.Msg.askQuestion(Ext.String.format('确定[升级]设备台账[{0}]?'.t(), data.Code), function () {
            view.execute({
                data: data.Id,
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) { //回调
                    view.reloadData();
                }
            });
        });
    }
});