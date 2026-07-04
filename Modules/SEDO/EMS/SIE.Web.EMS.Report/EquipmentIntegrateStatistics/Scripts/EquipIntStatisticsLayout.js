Ext.define('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.Scripts.EquipIntStatisticsLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'EquipIntStatisticsLayout',
    _isRunning: false,
    _token: null,
    _criteria: null,

    /**
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;
        regions.main._view._relations[0]._target.mainLayout = me;
        
        var pivotGrid = me.createPivotGrid();
        this._pivotGrid = pivotGrid;
        var chart = me.createLineChart();

        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [
                {
                    region: 'center',
                    layout: 'vbox',
                    xtype: 'panel',
                    id: "esdReportMainPanel",
                    border: false,
                    items: [
                        pivotGrid, {
                            width: "100%",
                            height: "5%",
                            layout: {
                                type: 'hbox',
                                pack: 'start',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'displayfield',
                                    id: 'displayMttrAndMtbfId',
                                    name: 'displayMttrAndMtbfId',
                                    value: '本月MTTR（故障平均修复时间）= 0 小时 MTBF（平均无故障工作时间）= 0 小时'.t(),
                                    fieldLabel: '',
                                    flex: 7
                                },
                                {
                                    xtype: 'displayfield',
                                    id: 'displayEquipmentCountId',
                                    name: 'displayEquipmentCountId',
                                    value: '本次统计设备数:0台'.t(),
                                    fieldLabel: '',
                                    flex: 1
                                }
                            ]
                        }, chart
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
    createLineChart: function (me) {
        return Ext.create('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipStatisticChart', {
            id: 'EquipStatisticChartId',
            region: 'north',
            layout: 'fit',
            width: "100%",
            height: "49%",
            flex: 1,
            border: false,
        });
    },
    /**
    * 创建表格
    * @method createPivotGrid
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createPivotGrid: function (me) {
        return Ext.create('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipmentIntegrateStatisticPivotGrid', {
            id: 'equipmentIntegrateStatisticPivotGridId',
            region: 'north',
            layout: 'fit',
            width: "100%",
            height: "49%",
            flex: 1,
            border: false,            
        });
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
            if (me._isRunning) {
                return;
            }

            me._isRunning = true;
            SIE.invokeDataQuery({
                method: 'GetEquipmentIntegrateStatisticViewModels',
                params: [criteria],
                action: 'queryer',
                async: true,
                type: 'SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipIntegrateStatisticsDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        var equipStaticViewModel = res.Result;
                        me.bindReportInfos(equipStaticViewModel);
                        Ext.getBody().unmask();
                    }
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
    bindReportInfos: function (equipStaticViewModel) {
        //利用率Chart
        var equipmentIntegrateStatisticInfoModelList
            = equipStaticViewModel.EquipStaticChart.EquipmentIntegrateStatisticInfoModelList;

        var lineChart = Ext.getCmp('EquipStatisticChartId');

        if (lineChart) {
            var chartStore = lineChart.items.items[0].getStore();
            if (Ext.isEmpty(equipmentIntegrateStatisticInfoModelList))
                chartStore.removeAll();
            else
                chartStore.setData(equipmentIntegrateStatisticInfoModelList);
            lineChart.items.items[0].setStore(chartStore);
        }

        //统计表
        var equipmentIntegrateStatisticViewModelList
            = equipStaticViewModel.EquipStaticMatrix.EquipmentIntegrateStatisticViewModelList;

        var pivotGrid = Ext.getCmp('equipmentIntegrateStatisticPivotGridId');

        if (pivotGrid) {
            var gridStore = pivotGrid.getStore();
            gridStore.setData(equipmentIntegrateStatisticViewModelList);
            pivotGrid.setStore(gridStore);
        }

        //设备台数
        var displayEquipmentCount = Ext.getCmp('displayEquipmentCountId');
        if (displayEquipmentCount) {
            displayEquipmentCount.setData('本次统计设备数:'.t() + equipStaticViewModel.EquipmentCount + '台'.t());
        }

        //MTBF MTTR
        var displayMttrAndMtbf = Ext.getCmp('displayMttrAndMtbfId');
        if (displayMttrAndMtbf) {
            displayMttrAndMtbf.setData('本月MTTR（故障平均修复时间）= '.t() + equipStaticViewModel.Mttr
                + '小时 MTBF（平均无故障工作时间）= '.t() + equipStaticViewModel.Mtbf + '小时'.t());
        }
    },
});

Ext.define('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipmentIntegrateStatisticViewModel', function () {
    return {
        extend: 'Ext.data.Model',
        fields: [
            { name: 'StatisticDate', type: 'string' },
            { name: 'IndexSeq', type: 'int' },
            { name: 'Value', type: 'float' },
            { name: 'ValueTitle', type: 'string' },
        ]
    };
});
//SIE:classEnd

Ext.define('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipmentIntegrateStatisticStore', {
    extend: 'Ext.data.Store',
    alias: 'store.EquipmentIntegrateStatisticStore',
    model: 'SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipmentIntegrateStatisticViewModel',
    autoLoad: false
});
//SIE:classEnd

Ext.define('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipmentIntegrateStatisticController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.EquipmentIntegrateStatisticController',
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
    dateLabelRenderer: function (value) {
        return Ext.Date.format(value, 'Y/m/d');
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

Ext.define('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipmentIntegrateStatisticPivotGrid', {
    extend: 'Ext.pivot.Grid',
    xtype: 'EquipmentIntegrateStatisticPivotGrid',
    controller: 'EquipmentIntegrateStatisticController',
    requires: [
        'SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipmentIntegrateStatisticController',
        'SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipmentIntegrateStatisticStore',
        /*'Ext.pivot.plugin.Exporter',*/
        'Ext.pivot.plugin.Configurator'
    ],
    width: '100%',
    height: '50%',
    multiSelect: false,
    collapsible: false,
    shadow: 'true',
    selModel: {
        type: 'rowmodel'
    },
    plugins: {
        pivotexporter: false,
    },
    // Set this to true to lock leftAxis dimensions
    enableLocking: true,
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
            type: 'EquipmentIntegrateStatisticStore'
        },
        aggregate: [{
            dataIndex: 'Value',
            header: '',
            width: 80,
            renderer: Ext.util.Format.numberRenderer('0000.0')
        },
        ],
        leftAxis: [
            {
                dataIndex: 'IndexSeq',
                header: '',
                width: 50,
            },
            {
                dataIndex: 'ValueTitle',
                header: '',
                width: 150,
            },
        ],
        topAxis: [{
            dataIndex: 'StatisticDate',
            header: ''
        }]
    },
    listeners: {
        beforeshowconfigfieldmenu: 'getCustomMenus',
        onExpandAll: 'expandAll',
    },    
});
//SIE:classEnd

Ext.define('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipStatisticChart', {
    extend: 'Ext.Panel',
    xtype: 'EquipStatisticChart',
    controller: 'EquipStatisticChartController',
    width: 650,
    height: '50%',
    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        //minHeight: 200,
        height: '50%',
        captions: {
            title: '设备利用率趋势'.t(),
        },
        legend: {
            type: 'sprite',
            docked: 'right'
        },
        interactions: {
            type: 'panzoom',
            zoomOnPanGesture: true
        },
        animation: {
            duration: 200
        },
        store: {
            fields: ['StatisticDate', 'UtilizationRate', 'TargetRate'],
        },
        innerPadding: {
            left: 40,
            right: 40
        },
        axes: [{
            type: 'category',
            position: 'bottom',
            fields: ['StatisticDate'],
            grid: false,
        }, {
            type: 'numeric',
            position: 'left',
            minimum: 0,
            maximum: 120,
            fields: ['UtilizationRate', 'TargetRate'],
            renderer: 'onLineChartAxisLabelRender'
        }],
        series: [{
            type: 'line',
            title: '设备利用率（%）'.t(),
            xField: 'StatisticDate',
            yField: 'UtilizationRate',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'rotate',
                field: 'UtilizationRate',
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
                renderer: 'onLineChartSeriesTooltipRender'
            }
        }, {
            type: 'line',
            title: '利用率标准'.t(),
            xField: 'StatisticDate',
            yField: 'TargetRate',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'rotate',
                field: 'TargetRate',
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
                renderer: 'onLineChartSeriesTooltipRender'
            }
        }]
    }
});

Ext.define('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.EquipStatisticChartController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.EquipStatisticChartController',

    /**
   * 获取折线图x标签
   * @method onLineChartAxisLabel
   * @param {axis} axis 轴对象
   * @param {label} label 标签
   * @param {layoutContext} layoutContext 内容
   * @return {string} x标签
   */
    onLineChartAxisLabelRender: function (axis, label, layoutContext) {
        return layoutContext.renderer(label) + '%';
    },

    /**
   * Chart报表浮动框
   * @param {any} tooltip
   * @param {any} record
   * @param {any} item
   */
    onBarSeriesTooltipRender: function (tooltip, record, item) {
        var fieldIndex = Ext.Array.indexOf(item.series.getYField(), item.field);
        title = item.series.getTitle()[fieldIndex];
        if (record) {
            var date = record.get('StatisticDate');
            var strUtilizationRate = record.get('UtilizationRate').toFixed(1);
            var strTargetRate = record.get('TargetRate').toFixed(1);
            tooltip.setHtml(
                date + "<br>" +
                '设备利用率: '.t() + strUtilizationRate + '%' + "<br>" +
                '利用率标准: '.t() + strTargetRate + '%'
            );
        }
    },

    /**
    * 序列标签
    * @param {any} value
    */
    onSeriesLabelRender: function (value) {
        return (value).toFixed(1) + '%';
    },

    /**
     * 折线图浮点浮动框
     * @method onLineChartSeriesTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 折线图浮点浮动框
     */
    onLineChartSeriesTooltipRender: function (tooltip, record, item) {
        if (record) {
            tooltip.setHtml(item.series._title + ':' + (record.get(item.series._yField)).toFixed(1) + '%');
        }
    },
});