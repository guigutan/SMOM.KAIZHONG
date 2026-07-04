SIE.defineCommand('SIE.Web.Packages.Packages.Commands.MoveTopItemPkgRuleDtlCommand', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveTopCommand',
    meta: { text: "置顶", group: "business", iconCls: "iconfont icon-AlignTop icon-blue" },
    canExecute: function (listView) {
        if (listView.getCurrent()) {
            var master = listView.getCurrent().getIsMasterUnit();
            if (master)
                return false;          
            if (listView.getData().data.items[0].getIsMasterUnit())
                return false;
        }
        return this.callParent(arguments);
    },
    execute: function (listView, source) {
        this.callParent(arguments);
        var store = listView.getData();
        for (var i = 0; i < store.data.length; i++) {
            if (i == 0) continue;
            if (store.data.items[i].getLevelQty() == null || store.data.items[i].getLevelQty() == 0)
                store.data.items[i].setLevelQty(1);
            store.data.items[i].setQty(store.data.items[i - 1].getQty() * store.data.items[i].getLevelQty());
        }

        if (listView.getCurrent() != null && (listView.getCurrent().getIsMasterUnit() || listView.getCurrent().getINDEX_() == 1)) {
            listView.reloadData();
        }
    },
});