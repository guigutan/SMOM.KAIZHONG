SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentReceives.Commands.ReceiveScanCommand', {
    meta: { text: "接收", group: "edit", iconCls: "icon-Import icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;

        //改成待接收和待提交时都可以接收
        if (p.data.ReceiveBillStatus !== 0 && p.data.ReceiveBillStatus !== 10) return false;
        return true;
    },
    execute: function (view, source) {
        var entity = view.getCurrent();
        CRT.Workbench.addPage({
            tabId: "menu_EquipmentReceivesScan",
            entityType: 'SIE.EMS.Purchases.EquipmentReceives.ReceiveScanViewModel',
            title: "设备-接收".t(),
            isDetail: true,
            isNew: true,
            module: view.module,
            params: {
                ReceiveId: entity.data.Id
            }
        });
    }
});