/**
 * 产线直通率报表布局 
 * @class SIE.Web.MES.DashBoard.Reports.LineFPY.Scripts.LineReportLayout
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Reports.LineFPY.Scripts.LineReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'LineReportLayout',
    /**
    * 子页签Id
    * @property {string} _tabId
    */
    tabId: null,

    /**
     * 是否正在运行中
     * @property {bool} _isRunning
     */
    _isRunning: false,

    /**
     * 凭证
     * @property {string} _token
     */
    _token: null,

    /**
     * 缓存产线直通率及其相关数据列表
     * @property {List<SIE.Web.MES.DashBoard.Reports.LineFPY.ProductionLineRateInfo>} prodLineRateInfos
     */
    prodLineRateInfos: null,

    /**
     * 缓存某车间班次的折线图信息数组
     * @property {SIE.Web.MES.DashBoard.Reports.LineFPY.LineChartInfo[]} lineChartInfos
     */
    lineChartInfos: null,

    /**
     * 缓存车间的折线图目标\警告设置信息数组
     * @property {SIE.Web.MES.DashBoard.Reports.LineFPY.LineChartSettingInfo[]} lineChartSettingInfos
     */
    lineChartSettingInfos: null,

    /**
     * 车间
     * @property {string} _lineName
     */
    _lineName: null,

    /**
     * 班次
     * @property {string} _shift
     */
    _shift: null,

    /**
     * 工序直通率控件
     * @property processDirectRateChart
     */
    processDirectRateChart: null,

    /**
     * 工序一次良品/不良品统计控件
     * @property processStatisticsChart
     */
    processStatisticsChart: null,

    /**
     * 缺陷控件
     * @property defectChart
     */
    defectChart: null,

    /**
     * 
     * @property me
     */
    me: null,

    /**
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;
        me.tabId = null;
        me.prodLineRateInfos = null;
        me.lineChartSettingInfos = [];
        me.processDirectRateChart = null;
        me.processStatisticsChart = null;
        me.defectChart = null;
        regions.main._view._relations[0]._target.mainLayout = me;
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });
        var directRateControl = me.createLineDirectRate(me);
        var lineChartControl = me.createLineChart(me);
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
                items: [directRateControl, lineChartControl]
            }]
        });
    },

    /**
     * 加载产线直通率报表
     * @method loadLineReportData
     * @param {SIE.Web.MES.DashBoard.Reports.LineFPY.LineReportViewModelCriteria} criteria 标签控件
     * @param {token} token 新激活子页签
     */
    loadLineReportData: function (criteria, token) {
        var me = this;
        me._token = token;
        me._lineName = "";
        me._shift = "";
        try {
            if (me._isRunning)
                return;
            me._isRunning = true;
            SIE.invokeDataQuery({
                method: 'GetProductionLineRateInfos',
                params: [criteria],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.MES.DashBoard.Reports.LineFPY.DataQuery.LineReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        me.prodLineRateInfos = res.Result;
                        me.bindProductLineRateInfos(me.prodLineRateInfos);
                    }
                }
            });
        } catch (e) {
            throw e;
        } finally {
            me._isRunning = false;
        }
    },

    /**
     * 绑定产线直通率信息
     * @method bindProductLineRateInfos
     * @param {List<SIE.Web.MES.DashBoard.Reports.LineFPY.ProductionLineRateInfo>} prodLineRateInfos 新激活子页签
     */
    bindProductLineRateInfos: function (prodLineRateInfos) {
        debugger;
        var me = this;
        var prodLineRateChart = Ext.getCmp('prodLineRateChartId');
        var store = prodLineRateChart.getStore();
        store.setData(prodLineRateInfos);
        prodLineRateChart.setStore(store);

        //遍历产线直通率列表
        let flag = true;
        prodLineRateInfos.forEach(function (prodLineRateInfo) {

            //遍历折线图设置信息列表
            if (prodLineRateInfo.LineChartSettingInfo) {
                me.lineChartSettingInfos.forEach(function (lineChartSettingInfo) {
                    if (lineChartSettingInfo.LineName == prodLineRateInfo.LineChartSettingInfo.LineName)
                        flag = false;
                });

                if (flag)
                    me.lineChartSettingInfos.push(prodLineRateInfo.LineChartSettingInfo);
                flag = true;
            }
        });
    },

    /**
     * 组单元格双击事件
     * @method prodLineRateGroupCellDblClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodLineRateGroupCellDblClick: function (params, e, eOpts) {
        params.grid.reportLayout.showProcessRelatedInfos(params, false);
    },

    /**
     * 单元格双击事件
     * @method prodLineRateItemCellDblClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodLineRateItemCellDblClick: function (params, e, eOpts) {
        params.grid.reportLayout.showProcessRelatedInfos(params, true);
    },

    /**
     * 单元格单击事件
     * @method prodLineRateCellClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodLineRateCellClick: function (params, e, eOpts) {
        var me = this;
        var data = params.grid.getMatrix().results.getByLeftKey(params.leftKey)[0].records[0].data;
        var lineName = data.LineName;
        var shift = data.Shift;
        var lineChart = Ext.getCmp('lineChartId');
        var store = lineChart.items.items[0].getStore();
        store.setData(null);
        lineChart.items.items[0].setStore(store);
        var layout = params.grid.reportLayout;
        if (layout) {
            layout._lineName = lineName;
            layout._shift = shift;
            layout.bindLineChartInfos(false, lineName, shift);
        }
    },

    /**
     * 组单元格单击事件
     * @method prodLineRateGroupCellClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodLineRateGroupCellClick: function (params, e, eOpts) {
        var me = this;
        var data = params.grid.getMatrix().results.getByLeftKey(params.leftKey)[0].records[0].data;
        var lineName = data.LineName;
        var shift = data.Shift;
        var lineChart = Ext.getCmp('lineChartId');
        var store = lineChart.items.items[0].getStore();
        store.setData(null);
        lineChart.items.items[0].setStore(store);

        var layout = params.grid.reportLayout;
        if (layout) {
            layout._lineName = lineName;
            layout._shift = shift;
            layout.bindLineChartInfos(true, lineName, shift);
        }
    },

    /**
     * 绑定折线图信息
     * @method bindLineChartInfos
     * @param {string} lineName 车间名称
     * @param {string} shift 班次名称
     */
    bindLineChartInfos: function (isGroup, lineName, shift) {
        var me = this;
        me.lineChartInfos = [];
        var yDatas = [];

        //遍历产线直通率列表
        me.prodLineRateInfos.forEach(function (prodLineRateInfo) {
            if (prodLineRateInfo.LineName === lineName && prodLineRateInfo.Shift === shift) {
                prodLineRateInfo.LineChartInfoList.forEach(function (lineChartInfo) {
                    me.lineChartInfos.push(lineChartInfo);
                    yDatas.push(lineChartInfo.YData);
                });
            }
        });

        var lineChart = Ext.getCmp('lineChartId');
        var store = lineChart.items.items[0].getStore();
        store.setData(me.lineChartInfos);
        lineChart.items.items[0].setStore(store);

        lineChart.fireEvent('onDesiredAlarmChange', null);
        for (i = 0; i < me.lineChartSettingInfos.length; i++) {
            var lineChartSettingInfo = me.lineChartSettingInfos[i];
            if (lineChartSettingInfo.LineName == lineName) {
                yDatas.push(lineChartSettingInfo.Desired);
                yDatas.push(lineChartSettingInfo.Alarm);
                lineChart.fireEvent('onDesiredAlarmChange', lineChartSettingInfo);
            }
        }

        me.setLeftAxisValue(isGroup, lineName, shift, lineChart, yDatas);
    },

    /**
     * 设置折线图的y轴坐标
     * @method setLeftAxisValue
     * @param {string} lineName 车间名称
     * @param {string} shift 班次名称
     * @param {lineChart} lineChart 控件
     * @param {yDatas} yDatas 数组
     */
    setLeftAxisValue: function (isGroup, lineName, shift, lineChart, yDatas) {
        var minValue = Ext.Array.min(yDatas);
        var maxValue = Ext.Array.max(yDatas);

        if (minValue === maxValue === 0) {
            if (minValue == 0)
                maxValue = 100;
        }
        else {
            minValue = minValue - 10;
            maxValue = maxValue + 10;
        }

        if (minValue < 0)
            minValue = 0;

        if (isGroup)
            lineChart.fireEvent('onLineChartTitleChange', lineName, null, minValue, maxValue);
        else
            lineChart.fireEvent('onLineChartTitleChange', lineName, shift, minValue, maxValue);
    },

    /**
     * 显示下转报表
     * @method showProcessRelatedInfos
     * @param {Object} params 配置对象
     * @param {bool} isHasShift 是否包含班次
     */
    showProcessRelatedInfos: function (params, isHasShift) {
        var me = this;
        var cell = params.grid.getMatrix().results.get(params.leftKey, params.topKey);
        if (cell) {
            var title = '班次工序直通率统计'.L10N();
            var topItemName = params.topItem.name;
            var data = JSON.parse(JSON.stringify(cell.records[0].data));
            data.LineChartInfoList = null;
            data.LineChartSettingInfo = null;
            me.tabId = 'menu_' + 'SIE.MES.Statistics.Fpy.DefectStatistics,SIE.MES.Statistics.Fpy'.replace(/[.|,]/g, '');
            var tabItem = CRT.Workbench.getTabById(me.tabId);
            if (tabItem) {
                CRT.Event.fire('lineReportClick', me._token, data, topItemName, isHasShift, title);
                return;
            }
            CRT.Workbench.addPage({
                tabId: me.tabId,
                title: title,
                pageClass: 'SIE.Web.MES.Reports.LineFPY.FpyReportPage',
                entityType: 'SIE.Web.MES.DashBoard.Reports.LineFPY.ReportPageViewModel',
                params: {
                    tabId: me.tabId,
                    data: data,
                    topItemName: topItemName,
                    isHasShift: isHasShift,
                    token: me._token,
                    report: 'line'
                }
            });
        }
    },

    /**
     * 绑定工序相关控件的信息
     * @method bindProcessRelatedInfos
     * @param {Object} me 当前视图对象
     * @param {Object} cell 单元格信息
     * @param { Object} topItemName 列头信息
     */
    bindProcessRelatedInfos: function (me, cell, topItemName, isHasShift) {
        var me = this;
        if (me.processDirectRateChart != null && me.processDirectRateChart)
            me.processDirectRateChart.destroy();
        if (me.processStatisticsChart != null && me.processStatisticsChart)
            me.processStatisticsChart.destroy();
        if (me.defectChart != null && me.defectChart)
            me.defectChart.destroy();

        var control = me.createProcessPanel();
        var data = cell.records[0].data;
        SIE.invokeDataQuery({
            method: 'GetProcessRelatedInfo',
            params: [topItemName, isHasShift, data],
            action: 'queryer',
            async: false,
            type: 'SIE.Web.MES.DashBoard.Reports.LineFPY.DataQuery.LineReportDataQueryer',
            token: _token,
            success: function (res) {
                if (res.Success) {
           
                    var processRelatedInfo = res.Result;
                    me.processDirectRateChart = Ext.getCmp('processDirectRateChartId');
                    var store = processDirectRateChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.ProcessDirectRateInfoList);
                    me.processDirectRateChart.items.items[0].setStore(store);

                    me.processStatisticsChart = Ext.getCmp('processStatisticsChartId');
                    var store = processStatisticsChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.ProcessStatisticsInfoList);
                    me.processStatisticsChart.items.items[0].setStore(store);

                    me.defectChart = Ext.getCmp('defectChartId');
                    var store = defectChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.DefectInfoList);
                    me.defectChart.items.items[0].setStore(store);

                }
            }
        });
        return control;
    },

    /**
     * 绑定工序缺陷分类信息
     * @method bindDefectCategoryInfos
     * @param {Object} defectPanel 缺陷分类面板控件 
     * @param {SIE.Web.MES.DashBoard.Reports.LineFPY.ProcessRelatedInfo} processRelatedInfo 工序相关信息
     */
    bindDefectCategoryInfos: function (defectPanel, processRelatedInfo) {

        var tpl = new Ext.XTemplate(
            '<div id="container" style="width: 100%;height: 100%;margin: 5px auto;">\
    <div style="width:100%;height: 45px;line-height: 45px;text-align:center;font-size: 22px;">缺陷代码TOP5</div>\
        <div id="main" style="width: 100%;height: calc(100% - 45px);">\
            <tpl for=".">\
                <div style="width:'+ '{ColumnWidth}' + '%;background:' + '{ColorName};float:left;height:100%;box-sizing: border-box;border:0.5px #000 solid;padding-top: 8px;">\
                    <span style="font-size: 25px;font-weight:bold;font-family:"SimHei"">{CategoryName}</span>\
                    <tpl for="DefectCodeList">\
                        <div style="width: 100%; text-align:center;height: '+ '{RowHeight}' + '%;line-height:' + '{LineHeight}' + 'px; border-bottom: 1px #000 solid;font-size: 20px;">{DefectName}</div>\
                    </tpl>\
                </div>\
            </tpl>\
        <div>\
    </div>'
        );

        tpl.overwrite(defectPanel.body, processRelatedInfo.DefectCategoryInfoList);
    },

    /**
     * 创建产线直通率报表控件
     * @method createLineDirectRate
     * @param {Object} me 当前视图对象
     * @return {Ext.create} 产线直通率报表控件
     */
    createLineDirectRate: function (me) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.ProductLineRateChart', {
            id: 'prodLineRateChartId',
            region: 'north',
            height: 400,
            border: false,
            reportLayout: me,
            listeners: {
                pivotitemcelldblclick: me.prodLineRateItemCellDblClick,
                pivotitemcellclick: me.prodLineRateCellClick,
                pivotgroupcelldblclick: me.prodLineRateGroupCellDblClick,
                pivotgroupcellclick: me.prodLineRateGroupCellClick
            },
        });
    },

    /**
    * 创建折线图控件
    * @method createLineChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createLineChart: function (me) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.LineChart', {
            id: 'lineChartId',
            region: 'center',
            layout: 'fit',
            border: false,
        });
    },

    /**
     * 创建缺陷分类面板控件
     * @method createLineChart
     * @return {Ext.create} 缺陷分类面板控件
     */
    createDefectPanel: function () {
        return Ext.create('Ext.panel.Panel', {
            renderTo: Ext.getBody(),
            height: '100%',
            width: '100%',
            layout: 'fit',
            flex: 1,
        });
    },
});
