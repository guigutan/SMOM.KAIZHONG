/**
 * 直通率报表控件Model定义
 * @class SIE.Web.MES.DashBoard.Common.ProductRate
 * @constructs
 */
Ext.define('SIE.Web.MES.TaskManagement.Reports.Common.TaskColumn', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'WorkOrderNo', type: 'string' },
        { name: 'ProcessName', type: 'string' },
        { name: 'ProductCode', type: 'string' },
        { name: 'WorkshopName', type: 'string' },
        { name: 'ResourceName', type: 'string' },
        { name: 'HeadTitle', type: 'string' },
        { name: 'Qty', type: 'float', allowNull: true }
    ]
});

/**
 * 直通率报表控件Store定义
 * @class SIE.Web.MES.DashBoard.Common.ProductRate
 * @constructs
 */
Ext.define('SIE.Web.MES.TaskManagement.Reports.Common.TaskStore', {
    extend: 'Ext.data.Store',
    alias: 'store.TaskStore',
    model: 'SIE.Web.MES.TaskManagement.Reports.Common.TaskColumn',
    autoLoad: false
});

/**
 * 直通率报表控件定义
 * @class SIE.Web.MES.DashBoard.Common.ProductRate
 * @constructs
 */
Ext.define('SIE.Web.MES.TaskManagement.Reports.Common.TaskGrid', {
    extend: 'Ext.pivot.Grid',
    xtype: 'taskGrid',
    controller: 'reportBaseController',
    requires: [
        'SIE.Web.MES.DashBoard.Common.ReportBaseController',
        'SIE.Web.MES.TaskManagement.Reports.Common.TaskStore',
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
                    dataIndex: 'WorkOrderNo',
                    header: '工单编号'
                }, {
                    dataIndex: 'ProcessName',
                    header: '工序'
                }, {
                    dataIndex: 'ProductCode',
                    header: '产品编码'
                }, {
                    dataIndex: 'WorkshopName',
                    header: '车间'
                }, {
                    dataIndex: 'ResourceName',
                    header: '资源'
                }, {
                    dataIndex: 'Qty',
                    header: 'Qty',
                    settings: {
                        allowed: 'aggregate',
                        aggregators: ['sum', 'avg', 'count'],
                        fixed: ['topAxis'],
                        style: {
                            fontWeight: 'bold'
                        }
                    }
                },{
                    dataIndex: 'HeadTitle',
                    header: '统计列',

                    settings: {
                        fixed: ['topAxis']
                    }
                }
            ]
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
            type: 'TaskStore'
        },
        aggregate: [{
            dataIndex: 'Qty',
            header: '数量',
            aggregator: 'sum',
            width: 100
        }],
        leftAxis: [
            {
                dataIndex: 'WorkOrderNo',
                header: '工单编号'
            }, {
                dataIndex: 'ProcessName',
                header: '工序'
            }, {
                dataIndex: 'ProductCode',
                header: '产品编码'
            }, {
                dataIndex: 'WorkshopName',
                header: '车间'
            }, {
                dataIndex: 'ResourceName',
                header: '资源'
            }
            
        ],
        topAxis: [{
            dataIndex: 'HeadTitle',
            header: '统计列'}]
        
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