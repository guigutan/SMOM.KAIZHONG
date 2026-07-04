SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.AddDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    /**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.WorkOrderId != null && entity.data.WorkOrderId != 0;
        }
        return false;
    },

    /**
     * onItemCreated 创建实体
     * @param {} entity
     * @returns {}
     */
    onItemCreated: function (entity) {
        var me = this;
        me.woId = me.view.getParent().getCurrent().getWorkOrderId();
        if (entity) {
            me.mon(entity, 'propertyChanged', SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun.onEntityPropertyChanged, me);
        }
        entity.setFixtureEncodeId(0);
    },
});