SIE.defineCommand('SIE.Web.MES.WorkOrders.WorkOrderProcessBomPropertyEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit" },
    canExecute: function (listView) {
        if (listView == null || listView.getCurrent() == null || listView.getSelection().length > 1) return false;
        return true;
    }
})