Ext.define('SIE.Web.EMS.Report.WorkOrderExcuteReports.Scripts.WorkOrderExcuteReportLineChart', {
    extend: 'Ext.Panel',
    xtype: 'WorkOrderExcuteReportLineChart',
    controller: 'WorkOrderExcuteReportLineChartController',
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
            fields: ['WorkOrderQty', 'CompleteQty'],
        }, {
            type: 'category',
            position: 'bottom',
                fields: ['Month'],
            grid: false,
        }, {
            type: 'numeric',
            position: 'right',
            minimum: 0,
            maximum: 1.2,
            fields: ['CompleteRate'],
            renderer: 'onLineChartAxisLabelRender'
        }],
        series: [
            {
            type: 'bar',
            axis: 'left',
            title: ['工单总数'.t(), '工单完成数'.t()],
                xField: 'Month',
            stacked: false,
                yField: ['WorkOrderQty', 'CompleteQty'],
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
                field: ['WorkOrderQty', 'CompleteQty'],
                renderer: Ext.util.Format.numberRenderer('0'),
                orientation: 'horizontal', //horizontal  vertical
            },
        },
            {
            type: 'line',
            title: '工单完成率'.t(),
                xField: 'Month',
                yField: 'CompleteRate',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'over',
                field: 'CompleteRate',
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

