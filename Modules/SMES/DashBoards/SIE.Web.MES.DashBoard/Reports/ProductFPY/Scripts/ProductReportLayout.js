/**
 * 产线直通率报表布局 
 * @class SIE.Web.MES.DashBoard.Reports.ProductFPY.Scripts.ProductReportLayout
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Reports.ProductFPY.Scripts.ProductReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'ProductReportLayout',
    /**
     * 是否正在运行中
     * @property {bool} _isRunning
     */
    _isRunning: false,

    /**
    * 子页签Id
    * @property {string} _proTabId
    */
    tabId: null,

    /**
     * 凭证
     * @property {string} _token
     */
    _token: null,

    /**
     * 缓存产线直通率及其相关数据列表
     * @property {List<SIE.Web.MES.DashBoard.Reports.ProductFPY.ProductionProductRateInfo>} prodProductRateInfos
     */
    prodProductRateInfos: null,

    /**
     * 缓存某车间班次的折线图信息数组
     * @property {SIE.Web.MES.DashBoard.Reports.ProductFPY.ProductChartInfo[]} productChartInfos
     */
    productChartInfos: null,

    /**
     * 缓存车间的折线图目标\警告设置信息数组
     * @property {SIE.Web.MES.DashBoard.Reports.ProductFPY.ProductChartSettingInfo[]} productChartSettingInfos
     */
    productChartSettingInfos: null,

    /**
     * 车间
     * @property {string} _productName
     */
    _productName: null,

    /**
     * 班次
     * @property {string} _productModelName
     */
    _productModelName: null,

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
    productDefectChart: null,

    /**
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;
        me.prodProductRateInfos = null;
        me.tabId = null;
        me.productChartSettingInfos = [];
        me.processDirectRateChart = null;
        me.processStatisticsChart = null;
        me.productDefectChart = null;
        regions.main._view._relations[0]._target.mainLayout = me;
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });
        var directRateControl = me.createProductDirectRate(me);
        var productChartControl = me.createProductChart(me);
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
                items: [directRateControl, productChartControl]
            }]
        });
    },

    /**
     * 加载产线直通率报表
     * @method loadProductReportData
     * @param {SIE.Web.MES.DashBoard.Reports.ProductFPY.ProductReportViewModelCriteria} criteria 标签控件
     * @param {token} token 新激活子页签
     */
    loadProductReportData: function (criteria, token) {
        var me = this;
        me._token = token;
        me._productName = "";
        me._productModelName = "";
        try {
            if (me._isRunning)
                return;
            me._isRunning = true;
            SIE.invokeDataQuery({
                method: 'GetProductionProductRateInfos',
                params: [criteria],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.MES.DashBoard.Reports.ProductFPY.DataQuery.ProductReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        me.prodProductRateInfos = res.Result;
                        me.bindProductProductRateInfos(me.prodProductRateInfos);
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
     * @method bindProductProductRateInfos
     * @param {List<SIE.Web.MES.DashBoard.Reports.ProductFPY.ProductionProductRateInfo>} prodProductRateInfos 新激活子页签
     */
    bindProductProductRateInfos: function (prodProductRateInfos) {
        var me = this;
        var prodProductRateChart = Ext.getCmp('prodProductRateChartId');
        var store = prodProductRateChart.getStore();
        store.setData(prodProductRateInfos);
        prodProductRateChart.setStore(store);
        //遍历产线直通率列表
        let flag = true;
        prodProductRateInfos.forEach(function (prodProductRateInfo) {
            if (prodProductRateInfo.ProductChartSettingInfo) {
                me.productChartSettingInfos.forEach(function (productChartSettingInfo) {
                    if (productChartSettingInfo.ProductName == prodProductRateInfo.ProductChartSettingInfo.ProductName)
                        flag = false;
                });

                if (flag)
                    me.productChartSettingInfos.push(prodProductRateInfo.ProductChartSettingInfo);
                flag = true;
            }
            //遍历折线图设置信息列表
            //prodProductRateInfo.ProductChartSettingInfoList.forEach(function (productChartSettingInfo) {
            //    productChartSettingInfos.push(productChartSettingInfo);
            //});
        });
    },

    /**
 * 绑定折线图信息
 * @method bindProductChartInfos
 * @param {string} productName 车间名称
 * @param {string} productModelName 班次名称
 */
    bindProductChartInfos: function (isGroup, productName, productModelName) {
        var me = this;
        me.productChartInfos = [];
        var yDatas = [];

        //遍历产线直通率列表
        me.prodProductRateInfos.forEach(function (prodProductRateInfo) {
            if (prodProductRateInfo.ProductName === productName && prodProductRateInfo.ProductModelName === productModelName) {
                prodProductRateInfo.ProductChartInfoList.forEach(function (productChartInfo) {
                    me.productChartInfos.push(productChartInfo);
                    yDatas.push(productChartInfo.YData);
                });
            }
        });

        var productChart = Ext.getCmp('productChartId');
        var store = productChart.items.items[0].getStore();
        store.setData(me.productChartInfos);
        productChart.items.items[0].setStore(store);

        productChart.fireEvent('onDesiredAlarmChange', null);
        for (i = 0; i < me.productChartSettingInfos.length; i++) {
            var productChartSettingInfo = me.productChartSettingInfos[i];
            if (productChartSettingInfo.ProductName == productName) {
                yDatas.push(productChartSettingInfo.Desired);
                yDatas.push(productChartSettingInfo.Alarm);
                productChart.fireEvent('onDesiredAlarmChange', productChartSettingInfo);
            }
        }

        me.setLeftAxisValue(isGroup, productName, productModelName, productChart, yDatas);
    },

    /**
     * 组单元格双击事件
     * @method prodProductRateGroupCellDblClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodProductRateGroupCellDblClick: function (params, e, eOpts) {
        params.grid.reportLayout.showProcessRelatedInfos(params, false);
    },

    /**
     * 单元格双击事件
     * @method prodProductRateItemCellDblClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodProductRateItemCellDblClick: function (params, e, eOpts) {
        params.grid.reportLayout.showProcessRelatedInfos(params, true);
    },

    /**
     * 显示下转报表
     * @method showProcessRelatedInfos
     * @param {Object} params 配置对象
     * @param {bool} isHasShift 是否包含产品
     */
    showProcessRelatedInfos: function (params, isHasShift) {
        var me = this;
        var cell = params.grid.getMatrix().results.get(params.leftKey, params.topKey);
        if (cell) {
            var title = isHasShift ? '产品工序直通率统计'.L10N() : '机型工序直通率统计'.L10N();
            var topItemName = params.topItem.name;
            var data = JSON.parse(JSON.stringify(cell.records[0].data));
            data.LineChartInfoList = null;
            data.LineChartSettingInfo = null;
            me.tabId = 'menu_' + 'SIE.MES.Statistics.Fpy.DefectStatistics,SIE.MES.Statistics.Fpy'.replace(/[.|,]/g, '');
            var tabItem = CRT.Workbench.getTabById(me.tabId);
            if (tabItem) {
                CRT.Event.fire('productReportClick', me._token, data, topItemName, isHasShift, title);
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
                    report: 'product'
                }
            });
        }
    },

    /**
     * 单元格单击事件
     * @method prodProductRateCellClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodProductRateCellClick: function (params, e, eOpts) {
        var layout = params.grid.reportLayout;
        var data = params.grid.getMatrix().results.getByLeftKey(params.leftKey)[0].records[0].data;
        var productName = data.ProductName;
        var productModelName = data.ProductModelName;
        if (layout && productName === layout._productName && productModelName === layout._productModelName)
            return;
        var productChart = Ext.getCmp('productChartId');
        var store = productChart.items.items[0].getStore();
        store.setData(null);
        productChart.items.items[0].setStore(store);
        if (layout) {
            layout._productName = productName;
            layout._productModelName = productModelName;
            layout.bindProductChartInfos(false, productName, productModelName);
        }
    },

    /**
     * 组单元格单击事件
     * @method prodProductRateGroupCellClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodProductRateGroupCellClick: function (params, e, eOpts) {
        var data = params.grid.getMatrix().results.getByLeftKey(params.leftKey)[0].records[0].data;
        var productName = data.ProductName;
        var productModelName = data.ProductModelName;
        var productChart = Ext.getCmp('productChartId');
        var store = productChart.items.items[0].getStore();
        store.setData(null);
        productChart.items.items[0].setStore(store);
        var layout = params.grid.reportLayout;
        if (layout) {
            layout._productName = productName;
            layout._productModelName = productModelName;
            layout.bindProductChartInfos(true, productName, productModelName);
        }
    },


    /**
     * 设置折线图的y轴坐标
     * @method setLeftAxisValue
     * @param {string} productName 车间名称
     * @param {string} productModelName 班次名称
     * @param {productChart} productChart 控件
     * @param {yDatas} yDatas 数组
     */
    setLeftAxisValue: function (isGroup, productName, productModelName, productChart, yDatas) {
        var minValue = Ext.Array.min(yDatas);
        var maxValue = Ext.Array.max(yDatas);

        if (minValue === maxValue === 0) {
            if (minValue == 0)
                maxValue = 120;
        }
        else {
            minValue = minValue - 20;
            maxValue = maxValue + 20;
        }

        if (minValue < 0)
            minValue = 0;

        if (isGroup)
            productChart.fireEvent('onLineChartTitleChange', productName, null, minValue, maxValue);
        else
            productChart.fireEvent('onLineChartTitleChange', productName, productModelName, minValue, maxValue);
    },

    /**
     * 绑定工序缺陷分类信息
     * @method bindDefectCategoryInfos
     * @param {Object} defectPanel 缺陷分类面板控件 
     * @param {SIE.Web.MES.DashBoard.Reports.ProductFPY.ProcessRelatedInfo} processRelatedInfo 工序相关信息
     */
    bindDefectCategoryInfos: function (defectPanel, processRelatedInfo) {
        var tpl = new Ext.XTemplate(
            '<div id="container" style="width: 100%;height: 100%;margin: 5px auto;">\
    <div style="width:100%;height: 45px;product-height: 45px;text-align:center;font-size: 22px;">缺陷代码TOP5</div>\
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
     * @method createProductDirectRate
     * @param {Object} me 当前视图对象
     * @return {Ext.create} 产线直通率报表控件
     */
    createProductDirectRate: function (me) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.ProductRateChart', {
            id: 'prodProductRateChartId',
            region: 'north',
            height: 400,
            border: false,
            reportLayout: me,
            listeners: {
                pivotitemcelldblclick: me.prodProductRateItemCellDblClick,
                pivotitemcellclick: me.prodProductRateCellClick,
                pivotgroupcelldblclick: me.prodProductRateGroupCellDblClick,
                pivotgroupcellclick: me.prodProductRateGroupCellClick
            },
        });
    },

    /**
    * 创建折线图控件
    * @method createProductChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createProductChart: function (me) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.LineChart', {
            id: 'productChartId',
            region: 'center',
            layout: 'fit',
            border: false,
        });
    },

    /**
     * 创建缺陷分类面板控件
     * @method createProductChart
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
    }
});
