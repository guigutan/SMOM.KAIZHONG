SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureAcceptances.Commands.OnekeyPassCommand', {
    meta: { text: "一键验收", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        let store = view.getData();
        SIE.each(store, function (entity) {
            if (entity.getInspectionResult() == null || entity.getInspectionResult()==0)
                entity.setInspectionResult(1);
        });
        view.setData(store);
    }
});