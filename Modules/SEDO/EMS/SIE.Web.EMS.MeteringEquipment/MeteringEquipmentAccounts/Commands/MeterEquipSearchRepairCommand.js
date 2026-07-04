SIE.defineCommand('SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands.MeterEquipSearchRepairCommand', {
    meta: { text: "查找", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (view) {
        if (view.getParent()) {
            var parent = view.getParent().getCurrent();
            var cbbState = view.getControl().dockedItems.items.first(function (p) { return p.xtype == "toolbar" }).items.items.first(function (p) { return p.name == "cbbState" });
            if (parent && cbbState)
                parent.setMeteringRepairStateDontMap(cbbState.getValue());
            view.loadChildData(true);
        }
    }
})