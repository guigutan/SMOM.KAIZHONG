SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.AddWoPropertyValueCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    canExecute: function (listView) {
        var parent = listView.getParent();
        if (parent == null) return false;
        var parData = parent.getData();
        if (parData == null) return false;
        return listView.canAddItem() && parent != null && parData.data != null && parData.data.ProductId > 0;
    },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var parent = me.view.getParent();
        if (parent != null) {
            var parData = parent.getData().data;
            var type = parData.Type;
            entity.setParentId(parData.Id);
            entity.setItemId(parData.ProductId);
            parent.getData().dirty = true;
            parent.syncCmdState(parent, true);
            me.view._parent.getData().dirty = true;
            me.view._parent.syncCmdState(me.view._parent, true);
        }
    }
});