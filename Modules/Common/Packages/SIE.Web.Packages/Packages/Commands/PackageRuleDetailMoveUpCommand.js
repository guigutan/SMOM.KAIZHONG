SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleDetailMoveUpCommand', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveUpCommand',
    meta: { text: "上移", group: "edit", iconCls: "icon-ArrowLongUp icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getCurrent() == null || view.getSelection().length == 0) {
            return false;
        }
        var store = view.getData();
        var sel = view.getSelection();
        for (i = 0; i < sel.length; i++) {
            var item = sel[i].data;
            if (item.IsMasterUnit == true) {
                return false;
                break;
            } 
            if (store.data.indexOf(item) <= 1) {
                return false;
                break;
            }
        }
        return true;

    },

    execute: function (view, source) {
        //***************************************************************************************************
        // 先用父类方法
        //***************************************************************************************************
        var me = view;
        var data = view.getData();
        var items = data.data.items;
        var entity = view.getCurrent();
        var index = this.getItemsIndex(items, entity);
        items.splice(index, 1);
        items.splice(index - 1, 0, entity);
        this.setItems(data, items);
        for (var i = 0; i < items.length; i++) {
            items[i].dirty = true;
            items[i].data.INDEX_ = i;
        }
        view.setCurrent(null);
        view.setCurrent(entity);
        //***************************************************************************************************

        var sel = view.getSelection();
        var store = view.getData();
        for (var i = 0; i < store.data.length; i++) {
            if (i == 0) continue;
            //store.data.items[i].setPackageRule(me.view.getParent().getCurrent());
            //store.data.items[i].setPackageRuleId(me.view.getParent().getCurrent().getId());
            if (store.data.items[i].getLevelQty() == null || store.data.items[i].getLevelQty() == 0)
                store.data.items[i].setLevelQty(1);
            store.data.items[i].setQty(store.data.items[i - 1].getQty() * store.data.items[i].getLevelQty());
        }
    }
});