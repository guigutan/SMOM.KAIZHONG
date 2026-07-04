SIE.defineCommand('SIE.Web.EMS.Equipments.Boms.Commands.UpgradeSparePartCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "升级", group: "business", iconCls: "icon-Upload icon-green" },
    canExecute: function (view) {
        var account = view.getCurrent();
        if (account == null || account.data.TreePId == null || account.data.TreePId == 0 || account.data.CreateDate == null) {
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
        SIE.Msg.askQuestion('确定升级该备件吗?'.t(), function () {
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