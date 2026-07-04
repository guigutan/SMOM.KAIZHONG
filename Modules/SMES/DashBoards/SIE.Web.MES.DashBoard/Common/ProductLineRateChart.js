/**
 * 直通率报表控件Model定义
 * @class SIE.Web.MES.DashBoard.Common.ProductLineRate
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProductLineRate', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'LineName', type: 'string' },
        { name: 'Shift', type: 'string' },
        { name: 'Date', type: 'date', dateFormat: 'c' },
        { name: 'DirectRate', type: 'float', allowNull: true },
        {
            name: 'year',
            calculate: function (data) {
                return parseInt(Ext.Date.format(data.Date, "Y"), 10);
            }
        }, {
            name: 'month',
            calculate: function (data) {
                return parseInt(Ext.Date.format(data.Date, "m"), 10);
            }
        }, {
            name: 'day',
            calculate: function (data) {
                return parseInt(Ext.Date.format(data.Date, "d"), 10);
            }
        }, {
            name: 'week',
            calculate: function (data) {
                return parseInt(Ext.Date.format(data.Date, "W"), 10);
            }
        }
    ]
});

/**
 * 直通率报表控件Store定义
 * @class SIE.Web.MES.DashBoard.Common.ProductLineRates
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProductLineRates', {
    extend: 'Ext.data.Store',
    alias: 'store.productLineRates',
    model: 'SIE.Web.MES.DashBoard.Common.ProductLineRate',
    autoLoad: false
});

/**
 * 直通率报表控件定义
 * @class SIE.Web.MES.DashBoard.Common.ProductLineRateChart
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProductLineRateChart', {
    extend: 'Ext.pivot.Grid',
    xtype: 'prodLineRateChart',
    controller: 'reportBaseController',
    requires: [
        //'SIE.Web.MES.DashBoard.Common.ReportBaseController',
        //'SIE.Web.MES.DashBoard.Common.ProductLineRates',
        'Ext.pivot.plugin.Exporter',
        'Ext.pivot.plugin.Configurator'
    ],
    width: 750,
    height: 350,
    collapsible: true,
    multiSelect: false,
    selModel: {
        type: 'cellmodel'
    },
    plugins: {
        pivotexporter: true,
        pivotconfigurator: {
            id: 'configurator',
            fields: [
                {
                    dataIndex: 'DirectRate',
                    header: '直通率值'.t(),
                    settings: {
                        allowed: 'aggregate',
                        aggregators: ['sum', 'avg', 'count'],
                        fixed: ['topAxis'],
                        style: {
                            fontWeight: 'bold'
                        },
                        renderers: {
                            'Colored 0,000.00': 'coloredRenderer'
                        },
                        formatters: {
                            '0%': 'number("0%")',
                            '0': 'number("0")',
                            '0.00': 'number("0.00")',
                            '0,000.00': 'number("0,000.00")',
                            '0.00%': 'number("0.00%")'
                        }
                    }
                },
                {
                    dataIndex: 'LineName',
                    header: '资源'.t(),

                    settings: {
                        aggregators: ['count'],
                        fixed: ['leftAxis']
                    }
                }, {
                    dataIndex: 'Shift',
                    header: '班次'.t(),

                    settings: {
                        aggregators: 'count',
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
    startRowGroupsCollapsed: false,
    matrix: {
        type: 'local',
        rowGrandTotalsPosition: 'none',
        colGrandTotalsPosition: 'none',
        //rowSubTotalsPosition: 'none',
        //colSubTotalsPosition: 'none',
        collapsibleRows: true,
        viewLayoutType: 'outline',
        store: {
            type: 'productLineRates'
        },
        aggregate: [{
            dataIndex: 'DirectRate',
            header: '直通率值'.t(),
            aggregator: 'avg',
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
                var directRate = null;
                var chartControl = Ext.getCmp('prodLineRateChartId').ownerGrid;
                var data = chartControl.getMatrix().results.getByLeftKey(record.data.leftAxisKey)[0].records[0].data;
                var lineName = data.LineName;
                if (value)
                    directRate = value * 100;
                var ownerLayout = chartControl.reportLayout; //报表布局类
                for (i = 0; i < ownerLayout.lineChartSettingInfos.length; i++) {
                    var lineChartSettingInfo = ownerLayout.lineChartSettingInfos[i];
                    if (lineChartSettingInfo.LineName == lineName) {
                        if (directRate) {
                            if (lineChartSettingInfo.Desired <= directRate) {
                                meta.style = "background-color: green;";
                            }
                            if (lineChartSettingInfo.Alarm >= directRate) {
                                meta.style = "background-color: red;";
                            }
                        }
                    }
                }

                return Ext.util.Format.number(directRate, '0%');
            },
        }],
        leftAxis: [
            {
                dataIndex: 'LineName',
                header: '资源'.t(),
                sortable: true,
            }, {
                dataIndex: 'Shift',
                header: '班次'.t(),
                sortable: true,
                width: 100
            }
        ],
        topAxis: [{
            dataIndex: 'day',
            header: '日'.t(),
            labelRenderer: 'dayLabelRenderer'
        }]
    },
    listeners: {
        beforeshowconfigfieldmenu: 'getCustomMenus',
        onExpandAll: 'expandAll',
    },
    header: {
        itemPosition: 1,
        items: [{
            ui: 'default-toolbar',
            xtype: 'button',
            text: 'Export to ...',
            menu: {
                defaults: {
                    handler: 'exportTo'
                },
                items: [{
                    text: 'Excel xlsx (pivot table definition)',
                    handler: 'exportToPivotXlsx'
                }, {
                    text: 'Excel xlsx (all items)',
                    cfg: {
                        type: 'excel07',
                        ext: 'xlsx'
                    }
                }, {
                    text: 'Excel xlsx (visible items)',
                    cfg: {
                        type: 'excel07',
                        onlyExpandedNodes: true,
                        ext: 'xlsx'
                    }
                }, {
                    text: 'Excel xml (all items)',
                    cfg: {
                        type: 'excel03',
                        ext: 'xml'
                    }
                }, {
                    text: 'Excel xml (visible items)',
                    cfg: {
                        type: 'excel03',
                        onlyExpandedNodes: true,
                        ext: 'xml'
                    }
                }, {
                    text: 'CSV (all items)',
                    cfg: {
                        type: 'csv'
                    }
                }, {
                    text: 'CSV (visible items)',
                    cfg: {
                        type: 'csv',
                        onlyExpandedNodes: true
                    }
                }, {
                    text: 'TSV (all items)',
                    cfg: {
                        type: 'tsv',
                        ext: 'csv'
                    }
                }, {
                    text: 'TSV (visible items)',
                    cfg: {
                        type: 'tsv',
                        onlyExpandedNodes: true,
                        ext: 'csv'
                    }
                }, {
                    text: 'HTML (all items)',
                    cfg: {
                        type: 'html'
                    }
                }, {
                    text: 'HTML (visible items)',
                    cfg: {
                        type: 'html',
                        onlyExpandedNodes: true
                    }
                }]
            }
        }]
    }
});