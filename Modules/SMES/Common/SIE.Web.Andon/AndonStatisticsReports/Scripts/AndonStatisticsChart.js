Ext.define('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticsChart', {
    extend: 'Ext.Panel',
    xtype: 'AndonStatisticsChart',
    controller: 'AndonStatisticstChartController',
    width: 350,
    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        minHeight: 200,
        captions: {
            title: '  '.t(),
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
            fields: ['GroupName', 'AndonNum', 'LineValue'],
        },
        innerPadding: {
            left: 40,
            right: 40
        },
        axes: [{
            type: 'numeric',
            position: 'left',
            minimum: 0,
            fields: ['AndonNum', 'LineValue'],
            renderer: 'onLineChartAxisLabelRender'
        }, {
            type: 'category',
            position: 'bottom',
            fields: ['GroupName'],
            grid: false,
        },],
        series: [{
            type: 'bar',
            title: ' '.t(),
            xField: 'GroupName',
            yField: 'AndonNum',
            showInLegend: false,
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'rotate',
                field: 'AndonNum',
                renderer: 'onSeriesLabelRender'
            },
            marker: {
                animation: {
                    duration: 200,
                    easing: 'backOut'
                }
            },
            tooltip: {
                trackMouse: true,
                renderer: 'onLineChartSeriesTooltipRender'
            }
        }, {
            type: 'line',
            title: ' '.t(),
            xField: 'GroupName',
            yField: 'LineValue',
            showInLegend: false,
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'rotate',
                field: 'LineValue',
                renderer: 'onBarSeriesLabelRender'
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
                renderer: 'onLineBarChartSeriesTooltipRender'
            }
        }
        ]
    }
});