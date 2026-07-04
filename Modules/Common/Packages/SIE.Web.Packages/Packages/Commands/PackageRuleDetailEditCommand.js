SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleDetailEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getCurrent() == null || view.getSelection().length == 0) {
            return false;
        }
        var sel = view.getSelection();
        for (i = 0; i < sel.length; i++) {
            var item = sel[i].data;
            //if (item.IsMasterUnit == true) {
            //    return false;
            //    break;
            //}
        }
        return true;

    },

    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },

    _onEntityPropertyChanged: function (e) {
        if (e.property.length > 0) {
            
                var data = e.entity.data;
                var value = e.value;
                var entity = e.entity;
                var me = this;
                var store = me.view.getData();
                if (e.entity.isDirty()) {
                    for (var i = 0; i < store.data.length; i++) {
                        store.data.items[i].dirty = true;
                    }
                }
             

            ////LevelQty改动
            if (e.property.indexOf('LevelQty') >= 0 || (e.property.indexOf('Qty') >= 0 && data.IsMasterUnit)) {
                var data = e.entity.data;
                var value = e.value;
                var entity = e.entity;
                var me = this;
                var store = me.view.getData();
                if (e.entity.isDirty()) {
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