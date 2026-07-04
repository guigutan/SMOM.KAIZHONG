SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property == 'AndonTypeId' && !e.entity.isNew()) {
            var andonMessageSendView = e.entity.belongsView.findChild('SIE.Andon.Andons.AndonMessageSend');
            var andonPushObjectView = andonMessageSendView.findChild('SIE.Andon.Andons.AndonPushObject');
            var andonMessageSendStore = andonMessageSendView.getData();
            andonMessageSendStore.removeAll();
            var andonTypeId = e.entity.data.AndonTypeId;
            SIE.invokeDataQuery({
                method: "GetAndonTypeInfo",
                params: [andonTypeId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.Andon.Andons.DataQuery.AndonDataQuery",
                token: this.view.token,
                success: function (res) {
                    var data = res.Result;
                    var view = me.view;
                    var andon = view.getCurrent();
                    andon.setAndonClass(data.AndonTypeClass);
                    //andon.setPushPlugId(data.PushPlugId);
                    //andon.setPushPlugId_Display(data.PushPlugId_Display);
                    data.AndonMessageList.forEach(item => {
                        var newEntity = andonMessageSendView.createNewItem();
                        newEntity.setNode(item.Node);
                        newEntity.setMinute(item.Minute);
                        newEntity.setPushPlugId_Display(item.PushPlugName);
                        newEntity.setPushPlugId(item.PushPlugId);
                        newEntity.setMessageTemplate(item.MessageTemplate);
                        andonMessageSendView.getData().add(newEntity);
                        andonMessageSendView.setCurrent(newEntity);
                        item.PushObjectList.forEach(item => {
                            var newChildEntity = andonPushObjectView.createNewItem();
                            newChildEntity.setType(item.Type);
                            newChildEntity.setCode(item.Code);
                            newChildEntity.setName(item.Name);
                            andonPushObjectView.getData().add(newChildEntity);
                        });
                        andonMessageSendView.setCurrent(null);
                    });
                }
            });
        }
    }
});
