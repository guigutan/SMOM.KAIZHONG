SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.ReceiveScanCommand', {
    meta: { text: "接收", group: "edit", iconCls: "icon-Import icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.ReceiveBillStatus !== 10) return false;
        return true;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();
        CRT.Workbench.addPage({
            tabId: "menu_SparePartReceiveScan",
            entityType: 'SIE.EMS.Purchases.SparePartReceives.ViewModels.ReceiveScanViewModel',
            title: "备件-接收".t(),
            isDetail: true,
            isNew: true,
            module: view.module,
            params: {
                ReceiveId: entity.data.Id
            }
        });
    }
});