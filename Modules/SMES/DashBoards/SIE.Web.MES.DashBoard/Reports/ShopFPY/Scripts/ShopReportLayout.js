/**
 * 产线直通率报表布局 
 * @class SIE.Web.MES.DashBoard.Reports.ShopFPY.Scripts.ShopReportLayout
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Reports.ShopFPY.Scripts.ShopReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'ShopReportLayout',
    /**
     * 是否正在运行中
     * @property {bool} _isRunning
     */
    _isRunning: false,

    /**
    * 子页签Id
    * @property {string} _tabId
    */
    tabId: null,

    /**
     * 凭证
     * @property {string} _token
     */
    _token: null,

    /**
     * 缓存产线直通率及其相关数据列表
     * @property {List<SIE.Web.MES.DashBoard.Reports.ShopFPY.ShopionShopRateInfo>} shopShopRateInfos
     */
    shopShopRateInfos: null,

    /**
     * 缓存某车间班次的折线图信息数组
     * @property {SIE.Web.MES.DashBoard.Reports.ShopFPY.ShopChartInfo[]} shopChartInfos
     */
    shopChartInfos: null,

    /**
     * 缓存车间的折线图目标\警告设置信息数组
     * @property {SIE.Web.MES.DashBoard.Reports.ShopFPY.ShopChartSettingInfo[]} shopChartSettingInfos
     */
    shopChartSettingInfos: null,

    /**
     * 车间
     * @property {string} _shopName
     */
    _shopName: null,

    /**
     * 班次
     * @property {string} _resourceName
     */
    _resourceName: null,

    /**
     * 缺陷控件
     * @property defectChart
     */
    defectChart: null,

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
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;
        me.shopShopRateInfos = null;
        me.defectChart = null;
        me.tabId = null;
        me.shopChartSettingInfos = [];
        me.processDirectRateChart = null;
        me.processStatisticsChart = null;
        regions.main._view._relations[0]._target.mainLayout = me;
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });
        //表格
        var directRateControl = me.createShopDirectRate(me);
        //折线图
        var shopChartControl = me.createShopChart(me);
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
                items: [directRateControl, shopChartControl]
            }]
        });
    },

    /**
     * 加载产线直通率报表
     * @method loadShopReportData
     * @param {SIE.Web.MES.DashBoard.Reports.ShopFPY.ShopReportViewModelCriteria} criteria 标签控件
     * @param {token} token 新激活子页签
     */
    loadShopReportData: function (criteria, token) {
        var me = this;
        me._token = token;
        me._shopName = "";
        me._resourceName = "";
        try {
            if (me._isRunning)
                return;
            me._isRunning = true;
            SIE.invokeDataQuery({
                method: 'GetShopRateInfos',
                params: [criteria],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.MES.DashBoard.Reports.ShopFPY.DataQuery.ShopReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        me.shopShopRateInfos = res.Result;
                        me.bindShopShopRateInfos(me.shopShopRateInfos);
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
     * @method bindShopShopRateInfos
     * @param {List<SIE.Web.MES.DashBoard.Reports.ShopFPY.ShopionShopRateInfo>} shopShopRateInfos 新激活子页签
     */
    bindShopShopRateInfos: function (shopShopRateInfos) {
        var me = this;
        var prodShopRateChart = Ext.getCmp('prodShopRateChartId');
        var store = prodShopRateChart.getStore();
        store.setData(shopShopRateInfos);
        prodShopRateChart.setStore(store);

        //遍历产线直通率列表
        let flag = true;
        shopShopRateInfos.forEach(function (prodShopRateInfo) {
            if (prodShopRateInfo.ShopChartSettingInfo) {
                me.shopChartSettingInfos.forEach(function (shopChartSettingInfo) {
                    if (shopChartSettingInfo.ResourceName == prodShopRateInfo.ShopChartSettingInfo.ResourceName)
                        flag = false;
                });

                if (flag)
                    me.shopChartSettingInfos.push(prodShopRateInfo.ShopChartSettingInfo);
                flag = true;
            }
        })
    },

    /**
 * 绑定折线图信息
 * @method bindShopChartInfos
 * @param {string} shopName 车间名称
 * @param {string} resourceName 班次名称
 */
    bindShopChartInfos: function (isGroup, shopName, resourceName) {
        var me = this;
        me.shopChartInfos = [];
        var yDatas = [];

        //遍历产线直通率列表
        me.shopShopRateInfos.forEach(function (prodShopRateInfo) {
            if (prodShopRateInfo.ShopName === shopName && prodShopRateInfo.ResourceName === resourceName) {
                prodShopRateInfo.ShopChartInfoList.forEach(function (shopChartInfo) {
                    me.shopChartInfos.push(shopChartInfo);
                    yDatas.push(shopChartInfo.YData);
                });
            }
        });

        var shopChart = Ext.getCmp('shopChartId');
        var store = shopChart.items.items[0].getStore();
        store.setData(me.shopChartInfos);
        shopChart.items.items[0].setStore(store);

        shopChart.fireEvent('onDesiredAlarmChange', null);
        for (i = 0; i < me.shopChartSettingInfos.length; i++) {
            var shopChartSettingInfo = me.shopChartSettingInfos[i];
            if (shopChartSettingInfo.ResourceName == resourceName) {
                yDatas.push(shopChartSettingInfo.Desired);
                yDatas.push(shopChartSettingInfo.Alarm);
                shopChart.fireEvent('onDesiredAlarmChange', shopChartSettingInfo);
            }
        }

        me.setLeftAxisValue(isGroup, shopName, resourceName, shopChart, yDatas);
    },

    /**
     * 组单元格双击事件
     * @method prodShopRateGroupCellDblClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodShopRateGroupCellDblClick: function (params, e, eOpts) {
        params.grid.reportLayout.showProcessRelatedInfos(params, false);
    },

    /**
     * 单元格双击事件
     * @method prodShopRateItemCellDblClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodShopRateItemCellDblClick: function (params, e, eOpts) {
        params.grid.reportLayout.showProcessRelatedInfos(params, true);
    },

    /**
     * 显示下转报表
     * @method showProcessRelatedInfos
     * @param {Object} params 配置对象
     * @param {bool} isHasShift 是否包含资源
     */
    showProcessRelatedInfos: function (params, isHasShift) {
        var me = this;
        var cell = params.grid.getMatrix().results.get(params.leftKey, params.topKey);
        if (cell) {
            var title = isHasShift ? '资源工序直通率统计'.L10N() : '车间工序直通率统计'.L10N();
            var topItemName = params.topItem.name;
            var data = JSON.parse(JSON.stringify(cell.records[0].data));
            data.LineChartInfoList = null;
            data.LineChartSettingInfo = null;
            me.tabId = 'menu_' + 'SIE.MES.Statistics.Fpy.DefectStatistics,SIE.MES.Statistics.Fpy'.replace(/[.|,]/g, '');
            var tabItem = CRT.Workbench.getTabById(me.tabId);
            if (tabItem) {
                CRT.Event.fire('shopReportClick', me._token, data, topItemName, isHasShift, title);
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
                    report: 'shop'
                }
            });
        }
    },

    /**
     * 单元格单击事件
     * @method prodShopRateCellClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodShopRateCellClick: function (params, e, eOpts) {
        var layout = params.grid.reportLayout;
        var data = params.grid.getMatrix().results.getByLeftKey(params.leftKey)[0].records[0].data;
        var shopName = data.ShopName;
        var resourceName = data.ResourceName;
        if (layout && shopName === layout._shopName && resourceName === layout._resourceName)
            return;
        var shopChart = Ext.getCmp('shopChartId');
        var store = shopChart.items.items[0].getStore();
        store.setData(null);
        shopChart.items.items[0].setStore(store); 
        if (layout) {
            layout._shopName = shopName;
            layout._resourceName = resourceName;
            layout.bindShopChartInfos(false, shopName, resourceName);
        }
    },

    /**
     * 组单元格单击事件
     * @method prodShopRateGroupCellClick
     * @param {Object} params 配置对象
     * @param {Ext.event.Event} e 事件参数
     * @param { Object} eOpts 参数
     */
    prodShopRateGroupCellClick: function (params, e, eOpts) {
        var data = params.grid.getMatrix().results.getByLeftKey(params.leftKey)[0].records[0].data;
        var shopName = data.ShopName;
        var resourceName = data.ResourceName;
        var shopChart = Ext.getCmp('shopChartId');
        var store = shopChart.items.items[0].getStore();
        store.setData(null);
        shopChart.items.items[0].setStore(store);
        var layout = params.grid.reportLayout;
        if (layout) {
            layout._shopName = shopName;
            layout._resourceName = resourceName;
            layout.bindShopChartInfos(true, shopName, resourceName);
        }
    },


    /**
     * 设置折线图的y轴坐标
     * @method setLeftAxisValue
     * @param {string} shopName 车间名称
     * @param {string} resourceName 班次名称
     * @param {shopChart} shopChart 控件
     * @param {yDatas} yDatas 数组
     */
    setLeftAxisValue: function (isGroup, shopName, resourceName, shopChart, yDatas) {
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
            shopChart.fireEvent('onLineChartTitleChange', shopName, null, minValue, maxValue);
        else
            shopChart.fireEvent('onLineChartTitleChange', shopName, resourceName, minValue, maxValue);
    },

    /**
     * 绑定工序缺陷分类信息
     * @method bindDefectCategoryInfos
     * @param {Object} defectPanel 缺陷分类面板控件 
     * @param {SIE.Web.MES.DashBoard.Reports.ShopFPY.ProcessRelatedInfo} processRelatedInfo 工序相关信息
     */
    bindDefectCategoryInfos: function (defectPanel, processRelatedInfo) {
        var tpl = new Ext.XTemplate(
            '<div id="container" style="width: 100%;height: 100%;margin: 5px auto;">\
    <div style="width:100%;height: 45px;shop-height: 45px;text-align:center;font-size: 22px;">缺陷代码TOP5</div>\
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
     * @method createShopDirectRate
     * @param {Object} me 当前视图对象
     * @return {Ext.create} 产线直通率报表控件
     */
    createShopDirectRate: function (me) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.ShopRateChart', {
            id: 'prodShopRateChartId',
            region: 'north',
            height: 400,
            border: false,
            reportLayout: me,
            listeners: {
                pivotitemcelldblclick: me.prodShopRateItemCellDblClick,
                pivotitemcellclick: me.prodShopRateCellClick,
                pivotgroupcelldblclick: me.prodShopRateGroupCellDblClick,
                pivotgroupcellclick: me.prodShopRateGroupCellClick
            },
        });
    },

    /**
    * 创建折线图控件
    * @method createShopChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createShopChart: function (me) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.LineChart', {
            id: 'shopChartId',
            region: 'center',
            layout: 'fit',
            border: false,
        });
    },

    /**
     * 创建缺陷分类面板控件
     * @method createShopChart
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

    /**
     * 创建工序相关控件
     * @method createProcessPanel
     * @param {Ext.panel.Panel} defectPanel 当前视图对象
     * @return {Ext.widget} 布局
     */
    createProcessPanel: function (defectPanel) {
        var me = this;
        var procDirectRateControl = me.createProcessDirectRateChart(me);
        var procStatisticsControl = me.createProcessStatisticsChart(me);
        var defectControl = me.createDefectChart(me);
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
    createProcessDirectRateChart: function (me) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.ProcessDirectRateChart', {
            id: 'proProcessDirectRateChartId',
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
    createProcessStatisticsChart: function (me) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.ProcessStatisticsChart', {
            region: 'center',
            id: 'proProcessStatisticsChartId',
            layout: 'fit',
            border: false,
        });
    },

    /**
    * 创建缺陷报表控件
    * @method createDefectChart
    * @param {Object} productMe 当前视图对象
    * @return {Ext.create} 缺陷报表控件
    */
    createDefectChart: function (productMe) {
        return Ext.create('SIE.Web.MES.DashBoard.Common.DefectChart', {
            id: 'shopDefectChartId',
            height: '100%',
            width: '100%',
            layout: 'fit',
            flex: 1,
            border: false,
        });
    }
});
