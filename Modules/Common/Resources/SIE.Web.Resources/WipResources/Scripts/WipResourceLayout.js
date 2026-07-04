/**
 * 生产资源日历控件定义
 * @class SIE.Web.Resource.WipResource.WipResourceCalendar
 * @constructs
 */
Ext.define("SIE.Web.Resource.WipResource.WipResourceCalendar", {
    extend: 'Ext.calendar.panel.Panel',
    xtype: 'wipResourceCalendar',
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
                    return false;
                }
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

    /**
     * 渲染后事件
     */
    afterRender: function () {
        this.callParent();
        ////由于日历外层嵌套一层panel，导致无法修改event，在渲染后取出外层panel 
        var childPanels = Ext.ComponentQuery.query(Ext.String.format('#{0} panel', this.id));
        childPanels.forEach(function (child) {
            var childDom = child.el.dom;
            if (childDom.className === 'x-panel x-border-item x-box-item x-panel-default' && child.referenceKey === 'sideBar')
                childDom.style.display = 'none';
        });
    }
});

/**
 * 生产资源布局
 * @class SIE.Web.Resource.WipResource.WipResourceLayout
 * @constructs
 */
Ext.define('SIE.Web.Resource.WipResource.WipResourceLayout', {
    extend: 'SIE.autoUI.layouts.Common',

    /**
     * 主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 生产资源
     * @property {SIE.Resources.WipResources.WipResource} wipResource
     */
    wipResource: null,

    /**
     * 界面布局
     * @method layout
     * @param {regions} regions 聚合块
     * @return {Ext.Container} 布局
     */
    layout: function (regions) {
        var me = this;
        me.mainView = regions.main._view;
        me.registerEvent();
        var childrenUI = this._layoutChildren(regions);
        me.addWeekCalendarTabItem(childrenUI);
        var res = this._layoutNaviCondition(regions, childrenUI);
        return res;
    },

    /**
     * 事件注册
     * @method registerEvent
     */
    registerEvent: function () {
        var me = this;
        var view = me.mainView;
        view.mon(view, 'beforeClosewin', me.beforeClosewin, me);
        view.mon(view, 'currentChanged', me.currentChanged, me);
    },

    /**
     * 添加资源日历子页签
     * @method addWeekCalendarTabItem
     * @param {Ext.Container} childrenUI 除查询块外的所有控件集合
     */
    addWeekCalendarTabItem: function (childrenUI) {
        var me = this;
        //添加资源日历页签
        me.weekCalendar = me.initWeekCalendar();
        me.calendar = me.initCalendar();
        var tabItem = Ext.widget('container', {
            layout: 'border',
            title: '资源日历'.L10N(),
            defaults: {
                scrollable: true
            },
            items: [{
                region: 'west',
                layout: 'fit',
                width: 440,
                split: true,
                minHeight:360,
                border: 0,
                items: me.weekCalendar
            },
            {
                region: 'center',
                layout: 'fit',
                split: true,
                minWidth: 440,
                minHeight: 360,
                border: 0,
                items: me.calendar
            }]
        });
        me.tabPanel = childrenUI.down('tabpanel');
        if (me.tabPanel) {
            me.tabPanel.clearListeners(); //清除框架的tabchange事件，因为第一个标签页为自定义控件，不需要框架加载数据
            me.tabPanel.mon(me.tabPanel, 'tabchange', me.tabchange, me);
            me.tabPanel.insert(0, tabItem);
            me.tabPanel.setActiveTab(0);
        }
    },

    /**
     * 主视图关闭事件
     * @method beforeClosewin
     * @param {returnObj} returnObj 参数
     */
    beforeClosewin: function (returnObj) {
        //取消消息订阅
        var me = this;
        var view = me.mainView;
        view.mun(view, 'beforeClosewin');
        view.mun(view, 'currentChanged');
        me.tabPanel.mun(me.tabPanel, 'tabchange');
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
        var me = this;
        if (newCard.title === '资源日历'.L10N())
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
     * 初始化周方案控件
     * @method initWeekCalendar 
     * @return {Ext.grid.Panel} 周方案控件
     */
    initWeekCalendar: function () {
        return Ext.create('Ext.grid.Panel', {
            columnLines: true,
            columns: [
                { xtype: 'rownumberer' },
                { text: '名称'.t(), dataIndex: 'Name' },
                { text: '班制'.t(), dataIndex: 'ShiftType' },
                { text: '预设启用日期'.t(), dataIndex: 'ActiveDate', format: 'Y/m/d H:i:s', width: 150 }
            ]
        });
    },

    /**
     * 初始化日历控件
     * @method initCalendar
     * @return {SIE.Web.Resource.WipResource.WipResourceCalendar} 日历控件
     */
    initCalendar: function () {
        var me = this;
        //创建日历面板
        var calendar = Ext.create('SIE.Web.Resource.WipResource.WipResourceCalendar', {
            border: false
        });
        var filter = {
            token: me.mainView.token,
            Method: 'GetShiftCalendar',
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
                        type: 'SIE.Web.Resources.WipResources.WipResourceDataQueryer',
                        filter: SIE.data.Utils.seriaizeRequest(filter),
                        token: me.mainView.token
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

    /**
     * 日历控件月份切换事件
     * @method monthChanged
     */
    monthChanged: function () {
        var me = this;
        me.loadCalendarDatas(me.wipResource);
    },

    /**
     * 主视图生产资源变更事件
     * @method currentChanged
     * @param {SIE.Resources.WipResources.WipResource} entity 生产资源 
     */
    currentChanged: function (entity) {
        var me = this;
        if (entity.oldValue !== null && entity.newValue !== null && entity.oldValue.data.Id === entity.newValue.data.Id)
            return;
        if (entity.newValue) {
            me.wipResource = entity.newValue.data;
            me.loadCalendarDatas(me.wipResource);
        }
        else {
            //清除数据
            me.setWeekCalendarDatas([]);
            me.setCalendarDatas([]);
        }
    },

    /**
     * 加载资源日历页数据
     * @method loadCalendarDatas
     * @param {SIE.Resources.WipResources.WipResource} wipResource 生产资源
     */
    loadCalendarDatas: function (wipResource) {
        var me = this;
        if (!wipResource || me.tabPanel.activeTab.title !== '资源日历'.L10N())
            return;
        var me = this;
        var param = [wipResource.Id, me.calendar.getValue()];
        SIE.invokeDataQuery({
            type: "SIE.Web.Resources.WipResources.WipResourceDataQueryer",
            method: 'GetShiftCalendarInfo',
            token: me.mainView.token,
            params: param,
            success: function (res) {
                me.setWeekCalendarDatas(res.Result.WeekList);
                me.setCalendarDatas(res.Result.ShiftList);
            }
        });
    },

    /**
     * 设置周方案数据
     * @method setWeekCalendarDatas
     * @param {regions} datas 周方案集合
     */
    setWeekCalendarDatas: function (datas) {
        var me = this;
        me.weekCalendar.setStore(datas);
    },

    /**
     * 设置日历控件数据
     * @method setCalendarDatas
     * @param {数组} datas 班制集合 日历数据  需转换成Ext.calendar.model.Event
     */
    setCalendarDatas: function (datas) {
        var me = this;
        try {
            if (me.monthChanging) return;
            me.monthChanging = true;
            var calendar = me.calendar;
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
        } catch (e) {
            throw e;
        } finally {
            me.monthChanging = false;
        }
    }
});