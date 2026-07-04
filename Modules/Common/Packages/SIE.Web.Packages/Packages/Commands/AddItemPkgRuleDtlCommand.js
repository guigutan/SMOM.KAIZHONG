SIE.defineCommand('SIE.Web.Packages.Packages.Commands.AddItemPkgRuleDtlCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },

    onItemCreated: function (entity) {
        var me = this;
        var maxIndex = -1;
        var store = me.view.getData();
        for (var i = 0; i < store.data.length; i++) {
            store.data.items[i].dirty = true;
            var index = store.data.items[i].getINDEX_();
            if (maxIndex < index)
                maxIndex = index;
            if (i == 0) continue;
            if (store.data.items[i].getLevelQty() == null || store.data.items[i].getLevelQty() == 0)
                store.data.items[i].setLevelQty(1);
            store.data.items[i].setQty(store.data.items[i - 1].getQty() * store.data.items[i].getLevelQty());
        }

        entity.setINDEX_(maxIndex + 1);
    }
});