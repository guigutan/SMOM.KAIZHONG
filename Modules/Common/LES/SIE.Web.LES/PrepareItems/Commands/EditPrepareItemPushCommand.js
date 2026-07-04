SIE.defineCommand('SIE.Web.LES.PrepareItems.Commands.EditPrepareItemPushCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.view.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this.view);
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