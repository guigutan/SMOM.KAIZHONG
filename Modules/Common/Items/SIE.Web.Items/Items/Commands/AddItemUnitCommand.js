SIE.defineCommand('SIE.Web.Items.Items.Commands.AddItemUnitCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view._parent && view._parent.getCurrent() && view._parent.getCurrent().data.CreateBy > 0) {
            return true;
        }
        return false;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            entity.setMainUnitId(me.view._parent.getCurrent().data.UnitId);
            this.view.execute({
                data: model,
                isSubmmit: false
            }, me.view);
        }
    }
});