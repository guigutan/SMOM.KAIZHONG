SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleDetailAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    //canExecute: function (view) {
    //    var par = view.getParent();
    //    if (par&&par.getCurrent()) {
    //        return !par.getCurrent().isDirty();//增加控制，防止空数据也能添加明细
    //    }
    //    return false;
    //},
    onItemCreated: function (entity) {
        var me = this;
        var store = me.view.getData();
        for (var i = 0; i < store.data.length; i++) {
            store.data.items[i].dirty = true;
            if (i == 0) continue;
            if (store.data.items[i].getLevelQty() == null || store.data.items[i].getLevelQty() == 0)
                store.data.items[i].setLevelQty(1);
            store.data.items[i].setQty(store.data.items[i - 1].getQty() * store.data.items[i].getLevelQty());
        }
        if (store.data.length > 1) {
            var index = store.data.items[store.data.length - 2].getINDEX_() + 1;
            if (index == 0) //如果为0，保存到数据库有bug
                index = 1;
            entity.setINDEX_(index);//添加的时候默认index设置+1
        }
        else {
            entity.setINDEX_(1);
        }
    }
});