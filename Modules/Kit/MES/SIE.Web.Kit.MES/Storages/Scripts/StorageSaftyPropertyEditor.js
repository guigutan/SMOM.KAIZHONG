Ext.define('SIE.Web.Kit.MES.Storages.StorageSaftyPropertyEditor', {
    extend: 'Ext.util.Observable',
    onClick: function (field, trigger, e) {
        var me = this;
        var dicPropertyConfig = {};
        var orgSelecteds = [];
        var cur = field.up().context.record;
        var parentListView = this.up().up().up().SIEView._parent;
        var itemId = cur.data.ItemId;
        var detailId = cur.data.Id;
        var arryValues = [];
        var itemPropertyValue = cur.getItemPropertyValueProperty();
        if (cur.phantom == true) {
            SIE.Msg.showError("当前物料库存未保存，请先保存再选择物料属性值！".t());
            return false;
        }
        if (!itemId) {
            SIE.Msg.showError("物料不能为空，请先选择物料！".t());
            return false;
        }
        SIE.AutoUI.getMeta({
            model: 'SIE.Kit.MES.Storages.StorageSaftyPropertyValue',
            module: "SIE.Kit.MES.Storages.StorageArea,SIE.MES",
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock = res;
                mainBlock.paramer = parentListView;
                var listView = SIE.AutoUI.createListView(mainBlock);
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "物料属性值选择".t(),
                    width: 800,
                    height: 450,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var totalItems = listView.getData().data.items;
                            var selection = listView.getSelection();
                            var selecteds = [];
                            var dicDefinitionId = {};//取不相同的属性定义
                            if (totalItems.length) {
                                for (var i = 0; i < totalItems.length; i++) {
                                    var definitionId = totalItems[i].getDefinitionId();
                                    if (!dicDefinitionId[definitionId]) {
                                        dicDefinitionId[definitionId] = definitionId;
                                    }
                                }
                            }
                            if (selection.length == 0) {
                                cur.setItemPropertyValueProperty("");
                            }
                            else {
                                for (var i = 0; i < selection.length; i++) {
                                    var definitionName = selection[i].getDefinitionId_Display();
                                    var definitionValue = selection[i].getValue();
                                    if (!dicPropertyConfig[definitionName])
                                        dicPropertyConfig[definitionName] = definitionValue;
                                }
                                var kk = 0;
                                for (var name in dicPropertyConfig) {
                                    var value = dicPropertyConfig[name];
                                    arryValues[kk] = name + "：" + value;
                                    kk++;
                                }

                                if (arryValues || arryValues.length > 0) {
                                    itemPropertyValue = arryValues.join("；");
                                    cur.setItemPropertyValueProperty(itemPropertyValue);
                                }

                                for (var i = 0; i < selection.length; i++) {
                                    selecteds.push(selection[i].data);
                                }
                            }
                            var savePropertyValue = Ext.create('SIE.Web.Kit.MES.Storages.Commands.StorageSaftyPropertySaveCommand');
                            savePropertyValue.execute(listView, {
                                Selecteds: selecteds,
                                DetailId: detailId,
                            }, win);
                            CRT.Event.fire('SIE.Kit.MES.Storages.StorageArea_refresh');
                            return false;
                        }
                    }
                });

                var filter = {
                    Method: 'GetStorageAreaPropertyList',
                    Parameters: [detailId, itemId]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: 'SIE.Web.Kit.MES.Storages.DataQuerys.StorageDataQuery',
                    callback: function (records, operation, success) {
                        var records = records;
                        if (itemPropertyValue.length > 0) {
                            var selectd = itemPropertyValue.split("；");
                            var selModel = listView.getSelectionModel();
                            for (var i = 0; i < records[0].length; i++) {
                                var definitionName = records[0][i].data.DefinitionId_Display;
                                var definitionValue = records[0][i].data.Value;
                                for (var j = 0; j < selectd.length; j++) {
                                    var name = selectd[j].split("：")[0];
                                    var value = selectd[j].split("：")[1];
                                    var values = value.split("、");
                                    for (var k = 0; k < values.length; k++) {
                                        if (definitionName == name && definitionValue == values[k]) {
                                            selModel.select(records[0][i], true);
                                            orgSelecteds.push(records[0][i]);
                                        }
                                    }
                                }
                            }
                        }
                    },
                });
            },
        });
    },
});