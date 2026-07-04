SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.DefaultSCDetailCommand', {
    meta: { text: "默认实盘", group: "edit", iconCls: "icon-Checkmark icon-green"},
    extend: 'SIE.cmd.ListEditableBase',
    canExecute: function (view) {
        var p = view.getParent();
        if (p == null) return false;
        var curPar = p.getCurrent();
        if (curPar == null) return false;
        //SIE.Warehouses.CountState.Audit.value(审批)=10
        //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
        if (curPar.getState() != 10 &&
            curPar.getState() != 30) {
            return false;
        }
        if (view.hasSelectedEntities()) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.data.State != 10 &&
                    item.data.State != 30) {
                    flag = false;
                }
            });
            return flag;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        if (view.hasSelectedEntities() && view.getSelection().length > 1) {
            Ext.each(view.getSelection(), function (item) {
                if (item.data.State == 10 && item.data.ActualCountQty == null) {
                    item.setActualCountQty(item.data.Qty);
                    item.setDiffCountQty(0);
                    var employee = CRT.Context.GlobalContext.getContext('userInfo');
                    item.setCountById_Display(employee.Name);
                    item.setCountById(employee.EmployeeId);
                    item.setCountDate(new Date());
                }
            });
        }
        else {
            Ext.each(view.getData().data.items, function (item) {
                if (item.data.State == 10 && item.data.ActualCountQty == null) {
                    item.setActualCountQty(item.data.Qty);
                    item.setDiffCountQty(0);
                    var employee = CRT.Context.GlobalContext.getContext('userInfo');
                    item.setCountById_Display(employee.Name);
                    item.setCountById(employee.EmployeeId);
                    item.setCountDate(new Date());
                }
            });
        }
        me.view.syncCmdState();
    }
});