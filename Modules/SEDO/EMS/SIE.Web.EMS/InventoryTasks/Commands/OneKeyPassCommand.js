SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.OneKeyPassCommand', {
    meta: { text: "一键盘点", group: "business", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var currentData = view.getParent().getCurrent();
        if (currentData == null) return false;
        var res = true;
        if (currentData.data.InventoryTaskStatus !== 20) {
            res = false;
            return false;
        }
        return res;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion('确定将未盘点的数据全部录入盘点正常？'.L10N(), function () {
            var selectId = view.getParent().getCurrent().getId();
            view.execute({
                withIds: true,
                selectIds: [selectId],
                success: function (res) {
                    SIE.Msg.showMessage("一键盘点完成!".t());
                    view.getParent().reloadData();
                }
            });
        });
    }
});