SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalInfos.Commands.EditAbnormalInfoDefinitionCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity && !entity.isNew()) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            var abnormalDefinition = e.entity;
            var definitionData = abnormalDefinition.data;
            var token = this.view.token;
            if (e.property === "AbnormalCategoryId") {
                var definitionSenderView = me.view.getChildren()[0];
                definitionSenderView.getData().data.removeAll();
                SIE.invokeDataQuery({
                    type: "SIE.Web.AbnormalInfo.AbnormalInfos.DataQuery.AbnormalInfoQueryer",
                    method: "ChangeSenderUpgrades",
                    params: [definitionData.AbnormalCategoryId],
                    token: token,
                    success: function (res) {
                        var data = res.Result.data;
                        var store = definitionSenderView.getData();
                        for (var j = 0; j < data.length; j++) {
                            var newEntity = data.getAt(j);
                            var model = new store.model();
                            model.generateId();
                            model.setAbnormalDefinitionId(definitionData.id);
                            model.setConditionType(newEntity.data.ConditionType);
                            model.setTimeType(newEntity.data.TimeType);
                            model.setUnitType(newEntity.data.UnitType);
                            model.setPusherId(newEntity.data.PusherId);
                            model.setPusherId_Display(newEntity.data.PusherId_Display);
                            store.insert(0, model);
                        }
                    },
                });
            }
        }
    }
});