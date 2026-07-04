Ext.define('SIE.Web.ERPInterface.Logs.DownloadExcReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'DownloadExcReportLayout',
    _isRunning: false,
    _token: null,

    _layoutChildren: function (regions) {
        var me = this;
        regions.main._view._relations[0]._target.mainLayout = me;
        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [{
                region: 'center',
                layout: 'border',
                xtype: 'panel',
                border: false,
                items: [{
                    region: 'north',
                    id: 'downloadExcPivotGrid',
                    xtype: 'DownloadExcPivotGrid',
                    height: '50%',
                    border: false,
                }, {
                    region: 'center',
                    id: 'downloadExcChart',
                    xtype: 'DownloadExcChart',
                    layout: 'fit',
                    border: false,
                }]
            }]
        });
    },
    setChartStore: function (res) {
        var fields = [];
        var xfields = ['JobType'];
        var yfields = ['UntreatedCount', 'FailCount', 'SuccessCount', 'DataCount'];
        var xfieldNames = ['未处理数'.t(), '失败数'.t(), '成功数'.t(), '总数据量'.t()];
        var datas = [];

        if (res.Result.length > 0) {
            fields.push(xfields);
            fields.push(yfields);

            Ext.each(res.Result, function (item) {
                var data = {};
                data['JobType'] = item.JobType;
                data['UntreatedCount'] = item.UntreatedCount;
                data['FailCount'] = item.FailCount;
                data['SuccessCount'] = item.SuccessCount;
                data['DataCount'] = item.DataCount;
                datas.push(data);
            });
        }
        else {
            fields.push('null');
            fields.push('null');
        }

        var myDataStore = Ext.create('Ext.data.Store', {
            fields: fields,
            data: datas
        });

        var chart = Ext.getCmp('downloadExcChart');
        chart.items.items[0].series.removeAll();

        var series = {
            type: 'bar',
            xField: xfields,
            yField: yfields,
            title: xfieldNames,
            stacked: false,//柱状图不叠加
            label: {
                field: yfields,
                display: 'over'
            },
        };
        chart.items.items[0].addSeries(series);
        chart.items.items[0].setStore(myDataStore);
    },
    loadReportData: function (criteria, token) {
        var me = this;
        _token = token;
        try {
            if (me._isRunning)
                return;
            me._isRunning = true;
            SIE.invokeDataQuery({
                method: 'GetDownloadExcs',
                params: [criteria],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.ERPInterface.Logs.DataQueryer.DownloadExcDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        var pivotGrid = Ext.getCmp('downloadExcPivotGrid');
                        var gridStore = pivotGrid.getStore();
                        gridStore.setData(res.Result);
                        pivotGrid.setStore(gridStore);
                        me.setChartStore(res);
                    }
                }
            });

        } catch (e) {
            throw e;
        } finally {
            me._isRunning = false;
        }
    },
});
//SIE:classEnd
Ext.define('SIE.Web.ERPInterface.Logs.DownloadExcModel', function () {
    return {
        extend: 'Ext.data.Model',
        fields: [
            { name: 'JobType', type: 'string' },
            { name: 'DataCount', type: 'int' },
            { name: 'SuccessCount', type: 'int' },
            { name: 'FailCount', type: 'int' },
            { name: 'UntreatedCount', type: 'int' },
        ]
    };
});
//SIE:classEnd
Ext.define('SIE.Web.ERPInterface.Logs.DownloadExcStore', {
    extend: 'Ext.data.Store',
    alias: 'store.DownloadExcStore',
    model: 'SIE.Web.ERPInterface.Logs.DownloadExcModel',
    autoLoad: false
});
//SIE:classEnd
Ext.define('SIE.Web.ERPInterface.Logs.DownloadExcController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.DownloadExcController',
    requires: [
        'Ext.exporter.text.CSV',
        'Ext.exporter.text.TSV',
        'Ext.exporter.text.Html',
        'Ext.exporter.excel.Xml',
        'Ext.exporter.excel.Xlsx',
        'Ext.exporter.excel.PivotXlsx'
    ],

    events: ['beforedocumentsave', 'documentsave', 'dataready'],
    changeDock: function (button, checked) {
        if (checked) {
            this.getView().getPlugin('configurator').setDock(button.text.toLowerCase());
        }
    },
    getCustomMenus: function (panel, options) {
        options.menu.add({
            text: 'Custom menu item',
            handler: function () {
                Ext.Msg.alert('Custom menu item', Ext.String.format('Do something for "{0}"', options.field.getHeader()));
            }
        });
    },
    expandAll: function () {
        this.getView().expandAll();
    },

    collapseAll: function () {
        this.getView().collapseAll();
    },

    exportToPivotXlsx: function () {
        this.doExport({
            type: 'pivotxlsx',
            matrix: this.getView().getMatrix(),
            title: 'Pivot grid export demo',
            fileName: 'ExportPivot.xlsx'
        });
    },

    exportTo: function (btn) {
        var cfg = Ext.merge({
            title: 'Pivot grid export demo',
            fileName: 'PivotGridExport' + (btn.cfg.onlyExpandedNodes ? 'Visible' : '') + '.' + (btn.cfg.ext || btn.cfg.type)
        }, btn.cfg);

        this.doExport(cfg)
    },

    doExport: function (config) {
        this.getView().saveDocumentAs(config).then(null, this.onError);
    },

    onError: function (error) {
        Ext.Msg.alert('Error', typeof error === 'string' ? error : 'Unknown error');
    },
    onBeforeDocumentSave: function (view) {
        view.mask('Document is prepared for export. Please wait ...');
    },

    onDocumentSave: function (view) {
        view.unmask();
    }
});
//SIE:classEnd
Ext.define('SIE.Web.ERPInterface.Logs.DownloadExcPivotGrid', {
    extend: 'Ext.pivot.Grid',
    xtype: 'DownloadExcPivotGrid',
    controller: 'DownloadExcController',
    requires: [
        'SIE.Web.ERPInterface.Logs.DownloadExcController',
        'SIE.Web.ERPInterface.Logs.DownloadExcStore',
        'Ext.pivot.plugin.Exporter',
        'Ext.pivot.plugin.Configurator'
    ],
    width: 750,
    height: 350,
    multiSelect: false,
    collapsible: true,
    shadow: 'true',
    selModel: {
        type: 'rowmodel'
    },
    plugins: {
        pivotexporter: true,
    },
    startRowGroupsCollapsed: false,
    matrix: {
        type: 'local',
        rowGrandTotalsPosition: 'none',
        colGrandTotalsPosition: 'none',
        rowSubTotalsPosition: 'none',
        colSubTotalsPosition: 'none',
        collapsibleRows: false,
        viewLayoutType: 'tabular',
        store: {
            type: 'DownloadExcStore'
        },
        //aggregate: [{
        //    dataIndex: 'DataCount',
        //    header: '总数据量'.t(),
        //    width: 80,
        //},
        //{
        //    dataIndex: 'SuccessCount',
        //    header: '成功数'.t(),
        //    width: 90,
        //},
        //{
        //    dataIndex: 'FailCount',
        //    header: '失败数'.t(),
        //    width: 80,
        //},
        //{
        //    dataIndex: 'UntreatedCount',
        //    header: '未处理数'.t(),
        //    width: 80,
        //}],
        leftAxis: [
            {
                dataIndex: 'JobType',
                header: '任务类型'.t(),
                fixed: 'leftAxis',
                width: 100,
            }, {
                dataIndex: 'DataCount',
                header: '总数据量'.t(),
                width: 80,
            },
            {
                dataIndex: 'SuccessCount',
                header: '成功数'.t(),
                width: 90,
            },
            {
                dataIndex: 'FailCount',
                header: '失败数'.t(),
                width: 80,
            },
            {
                dataIndex: 'UntreatedCount',
                header: '未处理数'.t(),
                width: 80,
            }
        ],
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
//SIE:classEnd
Ext.define('SIE.Web.ERPInterface.Logs.DownloadExcChart', {
    extend: 'Ext.Panel',
    xtype: 'DownloadExcChart',
    controller: 'DownloadExcChartController',
    width: 650,
    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        height: 500,
        interactions: [{
            type: 'crosszoom',
            zoomOnPanGesture: false
        }],
        animation: {
            duration: 200
        },
        innerPadding: {
            left: 40,
            right: 40
        },
        legend: {
            docked: 'right',
        },
        axes: [{
            type: 'numeric',
            position: 'left',
            grid: true,
            minimum: 0,
        }, {
            type: 'category',
            position: 'bottom',
            grid: false,
            label: {
                rotate: {
                    degrees: -45
                }
            }
        }],
        listeners: {
            itemhighlight: 'onItemHighlight'
        }
    },
});
//SIE:classEnd
Ext.define('SIE.Web.ERPInterface.Logs.DownloadExcChartController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.DownloadExcChartController',

    onItemHighlight: function (chart, newHighlightItem, oldHighlightItem) {
        this.setSeriesLineWidth(newHighlightItem, 4);
        this.setSeriesLineWidth(oldHighlightItem, 2);
    },
    onSeriesTooltipRender: function (tooltip, record, item) {

    },
    setSeriesLineWidth: function (item, lineWidth) {
        if (item) {
            item.series.setStyle({
                lineWidth: lineWidth
            });
        }
    },
    onPreview: function () {
        if (Ext.isIE8) {
            Ext.Msg.alert('Unsupported Operation', 'This operation requires a newer version of Internet Explorer.');
            return;
        }
        var chart = this.lookup('chart');
        chart.preview();
    }
});