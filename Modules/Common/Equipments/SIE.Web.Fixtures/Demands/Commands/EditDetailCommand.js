SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.EditDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.WorkOrderId != null && entity.data.WorkOrderId != 0;
        }
        return false;
    },
    onEditting: function (entity) {
        var me = this;
        me.woId = me.view.getParent().getCurrent().getWorkOrderId();
        if (entity) {
            this.mon(entity, 'propertyChanged', SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun.onEntityPropertyChanged, me);
        }
    },
})