/**
 * 直通率报表控件Model定义
 * @class SIE.Web.MES.DashBoard.Common.ProductRate
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProductRate', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'ProductModelName', type: 'string' },
        { name: 'ProductName', type: 'string' },
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
 * @class SIE.Web.MES.DashBoard.Common.ProductRate
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProductRates', {
    extend: 'Ext.data.Store',
    alias: 'store.productRates',
    model: 'SIE.Web.MES.DashBoard.Common.ProductRate',
    autoLoad: false
});

/**
 * 直通率报表控件定义
 * @class SIE.Web.MES.DashBoard.Common.ProductRate
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProductRateChart', {
    extend: 'Ext.pivot.Grid',
    xtype: 'prodRateChart',
    controller: 'reportBaseController',
    requires: [
        //'SIE.Web.MES.DashBoard.Common.ReportBaseController',
        //'SIE.Web.MES.DashBoard.Common.ProductRates',
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
                    dataIndex: 'ProductModelName',
                    header: '机型'.t(),

                    settings: {
                        aggregators: ['count'],
                        fixed: ['leftAxis']
                    }
                }, {
                    dataIndex: 'ProductName',
                    header: '产品'.t(),

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
    startRowGroupsCollapsed: true,
    matrix: {
        type: 'local',
        rowGrandTotalsPosition: 'none',
        colGrandTotalsPosition: 'none',
        collapsibleRows: false,
        viewLayoutType: 'outproduct',
        store: {
            type: 'productRates'
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
                var ownerGrid =  Ext.getCmp('prodProductRateChartId').ownerGrid;
                var data = ownerGrid.getMatrix().results.getByLeftKey(record.data.leftAxisKey)[0].records[0].data;
                var productName = data.ProductName;
                var reportLayout = ownerGrid.reportLayout;
                for (i = 0; i < reportLayout.productChartSettingInfos.length; i++) {
                    var productChartSettingInfo = reportLayout.productChartSettingInfos[i];
                    if (productChartSettingInfo.ProductName == productName) {
                        if (value) {
                            if (productChartSettingInfo.Desired <= value) {
                                meta.style = "background-color: green;";
                            }
                            if (productChartSettingInfo.Alarm >= value) {
                                meta.style = "background-color: red;";
                            }
                        }
                    }
                }
                return Ext.util.Format.number(value, '0%');
            },
        }],
        leftAxis: [
            {
                dataIndex: 'ProductModelName',
                header: '机型'.t(),
                sortable: true,
            }, {
                dataIndex: 'ProductName',
                header: '产品'.t(),
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