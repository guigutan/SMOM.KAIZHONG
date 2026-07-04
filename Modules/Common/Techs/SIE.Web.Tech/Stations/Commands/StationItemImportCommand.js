SIE.defineCommand('SIE.Web.Tech.Stations.Commands.StationItemImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-green" },
    canExecute: function (view) {
        if (view.getParent() == null || view.getParent().getCurrent() == null) return false;
        return true;
    }
});