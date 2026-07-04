SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemProDefinSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        var isCatalog = false;
        var current = view.getCurrent();
        if (current == null) return false;
        var store = view.getData();
        var index = store.data.items.indexOf(current);
        if (current.data.PropertyType !== 0 && current.data.CatalogTypeId !== null) {
            current.data.CatalogTypeId = null;
            current.data.CatalogTypeId_Display = "";
            isCatalog = true;
        }
        if (isCatalog == true) {
            store.insert(index, current); 
        }
        return view.getData().isDirty();
    },
});
