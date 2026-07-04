SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.FixtureInvSearchCommand', {
    meta: { text: "查询", group: "business", iconCls: "icon-Search icon-blue" },
    execute: function (view) {
        if (view.getParent()) {
            var parent = view.getParent().getCurrent();
            var inputCode = view.getControl().dockedItems.items.first(function (p) {
                return p.xtype == "toolbar"
            }).items.items.first(function (p) {
                return p.name == "inputCode"
            });
            var cbResumeState = view.getControl().dockedItems.items.first(function (p) {
                return p.xtype == "toolbar"
            }).items.items.first(function (p) {
                return p.name == "cbResumeState"
            });
            if (parent && cbResumeState) {
                parent.setInventorySeachResult(cbResumeState.getValue());
            }
            if (parent && inputCode) {
                parent.setFixtureCodeSnNotMap(inputCode.getValue());
            }
            view.loadChildData(true);
        }
    }
})