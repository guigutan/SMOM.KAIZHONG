Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.Scripts.TimelinessAbnormityReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    height: (window.innerHeight - 50) / 2,
    buttonType: 0,
    timevalue: 30,
    dateType: "(天)数据".t(),
    _layoutChildren: function (regions) {
        var me = this;
        let main = regions.main;
        me.view = main.getView();
        //加载思维导图数据结构
        me.view.pieChart = me.createPieChart();
        me.view.paretoChart = me.createParetoChart();
        me.view.lineChart = me.createLineChart();
        me.view.ajaxTabs = me.createAjaxTabs();
        //布局块


        me.mainView = Ext.create('Ext.Panel', {
            title: '容器组件'.t(),
            layout: 'border',
            header: false, // 将 header 设置为 false，移除标题栏
            bodyBorder: false,
            style: {
                border: 'none' // 设置边框样式为无边框
            },
            defaults: {
                collapsible: false,
                split: true,
                layout: "fit"
            },
            tbar: {
                style: {
                    borderBottom: 'none', // 隐藏底部边框线
                },
                layout: {
                    type: 'hbox',
                    align: 'middle',
                },
                items: [
                    {
                        xtype: 'segmentedbutton',
                        items: [
                            {
                                height: 35,
                                width: 110,
                                text: '日报'.t(),
                                pressed: true,
                                handler: function () {
                                    me.buttonType = 0;
                                    Ext.getCmp("TimelinessAbnormityReportLayoutTBarNumberId").setValue("30");//最近时间段
                                    Ext.getCmp("TimelinessAbnormityReportLayoutDateTypeId").setText("(天)数据".t());//最近时间段
                                }
                            },
                            {
                                height: 35,
                                width: 110,
                                text: '周报'.t(),
                                handler: function () {
                                    me.buttonType = 1;
                                    Ext.getCmp("TimelinessAbnormityReportLayoutTBarNumberId").setValue("7");//最近时间段
                                    Ext.getCmp("TimelinessAbnormityReportLayoutDateTypeId").setText("(周)数据".t());//最近时间段
                                },
                            },
                            {
                                height: 35,
                                width: 110,
                                text: '月报'.t(),
                                handler: function () {
                                    me.buttonType = 2;
                                    Ext.getCmp("TimelinessAbnormityReportLayoutTBarNumberId").setValue("12");//最近时间段
                                    Ext.getCmp("TimelinessAbnormityReportLayoutDateTypeId").setText("(月)数据".t());//最近时间段
                                },
                            }
                            , {
                                height: 35,
                                width: 110,
                                text: '年报'.t(),
                                handler: function () {
                                    me.buttonType = 3;
                                    Ext.getCmp("TimelinessAbnormityReportLayoutTBarNumberId").setValue("3");//最近时间段
                                    Ext.getCmp("TimelinessAbnormityReportLayoutDateTypeId").setText("(年)数据".t());//最近时间段
                                }
                            }]
                    },

                    '->', // 占位符组件，将后续的按钮推向右侧*/
                    {
                        xtype: 'tbtext',
                        style: {
                            lineHeight: '35px'
                        },
                        text: '最近'.t()
                    },
                    {
                        id: "TimelinessAbnormityReportLayoutTBarNumberId",
                        xtype: 'numberfield',
                        height: 35,
                        width: 100,
                        minValue: 0,
                        maxValue: 1000,
                        hideTrigger: true,
                        value: me.timevalue,
                    },
                    {
                        id: "TimelinessAbnormityReportLayoutDateTypeId",
                        xtype: 'tbtext',
                        style: {
                            marginLeft: "-5px",
                            lineHeight: '35px',
                        },
                        text: me.dateType
                    },
                    {
                        xtype: 'button',
                        height: 35,
                        width: 80,
                        text: '刷新'.t(),
                        handler: function () {
                            me.mainView.setLoading(true)
                            me.getChartStore(me, regions);
                        }
                    }
                ]
            },

            scrollable: true,
            items: [
                {
                    header: false, // 将 header 设置为 false，移除标题栏
                    title: 'Main Content',
                    height: 20,
                    collapsible: false,
                    region: 'center',
                    split: false, // 禁用边界拖动
                    items: {
                        xtype: "container",
                        layout: 'border',
                        bodyBorder: false,
                        defaults: {
                            collapsible: false,
                            split: true,
                            layout: "fit"
                        },
                        items: [
                            me.view.pieChart,//饼图
                            me.view.paretoChart //帕累托图图
                        ]
                    }
                }
                , {
                    header: false, // 将 header 设置为 false，移除标题栏
                    region: 'south',
                    height: "50%",
                    split: false, // 禁用边界拖动
                    minHeight: 200,
                    maxHeight: 600,
                    items: {
                        xtype: "container",
                        layout: 'border',
                        bodyBorder: false,
                        defaults: {
                            collapsible: false,
                            split: true,
                            layout: "fit"
                        },
                        items: [me.view.lineChart, me.view.ajaxTabs]
                    }
                },
            ]

        });
        return me.mainView
    },

    /**
     * 饼图
     * */
    createPieChart: function () {
        let me = this;
        return Ext.create('Ext.Panel', {
            controller: 'TimelinessAbnormityReportChartController',
            region: 'west',
            width: "50%",
            items: [{
                xtype: 'polar',
                reference: 'chart',
                captions: {
                    title: '异常任务总览'.t()
                },
                theme: 'default-gradients',
                width: '100%',
                height: me.height,
                insetPadding: 10,
                innerPadding: 10,
                interactions: ['rotate'],
                legend: {
                    docked: 'right'
                },
                series: [{
                    type: 'pie',
                    angleField: 'data',
                    label: {
                        field: 'taskState',
                        calloutLine: {
                            length: 35,
                            width: 2
                        }
                    },
                    highlight: true,
                    tooltip: {
                        trackMouse: true,
                        renderer: 'onSeriesTooltipRender'
                    }
                }]
            }]
        })
    },

    /**
     * 帕累托图 --异常任务分组分布图
     * */
    createParetoChart: function () {
        var me = this;
        return Ext.create('Ext.Panel', {
            controller: 'TimelinessAbnormityReportChartController',
            width: "40%",
            collapsible: false,
            region: 'center',
            items: [{
                xtype: 'cartesian',
                reference: 'chart',
                downloadServerUrl: '//svg.sencha.io',
                theme: 'category2',
                width: '100%',
                legend: {
                    docked: 'bottom',
                    padding: 10 // 设置图例的内边距
                },
                captions: {
                    title: '异常任务分组分布图'.t()
                },
                height: me.height,
                innerPadding: {
                    top: 27,
                },
                axes: [{
                    type: 'numeric',
                    position: 'left',
                    fields: ['count'],
                    majorTickSteps: 10,
                    reconcileRange: true,
                    grid: true,
                    minimum: 0,
                    renderer: function (axis, label, layoutContext) {
                        // 将标签值转换为整数
                        return Math.round(label);
                    }
                }, {
                    type: 'category',
                    position: 'bottom',
                    fields: 'complaint',
                }, {
                    type: 'numeric',
                    position: 'right',
                    fields: ['cumpercent'],
                    reconcileRange: true,
                    majorTickSteps: 10,
                    renderer: 'onAxisLabelRender',
                    minimum: 0,
                    maximum: 100
                }],
                series: [{
                    type: 'bar',
                    title: '原因'.t(),
                    xField: 'complaint',
                    yField: 'count',
                    style: {
                        opacity: 0.80
                    },
                    highlight: {
                        fillStyle: 'rgba(204, 230, 73, 1.0)',
                        strokeStyle: 'black'
                    },
                    tooltip: {
                        trackMouse: true,
                        renderer: 'onBarSeriesTooltipRender'
                    }
                }, {
                    type: 'line',
                    title: '百分比'.t(),
                    xField: 'complaint',
                    yField: 'cumpercent',
                    style: {
                        lineWidth: 2,
                        opacity: 0.80
                    },
                    marker: {
                        type: 'cross',
                        animation: {
                            duration: 200
                        }
                    },
                    label: {
                        field: 'cumpercentString',
                        display: 'outside'
                    },
                    highlightCfg: {
                        scaling: 2,
                        rotationRads: Math.PI / 4
                    },
                    tooltip: {
                        trackMouse: true,
                        renderer: 'onLineSeriesTooltipRender'
                    },
                }]
            }]
        })
    },

    /**
     * 折线图--异常处理时效图
     * */
    createLineChart: function () {
        let me = this;
        return Ext.create('Ext.Panel', {
            controller: 'TimelinessAbnormityReportChartController',
            width: "50%",
            region: 'west',
            items: {
                xtype: 'cartesian',
                reference: 'chart',
                width: '100%',
                region: 'center',
                height: me.height,
                interactions: {
                    type: 'crosszoom',
                    zoomOnPanGesture: false,
                    axes: {
                        left: {
                            allowZoom: false // 禁止纵坐标缩放
                        },
                        bottom: {
                            allowZoom: true // 允许横坐标缩放
                        }
                    }
                },
                animation: {
                    duration: 200
                },
                captions: {
                    title: '异常处理时效'.t(),
                },
                innerPadding: {
                    bottom: 15,
                },
                axes: [{
                    type: 'numeric',
                    position: 'left',
                    minimum: 0,
                    maximum: 12,
                }, {
                    type: 'category',
                    position: 'bottom',
                    minimum: 0,
                    maximum: 11.5,
                    grid: {
                        odd: {
                            opacity: 1,
                            stroke: '#bbb',
                            lineWidth: 1
                        }
                    },
                    label: {
                        rotate: {
                            degrees: -45
                        }
                    }
                }],
                series: [{
                    type: 'line',
                    xField: 'date',
                    yField: 'count',
                    style: {
                        lineWidth: 2
                    },
                    marker: {
                        radius: 6,
                        lineWidth: 2
                    },
                    label: {
                        field: 'count',
                        display: 'over'
                    },
                    highlight: {
                        fillStyle: '#000',
                        radius: 8,
                        lineWidth: 2,
                        strokeStyle: '#fff'
                    },
                    tooltip: {
                        trackMouse: true,
                        showDelay: 0,
                        dismissDelay: 0,
                        hideDelay: 0,
                        renderer: 'onLineChartSeriesTooltipRender'
                    }
                }],
                listeners: {
                    itemhighlight: 'onItemHighlight'
                }
            },
        })
    },

    /**
     * 图表及柱状图
     * */
    createAjaxTabs: function () {
        var me = this;
        me.view.GridPanel = me.createGrid();
        me.view.ColumnChart = me.createColumnChart();
        return Ext.create('Ext.tab.Panel', {
            region: 'center',
            margin: '5 0 0 0',
            frame: false,
            defaults: {
                bodyPadding: 10,
                scrollable: true
            },
            items: [
                me.view.ColumnChart,
                me.view.GridPanel,
            ]
        });
    },

    /**
     * 表格--异常统计列表
     * */
    createGrid: function () {
        return Ext.create('Ext.grid.Panel', {
            title: '异常统计列表'.t(),
            id: "TimelinessAbnormityReportGridPanel",
            height: 200,
            width: 400,

        });
    },

    //柱状图--异常任务分布
    createColumnChart: function () {
        var me = this;
        return Ext.create('Ext.Panel', {
            minHeight: 100,
            region: 'west',
            title: '异常任务分布图'.t(),
            controller: 'TimelinessAbnormityReportChartController',
            items: [{
                xtype: 'cartesian',
                width: '100%',
                height: me.height - 20,
                theme: 'Muted',
                interactions: {
                    type: 'crosszoom',
                    zoomOnPanGesture: false,
                    axes: {
                        left: {
                            allowZoom: false // 禁止纵坐标缩放
                        },
                        bottom: {
                            allowZoom: true // 允许横坐标缩放
                        }
                    }
                },
                animation: {
                    duration: 200
                },
                store: store,
                legend: {
                    type: 'dom',
                    docked: 'bottom'
                },
                axes: [{
                    type: 'numeric',
                    position: 'left',
                    fields: ['sumCount', 'ToDo', 'Doing', 'Done', 'Cancel'],
                    grid: true,
                    // renderer: 'onAxisLabelRender'
                }, {
                    type: 'category',
                    position: 'bottom',
                    fields: 'date',
                    grid: true
                }],
                series: [{
                    type: 'bar',
                    stacked: false,
                    title: ['异常任务数量'.t(), '未开始'.t(), '进行中'.t(), '完成'.t(), '取消'.t()],
                    xField: 'date',
                    yField: ['sumCount', 'ToDo', 'Doing', 'Done', 'Cancel'],
                    label: {
                        field: ['sumCount', 'ToDo', 'Doing', 'Done', 'Cancel'],
                        display: 'insideEnd',
                        orientation: 'horizontal',
                    },
                }]
            }]
        });

    },


    //请求所有图的数据
    getChartStore: function (me, regions) {
        if (me.view.getController() == null) {
            me.view.setController(new SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.Scripts.TimelinessAbnormityReportController)
        }
        var cls = me.view.getController()
        var timevalue = Ext.getCmp("TimelinessAbnormityReportLayoutTBarNumberId").getValue();//最近时间段
        var buttonType = me.buttonType//按钮(日报，周报，月报，年报)
        try {
            SIE.invokeDataQuery({
                method: 'GetChartStore',
                params: [timevalue, buttonType],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.TimelinessAbnormityReportDataQueryer',
                token: me.view.token,
                success: function (res) {
                    if (res.Success) {
                        cls.setPicChartStore(me.view.pieChart, res.Result.PieChartStore)
                        cls.setParetoChartStore(me.view.paretoChart, res.Result.paretoChartStores)
                        cls.setLineChartStore(me.view.lineChart, res.Result.lineChartChartStores)
                        cls.setGridPanelStore(me.view.ajaxTabs, res.Result.gridPanelStores)
                        cls.setColumnChartStore(me.view.ColumnChart, res.Result.gridPanelStores)
                    }
                }
            });

        } catch (e) {
            me.mainView.setLoading(false)
            throw e;
        } finally {
            me.mainView.setLoading(false)
        }
    }
})