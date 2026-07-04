Ext.define('SIE.Web.EMS.Report.MttrAndMtbfReports.Scripts.MttrAndMtbfReportLineChart', {
    extend: 'Ext.Panel',
    xtype: 'MttrAndMtbfReportLineChart',
    controller: 'MttrAndMtbfReportLineChartController',
    width: 650,
    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        minHeight: 200,
        captions: {
            title: ' '.t(),
        },
        legend: {
            type: 'sprite',
            docked: 'top'
        },
        interactions: {
            type: 'panzoom',
            zoomOnPanGesture: true
        },
        animation: {
            duration: 200
        },
        store: {
            fields: ['Month', 'Mtbf', 'Mttr'],
        },
        innerPadding: {
            left: 40,
            right: 40
        },
        axes: [{
            type: 'numeric',
            position: 'left',
            grid: false,
            minimum: 0,
            expandRangeBy: 2,
            fields: ['Mtbf']
        }, {
            type: 'category',
            position: 'bottom',
            fields: ['Month'],
            renderer: function (v1, v2) { return (v2.split('-')[1] + '月').t(); },
            grid: false,
        }, {
            type: 'numeric',
            position: 'right',
            minimum: 0,
            fields: ['Mttr'],
            renderer: 'onLineChartAxisLabelRender'
        }],
        series: [{
            type: 'line',
            title: 'MTBF(平均无故障工作时间)'.t(),
            xField: 'Month',
            yField: 'Mtbf',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'rotate',
                field: 'Mtbf',
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
                title: 'MTTR(故障平均修复时间)'.t(),
                xField: 'Month',
                yField: 'Mttr',
                style: {
                    lineWidth: 2,
                },
                label: {
                    display: 'rotate',
                    field: 'Mttr',
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