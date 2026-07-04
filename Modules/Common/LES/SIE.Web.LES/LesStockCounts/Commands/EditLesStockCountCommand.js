SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.EditLesStockCountCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) { return false; }
        //SIE.Warehouses.CountState.Finished.value(完工)=50
        //SIE.WMS.INV.Count.CountState.Close.value(关闭)=60
        if (p.getState() == 50 || p.getState() == 60) {
            return false;
        }
        return true;
    },
});