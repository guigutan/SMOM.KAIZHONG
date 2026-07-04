Ext.define('SIE.Web.AbnormalInfo.Reports.Scripts.CloseRateGrid', {
    extend: 'Ext.pivot.Grid',
    xtype: 'CloseRateGrid',
    controller: 'AbnormalInfoReportController',
    collapsible: true,
    requires: [
        'SIE.Web.Core.Reports.PivotExporter',
        'Ext.pivot.plugin.Configurator'
    ],
    mainViewLayout: null,   //当前布局
    selModel: {
        type: 'cellmodel'
    },
    matrix: {
        type: 'local',
        rowGrandTotalsPosition: 'none',
        colGrandTotalsPosition: 'none',
        rowSubTotalsPosition: 'none',
        colSubTotalsPosition: 'none',
        collapsibleRows: true,
        viewLayoutType: 'outline',
        store: {
            type: 'PassStore'
        },
        aggregate: [{
            dataIndex: 'CloseData',
            header: '异常关闭数'.t(),
            width: 100,
            cellConfig: {
                viewModel: {
                    type: 'pivot-cell-model'
                },
                bind: {
                    userCls: '{value:sign("pivotCellNegative","pivotCellPositive")}'
                }
            },
            renderer: function (value, meta, record, rowIndex, colIndex, store, view) {
                if (rowIndex == 2 && colIndex > 0) {
                    //关闭率
                    return value;
                }
                else return parseInt(value);
            },
            aggregator: "aggregateCustom"
        }
        ],
        leftAxis: [{
            dataIndex: 'ReportInfo',
            header: '统计信息'.t(),
            sortable: false,
            width: 100,
            settings: {
                fixed: ['leftAxis']
            },
        }],
        topAxis: [{
            dataIndex: 'month',
            header: '月'.t(),
            labelRenderer: 'monthLabelRenderer',
            sortIndex: 2
        }],
    },
    plugins: {
        "nototalpivotexporter": true, //使用自定义导出插件，不导出GrandTotal行
        pivotconfigurator: {
            fields: [
                {
                    dataIndex: 'year',
                    header: '年'.t(),
                    labelRenderer: 'yearLabelRenderer',
                    settings: {
                        aggregators: ['count'],
                        allowed: ['topAxis']
                    },
                    sortIndex: 1
                }, {
                    dataIndex: 'month',
                    header: '月'.t(),
                    labelRenderer: 'monthLabelRenderer',
                    settings: {
                        aggregators: ['count'],
                        allowed: ['topAxis']
                    },
                    sortIndex: 2
                }, {
                    dataIndex: 'day',
                    header: '日'.t(),
                    labelRenderer: 'dayLabelRenderer',
                    settings: {
                        aggregators: ['count'],
                        allowed: ['topAxis']
                    },
                    sortIndex: 4
                }, {
                    dataIndex: 'week',
                    header: '周'.t(),
                    labelRenderer: 'weekLabelRenderer',
                    settings: {
                        aggregators: ['count'],
                        allowed: ['topAxis']
                    },
                    sortIndex: 3
                }, {
                    dataIndex: 'ReportInfo',
                    header: '统计信息'.t(),
                    sortable: false,
                    width: 100,
                    settings: {
                        allowed: ['leftAxis']
                    },
                }],
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
    },
    listeners: {
        pivotbeforereconfigure: "onPivotBeforereConfigure",
    }
});