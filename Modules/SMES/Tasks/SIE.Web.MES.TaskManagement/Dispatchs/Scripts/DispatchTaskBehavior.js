Ext.define('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.DispatchTaskBehavior', {
    //requires: [
    //    'SIE.Web.MES.TaskManagement.Dispatchs.Commands.ShowTaskDetailCommand',
    //    'SIE.Web.MES.TaskManagement.Dispatchs.Commands.TaskPerformerCommand'
    //],
    /**
     * 之前选中的项Id
     * @param {double} oldRowIndex
     */
    oldRowIndex: null,

    /**
     * 产品族主视图
     * @param {DetailView} mainView
     */
    mainView: null,

    /**
     * 
     * @property me
     */
    me: null,

    /**
    * 派工任务
    * @param {dispatchTask} dispatchTask
    */
    dispatchTask: null,

    //isSelectedTaskPerformer: false,

    /**
          * view生命周期函数--view生成前
          * @param {*} meta 实体视图元数据
          * @param {*} curEntity 当前操作实体(可空)
          */
    beforeCreate: function (meta, curEntity) {
        oldRowIndex = -1;
        if (!meta)
            return;
        if (meta.model != 'SIE.MES.TaskManagement.Dispatchs.DispatchTask')
            return;
       // var gridConfig = meta.gridConfig;
       // gridConfig.selModel = {
       //     injectCheckbox: 0, //checkbox位于哪一列，默认值为0
       //     selType: 'checkboxmodel', //checkbox
       //     checkOnly: true, //只能通过checkbox选择
       //     mode: 'MULTI'   //是否多选
       // };

        meta.gridConfig.columns[5].xtype = 'setUrgentStyle';
    },

    /**
     * view生命周期函数--view生成后
     * @param {DetailView} view 生成的view
     */
    onCreated: function (view) {
        me = this;
        dispatchTask = null;        
        mainView = view;
        mainView.isSelectedTaskPerformer = false;
        view.mon(view, 'currentChanged', me.dispatchTaskPropertyChanged, me);
        var dispathControl = view.getControl();
        dispathControl.mon(dispathControl, 'cellclick', me._onControlCellClick, dispathControl);
    },

    /**
    * 视图关联后方法
    * @method onViewReady
    * @param {ListLogicView} view 产品族视图
    */
    onViewReady: function (view) {
        if (view._children.length <= 0)
            return;
        var tabPanel = view._children[0].getControl().up('tabpanel');     
        var comboStore = new Ext.data.SimpleStore({
            fields: ['AdoValue', 'AdoName'],
            data: []
        });
         var tabItem = Ext.create('Ext.Container', {
             title: '选择执行对象'.t(),
            layout: 'fit',
            bodyBorder: false,
            items: [
                {
                    xtype: 'panel',
                    bodyStyle: {
                        border: 0
                    },
                    layout: {
                        type: 'vbox',
                        pack: 'start',
                        align: 'stretch'
                    },
                    items: [{
                        heigth:120,
                        items: [
                            {
                                margin: '10 0 10 10',
                                xtype: 'fieldcontainer',
                                layout: 'hbox',
                                items: [{
                                    xtype: 'combobox',
                                    id: 'adoTypeBoxId',
                                    publishes: 'value',
                                    fieldLabel: '对象类型'.t() + '<span style="color: red;">*</span>',
                                    labelWidth: 68,
                                    displayField: 'name',
                                    valueField: 'abbr',
                                    editable: false,
                                    allowBlank: true,
                                    store: [
                                        { abbr: '', name: ''},
                                        { abbr: '班组', name: '班组'.t() },
                                        { abbr: '员工组', name: '员工组'.t() },
                                        { abbr: '工位', name: '工位'.t() },
                                    ],
                                    minChars: 0,
                                    queryMode: 'local',
                                    //value: '班组',
                                    margin: '0 20 0 0',
                                    tpl: Ext.create('Ext.XTemplate',
                                        '<tpl for=".">',
                                        '<div class="x-boundlist-item" style="height:21px">{name}</div>',
                                        '</tpl>'),
                                    listeners: {
                                        change: me.onAdoTypeChange,
                                    },
                                }, {
                                    xtype: 'combobox',
                                    id: 'adoNameBoxId',
                                    publishes: 'value',
                                    editable: false,
                                    allowBlank: true,
                                    displayField: 'AdoName',
                                    valueField: 'AdoValue',
                                    store: comboStore,
                                    minChars: 0,
                                    queryMode: 'local',
                                    margin: '0 20 0 0',
                                    tpl: Ext.create('Ext.XTemplate',
                                        '<tpl for=".">',
                                        '<div class="x-boundlist-item" style="height:21px">{AdoName}</div>',
                                        '</tpl>'),
                                    listeners: {
                                        change: me.onAdoNameChange,
                                    },
                                }]
                            }]
                    }, {
                        flex: 1,
                        layout: 'fit',
                        ui: 'light',
                        items: [{
                            xtype: 'gridtogrid',
                            id: 'taskPerfomerId',
                        }]
                    }]
                }],
            border: false,
        });
        if (tabPanel) {
            tabPanel.clearListeners(); //清除框架的tabchange事件，因为第一个标签页为自定义控件，不需要框架加载数据
            tabPanel.mon(tabPanel, 'tabchange', me.tabchange, this);
            tabPanel.insert(0, tabItem);
            tabPanel.setActiveTab(0);
            tabPanel.on('tabchange', function (tabPanel, newCard, oldCard, eOpts) {
                if (newCard.title == "选择执行对象".t()) {
               me.loadTaskPerfomerInfos(dispatchTask);
            }
            else if (newCard.title == "关联任务单".t()) {
                me.loadAssociatedTaskInfos(dispatchTask);
            }      
        }, me, { delay: 100 });
        }
    },

    /**
     * 选中数据变更处理事件
     * @param {} selection
     * @returns {}
     */
    _onControlCellClick: function (g, row, col, record, tr, rowindex) {
        if (!record.data)
            return;
        dispatchTask = record.data;
        if (oldRowIndex == dispatchTask.Id)
            return;
        else
            oldRowIndex = dispatchTask.Id;
        me.loadTaskPerfomerInfos(dispatchTask);
        me.loadAssociatedTaskInfos(dispatchTask);
    },

    /**
     * 任务单变更事件 
     * @method dispatchTaskPropertyChanged
     * @param {Object} arg 参数
     */
    dispatchTaskPropertyChanged: function (arg) {
        if (!arg.newValue)
        {
         if(arg.newValue===null)
           oldRowIndex=null;
           return;
        }
           
        dispatchTask = arg.newValue.data;
        if (arg.newValue && arg.oldValue && arg.newValue.getId() === arg.oldValue.getId())
            return;
        oldRowIndex = dispatchTask.Id;
        me.loadTaskPerfomerInfos(dispatchTask);
        me.loadAssociatedTaskInfos(dispatchTask);
    },

    /**
     * 加载对象控件数据
     * @method loadTaskPerfomerInfos
     * @param {SIE.MES.TaskManagement.Dispatchs.DispatchTask} dispatchTask 任务单
     */
    loadTaskPerfomerInfos: function (dispatchTask) {
        if(!dispatchTask)
            return;
        var associatedTaskLayout = Ext.Array.findBy(mainView._children, function (item) {
            if (item.model == 'SIE.MES.TaskManagement.Dispatchs.AssociatedTask') { return true; }
        });

        if (!associatedTaskLayout)
            return;
        var activeTitle=associatedTaskLayout.getControl().up('tabpanel').getActiveTab().title;
        if (activeTitle !== "选择执行对象".t())
            return;
        var adoTypeBoxControl = Ext.getCmp("adoTypeBoxId");
        var adoType = adoTypeBoxControl.value;
        var adoNameBoxControl = Ext.getCmp("adoNameBoxId");
        var adoName = adoNameBoxControl.value;
        if (adoName == null)
            adoName = "";

        var selectedIds = mainView.getSelectionIds();
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
            method: "GetTaskPerformerInfo",
            params: [selectedIds, dispatchTask.Id, adoType, adoName],
            async: false,
            token: mainView.token,
            callback: function (res) {
                if (res.Success) {
                   
                    var taskPerformerInfo = res.Result;
                    mainView.isSelectedTaskPerformer = taskPerformerInfo.IsSelectedTaskPerformer;

                    //2022/6/15 csp 此处产生 B0034306 BUG
                    //var adoNameBoxControl = Ext.getCmp("adoNameBoxId");
                    //var store = adoNameBoxControl.getStore();
                    //store.setData(taskPerformerInfo.ShiftEmployeeInfos);
                    //adoNameBoxControl.setStore(store);

                    var dragControl = Ext.getCmp('taskPerfomerId');
                    var grid1Id = dragControl.items.items[0].id;
                    var grid1Control = Ext.getCmp(grid1Id);
                    var grid2Id = dragControl.items.items[1].id;
                    grid2Control = Ext.getCmp(grid2Id);

                    var store = grid1Control.getStore();
                    store.setData(taskPerformerInfo.AdoInfos);
                    grid1Control.setStore(store);

                    var store = grid2Control.getStore();
                    store.setData(taskPerformerInfo.SelectedAdoInfos);
                    grid2Control.setStore(store);
                }
            }
        });
    },

    /**
    * 加载相关任务单列表数据
    * @method loadAssociatedTaskInfos
    * @param {SIE.MES.TaskManagement.Dispatchs.DispatchTask} dispatchTask 任务单
    */
    loadAssociatedTaskInfos: function (dispatchTask) {
        if (!mainView)
            return;
        if(!dispatchTask)
            return;
        var associatedTaskLayout = Ext.Array.findBy(mainView._children, function (item) {
            if (item.model == 'SIE.MES.TaskManagement.Dispatchs.AssociatedTask') { return true; }
        });

        if (!associatedTaskLayout)
            return;
        var activeTitle=associatedTaskLayout.getControl().up('tabpanel').getActiveTab().title;
        if (activeTitle !== "关联任务单".t())
            return;
        var associatedTaskControl = associatedTaskLayout.getControl();
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
            method: "GetAssociatedTasks",
            params: [dispatchTask.Id],
            async: false,
            token: mainView.token,
            callback: function (res) {
                if (res.Success) {
                    var associatedTasks = res.Result.data;
                    var store = associatedTaskControl.getStore();
                    store.setData(associatedTasks);
                    associatedTaskControl.setStore(store);
                }
            }
        });
    },

    /**
     * 子列表标签页切换事件
     * @method tabchange
     * @param {Ext.tab.Panel} tabPanel 标签控件
     * @param {newCard} newCard 新激活子页签
     * @param {oldCard} oldCard 旧子页签
     * @param {eOpts} eOpts 参数
     */
    tabchange: function (tabPanel, newCard, oldCard, eOpts) {
        if (newCard.title === '选择执行对象'.t())
            return;
        if (newCard.title === '关联任务单'.t())
            return;
        var control = newCard.down("gridpanel");
        if (control !== null && control.SIEView.getChildren().length === 0)
            if (newCard.down("form"))
                control = newCard.down("form").SIEView.getChildren().length > 0 ? newCard.down("form") : control;
        if (!control)
            control = newCard.down("form");
        if (!control)
            control = newCard.down("treepanel");
        var view = control.SIEView;
        view.inactive = false;
        view.loadChildData();
        if (view.hasListeners['isready']) {
            view.fireEvent('isReady', true);
        }
    },

    /**
     * 创建可选对象
     * @method createTaskPerformer
     * @param me me 标签控件
     */
    createTaskPerformer: function (me) {
        return Ext.create('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.TaskPerformer', {
            id: 'taskPerformerId',
            height: '100%',
            width: '100%',
            layout: 'fit',
            flex: 1,
            border: false,
        });
    },

    /**
     * 创建已选对象
     * @method createSelectedTaskPerformer
     * @param me me 标签控件
     */
    createSelectedTaskPerformer: function (me) {
        return Ext.create('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.SelectedTaskPerformer', {
            id: 'selectedTaskPerformerId',
            height: '100%',
            width: '100%',
            layout: 'fit',
            flex: 1,
            border: false,
        });
    },

    /**
     * 创建GridToGrid面板
     * @method createGridToGrid
     */
    createGridToGrid: function () {
        return Ext.create('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.GridToGrid', {
            id: 'gridToGridId',
            height: '100%',
            width: '100%',
            layout: 'fit',
            flex: 1,
            border: false,
        });
    },

    /**
     * 下拉值变更事件
     * @method onAdoTypeChange
     * @param {combo} combo
     * @param {newValue} newValue 新选中的值
     * @param {oldValue} oldValue 旧选中的值
     * @param {eOpts} eOpts 
     */
    onAdoTypeChange: function (combo, newValue, oldValue, eOpts) {
        var adoNameBoxControl = Ext.getCmp("adoNameBoxId");
        var store = adoNameBoxControl.getStore();
        store.setData([]);
        adoNameBoxControl.setStore(store);
        adoNameBoxControl.setValue(null);
        adoNameBoxControl.setDisabled(true);

        if (!dispatchTask)
            return;

        if (!newValue)
            return;

        var selectedIds = mainView.getSelectionIds();
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
            method: "GetTaskPerformerInfo",
            params: [selectedIds, dispatchTask.Id, newValue, ""],
            async: false,
            token: mainView.token,
            callback: function (res) {
                if (res.Success) {
                    var taskPerformerInfo = res.Result;
                    if (newValue != "工位") {
                        var store = adoNameBoxControl.getStore();
                        store.setData(taskPerformerInfo.ShiftEmployeeInfos);
                        adoNameBoxControl.setStore(store);
                        adoNameBoxControl.setDisabled(false);
                    }

                    var dragControl = Ext.getCmp('taskPerfomerId');
                    var grid1Id = dragControl.items.items[0].id;
                    var grid1Control = Ext.getCmp(grid1Id);
                    var store = grid1Control.getStore();
                    store.setData(taskPerformerInfo.AdoInfos);
                    grid1Control.setStore(store);

                    var grid2Id = dragControl.items.items[1].id;
                    grid2Control = Ext.getCmp(grid2Id);
                    var store = grid2Control.getStore();
                    store.setData(taskPerformerInfo.SelectedAdoInfos);
                    grid2Control.setStore(store);
                }
            }
        });
    },

    /**
     * 下拉值变更事件
     * @method onAdoNameChange
     * @param {combo} combo
     * @param {newValue} newValue 新选中的值
     * @param {oldValue} oldValue 旧选中的值
     * @param {eOpts} eOpts 
     */
    onAdoNameChange: function (combo, newValue, oldValue, eOpts) {
        var adoTypeBoxControl = Ext.getCmp("adoTypeBoxId");
        var adoType = adoTypeBoxControl.value;
        if (!dispatchTask)
            return;

        if (newValue == null)
            return;

        var selectedIds = mainView.getSelectionIds();
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
            method: "GetTaskPerformerInfo",
            params: [selectedIds, dispatchTask.Id, adoType, newValue],
            async: false,
            token: mainView.token,
            callback: function (res) {
                if (res.Success) {
                    var taskPerformerInfo = res.Result;
                    var dragControl = Ext.getCmp('taskPerfomerId');
                    var grid1Id = dragControl.items.items[0].id;
                    var grid1Control = Ext.getCmp(grid1Id);
                    var store = grid1Control.getStore();
                    store.setData(taskPerformerInfo.AdoInfos);
                    grid1Control.setStore(store);

                    var grid2Id = dragControl.items.items[1].id;
                    grid2Control = Ext.getCmp(grid2Id);
                    var store = grid2Control.getStore();
                    store.setData(taskPerformerInfo.SelectedAdoInfos);
                    grid2Control.setStore(store);
                }
            }
        });
    },
});

/**
 * 派工管理对象Model定义
 * @class SIE.Web.MES.TaskManagement.Dispatchs.Scripts.TaskPerformerModel
 * @constructs
 */
Ext.define('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.TaskPerformerModel', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'AdoName', type: 'string' },
        { name: 'AdoType', type: 'string' },
        { name: 'AdoGroup', type: 'string' },
        { name: 'TaskStatus', type: 'int' },
        { name: 'DispatchTaskId', type: 'float' },
        { name: 'SendQty', type: 'float', allowNull: true },
        { name: 'MatchDegree', type: 'float', allowNull: true },
        { name: 'DispatchEquipment', type: 'string' },
    ],
});

/**
 * 派工管理对象Store定义
 * @class SIE.Web.MES.TaskManagement.Dispatchs.Scripts.TaskPerformerInfo
 * @constructs
 */
Ext.define('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.TaskPerformerInfo', {
    extend: 'Ext.data.Store',
    alias: 'store.taskPerformerInfo',
    model: 'SIE.Web.MES.TaskManagement.Dispatchs.Scripts.TaskPerformerModel',
    autoLoad: false
});

/**
 * 派工管理对象控件控制器定义
 * @class SIE.Web.MES.TaskManagement.Dispatchs.Scripts.GridToGridController
 * @constructs
 */
Ext.define('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.GridToGridController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.gridController',

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
    onAddClick: function (grid, rowIndex, colIndex) {
        var me = this;
        var record = grid.getStore().getAt(rowIndex);
        var task = record.data;
        var selectedIds = mainView.getSelectionIds();
        //if (selectedIds.length == 0)
        //    return;
        //if (task.TaskStatus !== 0 && task.TaskStatus != 10)
        //    return;
        var unSelectTasks = this.lookupReference('grid1').getStore();
        unSelectTasks.remove(record);
        var selectedTasks = this.lookupReference('grid2').getStore();
        selectedTasks.insert(0, {
            SelectedTaskIds: selectedIds,
            DispatchTaskId: task.DispatchTaskId,
            TaskStatus: task.TaskStatus,
            AdoId: task.AdoId,
            AdoName: task.AdoName,
            AdoType: task.AdoType,
            AdoGroup:task.AdoGroup,
            SendQty: task.SendQty,
            MatchDegree: task.MatchDegree,
            DispatchEquipment: task.DispatchEquipment,
            Status: 0,
        });

        // 20231020 派工任务问题

        var saveTaskPerformer = Ext.create('SIE.Web.MES.TaskManagement.Dispatchs.Commands.TaskPerformerCommand');
        saveTaskPerformer.execute(mainView, {
            data: {SelectedTaskIds: selectedIds,
            DispatchTaskId: task.DispatchTaskId,
            TaskStatus: task.TaskStatus,
            AdoId: task.AdoId,
            AdoName: task.AdoName,
            AdoType: task.AdoType,
            AdoGroup:task.AdoGroup,
            SendQty: task.SendQty,
            MatchDegree: task.MatchDegree,
            DispatchEquipment: task.DispatchEquipment,
            Status: 0},
          success: function (res) {
         if (res.Result == true) {
                                           
        }
        }
        });
    },
    onRemoveClick: function (grid, rowIndex, colIndex) {
        var me = this;
        var selectedIds=[];
        var record = grid.getStore().getAt(rowIndex);
        var task = record.data;
        if(!dispatchTask)
           return;
        selectedIds.push(dispatchTask.Id);
        //if (selectedIds.length == 0)
        //    return;
        //if (task.TaskStatus !== 0 && task.TaskStatus != 10)
        //    return;
        var selectedTasks = this.lookupReference('grid2').getStore();
        selectedTasks.remove(record);
        var saveTaskPerformer = Ext.create('SIE.Web.MES.TaskManagement.Dispatchs.Commands.TaskPerformerCommand');
        saveTaskPerformer.execute(mainView, {
           data: { SelectedTaskIds: selectedIds,
            DispatchTaskId: task.DispatchTaskId,
            TaskStatus: task.TaskStatus,
            AdoId: task.AdoId,
            AdoName: task.AdoName,
            AdoType: task.AdoType,
            AdoGroup:task.AdoGroup,
            SendQty: task.SendQty,
            MatchDegree: task.MatchDegree,
            DispatchEquipment: task.DispatchEquipment,
            Status: 1},
          success: function (res) {
         if (res.Result == true) {
                                           
        }
        }
      });
    },
    onShowTaskDetailClick: function (grid, rowIndex, colIndex) {
        var task = grid.getStore().getAt(rowIndex).data;
        var showTaskDetail = Ext.create('SIE.Web.MES.TaskManagement.Dispatchs.Commands.ShowTaskDetailCommand');
        showTaskDetail.execute(mainView, task);
    },
});

/**
 * 派工管理对象控件定义
 * @class SIE.Web.MES.TaskManagement.Dispatchs.Scripts.GridToGrid
 * @constructs
 */
Ext.define('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.GridToGrid', {
    extend: 'Ext.container.Container',
    xtype: 'gridtogrid',
    controller: 'gridController',

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
        title: '可选对象'.t(),
        width: 325,
        flex: 1,
        multiSelect: true,
        margin: '0 5 0 0',
        listeners: {
            onResetGrid1Click: 'onResetGrid1Click'
        },
        store: {
            type: 'taskPerformerInfo'
        },
        columns: [{
            xtype: 'rownumberer'
        }, {
            width: 40,
            align: 'center',
            xtype: 'actioncolumn',
            iconCls: 'iconfont icon-AddEntity icon-green',
            items: [{
                handler: "onAddClick",
                isActionDisabled: function (view, rowIndex, colIndex, item, record) {
                    var isDisable = false;
                    var selectedIds = mainView.getSelectionIds();
                    if (selectedIds.length == 0) {
                        isDisable = true;
                    }
                    else {
                        if (dispatchTask.TaskStatus !== 0 && dispatchTask.TaskStatus != 10)
                            isDisable = true;
                        else if (mainView.isSelectedTaskPerformer) {
                            isDisable = true;
                        }
                        else
                            isDisable = false;
                    }
                    return isDisable;
                },
                getTip: function (value, metadata, record, row, col, store) {
                    return '添加执行对象'.t();
                }
            }],
        }, {
                text: "对象名称".t(),
            width: 120,
            sortable: true,
            dataIndex: 'AdoName'
        }, {
                text: "对象类型".t(),
            width: 120,
            sortable: true,
            dataIndex: 'AdoType'
        }, {
                text: "已派任务单数".t(),
            width: 120,
            sortable: true,
            dataIndex: 'SendQty'
        }, {
                text: '查看任务详细'.t(),
            xtype: 'actioncolumn',
            align: 'center',
            width: 110,
            iconCls: 'iconfont icon-PageSearch icon-blue',
            handler: 'onShowTaskDetailClick',
        }, {
                text: "工序技能匹配度".t(),
            width: 120,
            sortable: true,
            dataIndex: 'MatchDegree'
        }]
    }, {
        xtype: 'grid',
        reference: 'grid2',
            title: '已选对象'.t(),
        width: 325,
        flex: 1,
        stripeRows: true,
        listeners: {
            onResetGrid2Click: 'onResetGrid2Click'
        },
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
        store: {
            type: 'taskPerformerInfo'
        },
        columns: [{
            xtype: 'rownumberer'
        }, {
            text: '对象名称'.t() + '<span style="color: red;">*</span>',
            width: 120,
            sortable: true,
            dataIndex: 'AdoName'
        }, {
            text: '对象类型'.t() +'<span style="color: red;">*</span>',
            width: 120,
            sortable: true,
            dataIndex: 'AdoType'
        },
        //{
        //text: "指定设备",
        //width: 120,
        //sortable: true,
        //    dataIndex: 'DispatchEquipment',
        //    editor: {
        //        xtype: 'gridcombopopup',
        //        //typeAhead: false,
        //        //triggerAction: 'all',

        //    }
        //},
        {
            xtype: 'actioncolumn',
            width: 40,
            sortable: false,
            menuDisabled: true,
            align: 'center',
            iconCls: 'iconfont icon-DeleteEntity icon-red',
            items: [{
                handler: "onRemoveClick",
                isActionDisabled: function (view, rowIndex, colIndex, item, record) {
                    var isDisable = false;
                    var selectedIds = mainView.getSelectionIds();
                    if (dispatchTask === null) {
                        isDisable = true;
                    }
                    else {
                        if (dispatchTask.TaskStatus !== 0 && dispatchTask.TaskStatus != 10)
                            isDisable = true;
                        else
                            isDisable = false;
                    }
                    return isDisable;
                },
                getTip: function (value, metadata, record, row, col, store) {
                    return '移除已选对象'.t();
                }
            }]
        }]
    }]
});