SIE.defineCommand('SIE.Web.MES.Routings.RoutingBoms.Commands.ImportRoutingBomCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        //if (view.getCurrent() == null) return false;
        return true;
    }
});