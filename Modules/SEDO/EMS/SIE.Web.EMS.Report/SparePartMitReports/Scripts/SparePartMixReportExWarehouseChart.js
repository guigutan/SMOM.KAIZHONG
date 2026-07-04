Ext.define('SIE.Web.EMS.Report.SparePartMitReports.Scripts.SparePartMixReportExWarehouseChart', {
    extend: 'Ext.Panel',
    xtype: 'SparePartMixReportExWarehouseChart',
    controller: 'SparePartMixReportExWarehouseChartController',
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
            fields: ['Month', 'Value'],
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
            fields: ['Value']
        }, {
            type: 'category',
            position: 'bottom',
            fields: ['Month'],
            renderer: function (v1, v2) { return v2; },
            grid: false,
        }],
        series: [{
            type: 'line',
            title: '出库总金额'.t(),
            xField: 'Month',
            yField: 'Value',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'rotate',
                field: 'Value',
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
        }
        ]
    }
});