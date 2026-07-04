SIE.defineCommand('SIE.Web.MES.WorkOrders.WorkOrderCloseCommand', {
    meta: { text: "强制关闭", group: "edit", hierarchy: "状态", iconCls: "icon-NetworkError icon-blue" },
    canExecute: function (listView) {
        var current = listView.getCurrent();      
        if (current == null || current.data.State == 3 || current.data.State == 4)
            return false;
        else return current.data.IsPause == 1 || current.data.State == 2;
    },
    execute: function (listView, source) {
        var me = this;
        if (!this.viewMeta) {
            SIE.Web.MES.WoCommonFun.showStateChangeView(listView, "关闭工单".t());
        }
    }   
});
