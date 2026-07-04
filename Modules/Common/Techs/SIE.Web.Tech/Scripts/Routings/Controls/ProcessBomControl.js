/**
 * 工序bom对象
 * @for SIE.Tech.ProcessBomControl
 */
var store = Ext.create('Ext.data.Store', {
    fields: [
        { name: 'ItemId', type: 'number' },
        { name: 'Code', type: 'string' },
        { name: 'Name', type: 'string' },
        { name: 'ItemExtPropName', type: 'string' },
        { name: 'ItemExtProp', type: 'string' },
        { name: 'Qty', type: 'number' }
    ],
    data: [],
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});

var cellEditing = Ext.create('Ext.grid.plugin.CellEditing', { clicksToEdit: 1 });

/**
 * 工序bom控件
 * @class SIE.Tech.ProcessBomControl
 * @constructor
 */
Ext.define('SIE.Tech.ProcessBomControl', {
    extend: 'Ext.grid.Panel',
    xtype: 'techProcessBom',
    isLoaded: false,
    store: store,
    tbar: {
        items: [{
            xtype: 'button',
            text: '删除'.L10N(),
            iconCls: "iconfont icon-DeleteEntity icon-red",
            listeners: {
                click: function () {
                    var me = this;
                    var gridpanel = me.up('panel');
                    var store = gridpanel.store;
                    var bomRecord = gridpanel.getSelection()[0];
                    if (!bomRecord)
                        SIE.Msg.showMessage('请选择数据后再执行删除！'.L10N());

                    var idx = store.indexOf(bomRecord);
                    gridpanel.store.remove(bomRecord);

                    var bomData = [];
                    Ext.Array.forEach(gridpanel.store.getData().items, function (record) {
                        bomData.push(record.data);
                    });

                    if (gridpanel.CurNode) {
                        gridpanel.CurNode.designerData.Boms = bomData;
                    }
                }
            }
        }]
    },
    columns: [
        { xtype: 'rownumberer' },
        { text: '编码'.L10N(), dataIndex: 'Code' },
        { text: '名称'.L10N(), dataIndex: 'Name' },
        {
            text: '物料扩展属性'.L10N(), dataIndex: 'ItemExtPropName',
            editor: {
                xtype: 'textfield',
                readOnly: true,
            }
            //    listeners: {
            //        render: function (p) {
            //            p.getEl().on('dbclick', function (p) {
            //                
            //                var comp = this.component;
            //                var cur = this.component.up('tableview').getSelection()[0];
            //                var curData = this.component.up('tableview').getSelection()[0].data;
            //                var itemId = curData.ItemId;
            //                var itemExtProp = curData.ItemExtProp;
            //                var Id = p.target.id;//文本框的Id
            //                var isAllRequired = true;
            //                SIE.invokeDataQuery({
            //                    async: false,
            //                    type: "SIE.Web.Items.Common.DataQuery.ItemExtPropRecordsQueryer",
            //                    method: 'GetItemExtPropRecordsValue',
            //                    token: this.component.up('tableview').up().view.grid.mainView.token,
            //                    params: [itemId, itemExtProp, 0],
            //                    success: function (res) {
            //                        if (res.Result != null) {
            //                            var ui = SIE.Web.Items.ItemExtPropertyAction.CreateCtl(res.Result, itemExtProp);
            //                            var win = SIE.Window.show({
            //                                title: "物料扩展信息".t(),
            //                                width: 600,
            //                                height: 400,
            //                                items: ui,
            //                                ownComp:comp,
            //                                buttons: [
            //                                    {
            //                                        xtype: "button", text: "重置".t(), handler: function () {
            //                                            var me = this;
            //                                            var radio = this.up('window').query('[valueName=radioFieldName]');
            //                                            radio.forEach(function (p) {
            //                                                p.setValue(false);
            //                                            });
            //                                        }
            //                                    },
            //                                    {
            //                                        xtype: "button", text: "确定".t(), handler: function () {
            //                                            var radio = this.up('window').query('[valueName=radioFieldName]').where(function (p) { return p.checked; });
            //                                            if (isAllRequired) {
            //                                                var allradio = this.up('window').query('[xtype=radiogroup]');
            //                                                if (allradio.length != radio.length) {
            //                                                    SIE.Msg.showError("所有选项都必须选择一个！".t());
            //                                                    return;
            //                                                }
            //                                            }
            //                                            var value = "";
            //                                            var dbValue = "";
            //                                            radio.forEach(function (p) {
            //                                                value += p.headName + ":" + p.boxLabel + ";";
            //                                                dbValue += p.definitionId + ":" + p.boxLabel + ";";
            //                                            });
            //                                            if (value != "") {
            //                                                value = value.substring(0, value.length - 1);
            //                                            }
            //                                            if (dbValue != "") {
            //                                                dbValue = dbValue.substring(0, dbValue.length - 1);
            //                                            }
            //                                            //cur.data.ItemExtPropName = value;
            //                                            //cur.data.ItemExtProp = dbValue;
            //                                            setTimeout(function () {
            //                                                Ext.get(Id).dom.value = value;
            //                                                Ext.get(Id).dom.text = value;
            //                                                //this.up('window').ownComp.setValue(value);
            //                                            }, 500);
            //                                            this.up('window').close();

            //                                        }
            //                                    },
            //                                    {
            //                                        xtype: "button", text: "取消".t(), handler: function () {
            //                                            this.up('window').close();
            //                                        }
            //                                    }
            //                                ],
            //                            });
            //                        }
            //                    }
            //                });
            //                console.log(p.target);

            //            });
            //        },
            //    }
            //}
        },
        {
            text: '数量'.L10N(), dataIndex: 'Qty', editor: new Ext.form.NumberField({
                decimalPrecision: 2,
                allowDecimals: true,
                nanText: '请输入有效数字'.t(),
                allowNegative: false,
            }),
        },

    ],
    plugins: [cellEditing],
    listeners: {
        render: function (scop, eOpts) {
            var me = this;
            me.resetControl();
        },
        cellclick: function (e, record, index, row) {
            if (index == 3) {
                
                //var comp = this.component;
                var curData = this.getSelection()[0].data;
                var itemId = curData.ItemId;
                var itemExtProp = curData.ItemExtProp;
                /* var Id = p.target.id;//文本框的Id*/
                var token = this.view.grid.mainView.token;
                var me = this;
                var isAllRequired = true;
                SIE.invokeDataQuery({
                    async: false,
                    type: "SIE.Web.Items.Common.DataQuery.ItemExtPropRecordsQueryer",
                    method: 'GetItemExtPropRecordsValue',
                    token: token,
                    params: [itemId, itemExtProp, 0],
                    success: function (res) {
                        if (res.Result != null) {
                            var ui = SIE.Web.Items.ItemExtPropertyAction.CreateCtl(res.Result, itemExtProp);
                            var win = SIE.Window.show({
                                title: "物料扩展信息".t(),
                                width: 600,
                                height: 400,
                                items: ui,
                                buttons: [
                                    {
                                        xtype: "button", text: "重置".t(), handler: function () {
                                            var me = this;
                                            var radio = this.up('window').query('[valueName=radioFieldName]');
                                            radio.forEach(function (p) {
                                                p.setValue(false);
                                            });
                                        }
                                    },
                                    {
                                        xtype: "button", text: "确定".t(), handler: function () {
                                            var radio = this.up('window').query('[valueName=radioFieldName]').where(function (p) { return p.checked; });
                                            if (isAllRequired) {
                                                var allradio = this.up('window').query('[xtype=radiogroup]');
                                                if (allradio.length != radio.length) {
                                                    SIE.Msg.showError("所有选项都必须选择一个！".t());
                                                    return;
                                                }
                                            }
                                            var value = "";
                                            var dbValue = "";
                                            radio.forEach(function (p) {
                                                value += p.headName + ":" + p.boxLabel + ";";
                                                dbValue += p.definitionId + ":" + p.boxLabel + ";";
                                            });
                                            if (value != "") {
                                                value = value.substring(0, value.length - 1);
                                            }
                                            if (dbValue != "") {
                                                dbValue = dbValue.substring(0, dbValue.length - 1);
                                            }

                                            this.up('window').close();
                                            curData.ItemExtPropName = value;
                                            curData.ItemExtProp = dbValue;

                                            var gridpanel = me;
                                            var store = gridpanel.store;
                                            var bomRecord = gridpanel.getSelection()[0];
                                            gridpanel.setStore(gridpanel.store);

                                            //var idx = store.indexOf(bomRecord);
                                            //store.data.items[idx].data.ItemExtPropName = value;
                                            //store.data.items[idx].data.ItemExtProp = dbValue;
                                            var bomData = [];
                                            Ext.Array.forEach(gridpanel.store.getData().items, function (record) {
                                                bomData.push(record.data);
                                            });

                                            if (gridpanel.CurNode) {
                                                gridpanel.CurNode.designerData.Boms = bomData;
                                            }
                                        }
                                    },
                                    {
                                        xtype: "button", text: "取消".t(), handler: function () {
                                            this.up('window').close();
                                        }
                                    }
                                ],
                            });
                        }
                    }
                });

            }
        },
    },
    /**
     * 物料选择命令
     * @property {SIE.Web.Tech.Routings.Commands.SelectItemCommand} selectItemCommand
     */
    selectItemCommand: null,

    /**
     * 物料分类选择命令
     * @property {SIE.Web.Tech.Routings.Commands.SelectCategoryCommand} selectCategoryCommand
     */
    selectCategoryCommand: null,

    /**
     * 父主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 控件初始化
     * @method initComponent
     * @for SIE.Tech.ProcessBomControl
     */
    initComponent: function () {
        var me = this;
        me.initCommands();
        this.callParent();
    },

    /**
     * 重置bom控件
     * @method resetControl
     * @for SIE.Tech.ProcessBomControl
     */
    resetControl: function () {
        //1 清除bom数据；2 设置命令只读
        var me = this;
        me.disabledProcessBomCtrl(true);
        me.store.loadData([]);
    },

    /**
     * 加载工序bom数据
     * @method loadData
     * @for SIE.Tech.ProcessBomControl
     * @param {Object} node 工序节点信息
     */
    loadData: function (node) {
        var me = this;
        var designerData = {};
        me.store.loadData([]);
        if (node) {
            designerData = node.designerData;
            var itemIdList = [];
            Ext.Array.forEach(designerData.Boms, function (bom) {
                itemIdList.push(bom.ItemId);
            });
            SIE.invokeDataQuery({
                type: "SIE.Web.Tech.Routings.TechDataQueryer",
                method: "GetBoms",
                token: me.mainView.token,
                params: [itemIdList],
                callback: function (res) {
                    if (res.Success) {
                        
                        Ext.Array.forEach(res.Result, function (bom) {
                            var exsitedBom = designerData.Boms.find(m => m.ItemId == bom.ItemId);
                            if (exsitedBom) {
                                bom.ItemExtProp = exsitedBom.ItemExtProp;
                                bom.ItemExtPropName = exsitedBom.ItemExtPropName;
                                bom.Qty = exsitedBom.Qty;
                            }
                        });
                        designerData.Boms = res.Result;

                        me.store.loadData(designerData.Boms);
                        me.CurNode = node;
                    }
                }
            });
            var isPublish = me.mainView.CurRoutingVersion.get('state') === 1;
            if (node.nodeType === 'RoutingNode' && !isPublish && (designerData.ProcessType === 'BatchAssembly' || designerData.ProcessType === 'Assembly' || designerData.ProcessType === 'Rework')) {
                me.disabledProcessBomCtrl(false);
            }
            else {
                me.disabledProcessBomCtrl(true);
            }
        }
        else {
            me.disabledProcessBomCtrl(true);
        }
    },

    /**
     * 设置工序BOM的按钮状态
     * @method disabledProcessBomCtrl
     * @for SIE.Tech.ProcessBomControl
     * @param {Boolean} isDisabled 是否可用
     */
    disabledProcessBomCtrl: function (isDisabled) {
        var me = this;
        me.dockedItems.items[1].setDisabled(isDisabled);
    },

    /**
     * 初始化工序bom命令
     * @method initCommands
     * @for SIE.Tech.ProcessBomControl
     */
    initCommands: function () {
        var me = this;
        if (me.mainView === null || me.mainView === undefined)
            throw "主视图不能为空".L10N();
        if (me.isLoaded)
            return;
        me.createCommands();
        var commands = me.tbar.items;
        var selectItemCommand = commands.filter(function (cmd) {
            return cmd.command === 'SelectItemCommand';
        });
        if (selectItemCommand.length === 0)
            commands.unshift(me.getCommandConfig('SelectItemCommand', me.selectItemCommand));
        var selectCategoryCommand = commands.filter(function (cmd) {
            return cmd.command === 'SelectCategoryCommand';
        });
        if (selectCategoryCommand.length === 0)
            commands.unshift(me.getCommandConfig('SelectCategoryCommand', me.selectCategoryCommand));
        me.isLoaded = true;
    },

    /**
     * 创建工序bom命令
     * @method createCommands
     * @for SIE.Tech.ProcessBomControl
     */
    createCommands: function () {
        var me = this;
        me.selectItemCommand = Ext.create('SIE.Web.Tech.Routings.Commands.SelectItemCommand');
        me.selectCategoryCommand = Ext.create('SIE.Web.Tech.Routings.Commands.SelectCategoryCommand');
        me.selectItemCommand._ownerView = me.mainView;
        me.selectCategoryCommand._ownerView = me.mainView;
    },

    /**
     * 创建工序bom命令
     * @method getCommandConfig
     * @for SIE.Tech.ProcessBomControl
     * @param {string} name 命令名称
     * @param {Command} command 命令
     * @returns {{}} 命令配置信息
     */
    getCommandConfig: function (name, command) {
        var me = this;
        return {
            command: name,
            text: command.config.meta.text,
            group: command.config.meta.group,
            iconCls: command.config.meta.iconCls,
            handler: function () {
                command.ownerCtrl = me;
                command.tryExecute(me);
            }
        };
    },

    /**
     * 验证工序bom
     * @method validate
     * @for SIE.Tech.ProcessBomControl
     * @param {DesignCanvas} canvas 节点信息
     * @param {Array} nodes 节点集合
     * @param {Array} lines 连线集合
     */
    validate: function (canvas, nodes, lines) {
        nodes.forEach(function (node) {
            if (node.nodeType === 'RoutingNode') {
                if (node.designerData.ProcessId <= 0)
                    throw new Error(Ext.String.format('工序[{0}]的工序ID为空'.L10N(), node.designerData.Text));

                if (node.designerData.Boms) {
                    node.designerData.Boms.groupBy(function (p) {
                        return p.ItemId;
                    }).forEach(function (groupItem) {
                        if (groupItem.length > 1)
                            throw new Error(Ext.String.format('工序[{0}]工序BOM存在多个重复物料'.L10N(), node.designerData.Text));
                    });
                }
            }
        });
    },

    /**
     * 序列化
     * @method serialize
     * @for SIE.Tech.ProcessBomControl
     * @param {Object} nodeData 节点信息
     * @param {Object} activity 活动图
     * @param {String} pre 前缀
     */
    serialize: function (nodeData, activity, pre) {
        nodeData.Boms = [];
        if (activity.Boms && activity.Boms.Bom) {
            if (Ext.isArray(activity.Boms.Bom)) {
                Ext.Array.forEach(activity.Boms.Bom, function (bom) {
                    nodeData.Boms.push({
                        ItemId: bom[pre + "ItemId"],
                        ItemExtProp: bom[pre + "ItemExtProp"],
                        ItemExtPropName: bom[pre + "ItemExtPropName"],
                        Qty: bom[pre + "Qty"],
                    });
                });
            } else {
                nodeData.Boms.push({
                    ItemId: activity.Boms.Bom[pre + "ItemId"],
                    ItemExtProp: activity.Boms.Bom[pre + "ItemExtProp"],
                    ItemExtPropName:activity.Boms.Bom[pre + "ItemExtPropName"],
                    Qty: activity.Boms.Bom[pre + "Qty"],
                });
            }
        }
    },

    /**
     * 反序列化
     * @method deserialize
     * @for SIE.Tech.ProcessBomControl
     * @param {Object} nodeData 节点信息
     * @param {Object} activity 活动图
     * @param {String} pre 前缀
     */
    deserialize: function (nodeData, activity, pre) {
        activity.Boms = {};
        if (nodeData.Boms && nodeData.Boms.length > 0) {
            var bom_arr = [];
            Ext.Array.forEach(nodeData.Boms, function (bom) {
                var tmpBom = {};
                tmpBom[pre + "ItemId"] = bom.ItemId;
                tmpBom[pre + "ItemExtProp"] = bom.ItemExtProp;
                tmpBom[pre + "ItemExtPropName"] = bom.ItemExtPropName;
                tmpBom[pre + "Qty"] = bom.Qty;
                bom_arr.push(tmpBom);
            });
            activity.Boms['Bom'] = bom_arr;
        }
    }
});
(function () {
    var ctlConfig = { title: '工序BOM设置'.L10N(), type: 'SIE.Tech.ProcessBomControl', index: 10 };
    SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.addChildControlConfig(ctlConfig);
}());