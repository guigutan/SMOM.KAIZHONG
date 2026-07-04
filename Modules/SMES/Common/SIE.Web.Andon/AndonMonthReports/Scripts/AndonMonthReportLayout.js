Ext.define('SIE.Web.Andon.AndonMonthReports.Scripts.AndonMonthReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'AndonStatisticsReportLayout',
    _isRunning: false,
    _token: null,
    _criteria: null,
    _requestData: null,
    _listView: null,

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
        var lineChartControl = me.createLineChart(me, "andonRateChartId");
        var listView = me.createListView();
        this._listView = listView;
        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [{
                region: 'north',
                items: toolbar,
                border: false,
            }, {
                region: 'center',
                layout: 'vbox',
                xtype: 'panel',
                id: "esdReportMainPanel",
                _gListView: listView,
                border: false,
                items: [{
                    xtype: "panel",
                    height: 240,
                    region: "north",
                    layout: 'fit',
                    width: "100%",
                    flex: 1,
                    items: [listView.getControl()]
                },
                    lineChartControl
                ]
            }]
        });
    },
    /**
    * 创建折线图控件
    * @method createLineChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createLineChart: function (me, idname) {
        return Ext.create('SIE.Web.Andon.AndonMonthReports.Scripts.AndonMonthChart', {
            id: idname,
            region: 'south',
            layout: 'fit',
            width: "100%",
            height: "100%",
            flex: 1,
            border: false,
        });
    },
    createListView: function () {
        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.Andon.AndonMonthReports.AndonMonthReportViewModel',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            async: false,
            callback: function (res) {
                meta = res;
            }
        });
        meta.gridConfig.manageHeight = true;
        return SIE.AutoUI.createListView(meta);
    },

    /**
     * 查询(iqcreport)
     * @param {any} criteria
     * @param {any} token
     */
    loadReportData: function (criteria, token) {
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
                method: 'GetReportData',
                params: [criteria],
                action: 'queryer',
                async: true,
                type: 'SIE.Web.Andon.AndonMonthReports.AndonMonthReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        res.Result.andonMonthReportViewModels.forEach(function (item) {
                            item.SummaryDimension = item.SummaryDimension.t();
                        });
                        var resultdata = res.Result;
                        me.bindReportInfos(resultdata);
                        me._isRunning = false;
                        me.requestData = resultdata;//查询完后全局记录数据
                        Ext.getBody().unmask();
                    }
                    Ext.getBody().unmask();
                },
                error: function () {
                    Ext.getBody().unmask();
                }
            });

        } catch (e) {
            throw e;
        } finally {
            me._isRunning = false;
        }

    },

    /**
     * 绑定数据报表
     * @param {any} passRateInfos
     */
    bindReportInfos: function (passRateInfos) {
        var me = this;
        this._listView.getControl().enableColumnMove = false;
        this._listView.getControl().enableLocking = true;
        this._listView.getControl().enableColumnResize = false;

        this._listView.getData().loadData(passRateInfos.andonMonthReportViewModels);

        var datas = this._listView.getData().getData().items;
        datas.forEach(function (data) {
            if (data) data.markSaved();
        });

        this._requestData = passRateInfos.andonMonthReportViewModels;//缓存未加工数据
        var lineChart = Ext.getCmp('andonRateChartId');

        if (lineChart && passRateInfos.andonMonthReportViewModels) {
            var chartStore = lineChart.items.items[0].getStore();

            var andonNumData = [];
            var firstItem = passRateInfos.andonMonthReportViewModels[0];
            for (var index = 1; index < 13; index++) {
                var groupName = "";
                //中文
                if ("月份".t() === "月份") {
                    groupName = index + "月份";
                }
                //英文，若有其他的语言，请自行添加if条件，或改成switch case
                if ("月份".t() === "month") {
                    switch (index) {
                        case 1:
                            groupName = "January";
                            break;
                        case 2:
                            groupName = "February";
                            break;
                        case 3:
                            groupName = "March";
                            break;
                        case 4:
                            groupName = "April";
                            break;
                        case 5:
                            groupName = "May";
                            break;
                        case 6:
                            groupName = "June";
                            break;
                        case 7:
                            groupName = "July";
                            break;
                        case 8:
                            groupName = "August";
                            break;
                        case 9:
                            groupName = "September";
                            break;
                        case 10:
                            groupName = "October";
                            break;
                        case 11:
                            groupName = "November";
                            break;
                        case 12:
                            groupName = "December";
                            break;
                    }
                }
                andonNumData.push({
                    GroupName: groupName,
                    AndonNum: firstItem != undefined ? firstItem["AndonNum" + index] : 0,
                    AndonTime: firstItem != undefined ? firstItem["AndonTime" + index] : 0,
                    AndonStopNum: firstItem != undefined ? firstItem["AndonStopNum" + index] : 0,
                    AndonStopLine1: firstItem != undefined ? firstItem["AndonStopLine" + index] : 0,
                    TriggerAccuracy: firstItem != undefined ? firstItem["TriggerAccuracy" + index] : 0,
                });
            }
            chartStore.setData(andonNumData);
            lineChart.items.items[0].setStore(chartStore);
            if (firstItem) {
                //this._listView.gridConfig.columns[0].header = firstItem.SummaryDimensionTitle;
                //this._listView.gridConfig.columns[1].header = firstItem.GroupNameTitle;
                var grid = this._listView.getControl().ownerGrid.actionables[0].grid;

                //var gridColumns = grid.config.columns; 
                /*  grid.reconfigure(grid.store, gridColumns); 影响到导出代码 暂时取消列头变化 */
                grid.mon(grid, 'rowclick', me.rowclick, this._listView);
            }


        }

        var mainPanel = Ext.getCmp("esdReportMainPanel");
        if (mainPanel) {
            mainPanel._dataLoaded = true; //数据已加载
        }
    },
    rowclick: function (g, record, element, rowIndex, e, eOpts) {
        if (record.data) {
            var newData = record.data;
            var lineChart = Ext.getCmp('andonRateChartId');
            var chartStore = lineChart.items.items[0].getStore();

            var andonNumData = [];
            for (var index = 1; index < 13; index++) {
                var groupName = "";
                //中文
                if ("月份".t() === "月份") {
                    groupName = index + "月份";
                }
                //英文，若有其他的语言，请自行添加if条件，或改成switch case
                if ("月份".t() === "month") {
                    switch (index) {
                        case 1:
                            groupName = "January";
                            break;
                        case 2:
                            groupName = "February";
                            break;
                        case 3:
                            groupName = "March";
                            break;
                        case 4:
                            groupName = "April";
                            break;
                        case 5:
                            groupName = "May";
                            break;
                        case 6:
                            groupName = "June";
                            break;
                        case 7:
                            groupName = "July";
                            break;
                        case 8:
                            groupName = "August";
                            break;
                        case 9:
                            groupName = "September";
                            break;
                        case 10:
                            groupName = "October";
                            break;
                        case 11:
                            groupName = "November";
                            break;
                        case 12:
                            groupName = "December";
                            break;
                    }
                }
                andonNumData.push({
                    GroupName: groupName,
                    AndonNum: newData["AndonNum" + index],
                    AndonTime: newData["AndonTime" + index],
                    AndonStopNum: newData["AndonStopNum" + index],
                    AndonStopLine1: newData["AndonStopLine" + index],
                    TriggerAccuracy: newData["TriggerAccuracy" + index],
                });
            }
            chartStore.setData(andonNumData);
            lineChart.items.items[0].setStore(chartStore);
        }
    }
});