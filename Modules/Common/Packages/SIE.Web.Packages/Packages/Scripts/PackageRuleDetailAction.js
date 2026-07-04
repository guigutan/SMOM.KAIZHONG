Ext.define("SIE.Web.Packages.Packages.Scripts.PackageRuleDetailAction", {
    statics: {
        onEntityPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            var data = e.entity.data;
            var value = e.value;
            if (e.property.length > 0) {
                if (e.property.indexOf('LevelQty') >= 0 || (e.property.indexOf('Qty') >= 0 && data.IsMasterUnit)) {
                    var store = me.view.getData();
                    for (var i = 0; i < store.data.length; i++) {
                        store.data.items[i].dirty = true;
                        if (i == 0) continue;
                        store.data.items[i].setQty(store.data.items[i - 1].getQty() * store.data.items[i].getLevelQty());
                    }
                }
            }
        }
    }
});