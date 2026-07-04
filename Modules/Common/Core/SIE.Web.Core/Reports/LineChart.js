/**
 * 折线图定义
 * @class SIE.Web.Core.Reports.LineChart
 * @constructs
 */
Ext.define('SIE.Web.Core.Reports.LineChart', {
    extend: 'Ext.Panel',
    xtype: 'LineChartBase',
    controller: 'PivotControllerRateBase',
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
            expandRangeBy: 2,   //纵坐标最大值扩展显示
            fields: ['totalQty', 'passQty'],
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
            fields: ['passRate'],
            renderer: 'onLineChartAxisLabelRender'
        }],
        series: [{
            type: 'bar',
            axis: 'left',
            title: ['总批次数'.t(), '合格批次数'.t()],
            xField: 'monthDay',
            stacked: false,
            yField: ['totalQty', 'passQty'],
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
                field: ['totalQty', 'passQty'],
                renderer: Ext.util.Format.numberRenderer('0'),
                orientation: 'horizontal', //horizontal  vertical
            },
        }, {
            type: 'line',
                title: '批次合格率'.t(),
            xField: 'monthDay',
            yField: 'passRate',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'over',
                field: 'passRate',
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