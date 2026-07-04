/**
 * 异常信息折线图定义
 * @class SIE.Web.AbnormalInfo.Reports.Scripts.AbnormalLineChart
 * @constructs
 */
Ext.define('SIE.Web.AbnormalInfo.Reports.Scripts.AbnormalLineChart', {
    extend: 'Ext.Panel',
    xtype: 'AbnormalLineChart',
    controller: 'AbnormalInfoReportController',
    width: 650,
    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        height: 500,
        legend: {
            type: 'sprite',
            docked: 'bottom'
        },
        interactions: {
            type: 'panzoom',
            zoomOnPanGesture: true
        },
        animation: {
            duration: 200
        },
        store: {
            type: 'lineChartInfo'
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
            fields: ['totalQty', 'closeQty'],
        }, {
            type: 'category',
            position: 'bottom',
            fields: ['monthDay'],
            grid: false,
        }, {
            type: 'numeric',
            position: 'right',
            minimum: 0,
            maximum: 1.2,
            fields: ['closeRate'],
            renderer: 'onLineChartAxisLabelRender'
        }],
        series: [{
            type: 'bar',
            axis: 'left',
            title: ['异常发生数', '异常关闭数'],
            xField: 'monthDay',
            stacked: false,
            yField: ['totalQty', 'closeQty'],
            highlight: {
                fillStyle: 'yellow',
                strokeStyle: 'black'
            },
            tooltip: {
                trackMouse: true,
                renderer: 'onBarSeriesTooltipRender'
            },
            label: {
                display: 'insideEnd',
                'text-anchor': 'middle',
                field: ['totalQty', 'closeQty'],
                renderer: Ext.util.Format.numberRenderer('0'),
                orientation: 'horizontal', //horizontal  vertical
            },
        }, {
            type: 'line',
            title: '异常关闭率',
            xField: 'monthDay',
            yField: 'closeRate',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'over',
                field: 'closeRate',
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