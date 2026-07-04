Ext.define('SIE.Web.MES.TeamManagement.ShiftSchedules.SchedulePage', {
    extend: 'SIE.Page',
    beforeLoad: function (args) {
        this.isCustomize = true;
    },
    onLoad: function () {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        var shiftSchedule = new SIE.Web.MES.TeamManagement.ShiftSchedules();
        var control = shiftSchedule.initControl(params);
        Ext.create('Ext.container.Viewport', {
            layout: {
                type: 'border'
            },
            border: 0,
            defaults: {
                layout: 'fit'
            },
            items: {
                region: 'center',
                items: control
            },
            renderTo: Ext.getBody()
        });
    },
    onShow: function (args) {
    }
});

//SIE:classEnd
Ext.define('SIE.Web.MES.TeamManagement.ShiftSchedules', {
    mixins: ['Ext.mixin.Observable'],

    /**
     * @property {ShiftScheduleViewModel}
     * 排班主视图 
     */
    _mainView: null,

    /**
     * @property {scheduleCalendar}
     * 日历控件 
     */
    _calendar: null,

    /**
     * @property {字典}
     * 排班班次字典
     */
    _shifts: {},
    /**
     * @property {Boolean}
     * 是否运行中，避免多次触发导致多次请求程序卡死 
     */
    _isRunning: false,
    /**
     * @property {Boolean}
     * 是否月变更中，避免多次触发导致多次请求程序卡死 
     */
    monthChanging: false,

    /**
     * @property {Boolean}
     * 是否已经创建Store,减少重复创建 Store
     */
    isCreateStore: false,

    /**
     * @property {集合}
     * 排班信息集合
     */
    schedules: [],

    /**
     * @property {ChangeWorkGroupViewModel}
     * 班组切换视图模型
     */
    workGroupVm: null,

    /**
     * @property {selectWorkGroupId}
     * 已选中班组ID集合
     */
    selectWorkGroupId: [],
    /**
     * 
     */
    lastSelectWorkGroupId: "",
    listeners: {
        loadComplete: function () {
        }
    },

    constructor: function constructor() {
        var me = this;
        me.callParent(arguments);
        me.mixins.observable.constructor.call(me, arguments);
    },

    initControl: function (params) {
        var me = this;
        var module = params.module;
        var meta = null;
        SIE.AutoUI.getMeta({
            async: false,
            isAggt: true,
            module: module,
            model: "SIE.MES.TeamManagement.ShiftSchedules.ShiftScheduleViewModel",
            callback: function (res) {
                meta = res;
            }
        });
        var ui = SIE.AutoUI.generateAggtControl(meta);
        me._mainView = ui.getView();
        me._mainView.MainContainer = me;
        var mainControl = me._mainView.getControl();
        var dockedItems = mainControl.getDockedItems();
        //removeAll()会将toolbar项也清除掉
        var toolbar = dockedItems.first(function (p) { return p.xtype === 'toolbar'; });
        if (toolbar) {
            dockedItems.remove(toolbar);
        }
        //移除命令跟分页 
        var gridpager = dockedItems.first(function (p) { return p.xtype === 'gridpager'; });
        if (gridpager) {
            dockedItems.remove(gridpager);
        }
        var control = this.layout(me._mainView, toolbar);
        var view = ui.getView();
        me.registerEvent(view);
        control.view = view;
        return control;
    },

    /****************************************控件相关*****************************************/

    /**
     * 注册视图事件
     * @param {ListLogicalView} view 主逻辑视图
     */
    registerEvent: function (view) {
        view.mon(view, 'beforeClosewin', this.beforeClosewin, this);
    },

    /**
    * 排班标签页关闭前事件，取消消息订阅
    * @param {returnObj} returnObj 关闭结果
    */
    beforeClosewin: function (returnObj) {
        //取消消息订阅
        var me = this;
        me._shifts = {};
        me.schedules.removeAll();
        var view = me.view;
        view.mun(view, 'beforeClosewin');
        me._calendar.mun(me._calendar, 'monthChanged');
        me.mun(me, 'loadComplete');
    },

    /**
     * 初始化排班界面布局
     * @param {ListLogicalView} view 排班列表逻辑视图 ShiftScheduleViewModel
     * @param {Ext.toolbar.Toolbar} toolbar 命令控件
     * @returns {container} 整体界面布局
     */
    layout: function (view, toolbar) {
        var me = this;
        me._calendar = this.initCalendar(view.getToken());
        var conditionContol = this.initConditionContol();
        var scheduleControl = this.initScheduleControl(conditionContol, me._calendar);  //排班控件
        var mainControl = this.initMainControl(toolbar, scheduleControl);  //右侧控件
        return Ext.widget('container', {
            border: false,
            layout: 'border',
            scrollable: false,
            defaults: {
                layout: 'fit',
                border: false
            },
            items: [Ext.merge({
                title: '请先选择资源与车间查询排班情况'.t(),
                items: view.getConditionView().getControl()
            }, GlobalConfig.defaultConditionPanelConfig), {
                region: 'center',
                items: mainControl
            }]
        });
    },

    /**
     * 初始化排班主界面布局
     * @param {Ext.toolbar.Toolbar} toolbar 命令控件
     * @param {panel} scheduleControl 主界面右侧排班控件
     * @returns {container} 右侧排班布局
     */
    initMainControl: function (toolbar, scheduleControl) {
        return {
            xtype: 'container',
            layout: 'border',
            border: false,
            margin: 1,
            items: [{
                region: 'north',
                baseCls: 'my-panel-no-border',
                items: toolbar  //命令栏
            }, {
                region: 'center',
                layout: 'fit',
                baseCls: 'my-panel-no-border',
                items: scheduleControl
            }]
        };
    },

    /**
     * 初始化排班条件控件
     * @returns {ListLogicalView} 排班条件控件
     */
    initConditionContol: function () {
        Ext.tip.QuickTipManager.init();
        var workGroupContol = this.initWorkGroupControl();
        var weekControl = this.initWeekContorl();
        var shiftControl = this.initShiftControl();
        return {
            xtype: 'panel',
            border: false,
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'start'
            },
            style: 'padding:3px',
            items: [{
                items: weekControl,
                border: false
            }, {
                items: shiftControl,
                border: false
            }, {
                items: workGroupContol,
                border: false
            }
            ]
        };
    },

    /**
     * 初始化可排班班次控件
     * @returns {panel} 可排班班次控件
     */
    initShiftControl: function () {
        return {
            xtype: 'panel',
            border: false,
            layout: {
                type: 'hbox',
                align: 'stretch',
                pack: 'start'
            },
            style: 'margin:5px 10px 0px 10px',
            scrollable: true,
            items: [
                {
                    xtype: 'label',
                    text: '排班班次(必选)'.t(),
                }, {
                    xtype: 'panel',
                    id: 'shiftControl',
                    border: false,
                    layout: {
                        type: 'table',
                        tdAttrs: { style: 'padding: 0px 5px;' }
                    },
                    defaults: {
                        xtype: 'button'
                    }
                }]
        };
    },

    /**
     * 初始化排班班组控件
     * @returns {panel} 排班班组控件
     */
    initWorkGroupControl: function () {
        return {
            xtype: 'panel',
            border: false,
            style: 'margin:5px 10px 0px 10px',
            scrollable: true,
            layout: {
                type: 'hbox',
                align: 'stretch',
                pack: 'start'
            },
            items: [{
                xtype: 'label',
                text: '排班班组(必选)'.t()
            }, {
                xtype: 'container',
                layout: 'center',
                style: {
                    background: '#FFFFFF',
                    margin: '0px 5px 0px 5px'
                },
                items: [{
                    xtype: 'button',
                    text: '点击选择班组'.t(),
                    page: this,
                    id: 'btnSelectGroup',
                    tooltip: "选择要排班班组，然后点击班组排班".t(),
                    handler: this.onSelectWorkGroup,
                    style: 'height:20px;width:88px;background:#FF9912;border-width:0px;padding: 0px 0px 0px 5px;'
                }]
            },
            {
                xtype: 'container',
                layout: 'center',
                style: {
                    background: '#FFFFFF',
                    margin: '0px 5px 0px 5px'
                },
                items: [{
                    xtype: 'button',
                    text: '清空班次排班'.t(),
                    page: this,
                    id: 'btnReset',
                    tooltip: "清空当前所选班次的排班记录".t(),
                    handler: this.clearSchedule,
                    style: 'height:20px;width:88px;background:#E3170D;border-width:0px;padding: 0px 0px 0px 5px;'
                }]
            },
            {
                xtype: 'segmentedbutton',
                id: 'workGroupControl',
                layout: {
                    type: 'table',
                    align: 'stretch',
                    tdAttrs: {
                        style: {
                            padding: '0px 10px 0px 0px'
                        }
                    }
                },
                defaults: {
                    border: false
                },


            }]
        };
    },

    /**
     * 初始化排班周期控件
     * @returns {checkboxgroup} 排班周期控件
     */
    initWeekContorl: function () {
        return {
            xtype: 'checkboxgroup',
            fieldLabel: '按星期'.t(),
            id: 'weekControl',
            style: 'margin:0px 10px 0px 10px',
            labelWidth: 50,
            defaults: {
                width: 75,
                checked: true
            },
            items: [{
                boxLabel: '周日'.t(),
                inputValue: 64,
                checked: false
            }, {
                boxLabel: '周一'.t(),
                inputValue: 1
            }, {
                boxLabel: '周二'.t(),
                inputValue: 2
            }, {
                boxLabel: '周三'.t(),
                inputValue: 4
            }, {
                boxLabel: '周四'.t(),
                inputValue: 8
            }, {
                boxLabel: '周五'.t(),
                inputValue: 16
            }, {
                boxLabel: '周六'.t(),
                inputValue: 32,
                checked: false
            }]
        };
    },

    /**
     * 初始化右侧排班布局
     * @param {panel} conditionContol 排班条件控件
     * @param {SIE.Web.MES.TeamManagement.scheduleCalendar} calendar 主界面右侧排班控件
     * @returns {panel} 右侧排班布局
     */
    initScheduleControl: function (conditionContol, calendar) {
        return {
            xtype: 'panel',
            layout: 'border',
            border: false,
            items: [{
                region: 'north',
                items: conditionContol
            }, {
                region: 'center',
                items: calendar,
                layout: 'fit'
            }]
        };
    },

    /**
     * 初始化排班日历控件
     * @param {String} token 票据
     * @returns {SIE.Web.MES.TeamManagement.scheduleCalendar} 排班日历控件
     */
    initCalendar: function (token) {
        var me = this;
        //创建日历面板
        var calendar = Ext.create('SIE.Web.MES.TeamManagement.scheduleCalendar', {
            id: 'calendarControl',
            border: false,
            pageScope: me
        });
        var filter = {
            token: token,
            Method: 'GetSchedules',
            Parameters: []
        };
        var store = Ext.create("Ext.calendar.store.Calendars", {
            model: 'Ext.calendar.model.Calendar',
            data: [{
                id: 1,
                title: ""
            }],
            eventStoreDefaults: {
                model: 'Ext.calendar.model.Event',
                proxy: {
                    type: 'ajax',
                    url: '/api/DataPortal/Query',
                    actionMethods: {
                        read: 'POST' // Store设置请求的方法，与Ajax请求有区别
                    },
                    sortable: false,
                    extraParams: {
                        action: 'queryer',
                        type: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleDataQueryer',
                        filter: SIE.data.Utils.seriaizeRequest(filter),
                        token: token
                    },
                    reader: {
                        type: 'json',
                        rootProperty: 'Result'
                    }
                }
            }
        });
        calendar.setStore(store);
        calendar.mon(calendar, 'monthChanged', me.monthChanged, me);
        return calendar;
    },

    /****************************************逻辑相关*****************************************/

    /**
     * 月变更事件
     */
    monthChanged: function () {
        var me = this;
        me.setCalendarData(me.schedules);
    },

    /**
     * 班组选择事件，弹框选择待排班班组 
     */
    onSelectWorkGroup: function () {
        var me = this.page;
        SIE.AutoUI.getMeta({
            model: 'SIE.Resources.Employees.WorkGroup',
            ignoreChild: true,
            ignoreCommands: true,
            ignoreQuery: false,
            isAggt: true,
            viewGroup: 'ShiftScheduleSelectView',
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var ui = SIE.AutoUI.generateAggtControl(res);
                ui.Main = me;
                me.showWorkGroupDialog(ui);
            }
        });
    },
    /**
     * 清空已有班次
     */
    clearSchedule: function (me) {
        try {
            var me = this.page;
            if (me.schedules == null) {
                throw new Error('未找到预排班记录'.t());
            }
            if (me.schedules.length === 0) {
                throw new Error('未找到预排班记录'.t());
            }
            var schedules = me.schedules;
            var shiftId = me.getSelectShift();
            if (shiftId == undefined || shiftId == 0) {
                throw new Error('请先选择要清空的班次，再点击清空按钮!'.t());
            }
            schedules.forEach(function (item) {
               
                if (item != null && item.ShiftId == shiftId) {
                    item.WorkGroupId = 0;
                    item.WorkGroupName = " ";
                    item.title = " ";
                }
            });
            debugger;
            me.setCalendarData(schedules);
            SIE.Msg.showMessage('操作成功！'.t());
        } catch (e) {
            SIE.Msg.showError(e.message);
        }
    },

    /**
     * 弹出班组选择界面
     * @param {CotrolResult} ui 班组控件结果
     */
    showWorkGroupDialog: function (ui) {
        var me = this;
        var workGroupView = ui._view;
        SIE.Window.show({
            title: '班组选择'.t(),
            items: ui.getControl(),
            width: 800, height: 500,
            callback: function (btn) {
                if (btn === '确定'.t()) {
                    var workGroupControl = Ext.getCmp('workGroupControl');
                    if (!workGroupControl)
                        return;
                    workGroupControl.removeAll();
                    var selection = workGroupView.getSelection();
                    me.selectWorkGroupId = selection.select(function (p) { return p.data.Id; });
                    if (selection.length <= 0)
                        return;
                    for (i = 0; i < selection.length; i++) {
                        debugger;
                        var workGroup = selection[i].data;
                        workGroupControl.add({
                            xtype: 'button',
                            text: workGroup.Name,
                            workGroupId: workGroup.Id,
                            handler: ui.Main.schedule,
                            pageScope: ui.Main,
                            Height: '40px',
                            tooltip: workGroup.Name,
                            style: 'background: #91C1FD;border-radius:3px;'
                        });
                    }
                }
            }
        });
        workGroupView.mon(workGroupView, 'ondataloaded', function (res) {
            me.setSelected(workGroupView, res);
        }, me);
        workGroupView.loadData({
            callback: function (res) {
                me.setSelected(workGroupView, res);
            }
        });
    },

    /**
     * 设置已选中班组
     * @param {ListLogicalView} view 班组控件结果
     * @param {Object} res 班组控件结果
     */
    setSelected: function (view, res) {
        if (!view.getData().data)
            return;
        var me = this;
        var records = view.getData().data.items;
        var selectIds = me.selectWorkGroupId;
        if (records && records.length > 0 && selectIds && selectIds.length > 0) {
            var model = view.getSelectionModel();
            var recordList = records.where(function (p) { return selectIds.contains(p.data.Id); });
            recordList.forEach(function (record) {
                model.select(record, true, true);
            });
        }
    },
    showSelected: function (btn) {

        var shiftControl = Ext.getCmp('shiftControl');
        debugger;
        btn.pressed = true;
        btn.btnInnerEl.dom.style = "color:#000000";
        if (shiftControl.items.length > 0) {
            for (var i = 0; i < shiftControl.items.length; i++) {
                if (shiftControl.items.items[i].id != btn.id) {
                    shiftControl.items.items[i].pressed = false;
                    shiftControl.items.items[i].btnInnerEl.dom.style = "color:#FFFFFF";

                }
            }
        }
    },


    /**
     * 预排班命令逻辑
     */
    schedule: function () {

        var me = this;
        var command = me.pageScope;
        try {
            if (command._isRunning)
                return;
            command._isRunning = true;
            //存在预排班需先询问后再排班
            var newSchedules = command.isNewSchedule(false);
            if (newSchedules.length > 0) {
                SIE.Msg.askQuestion('页面存在未保存数据，是否先保存再进行操作？'.t(),
                    function () {
                        command.saveShiftSchedule(command._mainView.token);
                        command.newSchedules(me, command);
                    }, function () {
                        if (me.id == command.lastSelectWorkGroupId) {
                            me.setStyle({ background: '#0773FD' });
                        } else {
                            me.setStyle({ background: '#91C1FD' });
                        }
                    });
                //SIE.Msg.askQuestion('是否覆盖已有预排班信息？',
                //    function () {
                //        command.newSchedules(me, command);
                //        command.lastSelectWorkGroupId = me.id;
                //    }, function () {
                //        if (me.id == command.lastSelectWorkGroupId) {
                //            me.setStyle({ background: '#0773FD' });
                //        } else {
                //            me.setStyle({ background: '#91C1FD' });
                //        }
                //    });
            } else {
                command.newSchedules(me, command);
                command.lastSelectWorkGroupId = me.id;
            }

        } catch (e) {

            SIE.Msg.showError(e.message);
        } finally {
            command._isRunning = false;
        }
    },
    /**
     * 新排班
     * @param {any} me
     * @param {any} command
     * @returns
     */
    newSchedules(me, command) {
        var shiftId = me.shiftId;
        if (shiftId == undefined || shiftId == 0) {
            shiftId = command.getSelectShift();
        }
        var workGroupId = command.getSelectWorkGroup();
        var week = command.getSelectWeek();
        var criteria = command._mainView.getConditionView().getCurrent().data;
        if (!command.validateSchedule(criteria, workGroupId, shiftId, week)) {
            me.setStyle({ background: '#91C1FD' });
            return;
        }
        var workGroupControl = Ext.getCmp('workGroupControl');
        if (!workGroupControl)
            return null;
        workGroupControl.items.items.forEach(function (button) {
            if (button.id != me.id) {
                button.setStyle({ background: '#91C1FD' });
            } else {
                button.setStyle({ background: '#0773FD' });
            }
        });



        SIE.invokeDataQuery({
            method: 'GetHeforehandScheduleInfos',
            params: [criteria, week, workGroupId, shiftId, command._shifts],
            action: 'queryer',
            async: false,
            type: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleDataQueryer',
            token: command._mainView.getToken(),
            success: function (res) {
                if (res.Success) {
                    command.setCalendarData(res.Result);
                    SIE.Msg.showMessage('预排班完成！'.t());
                }
                if (!res.Success) {
                    SIE.Msg.showMessage(res.Message);
                }
            }
        });
    },

    /**
     * 获取排班班组ID
     * @returns {Number} 排班班组ID
     */
    getSelectWorkGroup: function () {
        var workGroupControl = Ext.getCmp('workGroupControl');
        if (!workGroupControl)
            return null;
        var workGroupId = 0;
        workGroupControl.items.items.forEach(function (button) {
            if (button.pressed === true)
                workGroupId = button.workGroupId;
        });
        return workGroupId;
    },
    /**
     * 获取选中班次
     * @returns
     */
    getSelectShift: function () {
        var shiftControl = Ext.getCmp('shiftControl');
        if (!shiftControl)
            return null;
        var shiftId = 0;
        shiftControl.items.items.forEach(function (button) {
            if (button.pressed === true)
                shiftId = button.shiftId;
        });
        return shiftId;
    },

    /**
     * 获取排班周期
     * @returns {Number} 排班周期枚举值
     */
    getSelectWeek: function () {
        var weekControl = Ext.getCmp('weekControl');
        var week = weekControl.getValue().weekControl;
        var value = 0;
        if (week === undefined)  //没选择
            return value;
        if (Ext.isNumber(week))  //选择一个
            return week;
        week.forEach(function (w) {  //选择多个
            value += parseInt(w);
        });
        return value;
    },

    /**
     * 验证排班查询条件
     * @param {ScheduleCriteria} criteria 排班查询实体ScheduleCriteria
     * @returns {Boolean} 查询条件验证通过返回true，否则返回false
     */
    validateCriteria: function (criteria) {
        var me = this;
        if (criteria === null)
            return false;
        if (criteria.WorkShopId === null || criteria.WorkShopId === 0) {
            me.showMessage('车间不能为空'.t());
            return false;
        }
        if (criteria.WipResourceId === null || criteria.WipResourceId === 0) {
            me.showMessage('资源不能为空'.t());
            return false;
        }
        if (criteria.ScheduleDate.BeginValue === null || criteria.ScheduleDate.EndValue === null) {
            me.showMessage('排班日期不能为空'.t());
            return false;
        }
        if (criteria.ScheduleDate.BeginValue > criteria.ScheduleDate.EndValue)
            return false;
        return true;
    },

    /**
     * 验证排班条件
     * @param {ScheduleCriteria} criteria 排班查询实体ScheduleCriteria
     * @param {Number} workGroupId 排班班组ID
     * @param {Number} shiftId 排班班次ID
     * @param {Number} week 排班周期
     * @returns {Boolean} 排班条件验证通过返回true，否则返回false
     */
    validateSchedule: function (criteria, workGroupId, shiftId, week) {
        var me = this;
        if (!me.validateCriteria(criteria))
            return false;
        if (workGroupId === null || workGroupId === 0) {
            me.showMessage('请选择排班班组'.t());
            return false;
        }
        if (shiftId === null || shiftId === 0) {
            me.showMessage('请选择排班班次'.t());
            return false;
        }
        if (week < 1) {
            me.showMessage('请选择排班周期'.t());
            return false;
        }
        return true;
    },

    /**
     * 弹框提示消息
     * @param {String} message 提示内容
     */
    showMessage: function (message) {
        Ext.Msg.alert('提示'.t(), message.t());
    },

    /**
     * 判断排班页签是否已打开，打开激活页签
     * @param {Number} id 页签ID
     * @returns {Boolean} 已打开返回true，否则返回false
     */
    isTabOpen: function (id) {
        var key = 'tab_' + id;
        var tabPanel = portal.getTabPanel();
        var tab = tabPanel.items.getByKey(key);
        if (tab) {
            tabPanel.setActiveTab(tab);
            return true;
        }
        return false;
    },

    /**
     * 刷新可排班班次字典，用于加载排班信息背景色
     * @param {字典} shifts 命令控件
     * @returns {字典} 可排班班次字典
     */
    refreshShift: function (shifts) {
        var me = this;
        var shiftControl = Ext.getCmp('shiftControl');
        if (!shiftControl)
            return;
        var scheduleShifts = [];
        shifts.forEach(function (shift) {
            me._shifts[shift.id] = shift.background;
            var btnConfig = {
                xtype: 'button',
                text: Ext.String.format('{0} {1}', shift.shiftName, shift.shiftTime),
                shiftId: shift.id,
                handler: me.showSelected,
                pageScope: me,
                disabled: shift.isExpire,
                style: Ext.String.format('border:0px;background:{0};border-radius:3px;', shift.background)
            };
            if (!shift.isExpire) {
                scheduleShifts.push(btnConfig);
            }
            else {
                Ext.merge(btnConfig, { style: Ext.String.format('border:1px dashed #C2C2C2;background:{0}', shift.background) });
                scheduleShifts.unshift(btnConfig);
            }
        });
        shiftControl.removeAll();
        if (scheduleShifts.length > 0)
            shiftControl.add(scheduleShifts);
        return me._shifts;
    },

    /**
     * 加载排班数据
     * @param {ScheduleCriteria} criteria 排班查询实体ScheduleCriteria
     * @param {String} token 排班登陆票据
     */
    loadScheduleData: function (criteria, token) {
        var me = this;
        try {
            if (me._isRunning)
                return;
            me._isRunning = true;
            if (!me.validateCriteria(criteria))
                return;
            //先查询出班次信息，设置班次颜色后再查询排班信息
            SIE.invokeDataQuery({
                method: 'GetShifts',
                params: [criteria],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        var shifts = res.Result;
                        var configShfit = me.refreshShift(shifts);
                        me.loadDate(configShfit, criteria, token);
                    }
                }
            });
        } catch (e) {
            SIE.Msg.showError(e.message);
        } finally {
            me._isRunning = false;
        }
    },

    /**
     * 加载排班数据
     * @param {Array} configShfit 可排班班次字典
     * @param {ScheduleCriteria} criteria 排班查询实体ScheduleCriteria
     * @param {String} token 票据
     */
    loadDate: function (configShfit, criteria, token) {
        var me = this;
        var calendar = me._calendar;
        if (!me.validateCriteria(criteria))
            return;
        SIE.invokeDataQuery({
            method: 'GetShiftSchedules',
            params: [criteria, configShfit],
            action: 'queryer',
            async: false,
            type: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleDataQueryer',
            token: token,
            callback: function (res) {
                if (res.Success) {
                    me.setCalendarData(res.Result);
                }
                if (!res.Success) {
                    SIE.Msg.showMessage(res.Message);
                }
            }
        });
    },

    /**
     * 设置排班数据
     * eventSource不支持批量添加
     * @param {Array} datas 排版数据 需转换成Ext.calendar.model.Event
     */
    setCalendarData: function (datas) {
        var me = this;
        try {
            if (me.monthChanging) return;
            me.monthChanging = true;
            var calendar = Ext.getCmp('calendarControl');
            if (!calendar)
                return;
            calendar.store.eventSource.removeAll();
            for (var i = 0, length = datas.length; i < length; i++) {
                var result = datas[i];
                var model = new Ext.calendar.model.Event();
                var data = model.data;
                Ext.merge(data, result);
                data.startDate = new Date(data.startDate);
                data.endDate = new Date(data.endDate);
                calendar.store.eventSource.add(model);
            }
            me.schedules = datas;
            me.fireEvent('loadComplete');
        } catch (e) {
            me.fireEvent('loadComplete');
            SIE.Msg.showError(e.message);
        } finally {
            me.monthChanging = false;
        }
    },

    /**
     * 重新加载排班日历，重新出发查询
     */
    refreshCalendar: function () {
        var me = this;
        var queryView = me._mainView._relations[0];
        if (queryView) {
            var cmd = queryView._target.findCmd(SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleQueryInfo);
            if (cmd)
                cmd.execute(queryView._target);
        }
    },

    /**
     * 保存预排班数据
     * @param {String} token 票据
     */
    saveShiftSchedule: function (token) {
        try {
            var me = this;
            if (me._isRunning)
                return;
            me._isRunning = true;
            var newSchedules = me.isNewSchedule(true);
            SIE.invokeDataQuery({
                method: 'SaveShiftSchedules',
                params: [newSchedules],
                action: 'queryer',
                type: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        me.refreshCalendar();
                        SIE.Msg.showMessage('排班成功'.t());
                    }
                    if (!res.Success) {
                        SIE.Msg.showMessage(res.Message);
                    }
                }
            });
        } catch (e) {
            SIE.Msg.showError(e.message);
        } finally {
            me._isRunning = false;
        }
    },

    /**
     * 判断是否存在预排班记录
     * @param {Boolean} isThrowExc 是否抛异常，预排班时不抛异常
     * @returns {Array} 待保存的预排班记录列表
     */
    isNewSchedule: function (isThrowExc) {
        var me = this;
        if (me.schedules.length === 0 && isThrowExc) {
            throw new Error('未找到预排班记录'.t());
        }
        var schedules = me.schedules;
        var toSave = [];
        schedules.forEach(function (schedule) {
              if (schedule.IsNew === true)
                toSave.push(schedule);
        });
        if (toSave.length < 1 && isThrowExc) {
            throw new Error('未找到预排班记录'.t());
        }
        return toSave;
    },

    /**
     * 切换班组
     * @param {Object} event 排班信息
     */
    changeWorkGroup: function (event) {
        var me = this;
        if (!me.workGroupVm)
            me.workGroupVm = new SIE.Web.MES.TeamManagement.ShiftSchedules.ViewModels.ChangeWorkGroupViewModel();
        var vm = me.workGroupVm;
        vm.setWorkGroupId(event.WorkGroupId);
        vm.setWorkGroupId_Display(event.WorkGroupName);
        var viewMeta = null;
        SIE.AutoUI.getMeta({
            async: false,
            isDetail: true,
            ignoreQuery: true,
            model: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ViewModels.ChangeWorkGroupViewModel',
            callback: function (meta) {
                viewMeta = meta;
                viewMeta.token = me._mainView.token;
            }
        });
        var cfg = {
            associateCmd: me,
            entity: vm,
            disableWinAutoSize: true,
            viewMeta: viewMeta,
            title: '班组切换'.t(),
            confirm: function (isNoSave) {
                //弹窗的确认后回调 event.WorkGroupId不可空，vm.data.WorkGroupId可空，传过去不能为空 
                if (vm.data.WorkGroupId === event.WorkGroupId)
                    return;
                if (vm.data.WorkGroupId === null)
                    vm.data.WorkGroupId = 0;
                event.WorkGroupId = vm.data.WorkGroupId;
                SIE.invokeDataQuery({
                    method: 'ChangeWorkGroup',
                    params: [event],
                    action: 'queryer',
                    type: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleDataQueryer',
                    token: viewMeta.token,
                    success: function (res) {
                        if (res.Success) {
                            me.refreshCalendar();
                            SIE.Msg.showMessage('班组切换成功'.t());
                        }
                        if (!res.Success) {
                            SIE.Msg.showMessage(res.Message);
                        }
                    }
                });
            }
        };
        SIE.App.showDialog(cfg);
    },
});

//SIE:classEnd
/**
 * 排班日历控件定义  
 */
Ext.define("SIE.Web.MES.TeamManagement.scheduleCalendar", {
    extend: 'Ext.calendar.panel.Panel',
    xtype: 'scheduleCalendar',
    views: {
        day: null,
        week: null,
        month: {
            xtype: 'calendar-month',
            titleTpl: '{start:date("M Y")}',
            label: '月'.t(),
            titleAlign: 'center',
            weight: 30,
            allowSelection: true,
            sortable: false,
            draggable: false,
            listeners: {
                beforeeventadd: function (calendar, context, eOpts) {
                    return false;
                },
                beforeeventedit: function (calendar, context, eOpts) {
                    //日期大于今天的可以排班
                    var me = this;
                    var event = context.event.data;
                    if (me.validateSchedule(event)) {
                        var calendarControl = Ext.getCmp("calendarControl");
                        if (calendarControl && calendarControl.pageScope)
                            calendarControl.pageScope.changeWorkGroup(event);
                    } else {
                        Ext.Msg.alert('提示'.t(), "无法切换班组，原因：时间早于今天或班次已开始！".t());
                    }
                    return false;
                }
            },
            validateSchedule: function (event) {
                var scheduleDate = Ext.Date.clearTime(event.startDate);
                return scheduleDate > Ext.Date.clearTime(new Date());
            }
        }
    },
    sideBar: null,
    timezoneOffset: 0,
    todayButton: {
        text: '今天'.t()
    },
    listeners: {
        /**
         * 自定义事件，月份变更事件
         * 点击上一月、下一月、今天按钮时触发，重新刷界面数据
         */
        monthChanged: function () {
        }
    },
    privates: {
        /**
         * 今天按钮点击事件
         */
        onTodayTap: function () {
            var me = this;
            if (me.monthChanging) return;
            this.setValue(new Date());
            me.fireEvent('monthChanged');
        }
    },

    /**
     * 下一月按钮点击事件
     */
    moveNext: function () {
        var me = this;
        if (me.monthChanging) return;
        this.getView().moveNext();
        me.fireEvent('monthChanged');
    },

    /**
     * 上一月按钮点击事件
     */
    movePrevious: function () {
        var me = this;
        if (me.monthChanging) return;
        this.getView().movePrevious();
        me.fireEvent('monthChanged');
    },
    afterRender: function () {
        this.callParent();
        ////由于日历外层嵌套一层panel，导致无法修改event，在渲染后取出外层panel
        var childPanels = Ext.ComponentQuery.query('#calendarControl panel');
        childPanels.forEach(function (child) {
            var childDom = child.el.dom;
            if (childDom.className === 'x-panel x-border-item x-box-item x-panel-default' && child.referenceKey === 'sideBar')
                childDom.style.display = 'none';
        });
    }
});