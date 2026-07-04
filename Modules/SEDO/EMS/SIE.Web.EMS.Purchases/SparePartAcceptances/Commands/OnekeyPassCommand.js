SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.OnekeyPassCommand', {
    meta: { text: "一键验收", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var parent = view._parent.getCurrent();
        if (parent == null || parent.data == null) {
            return false;
        }
        if (parent.data.ApprovalStatus !== 10 && parent.data.ApprovalStatus !== 50) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        let store = view.getData();
        SIE.each(store, function (entity) {
            if (entity.getAcceptanceResult() == null) {
                entity.setAcceptanceResult(1);
                if (view.model === "SIE.EMS.Purchases.SparePartAcceptances.SparePartAcceptanceLot") {
                    entity.setPassQty(entity.getQty());
                    entity.setUnqualifiedQty(0);
                }
            }
        });
        view.setData(store);
        view.syncCmdState();
    }
});