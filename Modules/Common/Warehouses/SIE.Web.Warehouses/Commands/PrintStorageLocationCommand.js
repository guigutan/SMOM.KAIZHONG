SIE.defineCommand('SIE.Web.Warehouses.Commands.PrintStorageLocationCommand', {
    meta: { text: "打印", group: "edit", iconCls: "iconfont icon-PrintData icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        } else {
            if (view.getData().isDirty() == true) {
                return false
            }
        }
        return true;
    },
    execute: function (view, source) {
        view.execute({
            data: 0,
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (r) {
                SIE.Web.Core.CommonFuns.ShowPrintPreview(r);
            }
        });
    }
});