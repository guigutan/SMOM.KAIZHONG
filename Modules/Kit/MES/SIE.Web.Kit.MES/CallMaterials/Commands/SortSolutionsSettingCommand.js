SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.SortSolutionsSettingCommand', {
    meta: { text: "排序方案设置", group: "edit", iconCls: "icon-UpList icon-blue" },

    /**
     * 行Index
     * @property {double} oldRowIndex
     */
    oldRowIndex: null,

    /**
    * 凭证
    * @property {string} token
    */
    token: null,

    /**
     * 排序方案设置
     * @property {SIE.Kit.MES.CallMaterials.SortSolutionsSetting} solutionSetting
     */
    solutionSetting: null,

    /**
     * 主视图
     * @property {ListLogicalView} view
     */
    view: null,

    /**
     * 拖动控件
     * @property {grid2Control} grid2Control
     */
    grid2Control: null,

    /**
     * 缓存选中排序优先级设置数组
     * @property {List<SIE.Web.Kit.MES.CallMaterials.PriorityInfo>} modelList
     */
    modelList: null,

    /**
     * 当前已选中的排序优先级设置对象
     * @property {SIE.Web.Kit.MES.CallMaterials.PriorityInfo} dragedSelectData
     */
    dragedSelectData: null,

    /**
     * 执行
     * @method execute
     * @param {ListLogicalView} view 
     * @param {source} 数据源
     */
    execute: function (view, source) {
        var me = this;
        me.oldRowIndex = -1;
        me.modelList = [];
        me.dragedSelectData = null;
        me.grid2Control = null;
        SIE.AutoUI.getMeta({
            model: "SIE.Kit.MES.CallMaterials.SortSolutionsSetting",
            module: "SIE.Kit.MES.CallMaterials.CallMaterialWorkOrder,SIE.MES",
            ignoreCommands: false,
            isDetail: false,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                me.token = mainBlock.token;
                var listView = SIE.AutoUI.createListView(mainBlock);
                me.view = listView;
                var ui = listView.getControl();
                ui.mon(ui, 'selectionchange', me.onControlSelectionChanged, me);
                var panelControl = me.createPanel(ui);
                var win = SIE.Window.show({
                    title: "排序方案设置".t(),
                    width: 650,
                    height: 500,
                    buttons: [
                        { xtype: "button", text: "确定".t(), hidden: true },
                        { xtype: "button", text: "取消".t(), hidden: true }
                    ],
                    items: panelControl,
                    id: "SortSolutionsSetting001",
                });

                var filter = {
                    Method: 'GetSortSolutionsSettings',
                    Parameters: []
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: view.token,
                    type: 'SIE.Web.Kit.MES.CallMaterials.DataQuery.CallMaterialWODataQueryer',
                });
            }
        });
    },

    /**
     * 创建面板控件
     * @method createPanel
     * @param {uiControl} uiControl ui控件
     * @return {Ext.panel.Panel} 面板控件
     */
    createPanel: function (uiControl) {
        return {
            xtype: 'panel',
            id: 'dragGridPanel',
            bodyStyle: {
                border: 0
            },
            layout: {
                type: 'vbox',
                pack: 'start',
                align: 'stretch'
            },
            items: [{
                flex: 1,
                layout: 'fit',
                items: uiControl
            }, {
                xtype: 'splitter'   // A splitter between the two child items
            }, {
                flex: 1,
                layout: 'fit',
                //title: '排序优先级设置',
                ui: 'light',
                //font: '20px Arial',
                items: [{
                    xtype: 'dd-grid-to-grid',
                    settingCommand: this,
                    id: 'dragGrid',
                }]
            }]
        };
    },

    /**
     * 选中变更事件
     * @method onControlSelectionChanged
     * @param {grid} grid
     * @param {selection} selection 选中对象
     * @param {eOpts} eOpts
     */
    onControlSelectionChanged: function (grid, selection, eOpts) {
        var me = this;
        var solSetIdList = [];
        if (selection.view.lastFocused && selection.view.lastFocused.record) {
            var data = selection.view.lastFocused.record.getData();
            if (data !== null) {
                //view = this.SIEView;
                var items = me.view.getData().data.items;
                items.forEach(function (item) {
                    var newData = item.data;
                    solSetIdList.push(newData.Id);
                    if (newData.Id === data.Id) {
                        me.solutionSetting = item;
                    }
                });

                if (me.oldRowIndex == data.Id || data.PriorityVMList.length !== 0)
                    return;
                else
                    me.oldRowIndex = data.Id;

                SIE.invokeDataQuery({
                    type: "SIE.Web.Kit.MES.CallMaterials.DataQuery.CallMaterialWODataQueryer",
                    method: "GetPrioritySetInfo",
                    params: [data.Id, solSetIdList],
                    async: false,
                    token: me.token,
                    callback: function (res) {
                        if (res.Success) {
                            var prioritySetInfo = res.Result;
                            var dragControl = Ext.getCmp('dragGrid');
                            var grid1Id = dragControl.items.items[0].id;
                            var grid1Control = Ext.getCmp(grid1Id);
                            var store = grid1Control.getStore();
                            store.setData(prioritySetInfo.SelectPriorityInfos);
                            grid1Control.setStore(store);

                            var grid2Id = dragControl.items.items[1].id;
                            grid2Control = Ext.getCmp(grid2Id);
                            var store = grid2Control.getStore();
                            store.setData(prioritySetInfo.SelectedPriortityInfos);
                            grid2Control.setStore(store);

                            me.modelList = [];
                            prioritySetInfo.SelectedPriortityInfos.forEach(function (e) {
                                var model = {
                                    SolutionSettingId: me.solutionSetting.data.Id,
                                    SortName: e.SortName,
                                    SortMode: e.SortMode,
                                    Priority: e.Priority
                                }
                                me.modelList.push(model);
                            });

                            for (var i = 0; i < items.length; i++) {
                                prioritySetInfo.SolSettingInfos.forEach(function (s) {
                                    if (items[i].data.Id == s.SolutionSettingId) {
                                        var priorityVMList = Ext.encode(s.SelectedPriortityInfos);
                                        items[i].setPriorityVMList(priorityVMList);
                                        //items[i].dirty = false;
                                    }
                                });
                            }
                            //items.forEach(function (item) { 不能在此标记未更改，会导致保存按钮不亮以及新增数据保存报错
                            //    item.markSaved();
                            //});
                            me.view.syncCmdState(me.view, true);
                        }
                    }
                });
            }
        }
    },
});

/**
 * 排序优先级控件Store的定义
 * @class SIE.Web.Kit.MES.CallMaterials.Commands.Simple
 * @constructs
 */
Ext.define('SIE.Web.Kit.MES.CallMaterials.Commands.Simple', {
    extend: 'Ext.data.Store',
    alias: 'store.simple',
    fields: [
        { name: 'SortName', type: 'string' },
        { name: 'SortMode' }]
});

/**
 * 排序优先级控件控制器的定义
 * @class SIE.Web.Kit.MES.CallMaterials.Commands.GridToGridController
 * @constructs
 */
Ext.define('SIE.Web.Kit.MES.CallMaterials.Commands.GridToGridController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.dd-grid-to-grid',

    /**
     * render之前事件
     * @method beforeRender
     */
    beforeRender: function () {
        var store = this.lookup('grid1').store,
            data = (this.myData = []),
            obj;

        // Keep a copy of the original data for reset:
        store.each(function (rec) {
            data.push(obj = Ext.apply({}, rec.data));
            delete obj.id;
        });
    },

    /**
     * 拖动事件
     * @method onDrop
     * @param {onRec} onRec
     * @param {rec} rec 记录数据
     * @param {dropPosition} dropPosition 拖动位置
     * @param {title} title 标题
     */
    onDrop: function (onRec, rec, dropPosition, title) {
        var dropOn = onRec ? ' ' + dropPosition + ' ' + onRec.get('name') : ' on empty view';

        //KitchenSink.toast(title, 'Dropped ' + rec.get('name') + dropOn);
    },

    /**
     * 拖动到第一个grid
     * @method onDropGrid1
     * @param {node} node 节点
     * @param {data} data 记录数据
     * @param {dropRec} dropRec 
     * @param {dropPosition} dropPosition 拖动位置
     */
    onDropGrid1: function (node, data, dropRec, dropPosition) {
        this.onDrop(dropRec, data.records[0], dropPosition, 'Drag from right to left'); //todo null ref
        var cmd = this.view.settingCommand;
        cmd.solutionSetting.dirty = true;
        cmd.view.syncCmdState(cmd.view, true);

        var initdata = {};
        cmd.modelList = [];

        var i = grid2Control.getStore().data.items.length;
        grid2Control.getStore().data.items.forEach(function (e) {
            var model = {
                SolutionSettingId: cmd.solutionSetting.data.Id,
                Solutions: cmd.solutionSetting.data,
                SortName: e.data.SortName,
                SortMode: e.data.SortMode,
                Priority: i
            }
            cmd.modelList.push(model);
            i--;
        });

        if (cmd.modelList.length > 0) {
            initdata = cmd.modelList;
            var priorityVMList = Ext.encode(initdata);
            cmd.solutionSetting.setPriorityVMList(priorityVMList);
        }
        else {
            cmd.solutionSetting.setPriorityVMList(" ");
        }
    },

    /**
     * 拖动到第二个grid
     * @method onDropGrid2
     * @param {node} node 节点
     * @param {data} data 记录数据
     * @param {dropRec} dropRec 
     * @param {dropPosition} dropPosition 拖动位置
     */
    onDropGrid2: function (node, data, dropRec, dropPosition) {
        this.onDrop(dropRec, data.records[0], dropPosition, 'Drag from left to right');
        var cmd = this.view.settingCommand;
        cmd.solutionSetting.dirty = true;
        cmd.view.syncCmdState(cmd.view, true);

        var initdata = {};
        cmd.modelList = [];

        var i = grid2Control.getStore().data.items.length;
        grid2Control.getStore().data.items.forEach(function (e) {
            var model = {
                SolutionSettingId: cmd.solutionSetting.data.Id,
                Solutions: cmd.solutionSetting.data,
                SortName: e.data.SortName,
                SortMode: e.data.SortMode,
                Priority: i
            }
            cmd.modelList.push(model);
            i--;
        });

        if (cmd.modelList.length > 0) {
            initdata = cmd.modelList;
            var priorityVMList = Ext.encode(initdata);
            cmd.solutionSetting.setPriorityVMList(priorityVMList);
        }
        else {
            cmd.solutionSetting.setPriorityVMList(" ");
        }
    },

    /**
     * 选中事件
     * @method onSelect
     * @param {grid} grid
     * @param {record} record 记录数据
     * @param {index} index
     * @param {eOpts} eOpts 
     */
    onSelect: function (grid, record, index, eOpts) {
        dragedSelectData = record.data;
    },

    /**
     * 下拉值变更事件
     * @method onChange
     * @param {combo} combo
     * @param {newValue} newValue 新选中的值
     * @param {oldValue} oldValue 旧选中的值
     * @param {eOpts} eOpts 
     */
    onChange: function (combo, newValue, oldValue, eOpts) {
        var cmd = this.view.settingCommand;
        cmd.solutionSetting.dirty = true;
        cmd.solutionSetting.crudState = "C";
        cmd.solutionSetting.crudStateWas = "C";
        cmd.view.syncCmdState(cmd.view, true);

        var initdata = {};
        cmd.modelList.forEach(function (e) {
            if (dragedSelectData.SortName == e.SortName) {
                e.SortMode = newValue;
            }
        });

        if (cmd.modelList.length > 0) {
            initdata = cmd.modelList;
            var priorityVMList = Ext.encode(initdata);
            cmd.solutionSetting.setPriorityVMList(priorityVMList);
        }
        else {
            cmd.solutionSetting.setPriorityVMList(" ");
        }
    },
});

/**
 * 排序优先级控件的定义
 * @class SIE.Web.Kit.MES.CallMaterials.Commands.GridToGrid
 * @constructs
 */
Ext.define('SIE.Web.Kit.MES.CallMaterials.Commands.GridToGrid', {
    extend: 'Ext.container.Container',
    xtype: 'dd-grid-to-grid',
    controller: 'dd-grid-to-grid',

    requires: [
        'Ext.grid.Panel',
        'Ext.layout.container.HBox'
    ],

    width: 650,
    height: 300,

    profiles: {
        classic: {
            width: 650,
            gridWidth: 325,
            columnTwoWidth: 80,
            columnThreeWidth: 80
        },
        neptune: {
            width: 650,
            gridWidth: 325,
            columnOneWidth: 80,
            columnTwoWidth: 80
        },
        graphite: {
            width: 730,
            gridWidth: 365,
            columnOneWidth: 100,
            columnTwoWidth: 100
        }
    },
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'grid',
        reference: 'grid1',
        width: 325,
        flex: 1,
        multiSelect: true,
        margin: '0 5 0 0',
        viewConfig: {
            plugins: {
                gridviewdragdrop: {
                    containerScroll: true,
                    dragGroup: 'dd-grid-to-grid-group1',
                    dropGroup: 'dd-grid-to-grid-group2'
                }
            },
            listeners: {
                drop: 'onDropGrid1'
            }
        },
        store: {
            type: 'simple',
        },
        columns: [{
            text: '名称'.t(),
            dataIndex: 'SortName',

            flex: 1,
            sortable: false
        }]
    }, {
        xtype: 'grid',
        reference: 'grid2',
        width: 325,
        flex: 1,
        stripeRows: true,
        requires: [
            'Ext.selection.CellModel'
        ],
        selModel: {
            type: 'cellmodel'
        },
        plugins: {
            cellediting: {
                clicksToEdit: 1
            }
        },
        viewConfig: {
            plugins: {
                gridviewdragdrop: {
                    containerScroll: true,
                    dragGroup: 'dd-grid-to-grid-group2',
                    dropGroup: 'dd-grid-to-grid-group1',
                    dropZone: {
                        overClass: 'dd-over-gridview'
                    }
                }
            },
            listeners: {
                drop: 'onDropGrid2',
                select: 'onSelect'
            }
        },
        store: {
            type: 'simple'
        },
        columns: [{
            text: '名称'.t(),
            dataIndex: 'SortName',

            flex: 1,
            sortable: false
        }, {
            text: '顺序'.t(),
            dataIndex: 'SortMode',
            flex: 1,
            sortable: true,
            editor: {
                xtype: 'combo',
                typeAhead: false,
                triggerAction: 'all',
                listeners: {
                    change: 'onChange'
                },
                store: [
                    ['升序'.t(), '升序'.t()],
                    ['降序'.t(), '降序'.t()]
                ],
            }
        }]
    }]
});
