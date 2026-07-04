SIE.defineCommand('SIE.Web.EMS.DevicePurs.Commands.EquipSearchRepairCommand', {
    meta: { text: "查找", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (view) {
        if (view.getParent()) {
            var parent = view.getParent().getCurrent();
            var txtKeyword = view.getControl().dockedItems.items.first(function (p) { return p.xtype == "toolbar" }).items.items.first(function (p) { return p.name == "txtKeyword" });
            if (parent && txtKeyword)
                parent.setSearchKeywordDontMap(txtKeyword.value.trim());
            view.loadChildData(true);
        }
    }
})