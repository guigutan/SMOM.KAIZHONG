SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.ImportInspectionCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Import icon-blue" },
    canExecute: function (view) {
        return true;
    }
});