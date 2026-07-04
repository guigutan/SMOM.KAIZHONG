SIE.defineCommand('SIE.Web.MES.WorkOrders.WorkOrderResumeCommand', {
    meta: { text: "恢复", group: "edit", hierarchy: "状态", iconCls: "icon-Reload icon-blue" },
    canExecute: function (listView) {
        var current = listView.getCurrent();       
        if (current == null || current.data.IsPause == 0 || current.data.State == 4)
            return false;
        else return current.data.State == 0 || current.data.State == 1;
    },
    execute: function (listView, source) {
        var me = this;
        if (!this.viewMeta) {
            SIE.Web.MES.WoCommonFun.showStateChangeView(listView, "恢复工单");
        }
    } 
});
