SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.ImportTaskFixtureCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Import icon-blue" },
    canExecute: function (view) {
        if (view.getParent() == null)
            return false;
        var cur = view.getParent().getCurrent();
        if (cur == null)
            return false;
        //盘点状态为【盘点中、初盘完成、复盘中】才允许导入
        if (cur.data.InventoryTaskStatus !== 20 && cur.data.InventoryTaskStatus !== 30 && cur.data.InventoryTaskStatus !== 40)
            return false;
        return true;
    }
});