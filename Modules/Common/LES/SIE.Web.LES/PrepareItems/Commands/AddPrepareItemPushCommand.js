SIE.defineCommand('SIE.Web.LES.PrepareItems.Commands.AddPrepareItemPushCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
                }
            }, me.view);
        }
    },
    onEntityPropertyChanged: function (e) {
        var entity = e.entity;
        var view = e.entity.belongsView;
        if (e.property.length > 0 && !(e.value == e.oldvalue)) {
            if (e.property == 'TriggerType') {
                if (entity.getTriggerType() == 1) {
                    entity.setSatisfactionTime(null);
                } else if (entity.getTriggerType() == 2) {
                    entity.setAdvanceHours(null);
                }
            }
            if (e.property == 'DemandType') {
                if (entity.getDemandType() == 4) {
                    entity.setPreparationTime(null);
                } else if (entity.getDemandType() == 2 || entity.getDemandType() == 3) {
                    entity.setFixedQuantity(null);
                }
            }
        }
    }
});