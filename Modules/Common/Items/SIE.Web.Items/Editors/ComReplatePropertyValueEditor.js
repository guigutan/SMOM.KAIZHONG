Ext.define('SIE.Web.Items.Editors.ComReplatePropertyValueEditor', {
    extend: 'Ext.util.Observable',
    dicPropertyConfig: {},
    orgSelecteds: [],
    onClick: function (field, trigger, e) {
        var me = this;
        dicPropertyConfig = {};
        orgSelecteds = [];
        var cur;  // 当前行数据(组合替代)
        if (me.up('form')) {
            cur = field.up().SIEView.getCurrent();
        }
        else {
            cur = field.up().context.record;
        }
        var token = this.up().grid.SIEView.token;
        var parentListView = this.up().grid.SIEView._parent;
        var itemId = cur.data.ItemId;   // 组合替代对应的物料
        var comReplateId = cur.data.Id; // 组合替代对应的ID
        var enableExtendProperty = cur.data.EnableExtendProperty;
        var arryValues = [];
        var itemPropertyValue = cur.getItemPropertyValueProperty();

        // 当前组合替代的数据验证
        //if (cur.phantom == true) {
        //    SIE.Msg.showError("当前组合替代未保存，请先保存再选择物料属性值！".t());
        //    return false;
        //}
        if (!itemId) {
            SIE.Msg.showError("物料不能为空，请先选择物料！".t());
            return false;
        }

        SIE.AutoUI.getMeta({
            model: 'SIE.Items.CombinationReplatePropertyValue',
            module: "SIE.Items.ProductBom,SIE.Items",
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            viewGroup: "BomPropertyLookupView",
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
                            /** 界面显示的数据 */
                            var totalItems = listView.getData().data.items;
                            /** 界面被勾选的数据 */
                            var selection = listView.getSelection();
                            /** 所有数据的属性定义Id */
                            var totalDefinitionId = [];
                            /** 选择数据行Id */
                            var selecteds = [];
                            /** 选择数据的属性定义Id */
                            var selDefinitionId = [];
                            /** 选择数据以字符串显示 */
                            var dicPropertyConfig = {};
                            /** 物料属性vs属性组*/
                            var selDefinitionProperty = {};

                            if (totalItems.length) {
                                for (var i = 0; i < totalItems.length; i++) {
                                    var definitionId = totalItems[i].getDefinitionId();
                                    if (totalDefinitionId.indexOf(definitionId) == -1) {
                                        totalDefinitionId.push(definitionId);
                                    }
                                }
                            }
                            if (selection.length == 0) {
                                cur.setItemPropertyValueProperty("");
                                if (totalItems.length > 0) {
                                    SIE.Msg.showError("物料启用扩展属性，扩展属性定义必须每项选择一个！".t());
                                    return false;
                                }
                            }
                            else {
                                for (var i = 0; i < selection.length; i++) {
                                    var definitionId = selection[i].getDefinitionId();
                                    var definitionName = selection[i].getDefinitionId_Display();
                                    var definitionValue = selection[i].getValue();
                                    var propertyGroup = selection[i].getPropertyGroup();
                                    var dp = definitionId.toString() + "|" + propertyGroup.toString();
                                    // 记录属性名称与值
                                    if (!dicPropertyConfig[definitionName])
                                        dicPropertyConfig[definitionName] = definitionValue;
                                    else {
                                        //SIE.Msg.showError("扩展属性定义每项只能选择一个！".t());
                                        //return false;
                                        dicPropertyConfig[definitionName] += "、" + definitionValue;
                                    }

                                    // 记录物料属性定义与属性组
                                    if (!selDefinitionProperty[definitionId])
                                        selDefinitionProperty[definitionId] = dp;
                                    else if (selDefinitionProperty[definitionId] != dp) {
                                        SIE.Msg.showError("勾选的物料属性只能匹配一个属性组！".t());
                                        return false;
                                    }
                                    // 记录选择过的物料属性
                                    if (selDefinitionId.indexOf(definitionId) == -1) {
                                        selDefinitionId.push(definitionId);
                                    }

                                    //selecteds.push(selection[i].data);
                                    selecteds.push({
                                        CombinationReplateId: selection[i].data.CombinationReplateId,
                                        DefinitionId: selection[i].data.DefinitionId,
                                        PropertyGroup: selection[i].data.PropertyGroup,
                                        Value: selection[i].data.Value,
                                    });
                                }

                                if (totalDefinitionId.length != selDefinitionId.length) {
                                    SIE.Msg.showError("所有扩展物料属性定义至少选择一项！".t());
                                    return false;
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
                                    cur.setPropertyValueStr(itemPropertyValue);
                                    //cur.dirty = true;
                                    //listView.paramer.syncCmdState(listView.paramer, true);
                                }

                                cur.setPropertyValueJson(Ext.encode(selecteds));
                            }
                            //var savePropertyValue = Ext.create('SIE.Web.Items.ProductBoms.Commands.ComReplatePropertyValueSaveCommand');
                            //savePropertyValue.execute(listView, {
                            //    Selecteds: selecteds,
                            //    DetailId: comReplateId,
                            //}, win);
                            //CRT.Event.fire('SIE.Items.ProductBom.ProductBom_refresh');
                            //return false;
                        }
                    }
                });

                var filter = {
                    Method: 'GetComReplatePropertyValues',
                    Parameters: [comReplateId, itemId]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: 'SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery',
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