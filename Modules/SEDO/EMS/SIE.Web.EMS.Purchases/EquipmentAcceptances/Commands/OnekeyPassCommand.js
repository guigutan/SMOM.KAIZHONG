SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.OnekeyPassCommand', {
    meta: { text: "一键验收", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        let store = view.getData();
        SIE.each(store, function (entity) {
            if (entity.getAcceptanceStatus() == null)
                entity.setAcceptanceStatus(1);
        });
        view.setData(store);
    }
});