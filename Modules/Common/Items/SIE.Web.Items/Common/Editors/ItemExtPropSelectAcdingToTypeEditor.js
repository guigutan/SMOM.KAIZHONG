/*物料扩展属性编辑器--根据物料的类型：半成品 单选；原材料 多选*/
Ext.define('SIE.Web.Items.Common.Editors.ItemExtPropSelectAcdingToTypeEditor', {
    extend: 'Ext.util.Observable',
    dicPropertyConfig: {},
    orgSelecteds: [],
    onClick: function (field, trigger, e) {
        var me = this;
        dicPropertyConfig = {};
        dicIdPropertyConfig = {};
        dicGroupPropertyConfig = {};
        isGroupSelected = {};
        orgSelecteds = [];
        var cur;
        if (me.up('form')) {
            cur = field.up().SIEView.getCurrent();
        }
        else {
            cur = field.up().context.record;
        }

        var token = this.up().grid.SIEView.token;
        var parentListView = this.up().grid.SIEView._parent;
        var itemId = cur.data.ItemId;
        var wipMaterialRequirementId = cur.data.Id;
        var arryValues = [];
        var arryIdValues = [];
        var itemPropertyValue = cur.getItemExtPropName();
        var itemExtPropName = cur.getItemExtPropName();
        var itemExtProp = cur.getItemExtProp();

        //if (cur.phantom == true) {
        //    SIE.Msg.showError("当前数据未保存，请先保存再选择物料属性值！".t());
        //    return false;
        //}
        if (!itemId) {
            SIE.Msg.showError("物料不能为空，请先选择物料！".t());
            return false;
        }
        //枚举值
        var type = -1;
        //枚举中，原材料是1，半成品是2
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.Items.Common.DataQuery.ItemExtPropRecordsQueryer",
            method: 'GetItemTypeById',
            token: token,
            params: [itemId],
            success: function (res) {
                type = res.Result;
            }
        });
        SIE.AutoUI.getMeta({
            model: 'SIE.Items.ItemPropertyValue',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            viewGroup: "SelectList",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var gridConfig = mainBlock.gridConfig;
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
                            var selection = listView.getSelection();
                            var selecteds = [];
                            if (selection.length == 0) {
                                cur.setItemExtProp("");
                                cur.setItemExtPropName("");
                            }
                            else {


                                //校验
                                dicGroupPropertyConfig = {};
                                isGroupSelected = {};
                                for (var i = 0; i < selection.length; i++) {
                                    var definitionId = selection[i].getDefinitionId();
                                    var propertyGroup = selection[i].getPropertyGroup();
                                    if (propertyGroup!=""&&!dicGroupPropertyConfig[definitionId])
                                        dicGroupPropertyConfig[definitionId] = propertyGroup;
                                    else {
                                        if (propertyGroup != "" &&dicGroupPropertyConfig[definitionId] != propertyGroup) {
                                            SIE.Msg.showError("相同物料属性只能选相同的组别！".t());
                                            return false;
                                        }
                                    }
                                    if (type == 2) {
                                        //半成品才做限制：同一属性组只能勾选一个属性
                                        if (!isGroupSelected[definitionId+" "+propertyGroup]) {
                                            isGroupSelected[definitionId + " " + propertyGroup] = true;
                                        }
                                        else {
                                            SIE.Msg.showError("半成品物料同类属性只能选一个！".t());
                                            return false;
                                        }
                                    }
                                }


                                for (var i = 0; i < selection.length; i++) {
                                    var definitionId = selection[i].getDefinitionId();
                                    var definitionName = selection[i].getDefinitionName();
                                    var definitionValue = selection[i].getValue();

                                    if (!dicPropertyConfig[definitionName])
                                        dicPropertyConfig[definitionName] = definitionValue;
                                    else
                                        dicPropertyConfig[definitionName] += "," + definitionValue;

                                    if (!dicIdPropertyConfig[definitionId])
                                        dicIdPropertyConfig[definitionId] = definitionValue;
                                    else
                                        dicIdPropertyConfig[definitionId] += "," + definitionValue;
                                }

                                var kk = 0;
                                for (var name in dicPropertyConfig) {
                                    var value = dicPropertyConfig[name];
                                    arryValues[kk] = name + ":" + value;
                                    kk++;
                                }

                                var idkk = 0;
                                for (var id in dicIdPropertyConfig) {
                                    var value = dicIdPropertyConfig[id];
                                    arryIdValues[idkk] = id + ":" + value;
                                    idkk++;
                                }

                                if (arryIdValues || arryIdValues.length > 0) {
                                    itemExtProp = arryIdValues.join(";");
                                    cur.setItemExtProp(itemExtProp);

                                }

                                if (arryValues || arryValues.length > 0) {
                                    itemExtPropName = arryValues.join(";");
                                    cur.setItemExtPropName(itemExtPropName);
                                }

                                for (var i = 0; i < selection.length; i++) {
                                    selecteds.push(selection[i].data);
                                }

                            }
                            this.close();
                        }
                    }
                });

                var filter = {
                    Method: 'GetItemPropertys',
                    Parameters: [itemId]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: "SIE.Web.Items.Common.DataQuery.ItemExtPropRecordsQueryer",
                    callback: function (records, operation, success) {
                        var records = records;
                        if (itemExtPropName.length > 0) {
                            var selectd = itemExtPropName.split(";");
                            var selModel = listView.getSelectionModel();
                            for (var i = 0; i < records[0].length; i++) {
                                var definitionName = records[0][i].data.DefinitionName;
                                var definitionValue = records[0][i].data.Value;
                                for (var j = 0; j < selectd.length; j++) {
                                    var name = selectd[j].split(":")[0];
                                    var value = selectd[j].split(":")[1];
                                    var values = value.split(",");
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