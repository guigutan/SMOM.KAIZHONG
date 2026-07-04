Ext.define('SIE.Web.AbnormalInfo.Reports.Scripts.AbnormalInfoReportLayout', {
    extend: 'SIE.Web.Core.Reports.RateReportLayoutBase',
    xtype: 'AbnormalInfoReportLayout',
    _isRunning: false,
    _token: null,
    _criteria: null,
    chartId: 'closeRateReport-chart',
    /**
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;
        regions.main._view._relations[0]._target.mainLayout = me;
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });
        var closeRateControl = me.createAbnormalCloseRate(me);
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
                items: [closeRateControl, lineChartControl]
            }]
        });
    },

    /**
     * 创建异常信息报表控件
     * @method createLineDirectRate
     * @param {Object} me 当前视图对象
     * @return {Ext.create} 异常信息报表控件
     */
    createAbnormalCloseRate: function (me) {
        return Ext.create('SIE.Web.AbnormalInfo.Reports.Scripts.CloseRateGrid', {
            id: 'abnormalCloseRateChartId',
            region: 'north',
            flex: 1,
            border: false,
            mainViewLayout: me,   //当前布局
            controller: 'AbnormalInfoReportController',
            listeners: {
                pivotreconfigure: function () {
                    var criteria = this.mainViewLayout._criteria;
                    var token = this.mainViewLayout._token;
                    this.mainViewLayout.loadAbnInfoReportData(criteria, token, this.mainViewLayout); //重新获取数据
                }
            }
        });
    },

    /**
    * 创建折线图控件
    * @method createLineChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createLineChart: function (me) {
        return Ext.create('SIE.Web.AbnormalInfo.Reports.Scripts.AbnormalLineChart', {
            id: 'abnLineChartId',
            region: 'center',
            layout: 'fit',
            border: false,
        });
    },


    /**
    * 获取表格控件 
    */
    getPivotGridPassRateControl: function () {
        return Ext.getCmp("abnormalCloseRateChartId");
    },

    /**
     * 查询(iqcreport)
     * @param {any} criteria
     * @param {any} token
     */
    loadAbnInfoReportData: function (criteria, token, closeRateGrid) {
        var me = this;
        if (Ext.isEmpty(token))
            return;
        this._token = token;
        this._criteria = criteria;
        try {
            if (me._isRunning)
                return;
            me._isRunning = true;
            SIE.invokeDataQuery({
                method: 'GetAbnormalInfoReportData',
                params: [criteria],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.AbnormalInfo.Reports.AbnormalInfoReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        var Resultdata = res.Result;
                        me.bindAbnReportRateInfos(Resultdata, closeRateGrid);
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
     * 绑定异常关闭率
     * @param {any} closeRateInfos
     */
    bindAbnReportRateInfos: function (closeRateInfos, closeRateGrid) {
        var pivotGrid = Ext.getCmp('abnormalCloseRateChartId');
        var store = pivotGrid.getStore();
        var processDataList = closeRateInfos.ProcessDataList;
        store.setData(processDataList);
        pivotGrid.setStore(store);
        var chartJsonData = closeRateInfos.ChartJsonData;
        if (!chartJsonData) return;
        var minTimeDimension = closeRateGrid.getMinTimeDimension();
        var chartData = closeRateGrid.getChartData(chartJsonData, minTimeDimension);
        var lineChart = Ext.getCmp('abnLineChartId');
        var chartStore = lineChart.items.items[0].getStore();
        chartData.forEach(function (data) {
            if (data.monthDay.length > 7) {
                var monthDay = new Date(Date.parse(data.monthDay));
                var monthDayFormat = Ext.Date.format(monthDay, 'Y-m-d');
                data.monthDay = monthDayFormat;
            }
        });
        chartStore.setData(chartData);
        lineChart.items.items[0].setStore(chartStore);
    },
});