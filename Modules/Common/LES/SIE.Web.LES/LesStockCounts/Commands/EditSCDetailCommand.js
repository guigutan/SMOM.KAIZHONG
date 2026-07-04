SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.EditSCDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) { return false; }
        var parentCur = view.getParent().getCurrent();
        if (parentCur == null) return false;
        if (parentCur != null) {
            //SIE.Warehouses.CountState.Audit.value(审批)=10
            //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
            //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
            if (parentCur.data.State != 10 &&
                parentCur.data.State != 30 &&
                parentCur.data.State != 40)
                return false;
        }
        return true;
    },
});