Ext.define('SIE.Web.Andon.Andons.Behaviors.AndonBehavior', {
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getData();
        view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var andonTypeMessageSendIds = [];
        var andonMessageSendIds = [];
        var site = 0;
        if (e.property == 'AndonTypeId') {
            var andonMessageSendView = e.entity.belongsView.findChild('SIE.Andon.Andons.AndonMessageSend');
            var andonMessageSendStore = andonMessageSendView.getData();
            andonMessageSendStore.removeAll();
            var andonTypeId = e.entity.data.AndonTypeId;
            SIE.invokeDataQuery({
                method: "GetAndonTypes",
                params: [andonTypeId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.Andon.Andons.DataQuery.AndonDataQuery",
                token: this.token,
                success: function (res) {
                    var data = res.Result.data.items[0].data;
                    var view = me;
                    var andon = view.getCurrent();
                    andon.setAndonClass(data.AndonTypeClass);
                    andon.setPushPlugId(data.PushPlugId);
                    andon.setPushPlugId_Display(data.PushPlugId_Display);
                }
            });
            SIE.invokeDataQuery({
                method: "GetAndonTypeMessageSends",
                params: [andonTypeId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.Andon.Andons.DataQuery.AndonDataQuery",
                token: this.token,
                success: function (res) {
                    if (res.Success) {
                        var items = res.Result.data.items;
                        items.forEach(function (item) {
                            var newEntity = andonMessageSendView.createNewItem();
                            newEntity.data.Node = item.data.Node;
                            newEntity.data.Minute = item.data.Minute;
                            newEntity.data.PushPlugId_Display = item.data.PushPlugId_Display;
                            newEntity.data.PushPlugId = item.data.PushPlugId;
                            newEntity.data.MessageTemplate = item.data.MessageTemplate;
                            newEntity.data.CreateBy = item.data.CreateBy;
                            newEntity.data.CreateByName = item.data.CreateByName;
                            newEntity.data.CreateDate = item.data.CreateDate;
                            newEntity.data.UpdateBy = item.data.UpdateBy;
                            newEntity.data.UpdateByName = item.data.UpdateByName;
                            newEntity.data.UpdateDate = item.data.UpdateDate;
                            andonMessageSendView.getData().add(newEntity);
                            andonTypeMessageSendIds.push(item.data.Id);
                            andonMessageSendIds.push(newEntity.data.Id);
                        });
                    }
                }
            });
            var andonPushObjectView = andonMessageSendView.findChild('SIE.Andon.Andons.AndonPushObject');
            SIE.invokeDataQuery({
                method: "GetAndonTypePushObjects",
                params: [andonTypeMessageSendIds],
                async: false,
                action: 'queryer',
                type: "SIE.Web.Andon.Andons.DataQuery.AndonDataQuery",
                token: this.token,
                success: function (res) {
                    if (res.Success) {
                        var pushObjectList = res.Result.data;
                        for (var i = 0; i < andonMessageSendIds.length; i++) {
                            var messageSend = andonMessageSendStore.getData().filterBy(function (p) {
                                return p.getId() == andonMessageSendIds[i];
                            });
                            var pushObjects = pushObjectList.filterBy(function (p) {
                                return p.getMessageSendId() == andonTypeMessageSendIds[i];
                            });
                            if (pushObjects.length != 0) {
                                andonPushObjectView._parent.setCurrent(messageSend.items[0]);
                                pushObjects.items.forEach(item => {
                                    var newEntity = andonPushObjectView.createNewItem();
                                    newEntity.data.Type = item.data.Type;
                                    newEntity.data.Code = item.data.Code;
                                    newEntity.data.Name = item.data.Name;
                                    newEntity.data.CreateBy = item.data.CreateBy;
                                    newEntity.data.CreateByName = item.data.CreateByName;
                                    newEntity.data.CreateDate = item.data.CreateDate;
                                    newEntity.data.UpdateBy = item.data.UpdateBy;
                                    newEntity.data.UpdateByName = item.data.UpdateByName;
                                    newEntity.data.UpdateDate = item.data.UpdateDate;
                                    andonPushObjectView.getData().add(newEntity);
                                });
                            }
                        }
                    }
                }
            });
        }
    }
});
