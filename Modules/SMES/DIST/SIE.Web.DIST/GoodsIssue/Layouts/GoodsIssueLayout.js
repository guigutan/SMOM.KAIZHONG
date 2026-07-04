/**
 *  
 */
Ext.define('SIE.Web.DIST.GoodsIssue.GoodsIssueLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'GoodsIssueLayout',

    /**
     * @property {整型}
     * 当前显示年份
     */
    year: null,

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
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
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
                border: false,
            }, {
                region: 'center',
                layout: 'border',
                xtype: 'panel',
                border: false,
                items: [{
                    region: 'north',
                    items: shiftControl,
                    height: 50,
                    border: false,
                }, {
                    region: 'west',
                    xtype: 'container',
                    layout: 'center',
                    width: 80,
                    style: {
                        background: '#FFFFFF',
                    },
                    items: this.createButton('btnPrevious', 'iconfont icon-ArrowLineLeft icon-blue', this.onPrevious)
                }, {
                    region: 'east',
                    xtype: 'container',
                    layout: 'center',
                    width: 80,
                    style: {
                        background: '#FFFFFF',
                    },
                    items: this.createButton('btnNext', 'iconfont icon-ArrowLineRight icon-blue', this.onNext)
                }, {
                    region: 'center',
                    xtype: 'panel',
                    layout: 'fit',
                    items: mainControl,
                    border: false,
                }]
            }]
        });
    },

    /**
     * 创建排班表控件
     * @returns 排班表控件GridPanel
     */
    createGridPanel: function () {
        var me = this;
        var columns = me.createColumns();
        var gridPanel = Ext.create('Ext.grid.Panel', {
            enableLocking: true,
            enableColumnMove: false,
            columns: columns,
        });
        me.setColumnVisible(gridPanel, new Date(), true);
        return gridPanel;
    },
    
});