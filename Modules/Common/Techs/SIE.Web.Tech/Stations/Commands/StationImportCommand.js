SIE.defineCommand('SIE.Web.Tech.Stations.Commands.StationImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-green" },
    canExecute: function (view) {        
        return true;
    }
});