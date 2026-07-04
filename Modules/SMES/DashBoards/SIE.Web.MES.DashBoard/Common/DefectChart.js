Ext.define('SIE.Web.MES.DashBoard.Common.DefectCategory', {
    extend: 'Ext.data.Store',
    alias: 'store.defectCategory',
    fields: ['DefectName', 'Qty', 'CumQty', 'CumPercent'],
});
//SIE:classEnd
Ext.define('SIE.Web.MES.DashBoard.Common.DefectChart', {
    extend: 'Ext.Panel',
    requires: 'Ext.chart.theme.Category2',
    xtype: 'defectChart',
    controller: 'reportBaseController',
    //requires: [
    //    'SIE.Web.MES.DashBoard.Common.ReportBaseController',
    //    'SIE.Web.MES.DashBoard.Common.DefectTheme'
    //],
    width: 650,
    profiles: {
        classic: {
            columnWidth: 100
        },
        neptune: {
            columnWidth: 100
        },
        graphite: {
            columnWidth: 150
        }
    },
    items: [{
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        height: 400,
        store: {
            type: 'defectCategory'
        },
        theme: 'defectTheme',
        legend: {
            docked: 'bottom'
        },
        captions: {
            title: '缺陷代码TOP5'
        },
        axes: [{
            type: 'numeric',
            position: 'left',
            fields: ['Qty'],
            //majorTickSteps: 10,
            reconcileRange: true,
            grid: false,
            minimum: 0
        }, {
            type: 'category',
            position: 'bottom',
            fields: 'DefectName',
            label: {
                rotate: {
                    //degrees: -45
                }
            }
        }, {
            type: 'numeric',
            position: 'right',
            fields: ['CumQty'],
            reconcileRange: true,
            //majorTickSteps: 10,
            renderer: 'onDefectChartAxisLabel'
        }],
        series: [{
            type: 'bar',
            title: '不良数',
            xField: 'DefectName',
            yField: 'Qty',
            style: {
                opacity: 0.80,
                //width:10
            },
            barWidth: '50%',
            highlight: {
                fillStyle: '#fa8c16',
                strokeStyle: 'black'
            },
            tooltip: {
                trackMouse: true,
                renderer: 'onDefectChartBarSeriesTooltip'
            }
        }, {
            type: 'line',
            title: '占比',
            xField: 'DefectName',
            yField: 'CumQty',
            style: {
                lineWidth: 2,
                opacity: 0.80
            },
            marker: {
                type: 'circle',
                radius: 4,
                animation: {
                    duration: 200
                }
            },
            highlightCfg: {
                scaling: 2,
                rotationRads: Math.PI / 4
            },
            tooltip: {
                trackMouse: true,
                renderer: 'onDefectChartLineSeriesTooltip'
            }
        }]
    }]
});