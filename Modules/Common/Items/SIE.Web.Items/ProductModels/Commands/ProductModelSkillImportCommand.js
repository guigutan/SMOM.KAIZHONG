SIE.defineCommand('SIE.Web.Items.ProductModels.Commands.ProductModelSkillImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        if (view.getParent() == null || view.getParent().getCurrent() == null) return false;
        return true;
    }
});