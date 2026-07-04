SIE.defineCommand('SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomDetailAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getParent() == null || view.getParent().getCurrent() == null) {
            return false;
        }

        return !(view.getParent().getCurrent().isNew());
    },
    onItemCreated: function (entity) {
        this.callParent();
        entity.mon(entity, 'propertyChanged', SIE.Web.MES.Routings.Scripts.RoutingBomCommonFun.onRoutingBomDetailAddPropertyChanged, this)
    },
});