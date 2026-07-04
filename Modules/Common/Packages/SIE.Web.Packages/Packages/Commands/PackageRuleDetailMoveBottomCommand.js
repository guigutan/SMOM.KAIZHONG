SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleDetailMoveBottomCommand', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveBottomCommand',
    meta: { text: "置底", group: "business", iconCls: "iconfont icon-AlignBottom icon-blue" },

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
        }
        //最后一行数据屏蔽置底按钮
        var items = store.data.items;
        var item = view.getCurrent();
        if (this.getItemsIndex(items, item) == (items.length - 1)) {
            return false;
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
        items.push(entity);
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