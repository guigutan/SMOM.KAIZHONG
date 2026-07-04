SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleDetailDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getCurrent() == null || view.getSelection().length == 0) {
            return false;
        }
        var sel = view.getSelection();
        for (i = 0; i < sel.length; i++) {
            var item = sel[i].data;
            if (item.IsMasterUnit == true) {
                return false;
                break;
            }  
        }
        return true;

    },

    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？确认后直接删除'.t(), sel.length), function () {
            view.removeSelection();
            view.setCurrent(null);
            var store = view.getData();
            for (var i = 0; i < store.data.length; i++) {
                store.data.items[i].dirty = true;
                if (i == 0) continue;
                //store.data.items[i].setPackageRule(me.view.getParent().getCurrent());
                //store.data.items[i].setPackageRuleId(me.view.getParent().getCurrent().getId());
                if (store.data.items[i].getLevelQty() == null || store.data.items[i].getLevelQty() == 0)
                    store.data.items[i].setLevelQty(1);
                store.data.items[i].setQty(store.data.items[i - 1].getQty() * store.data.items[i].getLevelQty());
            }      
        }); 
    }
});