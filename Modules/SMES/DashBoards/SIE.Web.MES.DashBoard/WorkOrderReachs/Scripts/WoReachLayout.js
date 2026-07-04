Ext.define('SIE.Web.MES.DashBoard.WorkOrderReachs.ReachModel', function () {
    return {
        extend: 'Ext.data.Model',
        fields: [
            { name: 'id', type: 'int' },
            { name: 'woInfo', type: 'string' },
            { name: 'Date', type: 'date', dateFormat: 'c' },
            { name: 'ReachData', type: 'float', allowNull: true },
            { name: 'IsRate', type: 'bool' },
            {
                name: 'year',
                calculate: function (data) {
                    return parseInt(Ext.Date.format(data.Date, "Y"), 10);
                }
            }, {
                name: 'month',
                calculate: function (data) {
                    return parseInt(data.Date.getFullYear() + '' + (data.Date.getMonth() + 1));
                }
            }, {
                name: 'day',
                calculate: function (data) {
                    return parseInt(data.Date.getFullYear() + '' + (Ext.Date.format(data.Date, "m")) + '' + data.Date.getDate());
                }
            }, {
                name: 'week',
                calculate: function (data) {
                    return parseInt(Ext.Date.format(data.Date, "W"), 10);
                }
            }
        ]
    };
});
Ext.define('SIE.Web.MES.DashBoard.WorkOrderReachs.ReachStore', {
    extend: 'Ext.data.Store',
    alias: 'store.reachStore',
    model: 'SIE.Web.MES.DashBoard.WorkOrderReachs.ReachModel',
    autoLoad: false
});
Ext.define('SIE.Web.MES.DashBoard.WorkOrderReachs.ReachRateGrid', {
    extend: 'SIE.Web.MES.DashBoard.Common.ProductLineRateChart',
    xtype: 'ReachRateGrid',
    controller: 'PivotControllerReach',
    requires: [
        'Ext.pivot.plugin.Exporter',
        'Ext.pivot.plugin.Configurator'
    ],
    matrix: {
        type: 'local',
        rowGrandTotalsPosition: 'none',
        colGrandTotalsPosition: 'none',
        rowSubTotalsPosition: 'none',
        colSubTotalsPosition: 'none',
        collapsibleRows: true,
        viewLayoutType: 'outline',
        store: {
            type: 'reachStore'
        },
        aggregate: [{
            dataIndex: 'ReachData',
            header: 'Value',
            width: 100,
            cellConfig: {
                viewModel: {
                    type: 'pivot-cell-model'
                },
                bind: {
                    userCls: '{value:sign("pivotCellNegative","pivotCellPositive")}'
                }
            },
            renderer: function (value, meta, record) {
                var chartControl = Ext.getCmp('pivotGridReach').ownerGrid;
                var data = chartControl.getMatrix().results.getByLeftKey(record.data.leftAxisKey)[0].records[0].data;
                return data.IsRate ? Ext.util.Format.number(value * 100, '0.00%') : value;
            }
        }],
        leftAxis: [{
            dataIndex: 'WoInfo',
            header: '工单信息'.t(),
            sortable: true,
            width: 100
        }],
        topAxis: [{
            dataIndex: 'day',
            header: '日'.t(),
            labelRenderer: 'dayLabelRenderer'
        }]
    },
    plugins: {
        pivotexporter: true,
        pivotconfigurator: {
            fields: [
                {
                    dataIndex: 'WoInfo',
                    header: '工单信息'.t(),
                    settings: {
                        aggregators: ['count'],
                        fixed: ['leftAxis']
                    }
                },
                {
                    dataIndex: 'year',
                    header: '年'.t(),
                    labelRenderer: 'yearLabelRenderer',
                    settings: {
                        aggregators: ['count'],
                        fixed: ['leftAxis', 'topAxis']
                    }
                }, {
                    dataIndex: 'month',
                    header: '月'.t(),
                    labelRenderer: 'monthLabelRenderer',

                    settings: {
                        aggregators: ['count'],
                        allowed: ['leftAxis', 'topAxis']
                    }
                }, {
                    dataIndex: 'day',
                    header: '日'.t(),
                    labelRenderer: 'dayLabelRenderer',

                    settings: {
                        aggregators: ['count'],
                        allowed: ['leftAxis', 'topAxis']
                    }
                }, {
                    dataIndex: 'week',
                    header: '周'.t(),
                    labelRenderer: 'weekLabelRenderer',
                    settings: {
                        aggregators: ['count'],
                        allowed: ['leftAxis', 'topAxis']
                    }
                }]
        }
    },
    header: {
        itemPosition: 1,
        items: [{
            xtype: 'button',
            text: '导出'.t(),
            handler: 'exportTo',
            cfg: {
                type: 'excel07',
                ext: 'xlsx'
            }
        }]
    }
});
Ext.define('SIE.Web.MES.DashBoard.WorkOrderReachs.WoReachLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'WoReachLayout',
    _isRunning: false,
    _token: null,
    chartId: 'reachReport-chart',
    chartControl: null,
    _dateType: 'day',
    _layoutChildren: function (regions) {
        var me = this;
        var main = regions.main;
        var mainControl = main.getControl();
        control = mainControl;
        view = main.getView();
        view.mainLayout = me;
        var toolbar = mainControl.getDockedItems()[0];
        chartControl = me.initChartControl();
        return Ext.widget('container', {
            layout: {
                type: 'vbox',
                pack: 'start',
                align: 'stretch'
            },
            defaults: {
                frame: true,//底色变化
                bodyPadding: 0
            },
            xtype: 'layout-vertical-box',
            requires: ['Ext.layout.container.VBox'],
            items: [{
                baseCls: 'my-panel-no-border',
                items: toolbar,
            }, {
                id: 'pivotGridReach',
                xtype: 'ReachRateGrid',
                mainView: view,
                height: (window.innerHeight - 148) / 2,
                listeners: {
                    pivotitemcelldblclick: me._pivotitemcelldblclick,
                    configChange: function (panel, config, eOpts) {
                        var nowType = config.topAxis[0].dataIndex;
                        if (nowType != me._dateType) {
                            me._dateType = nowType;
                            var target = this.view.up().up().up().up().view._relations[0]._target;
                            var queryData = target.getCurrent().data;
                            if (nowType == "year") {
                                queryData.DateType = 0;
                            }
                            else if (nowType == "month") {
                                queryData.DateType = 1;
                            }
                            else if (nowType == "week") {
                                queryData.DateType = 3;
                            }
                            else if (nowType == "day") {
                                queryData.DateType = 2;
                            }
                            SIE.invokeDataQuery({
                                method: 'GetWoReachData',
                                params: [queryData],
                                action: 'queryer',
                                async: false,
                                type: 'SIE.Web.MES.DashBoard.WorkOrderReachs.WoReachDataQueryer',
                                token: target.token,
                                success: function (res) {
                                    if (res.Success) {
                                        var chartData = res.Result.ChartJsonData;
                                        chartControl.initStore(chartData);
                                    }
                                }
                            });
                        }
                    },
                },
                border: false
            }, chartControl
            ]
        });
    },
    loadReachData: function (criteria, token) {
        var me = this;
        _token = token;
        try {
            if (me._isRunning)
                return;
            me._isRunning = true;
            if (me._dateType == "year") {
                criteria.DateType = 0;
            }
            else if (me._dateType == "month") {
                criteria.DateType = 1;
            }
            else if (me._dateType == "week") {
                criteria.DateType = 3;
            }
            else if (me._dateType == "day") {
                criteria.DateType = 2;
            }
            SIE.invokeDataQuery({
                method: 'GetWoReachData',
                params: [criteria],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.MES.DashBoard.WorkOrderReachs.WoReachDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        var reachDataList = res.Result.ReachDataList;
                        var pivotGrid = Ext.getCmp('pivotGridReach');
                        var store = pivotGrid.getStore();
                        store.setData(reachDataList);
                        pivotGrid.setStore(store);
                        var chartData = res.Result.ChartJsonData;
                        chartControl.initStore(chartData);
                    }
                }
            });

        } catch (e) {
            throw e;
        } finally {
            me._isRunning = false;
        }
    },
    _pivotitemcelldblclick: function (params, e, eOpts) {
        //var data = params.grid.getMatrix().results.getByLeftKey(params.leftKey)[0].records[0].data;

        var datas = params.grid.getMatrix().results.getByLeftKey(params.leftKey);
        var data = datas.find(m => m.topKey == params.topKey).records[0].data;
        if (data.WoInfo == '准时达成率'.t()) {
            var columnFieldValue = "";
            var xview = this.view.ownerGrid.ownerCt.items.items[1].mainView;
            var criData = xview._relations[0]._target.getData().data;
            if (params.column.group) {
                //判断是否有分组，转换成后台需要的数据格式
                if (params.column.group.name.indexOf('月'.t()) != -1) {
                    columnFieldValue = data.year + '年'.t() + data.month.toString().replace(data.year, '') + '月'.t();
                    criData.DateType = 1;
                }
                else if (params.column.group.name.indexOf('年'.t()) != -1) {
                    columnFieldValue = data.year;
                    criData.DateType = 0;
                }
                else if (params.column.group.name.indexOf('周'.t()) != -1) {
                    columnFieldValue = data.year + '年'.t() + params.column.group.name;
                    criData.DateType = 3;
                }
                else return;
            }
            else {
                columnFieldValue = data.Date.toLocaleDateString();
                criData.DateType = 2;
            }
            var title = Ext.String.format('{0}工单准时达成率报表'.L10N(), columnFieldValue);
            var tabId = "WoReachLayoutDetail" + columnFieldValue.replace('年'.t(), '').replace('月'.t(), '').replace('周'.t(), '').replace('/', '').replace('/', '');
            var tabItem = CRT.Workbench.getTabById(tabId);
            if (tabItem) {
                CRT.Event.fire('woReachClick', columnFieldValue, criData, title);
                return;
            }
            CRT.Workbench.addPage({
                tabId: tabId,
                entityType: 'SIE.MES.DashBoard.WorkOrderReachs.WoReachDetailViewModel',
                title: title,
                ignoreQuery: true,
                module: view.module,
                params: {
                    tabId: tabId,
                    columnFieldValue: columnFieldValue,
                    criData: criData,
                    token: _token
                }
            });
        }

    }, //初始化图形控件
    initChartControl: function () {
        return {
            xtype: 'container',
            id: 'ReachLayoutchartContent',
            chartId: this.chartId,
            parent: this,
            width: '100%',
            html: "<div id='chartDivReach' style='width:100%;height:100%;'><div>",
            listeners: {
                afterRender: function (comp) {
                    var cont = Ext.getCmp(this.chartId);
                    var store = Ext.create('Ext.data.Store', {
                        data: null,
                    });
                    this.parent.loadChartData(store);
                }
            },
            initStore: function (queryData) {
                var cont = Ext.getCmp(this.chartId);
                var store = Ext.create('Ext.data.Store', {
                    data: queryData,
                });
                this.parent.loadChartData(store);
            },

        }
    },
    //加载图形
    loadChartData: function (store) {
        var x = window.innerHeight - 148;
        if (!Ext.Object.isEmpty(Ext.getCmp(this.chartId))) {
            Ext.destroy(Ext.getCmp(this.chartId));
        }

        Ext.create('Ext.chart.Chart', {
            renderTo: "chartDivReach",
            controller: 'WoReachController',
            id: this.chartId,
            bodyStyle: 'border-width:0px;',
            width: '100%',
            height: x / 2,
            interactions: {
                type: 'panzoom',
                zoomOnPanGesture: true
            },
            animation: {
                duration: 200
            },
            store: store,
            innerPadding: {
                left: 40,
                right: 40
            },
            legend: {
                type: 'sprite',
                docked: 'bottom'
            },
            axes: [{
                type: 'numeric',
                position: 'left',
                grid: true,
                minimum: 0,
                //maximum: max,             
                fields: ['totalQty', 'completeQty', 'closedQty'],
            }, {
                type: 'category',
                position: 'bottom',
                fields: ['monthDay'],
                grid: true,
                label: {
                    rotate: {
                        degrees: -15
                    }
                }
            },
            {
                type: 'numeric',
                position: 'right',
                minimum: 0,
                maximum: 1,
                fields: ['reachRate', 'closedRate'],
                reconcileRange: true,
                majorTickSteps: 10,
                renderer: 'onAxisLabelRender'
            }],
            series: [{
                type: 'bar',
                axis: 'left',
                title: ['工单总数'.t(), '准时完工数'.t(), '工单完工数'.t()],
                xField: 'monthDay',
                stacked: false,
                yField: ['totalQty', 'completeQty', 'closedQty'],
                highlight: {
                    fillStyle: 'yellow',
                    strokeStyle: 'black'
                },
                tooltip: {
                    trackMouse: true,
                    renderer: 'onBarSeriesTooltipRender'
                },
                label: {
                    display: 'insideEnd',
                    'text-anchor': 'middle',
                    field: ['totalQty', 'completeQty', 'closedQty'],
                    renderer: Ext.util.Format.numberRenderer('0')
                },
            },
            {
                type: 'line',
                title: '准时达成率'.t(),
                xField: 'monthDay',
                yField: 'completeRate',
                style: {
                    lineWidth: 2,
                    opacity: 0.80,
                },
                label: {
                    display: 'over',
                    field: 'completeRate',
                    renderer: 'onSeriesLabelRender'
                },
                marker: {
                    animation: {
                        duration: 200,
                        easing: 'backOut'
                    }
                },
                highlightCfg: {
                    scaling: 2,
                    rotationRads: Math.PI / 4
                },
                tooltip: {
                    trackMouse: true,
                    renderer: 'onLineSeriesTooltipRender'
                }
            },
            {
                type: 'line',
                title: '工单完工率'.t(),
                xField: 'monthDay',
                yField: 'closedRate',
                style: {
                    lineWidth: 2,
                    opacity: 0.80,
                },
                label: {
                    display: 'over',
                    field: 'closedRate',
                    renderer: 'onSeriesLabelRender'
                },
                marker: {
                    animation: {
                        duration: 200,
                        easing: 'backOut'
                    }
                },
                highlightCfg: {
                    scaling: 2,
                    rotationRads: Math.PI / 4
                },
                tooltip: {
                    trackMouse: true,
                    renderer: 'onLineSeriesTooltipRender'
                }
            }
            ],
        })
    }
});