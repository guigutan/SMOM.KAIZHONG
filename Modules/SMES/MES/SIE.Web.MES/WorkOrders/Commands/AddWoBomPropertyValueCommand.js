SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.AddWoBomPropertyValueCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    canExecute: function (listView) {
        var parent = listView.getParent();
        if (parent == null) return false;
        var parData = parent.getCurrent();
        if (parData == null) return false;
        return listView.canAddItem() && parent != null && parData.data != null && parData.data.ItemId > 0;
    },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var parent = me.view.getParent().getCurrent();
        if (parent != null) {
            var parData = parent.getData();
            var type = parData.Type;
            entity.setParentId(parData.Id);
            entity.setItemId(parData.ItemId);
            me.view._parent._parent.getData().dirty = true;
            me.view._parent._parent.syncCmdState(me.view._parent._parent, true);
        }
    }
});