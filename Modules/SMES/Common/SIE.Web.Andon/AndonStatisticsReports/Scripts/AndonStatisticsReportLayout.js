Ext.define('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticsReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'AndonStatisticsReportLayout',
    _isRunning: false,
    _token: null,
    _criteria: null,
    andonTimeCommand: null,
    andonNumCommand: null,
    andonStopLineCommand: null,
    andonStopNumCommand: null,
    triggerAccuracyCommand: null,
    requestData: null,
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
        var barChart = me.createBarChart(me, "andonBarChartId");
        var pieChart = me.createPieChart(me, "andonPieChartId");
        me.createCommands();
        var listView = me.createListView();
        this._listView = listView;
       // _gListView=listView;
        var childItems = [];   //子页签
        childItems.push({
            title: '柏拉图'.L10N(),
            layout: 'border',
            items: [
                {
                    region: 'north',
                    items: {
                        xtype: 'toolbar',
                        border: 0,
                        margin: '5 0 0 0 ',
                        items: [me.getCommandConfig("andonNumCommand", me.andonNumCommand),
                            me.getCommandConfig("andonTimeCommand", me.andonTimeCommand),
                            me.getCommandConfig("andonStopLineCommand", me.andonStopLineCommand),
                            me.getCommandConfig("andonStopNumCommand", me.andonStopNumCommand),
                            me.getCommandConfig("triggerAccuracyCommand", me.triggerAccuracyCommand)
                        ]
                    },
                    border: false,
                },
                {
                    region: 'center',
                    layout: 'vbox',
                    items: [lineChartControl],
                }
            ]
        }, {
            title: '柱状图'.L10N(),
            layout: 'border',
            items: [{
                region: 'center',
                layout: 'vbox',
                items: [barChart],
            }]
        }, {
            title: '饼图'.L10N(),
            layout: 'border',
            items: [{
                region: 'north',
                items: {
                    xtype: 'toolbar',
                    border: 0,
                    margin: '5 0 0 0 ',
                    items: [me.getCommandConfig("andonNumCommand", me.andonNumCommand),
                        me.getCommandConfig("andonTimeCommand", me.andonTimeCommand),
                        me.getCommandConfig("andonStopLineCommand", me.andonStopLineCommand),
                        me.getCommandConfig("andonStopNumCommand", me.andonStopNumCommand),
                        me.getCommandConfig("triggerAccuracyCommand", me.triggerAccuracyCommand)
                    ]
                },
                border: false,
            },
            {
                region: 'center',
                layout: 'vbox',
                border: false,
                items: [pieChart],
            }]
        });
        var tabpanel = Ext.create('Ext.tab.Panel', {
            border: false,
            region: 'south',
            layout: 'fit',
            width: "100%",
            tabPosition: 'top',
            split: true,
            id: "andonReportTabPanel",
            flex: 1,
            defaults: {
                scrollable: true,
                closable: false,
                border: false
            },
            items: childItems
        });
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
                    tabpanel
                    //lineChartControl
                ]
            }]
        });
    },
    createPieChart: function (me, idname) {
        return Ext.create('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticsPieChart', {
            id: idname,
            region: 'south',
            layout: 'fit',
            width: "100%",
            flex: 1,
            border: false,
        });
    },

    /**
     * 创建柱状图
     * @param {any} me
     * @param {any} idname
     */

    createBarChart: function (me, idname) {
        return Ext.create('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticsBarChart', {
            id: idname,
            region: 'south',
            layout: 'fit',
            width: "100%",
            flex: 1,
            border: false,
        });
    },
    /**
    * 创建折线图控件
    * @method createLineChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createLineChart: function (me, idname) {
        return Ext.create('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticsChart', {
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
            model: 'SIE.Andon.AndonStatisticsReports.AndonStatisticsViewModel',
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
    createCommands: function () {
        var me = this;
        me.andonTimeCommand = Ext.create('SIE.Web.Andon.AndonStatisticsReports.Commands.AndonTimeCommand');
        me.andonNumCommand = Ext.create('SIE.Web.Andon.AndonStatisticsReports.Commands.AndonNumCommand');

        me.andonStopLineCommand = Ext.create('SIE.Web.Andon.AndonStatisticsReports.Commands.AndonStopLineCommand');
        me.andonStopNumCommand = Ext.create('SIE.Web.Andon.AndonStatisticsReports.Commands.AndonStopNumCommand');
        me.triggerAccuracyCommand = Ext.create('SIE.Web.Andon.AndonStatisticsReports.Commands.TriggerAccuracyCommand');

        me.andonTimeCommand._ownerView = me;
        me.andonNumCommand._ownerView = me;

        me.andonStopLineCommand._ownerView = me;
        me.andonStopNumCommand._ownerView = me;
        me.triggerAccuracyCommand._ownerView = me;

    },
    getCommandConfig: function (name, command) {
        var me = this;
        return {
            xtype: 'button',
            command: name,
            text: command.config.meta.text,
            tooltipType: "title",
            tooltip: command.config.meta.tooltip,
            disabled: false,
            handler: function () {
                /*console.log(command.config.meta.text);*/
                me.switchData(command.config.meta.text, me);
                //command.tryExecute(me);
            }
        };
    },
    /**
     * 切换数据
     * */
    switchData: function (commanName, me) {
        debugger;
        var tabPanel = Ext.getCmp("andonReportTabPanel");
        var lineChart = Ext.getCmp('andonRateChartId');
        var pieChart = Ext.getCmp('andonPieChartId');

        switch (commanName) {
            case "安灯次数".L10N():
                if (tabPanel.activeTab.title == "柏拉图".L10N() && lineChart) {
                var chartStore = lineChart.items.items[0].getStore();
                    chartStore.removeAll();
                    var barData = [];
                    var totalNum = 0;
                    me.requestData.ChartsStatisticsDatas.forEach(function (item) {
                        barData.push({ GroupName: item.GroupName, AndonNum: item.AndonNum, LineValue: 0 });
                        totalNum += item.AndonNum;
                    });
                    //排序
                    var barSortData = barData.sort((a, b) => {
                        return b.AndonNum - a.AndonNum
                    })
                    //计算曲线
                    var totalLineValue = 0;
                    barSortData.forEach(function (item) {
                        totalLineValue += item.AndonNum;
                        var lineVaule = (totalLineValue / totalNum) * 100;
                        item.LineValue = lineVaule.toFixed(2);
                    });
                    chartStore.setData(barData);
                    lineChart.items.items[0].setStore(chartStore);
                }
                if (tabPanel.activeTab.title == "饼图".L10N() && pieChart) {
                    var chartStore = pieChart.items.items[0].getStore();
                    chartStore.removeAll();
                    var pieData = [];
                    me.requestData.PieChartsStatisticsDatas.forEach(function (item) {
                        pieData.push({ GroupName: item.GroupName, AndonNum: item.AndonNum })
                    });
                    chartStore.setData(pieData);
                    pieChart.items.items[0].setStore(chartStore);
                }
                break;
            case "安灯时长".L10N():
                if (tabPanel.activeTab.title == "柏拉图".L10N() && lineChart) {
                    var chartStore = lineChart.items.items[0].getStore();
                    chartStore.removeAll();

                    var barData = [];
                    var totalNum = 0;
                    me.requestData.ChartsStatisticsDatas.forEach(function (item) {
                        barData.push({ GroupName: item.GroupName, AndonNum: item.AndonTime, LineValue: 0 });
                        totalNum += item.AndonTime;
                    });
                    //排序
                    var barSortData = barData.sort((a, b) => {
                        return b.AndonNum - a.AndonNum
                    })
                    //计算曲线
                    var totalLineValue = 0;
                    barSortData.forEach(function (item) {
                        totalLineValue += item.AndonNum;
                        var lineVaule = (totalLineValue / totalNum) * 100;
                        item.LineValue = lineVaule.toFixed(2);
                    });

                    chartStore.setData(barData);
                    lineChart.items.items[0].setStore(chartStore);
                }
                if (tabPanel.activeTab.title == "饼图".L10N() && pieChart) {
                    var chartStore = pieChart.items.items[0].getStore();
                    chartStore.removeAll();
                    var pieData = [];
                    me.requestData.PieChartsStatisticsDatas.forEach(function (item) {
                        pieData.push({ GroupName: item.GroupName, AndonNum: item.AndonTime })
                    });

                    chartStore.setData(pieData);
                    pieChart.items.items[0].setStore(chartStore);
                }
                break;
            case "停线次数".L10N():
                if (tabPanel.activeTab.title == "柏拉图".L10N() && lineChart) {
                    var chartStore = lineChart.items.items[0].getStore();
                    chartStore.removeAll();
                    var barData = [];
                    var totalNum = 0;
                    me.requestData.ChartsStatisticsDatas.forEach(function (item) {
                        barData.push({ GroupName: item.GroupName, AndonNum: item.AndonStopNum, LineValue: 0 });
                        totalNum += item.AndonStopNum;
                    });
                    //排序
                    var barSortData = barData.sort((a, b) => {
                        return b.AndonNum - a.AndonNum
                    })
                    //计算曲线
                    var totalLineValue = 0;
                    barSortData.forEach(function (item) {
                        totalLineValue += item.AndonNum;
                        var lineVaule = (totalLineValue / totalNum) * 100;
                        item.LineValue = lineVaule.toFixed(2);
                    });

                    chartStore.setData(barData);
                    lineChart.items.items[0].setStore(chartStore);
                }
                if (tabPanel.activeTab.title == "饼图".L10N() && pieChart) {
                    var chartStore = pieChart.items.items[0].getStore();
                    chartStore.removeAll();
                    var pieData = [];
                    me.requestData.PieChartsStatisticsDatas.forEach(function (item) {
                        pieData.push({ GroupName: item.GroupName, AndonNum: item.AndonStopNum })
                    });
                    chartStore.setData(pieData);
                    pieChart.items.items[0].setStore(chartStore);
                }
                break;

            case "停线时长".L10N():
                if (tabPanel.activeTab.title == "柏拉图".L10N() && lineChart) {
                    var chartStore = lineChart.items.items[0].getStore();
                    chartStore.removeAll();

                    var barData = [];
                    var totalNum = 0;
                    me.requestData.ChartsStatisticsDatas.forEach(function (item) {
                        barData.push({ GroupName: item.GroupName, AndonNum: item.AndonStopLine, LineValue: 0 });
                        totalNum += item.AndonStopLine;
                    });
                    //排序
                    var barSortData = barData.sort((a, b) => {
                        return b.AndonNum - a.AndonNum
                    })
                    //计算曲线
                    var totalLineValue = 0;
                    barSortData.forEach(function (item) {
                        totalLineValue += item.AndonNum;
                        var lineVaule = (totalLineValue / totalNum) * 100;
                        item.LineValue = lineVaule.toFixed(2);
                    });

                    chartStore.setData(barData);
                    lineChart.items.items[0].setStore(chartStore);
                }
                if (tabPanel.activeTab.title == "饼图".L10N() && pieChart) {
                    var chartStore = pieChart.items.items[0].getStore();
                    chartStore.removeAll();
                    var pieData = [];
                    me.requestData.PieChartsStatisticsDatas.forEach(function (item) {
                        pieData.push({ GroupName: item.GroupName, AndonNum: item.AndonStopLine })
                    });
                    chartStore.setData(pieData);
                    pieChart.items.items[0].setStore(chartStore);
                }
                break;
            case "安灯名称变更率".L10N():
                if (tabPanel.activeTab.title == "柏拉图".L10N() && lineChart) {
                    var chartStore = lineChart.items.items[0].getStore();
                    chartStore.removeAll();

                    var barData = [];
                    var totalNum = 0;
                    me.requestData.ChartsStatisticsDatas.forEach(function (item) {
                        barData.push({ GroupName: item.GroupName, AndonNum: item.TriggerAccuracy, LineValue: 0 });
                        totalNum += item.TriggerAccuracy;
                    });
                    //排序
                    var barSortData = barData.sort((a, b) => {
                        return b.AndonNum - a.AndonNum
                    })
                    //计算曲线
                    var totalLineValue = 0;
                    barSortData.forEach(function (item) {
                        totalLineValue += item.AndonNum;
                        var lineVaule = (totalLineValue / totalNum).toFixed(2) * 100;
                        item.LineValue = lineVaule.toFixed(2);
                    });
                }
                if (tabPanel.activeTab.title == "饼图".L10N() && pieChart) {
                    var chartStore = pieChart.items.items[0].getStore();
                    chartStore.removeAll();
                    var pieData = [];
                    me.requestData.PieChartsStatisticsDatas.forEach(function (item) {
                        pieData.push({ GroupName: item.GroupName, AndonNum: item.TriggerAccuracy })
                    });
                    chartStore.setData(pieData);
                    pieChart.items.items[0].setStore(chartStore);
                }
                break;

        }
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
                type: 'SIE.Web.Andon.AndonStatisticsReports.AndonStatisticsReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        res.Result.StatisticsResultList.forEach(function (item) {
                            item.AndonClass = item.AndonClass;
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
        this._listView.getData().loadData(passRateInfos.StatisticsResultList);

        var datas = this._listView.getData().getData().items;
        datas.forEach(function (data) {
            if (data) data.markSaved();
        });

        var passRateData = passRateInfos.ChartsStatisticsDatas;
        var pieData = passRateInfos.PieChartsStatisticsDatas;
        var lineChart = Ext.getCmp('andonRateChartId');
        var barChart = Ext.getCmp('andonBarChartId');
        var pieChart = Ext.getCmp('andonPieChartId');
        
        if (lineChart) {
            var chartStore = lineChart.items.items[0].getStore();

            var barData = [];
            var totalNum = 0;
            passRateInfos.ChartsStatisticsDatas.forEach(function (item) {
                barData.push({ GroupName: item.GroupName, AndonNum: item.AndonNum, LineValue: 0 });
                totalNum += item.AndonNum;
            });
            //排序
           var barSortData= barData.sort((a, b) => {
               return b.AndonNum - a.AndonNum
           })
            //计算曲线
            var totalLineValue = 0;
            barSortData.forEach(function (item) {
                totalLineValue = totalLineValue+ item.AndonNum;
                var lineVaule = (totalLineValue / totalNum)  * 100;
                item.LineValue = lineVaule.toFixed(2);
                console.log(item.LineValue );
            });

            if (Ext.isEmpty(barSortData))
                chartStore.removeAll();
            else
                chartStore.setData(barSortData);
            lineChart.items.items[0].setStore(chartStore);
        }
        if (barChart) {
            var chartStore = barChart.items.items[0].getStore();
            if (Ext.isEmpty(passRateData))
                chartStore.removeAll();
            else
                chartStore.setData(passRateData);
            barChart.items.items[0].setStore(chartStore);
        }
        if (pieChart) {
            var chartStore = pieChart.items.items[0].getStore();
            if (Ext.isEmpty(pieData))
                chartStore.removeAll();
            else
                chartStore.setData(pieData);
            pieChart.items.items[0].setStore(chartStore);
        }
        
        var mainPanel = Ext.getCmp("esdReportMainPanel");
        if (mainPanel) {
            mainPanel._dataLoaded = true; //数据已加载
        }
    },
});