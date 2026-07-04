SIE.defineCommand('SIE.Web.MES.WorkOrders.WorkOrderAdvanceCommand', {
    meta: { text: "手工提前完工", group: "edit", hierarchy: "状态", iconCls: "icon-Reload icon-blue" },
    canExecute: function (listView) {
        var current = listView.getCurrent();
        if (current == null || (current.data.IsPause != 0 || current.data.State != 1))
            return false;
        else return current.data.State == 0 || current.data.State == 1;
    },
    execute: function (listView, source) {
        var me = this;
        if (!this.viewMeta) {
            SIE.Web.MES.WoCommonFun.showStateChangeView(listView, "手工提前完工".t());
        }
    }
});
