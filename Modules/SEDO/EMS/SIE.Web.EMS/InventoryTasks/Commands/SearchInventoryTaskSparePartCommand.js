SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.SearchInventoryTaskSparePartCommand', {
    meta: { text: "查找", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (view) {
        if (view.getParent()) {
            var parent = view.getParent().getCurrent();

            var txtSparePartCode = view.getControl().dockedItems.items
                .first(function (p) { return p.xtype == "toolbar" })
                .items.items.first(function (p) { return p.name == "txtSparePartCode" });

            if (parent && txtSparePartCode) {
                parent.setSparePartCode(txtSparePartCode.value.trim());
            }
            view.loadChildData(true);
        }
    }
})