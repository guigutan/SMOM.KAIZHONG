SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.HistoryOrderCommand', {
    meta: { text: "历史订单查询", group: "edit", iconCls: "icon-Search icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        return true;
    },
    execute: function (view, source) {
        var editEntity = view.getCurrent();
        var objectType = editEntity.getPurchaseObjectType();
        var objectCode = editEntity.getObjectCode();
        var objectName = editEntity.getDescription();
        var supplierId = editEntity.getSupplierId();
        var supplierIdDisplay = editEntity.getSupplierId_Display();
        var supplierName = editEntity.getSupplierName();
        var tabId = ('HistoryOrder_' + editEntity.getId());
        CRT.Workbench.addPage({
            title: "历史订单查询".t(),
            tabId: tabId,
            entityType: 'SIE.EMS.Purchases.PurchaseOrders.HistoryOrderViewModel',
            isAggt: true,
            params: {
                tabId: tabId,
                PurchaseObjectType: objectType,
                OldObjectCode: objectCode,
                OldObjectName: objectName,
                OldSupplierId: supplierId,
                OldSupplierIdDisplay: supplierIdDisplay,
                OldSupplierName: supplierName
            }
        });
    }
});