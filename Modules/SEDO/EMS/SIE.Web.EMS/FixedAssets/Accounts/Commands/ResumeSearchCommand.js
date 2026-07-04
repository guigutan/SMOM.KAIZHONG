SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.EquipResumeSearchCommand', {
    meta: { text: "查找", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (view) {
        if (view.getParent()) {
            var parent = view.getParent().getCurrent();
            var cbResumeState = view.getControl().dockedItems.items.first(function (p) { return p.xtype == "toolbar" }).items.items.first(function (p) { return p.name == "cbResumeState" });
            if (parent && cbResumeState)
                parent.setResumeState(cbResumeState.getValue());
            view.loadChildData(true);
        }
    }
})