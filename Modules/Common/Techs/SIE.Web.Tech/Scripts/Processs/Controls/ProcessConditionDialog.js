Ext.define('SIE.Web.Tech.Processs.Controls.Stores', {
    statics: {
        operatorStore: Ext.create('Ext.data.Store', {
            fields: ['value', 'code'],
            data: [
                [-1, ''],
                [0, '='],
                [1, '!='],
                [2, '>'],
                [3, '<'],
                [4, '<='],
                [5, '>='],
                [6, 'Like']
            ],
            proxy: {
                type: 'memory',
                reader: {
                    type: 'json'
                }
            }
        }),
        relationStore: Ext.create('Ext.data.Store', {
            fields: ['value', 'code'],
            data: [
                [-1, ''],
                [0, '并且'],
                [1, '或']
            ],
            proxy: {
                type: 'memory',
                reader: {
                    type: 'json'
                }
            }
        }),
        conditionItemStore: Ext.create('Ext.data.Store', {
            fields: [
                { name: 'Key', type: 'string' },
                { name: 'Value', type: 'string' },
            ],
            proxy: {
                type: 'webapi',
                url: '/api/DataPortal/Query',
                extraParams: {
                    action: 'queryer',
                    type: 'SIE.Web.Tech.DataQuery.ProcessConditionDataQuery',
                    filter: Ext.encode({ "Method": "GetConditionItems", "Parameters": [] }),
                    token: 'GetConditionItems'
                },
                actionMethods: {
                    read: 'POST',
                },
                reader: {
                    type: 'json',
                    rootProperty: 'Result'
                }
            }
        })
    },
});

Ext.define('SIE.Web.Tech.Processs.Controls.ProcessConditionDialog',
    {
        extend: 'Ext.window.Window',
        alias: 'widget.ProcessConditionDialog',
        // closeAction: 'hide',
        // closable: false,
        bindcontent: '',
        viewModel: {},
        modal: true,
        border: true,
        width: 600,
        height: 400,
        layout: 'fit',
        _nodeId: 0,
        buttons: [
            {
                text: '确定'.t(),
                handler: function () {
                    //this.up('window').commitData();
                    var callback = this.up('window').callback;
                    if (callback) callback(Ext.window.MessageBox.OK);
                }
            },
            {
                text: '取消'.t(),
                handler: function () {
                    var callback = this.up('window').callback;
                    if (callback) callback(Ext.window.MessageBox.CANCEL);
                }
            }],
        tbar: [
            {
                text: '添加条件组',
                xtype: 'button'
            },
            {
                text: '添加条件',
                xtype: 'button',
                handler: function () {
                    var args = arguments;
                    var nodeId = this.up('window')._getNodeId();
                    var treePanel = args[0].up('window').down('treepanel')
                    var target = treePanel.selModel.getSelection()[0] || treePanel.getRootNode();
                    if (target.isLeaf())
                        target = target.parentNode;
                    // var store = treePanel.getStore();
                    var node = { Id: nodeId, TreePId: target.getId(), Condition: 'ga', leaf: true };
                    node = target.appendChild(node);
                    node.expand();
                    if (!target.isExpanded()) { target.expand(); }
                    treePanel.selModel.select(node);
                }
            },
            {
                text: '删除',
                xtype: 'button',
                handler: function () {
                    var args = arguments;
                    var panel = args[0].up('window').down('treepanel')
                    var target = panel.selModel.getSelection()[0] || panel.getRootNode();
                    if (target.parentNode) {
                        target.parentNode.removeChild(target);
                    }
                }
            },
            {
                text: '生成脚本',
                xtype: 'button',
                handler: function () {
                    var args = arguments;
                    var script = args[0].up('window').getScript();
                    var scriptPanel = args[0].up('window').down('textarea[itemRole=scriptPanel]');
                    scriptPanel.setValue(script);
                }
            }
        ],

        commitData: function () {
            var treePanel = this.down('treepanel');
            var store = treePanel.getStore();
            var inputData = [];
            var data = store.getData();

            data.items.forEach(function (m) {
                if (m.isLeaf()) { inputData.push(m.data); }
            })
            var indata = {};
            indata.Data = Ext.encode({
                conditionItems: inputData
            });
            SIE.invokeCommand({
                cmd: 'SIE.Web.Tech.Processs.Commands.SaveProcessConditionCommand',
                data: indata,
                callback: function (res) {
                    if (res.Success) {
                        debugger
                    } else {

                    }
                }
            });
        },

        initComponent: function () {
            this.dataStore = Ext.create('Ext.data.TreeStore', {
                model: 'SIE.Tech.Processs.Scripts.ScriptCondition',
                proxy: {
                    type: 'webapi',
                    url: '/api/DataPortal/Query',
                    extraParams: {
                        action: 'queryer',
                        type: 'SIE.Web.Tech.DataQuery.ProcessConditionDataQuery',
                        filter: Ext.encode({ "Method": "GetScriptConditions", "Parameters": [] }),
                        token: 'GetScriptConditions'
                    },
                    actionMethods: {
                        read: 'POST',
                    },
                    reader: {
                        type: 'json',
                        rootProperty: 'entities'
                    }
                },
                root: {
                    Id: 0.0,
                    leaf: false,
                    text: "查询树",
                    Condition: '查询树',
                    expanded: true,
                    editable: false
                },
                rootVisible: true,
                parentIdProperty: "TreePId",
                listeners: {
                    load: function (tStore, records, successful, operation, node, eOpts) {
                        node.expand(true);
                    }
                }
            });

            this.callParent();
        },

        _getNodeId: function () {
            var me = this;
            me._nodeId = me._nodeId + 1;
            return me._nodeId;
        },
        items: [
            {
                xtype: 'panel',
                layout: {
                    type: 'vbox',
                    pack: 'start',
                    align: 'stretch'
                },
                border: false,
                itemRole: 'conditionPanel',
                items: [
                    {
                        xtype: 'treepanel',
                        store: this.dataStore,
                        flex: 2,
                        reserveScrollbar: true,
                        plugins: {
                            ptype: 'cellediting',
                            clicksToEdit: 1
                        },
                        columns: [
                            {
                                xtype: 'treecolumn',
                                sortable: false,
                                text: '条件',
                                flex: 2,
                                dataIndex: 'Condition',
                                editable: false,
                                editor: {
                                    xtype: 'combobox',
                                    editable: false,
                                    triggerAction: 'all',
                                    store: SIE.Web.Tech.Processs.Controls.Stores.conditionItemStore,
                                    displayField: 'Key', //值
                                    valueField: 'Value', //代码
                                    listeners: {
                                        change: function (editor, newValue, oldValue, eOpts) {
                                            var record = editor.ownerCt.context.record;
                                            if (record) {
                                                record.set('Value', null);
                                            }
                                        }
                                    }
                                },
                                renderer: function (v, cellValues, record) {
                                    if (v === "")
                                        return "";
                                    var opStore = cellValues.column.config.editor.store;
                                    //通过匹配value取得ds索引
                                    var index = opStore.find('Value', v);
                                    if (index === -1)
                                        return "";
                                    //通过索引取得记录ds中的记录集
                                    var operatorRecord = opStore.getAt(index);
                                    //返回记录集中的value字段的值
                                    return operatorRecord.data.Key;
                                }
                            },
                            {
                                text: '比较符',
                                dataIndex: 'OperatorType',
                                width: 80,
                                sortable: false,
                                editor: {
                                    xtype: 'combobox',
                                    editable: false,
                                    triggerAction: 'all',
                                    store: SIE.Web.Tech.Processs.Controls.Stores.operatorStore, //引入ds 
                                    displayField: 'code', //值
                                    valueField: 'value', //代码
                                    mode: 'local'
                                },
                                renderer: function (v, cellValues, record) {
                                    if (v === "")
                                        return "";

                                    var opStore = cellValues.column.config.editor.store;
                                    //通过匹配value取得ds索引
                                    var index = opStore.find('value', v);

                                    if (index === -1)
                                        return "";

                                    //通过索引取得记录ds中的记录集
                                    var operatorRecord = opStore.getAt(index);

                                    //返回记录集中的value字段的值
                                    return operatorRecord.data.code;
                                }
                            },
                            {
                                text: '比较值',
                                sortable: false,
                                flex: 1,
                                dataIndex: 'Value',
                                editor: {
                                    xtype: 'textfield'
                                }
                            },
                            {
                                text: '关系',
                                sortable: false,
                                width: 80,
                                dataIndex: 'RelationType',
                                editor: {
                                    xtype: 'combobox',
                                    editable: false,
                                    triggerAction: 'all',
                                    store: SIE.Web.Tech.Processs.Controls.Stores.relationStore, //引入ds 
                                    displayField: 'code', //值
                                    valueField: 'value', //代码
                                    mode: 'local'
                                },
                                renderer: function (v, cellValues, record) {
                                    if (v === "")
                                        return "";
                                    var rStore = cellValues.column.config.editor.store;
                                    //通过匹配value取得ds索引
                                    var index = rStore.find('value', v);
                                    if (index === -1)
                                        return "";
                                    //通过索引取得记录ds中的记录集
                                    var relationRecord = rStore.getAt(index);
                                    //返回记录集中的value字段的值
                                    return relationRecord.data.code;
                                }
                            }
                        ],
                        listeners: {
                            cellclick: function (view, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                                var record = view.getStore().getAt(rowIndex);
                                var column = view.getGridColumns()[cellIndex];
                                if (record.isRoot() ||
                                    (record.get('Condition') == 0 && (column.dataIndex == 'Condition' || column.dataIndex == 'OperatorType' || column.dataIndex == 'Value')) ||
                                    (column.dataIndex == 'RelationType' && record.isFirst()))
                                    return false;
                            },
                        }
                    }
                    ,
                    {
                        xtype: 'panel',
                        layout: 'fit',
                        flex: 1,
                        padding: 10,
                        border: false,
                        items: [
                            {
                                border: false,
                                itemRole: 'scriptPanel',
                                fieldLabel: '脚本',
                                xtype: 'textarea'
                            }
                        ]
                    },
                ]
            },
        ],

        onRender: function (parentNode, containerIdx) {
            this.down('textarea[itemRole=scriptPanel]').setBind(this.bindcontent);
            this.callParent()
        },

        getScript: function () {
            var script = '';
            this.down('treepanel').getStore().getData().items.forEach(function (item) {
                if (item.isLeaf()) {
                    if (typeof item.data.RelationType !== 'undefined') script += item.data.RelationType == 0 ? '&&' : '||';
                    script += 'collectData.' + item.data.Condition + SIE.Web.Tech.Processs.Controls.Stores.operatorStore.query('value', item.data.OperatorType).items[0].data.code + item.data.Value;
                }
            });
            return 'def GetNextProcess(collectData):\n  return ' + script;
        }
    }
);
