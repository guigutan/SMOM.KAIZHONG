SIE.defineCommand('SIE.Web.Inventory.Transactions.Commands.SetTransactionIsUploadCommand', {
    meta: { text: "启用/禁止整单上传", group: "business", iconCls: "icon-NetworkNormal icon-green" },

    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }

        for (var i = 0; i < sel.length; i++) {
            if (sel[i].data.State != SIE.Common.SourceType.External.value) return false;
        }

        return true;
    },
    execute: function (view, source) {
        view.execute({
            data: view.getCurrent().data,
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) { //回调
                view.reloadData();
            }
        });
    }
});