SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.ReceiveScanCommand', {
    meta: { text: "接收", group: "edit", iconCls: "icon-Import icon-blue" },
    extend: 'SIE.cmd.Edit',
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
            tabId: "menu_FixtureReceivesScan",
            entityType: 'SIE.EMS.Purchases.FixtureReceives.ReceiveScanViewModel',
            title: "工治具接收-接收".t(),
            isDetail: true,
            isNew: false,
            module: view.module,
            params: {
                ReceiveId: entity.data.Id
            }
        });
    }
});