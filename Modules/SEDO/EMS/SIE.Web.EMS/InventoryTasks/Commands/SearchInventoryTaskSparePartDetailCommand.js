SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.SearchInventoryTaskSparePartDetailCommand', {
    meta: { text: "查找", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (view) {
        if (view.getParent()) {
            var parent = view.getParent().getCurrent();

            var txtSparePartDetailKeyWord = view.getControl().dockedItems.items
                .first(function (p) { return p.xtype == "toolbar" })
                .items.items.first(function (p) { return p.name == "txtSparePartDetailKeyWord" });

            if (parent && txtSparePartDetailKeyWord) {
                parent.setSparePartDetailKeyWord(txtSparePartDetailKeyWord.value.trim());
            }
            view.loadChildData(true);
        }
    }
})