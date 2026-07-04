SIE.defineCommand('SIE.Web.MES.WorkOrders.WorkOrderPauseCommand', {
    meta: { text: "暂停", group: "edit", hierarchy: "状态", iconCls: "icon-Delete icon-blue" },
    canExecute: function (listView) {
        var current = listView.getCurrent();
        if (current == null || current.data.IsPause == 1 || current.data.State == 4)
            return false;
        else return current.data.State == 0 || current.data.State == 1;
    },
    execute: function (listView, source) {
        var me = this;
        if (!this.viewMeta) {
            SIE.Web.MES.WoCommonFun.showStateChangeView(listView, "暂停工单".t());
        }
    },
});