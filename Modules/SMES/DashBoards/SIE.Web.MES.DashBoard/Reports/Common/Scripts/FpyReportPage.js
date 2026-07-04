Ext.define('SIE.Web.MES.Reports.LineFPY.FpyReportPage', {
    extend: 'SIE.Page',
    beforeLoad: function (args) {
        this.isCustomize = true;
    },
    control: null,
    tabId: null,
    onLoad: function () {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        var data = params.data;
        var topItemName = params.topItemName;
        var isHasShift = params.isHasShift;
        me.tabId = params.tabId;
        if (!me.control) {
            me.control = me.createProcessPanel();
        }
        me.loadData(params.report, params.token, data, topItemName, isHasShift);
        me.registerEvent();
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
                items: me.control
            },
            renderTo: Ext.getBody()
        });
    },

    registerEvent: function () {
        var me = this;
        //产线直通率报表单元格点击
        CRT.Event.listen('lineReportClick', function (token, data, topItemName, isHasShift, title) {
            var tab = CRT.Workbench.getTabById(me.tabId);
            if (tab) {
                CRT.Workbench.getTabPanel().setActiveItem(tab);
                tab.setTitle(title);
            }
            me.loadData('line', token, data, topItemName, isHasShift);
        });
        //产品直通率报表单元格点击
        CRT.Event.listen('productReportClick', function (token, data, topItemName, isHasShift, title) {
            var tab = CRT.Workbench.getTabById(me.tabId);
            if (tab) {
                CRT.Workbench.getTabPanel().setActiveItem(tab);
                tab.setTitle(title);
            }
            me.loadData('product', token, data, topItemName, isHasShift);
        });
        //车间直通率报表单元格点击
        CRT.Event.listen('shopReportClick', function (token, data, topItemName, isHasShift, title) {
            var tab = CRT.Workbench.getTabById(me.tabId);
            if (tab) {
                CRT.Workbench.getTabPanel().setActiveItem(tab);
                tab.setTitle(title);
            }
            me.loadData('shop', token, data, topItemName, isHasShift);
        });
    },

    loadData: function (report, token, data, topItemName, isHasShift) {
        var me = this;
        if (report === 'line')//产线直通率报表
            me.loadLineReportData(token, data, topItemName, isHasShift);
        else if (report === 'product')//产品直通率报表
            me.loadProductReportData(token, data, topItemName, isHasShift);
        else if (report === 'shop')//车间直通率报表
            me.loadShopReportData(token, data, topItemName, isHasShift);
    },

    loadLineReportData: function (token, data, topItemName, isHasShift) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetProcessRelatedInfo',
            params: [topItemName, isHasShift, data],
            action: 'queryer',
            async: false,
            type: 'SIE.Web.MES.DashBoard.Reports.LineFPY.DataQuery.LineReportDataQueryer',
            token: token,
            success: function (res) {
                if (res.Success) {
                    var processRelatedInfo = res.Result;
                    var processDirectRateChart = Ext.getCmp('processDirectRateChartId');
                    var store = processDirectRateChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.ProcessDirectRateInfoList);
                    processDirectRateChart.items.items[0].setStore(store);

                    var processStatisticsChart = Ext.getCmp('processStatisticsChartId');
                    var store = processStatisticsChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.ProcessStatisticsInfoList);
                    processStatisticsChart.items.items[0].setStore(store);

                    var defectChart = Ext.getCmp('defectChartId');
                    var store = defectChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.DefectInfoList);
                    defectChart.items.items[0].setStore(store);
                    var date = '(' + data.year + '年'.t() + data.month + '月'.t() + data.day + '日'.t() + ')';
                    var title = isHasShift ? data.Shift : data.LineName;
                    processDirectRateChart.fireEvent('onProcessDirectRateTitle', title + "工序直通率统计图".t() + date);
                }
            }
        });
    },

    loadProductReportData: function (token, data, topItemName, isHasShift) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetProcessRelatedInfo',
            params: [topItemName, isHasShift, data],
            action: 'queryer',
            async: false,
            type: 'SIE.Web.MES.DashBoard.Reports.ProductFPY.DataQuery.ProductReportDataQueryer',
            token: token,
            success: function (res) {
                if (res.Success) {
                    var processRelatedInfo = res.Result;
                    var processDirectRateChart = Ext.getCmp('processDirectRateChartId');
                    var store = processDirectRateChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.ProcessDirectRateInfoList);
                    processDirectRateChart.items.items[0].setStore(store);

                    var processStatisticsChart = Ext.getCmp('processStatisticsChartId');
                    var store = processStatisticsChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.ProcessStatisticsInfoList);
                    processStatisticsChart.items.items[0].setStore(store);

                    var productDefectChart = Ext.getCmp('defectChartId');
                    var store = productDefectChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.DefectInfoList);
                    productDefectChart.items.items[0].setStore(store);
                }
            }
        });
    },

    loadShopReportData: function (token, data, topItemName, isHasShift) {
        SIE.invokeDataQuery({
            method: 'GetProcessRelatedInfo',
            params: [topItemName, isHasShift, data],
            action: 'queryer',
            async: false,
            type: 'SIE.Web.MES.DashBoard.Reports.ShopFPY.DataQuery.ShopReportDataQueryer',
            token: token,
            success: function (res) {
                if (res.Success) {
                    var processRelatedInfo = res.Result;
                    var processDirectRateChart = Ext.getCmp('processDirectRateChartId');
                    var store = processDirectRateChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.ProcessDirectRateInfoList);
                    processDirectRateChart.items.items[0].setStore(store);

                    var processStatisticsChart = Ext.getCmp('processStatisticsChartId');
                    var store = processStatisticsChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.ProcessStatisticsInfoList);
                    processStatisticsChart.items.items[0].setStore(store);

                    var defectChart = Ext.getCmp('defectChartId');
                    var store = defectChart.items.items[0].getStore();
                    store.setData(processRelatedInfo.DefectInfoList);
                    defectChart.items.items[0].setStore(store);
                    var date = '(' + data.year + '年'.t() + data.month + '月'.t() + data.day + '日'.t() + ')';
                    var title = isHasShift ? data.ResourceName : data.ShopName;
                    processDirectRateChart.fireEvent('onProcessDirectRateTitle', title + "工序直通率统计图" + date);
                }
            }
        });
    },

    /**
     * 创建工序相关控件
     * @method createProcessPanel
     * @param {Ext.panel.Panel} defectPanel 当前视图对象
     * @return {Ext.widget} 布局
     */
    createProcessPanel: function () {
        var me = this;
        var procDirectRateControl = me.createProcessDirectRateChart();
        var procStatisticsControl = me.createProcessStatisticsChart();
        var defectControl = me.createDefectChart();
        return Ext.widget('container', {
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'start',
            },
            bodyBorder: false,
            items: [{
                xtype: 'panel',
                flex: 1,
                layout: {
                    type: 'hbox',
                    align: 'stretch',
                    pack: 'start',
                },
                items: [procDirectRateControl, defectControl],
                border: false,
            }, {
                layout: 'border',
                xtype: 'panel',
                flex: 1,
                border: false,
                items: [procStatisticsControl]
            }],
            border: false,
        });
    },

    /**
    * 创建工序直通率报表控件
    * @method createProcessDirectRateChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 工序直通率报表控件
    */
    createProcessDirectRateChart: function () {
        return Ext.create('SIE.Web.MES.DashBoard.Common.ProcessDirectRateChart', {
            id: 'processDirectRateChartId',
            layout: 'fit',
            flex: 1,
            border: false,
        });
    },

    /**
    * 创建工序一次良品/不良品统计报表控件
    * @method createProcessStatisticsChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 工序一次良品/不良品统计报表控件
    */
    createProcessStatisticsChart: function () {
        return Ext.create('SIE.Web.MES.DashBoard.Common.ProcessStatisticsChart', {
            region: 'center',
            id: 'processStatisticsChartId',
            layout: 'fit',
            border: false,
        });
    },
    /**
    * 创建缺陷报表控件
    * @method createDefectChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 缺陷报表控件
    */
    createDefectChart: function () {
        return Ext.create('SIE.Web.MES.DashBoard.Common.DefectChart', {
            id: 'defectChartId',
            height: '100%',
            width: '100%',
            layout: 'fit',
            flex: 1,
            border: false,
        });
    },
});