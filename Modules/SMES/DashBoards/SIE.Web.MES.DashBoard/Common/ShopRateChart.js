/**
 * 直通率报表控件Model定义
 * @class SIE.Web.MES.DashBoard.Common.ProductRate
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ShopRate', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'ShopName', type: 'string' },
        { name: 'ResourceName', type: 'string' },
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
Ext.define('SIE.Web.MES.DashBoard.Common.ShopRates', {
    extend: 'Ext.data.Store',
    alias: 'store.shopRates',
    model: 'SIE.Web.MES.DashBoard.Common.ShopRate',
    autoLoad: false
});

/**
 * 直通率报表控件定义
 * @class SIE.Web.MES.DashBoard.Common.ProductRate
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ShopRateChart', {
    extend: 'Ext.pivot.Grid',
    xtype: 'shopRateChart',
    controller: 'reportBaseController',
    requires: [
        //'SIE.Web.MES.DashBoard.Common.ReportBaseController',
        //'SIE.Web.MES.DashBoard.Common.ShopRates',
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
                    dataIndex: 'ShopName',
                    header: '车间'.t(),

                    settings: {
                        aggregators: ['count'],
                        fixed: ['leftAxis']
                    }
                }, {
                    dataIndex: 'ResourceName',
                    header: '资源'.t(),

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
            type: 'shopRates'
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
                var ownerGrid = Ext.getCmp('prodShopRateChartId').ownerGrid;
                var data = ownerGrid.getMatrix().results.getByLeftKey(record.data.leftAxisKey)[0].records[0].data;
                var resourceName = data.ResourceName;
                var reportLayout = ownerGrid.reportLayout;
                for (i = 0; i < reportLayout.shopChartSettingInfos.length; i++) {
                    var productChartSettingInfo = reportLayout.shopChartSettingInfos[i];
                    if (productChartSettingInfo.ResourceName == resourceName) {
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
                dataIndex: 'ShopName',
                header: '车间'.t(),
                sortable: true,
            }, {
                dataIndex: 'ResourceName',
                header: '资源'.t(),
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