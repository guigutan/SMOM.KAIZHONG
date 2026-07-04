Ext.define('SIE.EMS.Report.EquipmentMixReport.Scripts.EmsEquipmentChart', {
    extend: 'Ext.Panel',
    xtype: 'EmsEquipmentChart',
    controller: 'EmsEquipmentChartController',
    width: 650,
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
            fields: ['Month', 'UtilizationStandards', "UtilizationRate"],
        },
        innerPadding: {
            left: 40,
            right: 40
        },
        axes: [{
            type: 'numeric',
            position: 'left',
            minimum: 0,
            fields: ['UtilizationRate','UtilizationStandards'],
            renderer: 'onLineChartAxisLabelRender'
        }, {
            type: 'category',
            position: 'bottom',
            fields: ['Month'],
            grid: false,
        },],
        series: [{
            type: 'line',
            title: '设备利用率'.t(),
            xField: 'Month',
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
            xField: 'Month',
            yField: 'UtilizationStandards',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'rotate',
                field: 'UtilizationStandards',
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