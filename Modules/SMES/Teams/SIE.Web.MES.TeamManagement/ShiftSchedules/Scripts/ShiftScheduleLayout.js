/**
 * 排班表布局
 */
Ext.define('SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'ShiftScheduleLayout',

    /**
     * @property {整型}
     * 当前显示年份
     */
    year: null,

    /**
     * @property {整型}
     * 开始年份
     */
    startYear: null,

    /**
     * @property {整型}
     * 结束年份
     */
    endYear: null,

    /**
     * @property {整型}
     * 开始显示的月份,从0开始计数
     */
    startMonth: null,

    /**
     * @property {整型}
     * 当前月份
     */
    currentMonth: null,

    /**
     * @property {整型}
     * 结束月份
     */
    endMonth: null,

    /**
     * @property {数组}
     * 周数组
     */
    weeks: null,

    /**
     * @property
     * 主视图 GridPanel视图
     * 界面视图
     */
    view: null,

    /**
     * @property {逻辑视图}
     * 主逻辑视图非界面视图，用于挂事件 实体类型ShiftSchedule
     */
    logicView: null,

    /**
     * @property {GridPanel}
     * 主视图控件
     */
    control: null,

    /**
     * 初始化界面布局
     * @param {Object} regions 聚合块
     * @returns {container} 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;
        regions.main._view._relations[0]._target.mainLayout = me;
        //逻辑视图，非界面主视图，用于挂事件
        me.logicView = regions.main.getView();
        me.registerEvent(me.logicView);
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });
        //创建排班表控件
        var mainControl = me.createGridPanel();
        me.control = mainControl;
        me.view = me.control.getView();
        var shiftControl = me.createShiftControl();
        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [{
                region: 'north',
                items: toolbar,
                border: false
            }, {
                region: 'center',
                layout: 'border',
                xtype: 'panel',
                border: false,
                items: [{
                    region: 'north',
                    items: shiftControl,
                    height: 50,
                    border: false
                }, {
                    region: 'west',
                    xtype: 'container',
                    layout: 'center',
                    width: 80,
                    style: {
                        background: '#FFFFFF'
                    },
                    scheduleTableScope: me,
                    items: this.createButton('btnPrevious', 'iconfont icon-ArrowLineLeft icon-blue', this.onPrevious)
                }, {
                    region: 'east',
                    xtype: 'container',
                    layout: 'center',
                    width: 80,
                    style: {
                        background: '#FFFFFF'
                    },
                    scheduleTableScope: me,
                    items: this.createButton('btnNext', 'iconfont icon-ArrowLineRight icon-blue', this.onNext)
                }, {
                    region: 'center',
                    xtype: 'panel',
                    layout: 'fit',
                    items: mainControl,
                    border: false
                }]
            }]
        });
    },

    /**
     * 创建排班表控件
     * @returns {Ext.grid.Panel} 排班表控件GridPanel
     */
    createGridPanel: function () {
        var me = this;
        var columns = me.createColumns();
        var gridPanel = Ext.create('Ext.grid.Panel', {
            enableLocking: true,
            enableColumnMove: false,
            columns: columns,
            scheduleTableScope: me
        });
        me.setColumnVisible(gridPanel, new Date(), true);
        return gridPanel;
    },

    /**
     * 创建列集合
     * @returns {Array} 列集合
     */
    createColumns: function () {
        var me = this;
        var columns = [];
        columns.push({ xtype: 'rownumberer' });
        columns.push(me.getColumnConfig('WorkGroup', 'WorkGroup', '班组'.t(), 0, 120, true));
        columns.push(me.getColumnConfig('WipResource', 'WipResource', '资源'.t(), 0, 120, true));
        var date = new Date();
        this.year = date.getFullYear();//设置当前显示年份 例如：2019
        this.currentMonth = this.startMonth = this.endMonth = date.getMonth();  //设置当前显示月份   
        var day = Ext.Date.getFirstDateOfMonth(date);
        for (var i = 1; i <= 31; i++) {
            var week = this.getWeek(day);
            columns.push(me.getColumnConfig('Shift' + i, 'Shift' + i, i + '/' + week, i, 60, false, me.columnrenderer));
            day = Ext.Date.add(day, Ext.Date.DAY, 1);
        }
        return columns;
    },

    /**
     * 创建列配置
     * @param {String} name 列名称
     * @param {Number} dataIndex 数据索引
     * @param {String} header 列标题
     * @param {Number} dayIndex 数据标识
     * @param {Number} width 列宽度
     * @param {Boolean} locked 是否固定列
     * @param {回调函数} renderer 列渲染函数
     * @returns {Object} 列配置
     */
    getColumnConfig: function (name, dataIndex, header, dayIndex, width, locked, renderer) {
        return { name: name, dataIndex: dataIndex, header: header, width: width, sortable: false, hideable: false, renderer: renderer, tag: dayIndex, locked: locked };
    },

    /**
     * 设置排班表数据
     * @param {Array} result 排班信息集合
     * @param {DateTime} beginValue 排班开始时间
     * @param {DateTime} endValue 排班结束时间
     */
    setGridPanelData: function (result, beginValue, endValue) {
        var me = this;
        var control = me.control;
        me.beforeLoadData(beginValue, endValue);
        control.setStore(null);
        control.setStore(result);
        this.initShiftData();
    },

    /**
     * 注册视图事件
     * @param {DateTime} id 按钮id
     * @param {DateTime} icon 图标
     * @param {DateTime} handler 按钮行为
     * @returns {DateTime} 按钮配置
     */
    createButton: function (id, icon, handler) {
        return {
            id: id,
            xtype: 'button',
            width: '50%',
            handler: handler,
            iconCls: icon,
            style: 'height:75px;background:#E5E5E5;border-radius:4px;border-width:0px;'
        };
    },

    /**
     * 注册视图事件
     * @param {DateTime} view 主逻辑视图
     */
    registerEvent: function (view) {
        view.mon(view, 'beforeClosewin', this.beforeClosewin, this);
        view.mon(view, 'isReady', this.isReady, this);
    },

    /**
     * 初始化排班表界面配置
     * @returns {DateTime} 排班表布局配置
     */
    createShiftControl: function () {
        return {
            xtype: 'panel',
            border: false,
            layout: {
                type: 'hbox',
                align: 'stretch',
                pack: 'start'
            },
            style: 'padding:10px',
            scrollable: true,
            items: [{
                xtype: 'displayfield',
                id: 'labelMonth-id',
                fieldStyle: 'font-Weight:bold;font-Size:22px;',
                style: 'margin:0px 10px 0px 10px'
            }, {
                xtype: 'panel',
                id: 'shiftControl-id',
                border: false,
                layout: {
                    type: 'table',
                    tdAttrs: { style: 'padding: 0px 5px;' }
                }
            }]
        };
    },

    /**
     * 视图加载完成事件
     */
    isReady: function () {
        this.updateControlState(this, false);
    },

    /**
     * 根据月份天数动态隐藏多余列
     * @param {Ext.grid.Panel} control 主控件
     * @param {DateTime} date 当前日期
     * @param {Boolean} isReady 主界面数据是否已加载
     */
    setColumnVisible: function (control, date, isReady) {
        if (isReady === false)
            return;
        var days = Ext.Date.getDaysInMonth(date);  //月天数
        var columns = control.columns;
        var length = columns.length;  //34列
        //遍历后面四天 28,29,30,31根据月天数隐藏列 
        for (var i = length - 1; i > length - 4; i--) {
            var column = columns[i];
            if (column.tag > days)
                column.setVisible(false);
            else
                column.setVisible(true);
        }
    },

    /**
     * 排班表标签页关闭前事件，取消消息订阅
     * @param {returnObj} returnObj 关闭结果
     */
    beforeClosewin: function (returnObj) {
        //取消消息订阅
        var me = this;
        me.logicView.mun(me.logicView, 'isReady');
        me.logicView.mun(me.logicView, 'beforeClosewin');
    },

    /**
     * 排班表数据变更事件
     * @param {DateTime} startDate 排班表查询开始时间
     * @param {DateTime} endDate 排班表查询结束时间
     */
    beforeLoadData: function (startDate, endDate) {
        var sDate = new Date(startDate);
        var eDate = new Date(endDate);
        this.year = sDate.getFullYear();
        this.startYear = this.year;
        this.endYear = eDate.getFullYear();
        this.startMonth = sDate.getMonth();
        this.currentMonth = this.startMonth;
        this.endMonth = eDate.getMonth();
        this.updateControlState(this, true);
    },

    /**
     * 初始化班次信息
     */
    initShiftData: function () {
        var me = this;
        var shifts = {};
        me.control.store.data.items.forEach(function (schedule) {
            schedule.data.DetailList.forEach(function (detail) {
                if (shifts[detail.ShiftId] === undefined)
                    shifts[detail.ShiftId] = detail;
            });
        });
        var shiftControl = Ext.getCmp('shiftControl-id');
        if (!shiftControl)
            return;
        shiftControl.removeAll();
        var config = [];
        for (var key in shifts) {
            var model = shifts[key];
            config.push({
                border: false,
                html: Ext.String.format('<div style=\'padding:5px;background:{2};color: #FFFFFF;border-radius:5px;text-align:center;\'>{0}  {1}</div>', model.Shift.t(), model.ShiftTime, model.Background)
            });
        }
        if (config.length > 0)
            shiftControl.add(config);
    },

    /**
     * 获取星期
     * @param {DateTime} date 日期
     * @returns {String} 星期
     */
    getWeek: function (date) {
        var day = date.getDay();
        if (Ext.isEmpty(this.weeks))
            this.weeks = new Array("周日".t(), "周一".t(), "周二".t(), "周三".t(), "周四".t(), "周五".t(), "周六".t());
        var week = this.weeks[day];
        return week;
    },

    /**
     * 初始化界面布局
     * @param {Object} value 列值
     * @param {meta} meta 列元数据
     * @param {Object} record 数据记录
     * @param {Number} rowIndex 行索引
     * @param {Number} colIndex 列索引
     * @param {Array} store 数据存储
     * @param {view} view 视图
     * @returns {String} 班次名称
     */
    columnrenderer: function (value, meta, record, rowIndex, colIndex, store, view) {
        var schedule = record.data;
        var me = this.ownerGrid.scheduleTableScope;
        var currentDate = new Date(me.year, me.currentMonth, colIndex + 1); //当前列代表日期 
        var res = null;
        schedule.DetailList.forEach(function (detail) {
            if (detail.StrDate === Ext.Date.format(currentDate, 'Y-m-d')) {
                res = detail;
                return true;
            }
        });
        var background = '#CCCCCC';  //未排班颜色
        var shiftName = '';
        if (res !== null) {
            background = res.Background;
            shiftName = res.Shift;
        }
        meta.style = Ext.String.format("background-color:{0};margin:0.5px;", background);
        return shiftName;
    },

    /**
     * 上一月按钮点击事件
     */
    onPrevious: function () {
        var me = this.ownerCt.scheduleTableScope;
        me.setCurrentDate(me, false);
        me.updateControlState(me, true);
        me.view.refresh();
    },

    /**
     * 下一月按钮点击事件
     */
    onNext: function () {
        var me = this.ownerCt.scheduleTableScope;
        me.setCurrentDate(me, true);
        me.updateControlState(me, true);
        me.view.refresh();
    },

    /**
     * 设置当前年月日信息
     * @param {ShiftScheduleLayout} layout 布局
     * @param {Boolean} isNext 是否下一月
     */
    setCurrentDate: function (layout, isNext) {
        var me = layout;
        if (isNext) {
            if (me.currentMonth === 11) {
                me.year++;
                me.currentMonth = 0;
            }
            else
                me.currentMonth++;
        }
        else {
            if (me.currentMonth === 0) {
                me.year--;
                me.currentMonth = 11;
            }
            else
                me.currentMonth--;
        }
    },

    /**
     * 初始化控件状态
     * @param {ShiftScheduleLayout} layout 布局
     * @param {Boolean} isReady 是否数据已加载
     */
    updateControlState: function (layout, isReady) {
        var me = layout;
        var control = me.control;
        var date = new Date(me.year, me.currentMonth, 1);
        me.setMonth(me.year, me.currentMonth);
        me.setColumnText(control, date);
        me.setColumnVisible(control, date, isReady);
        me.setButtonState(me);
    },

    /**
     * 设置列标题
     * @param {GridControl} control 当前排班控件
     * @param {DateTime} date 当前月份第一天
     */
    setColumnText: function (control, date) {
        var me = this;
        for (var i = 3; i < control.columns.length; i++) {
            var day = i - 2;
            var week = me.getWeek(new Date(me.year, me.currentMonth, day));
            control.columns[i].setText(day + '/' + week);
        }
    },

    /**
     * 设置当前年月
     * @param {Number} year 当前年份
     * @param {Number} month 当前月份
     */
    setMonth: function (year, month) {
        var label = Ext.getCmp('labelMonth-id');
        label.setValue(Ext.String.format('{0}/{1}', year, month + 1));
    },

    /**
     * 设置上一月跟下一月按钮状态
     * @param {ShiftScheduleLayout} layout 当前布局
     */
    setButtonState: function (layout) {
        var me = layout;
        if (me.year === me.endYear) {
            //2020==2020  年份相同比较月份
            if (me.startMonth === me.currentMonth)
                Ext.getCmp('btnPrevious').disable();
            else
                Ext.getCmp('btnPrevious').enable();
            if (me.endMonth === me.currentMonth)
                Ext.getCmp('btnNext').disable();
            else
                Ext.getCmp('btnNext').enable();
        }
        else if (me.year < me.endYear) {
            //2019<2020
            if (me.startMonth === me.currentMonth && me.startYear === me.year)
                Ext.getCmp('btnPrevious').disable();
            else
                Ext.getCmp('btnPrevious').enable();
            Ext.getCmp('btnNext').enable();
        }
    }
});