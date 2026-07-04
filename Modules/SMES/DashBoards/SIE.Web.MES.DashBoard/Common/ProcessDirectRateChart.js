/**
 * 工序直通率报表控件Store定义
 * @class SIE.Web.MES.DashBoard.Common.ProcessDirectRate
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProcessDirectRate', {
    extend: 'Ext.data.Store',
    alias: 'store.processDirectRate',
    fields: ['ProcessName', 'PasssRate'],
});

/**
 * 工序直通率报表控件定义
 * @class SIE.Web.MES.DashBoard.Common.ProcessDirectRateChart
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProcessDirectRateChart', {
    extend: 'Ext.Panel',
    xtype: 'processDirectRateChart',
    controller: 'reportBaseController',
    //requires: [
    //    'SIE.Web.MES.DashBoard.Common.ReportBaseController',
    //    'SIE.Web.MES.DashBoard.Common.ChartTheme'
    //],
    width: 650,
    listeners: {
        onProcessDirectRateTitle: 'onProcessDirectRateTitle'
    },
    profiles: {
        classic: {
            width: 650
        },
        neptune: {
            width: 650
        },
        graphite: {
            width: 800
        }
    },
    items: [{
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        height: 400,
        insetPadding: '10 30 10 10',
        flipXY: true,
        captions: {
            title: '工序直通率统计图'
        },
        interactions: {
            type: 'itemedit',
            style: {
                lineWidth: 2
            },
            tooltip: {
                renderer: 'onProcessDirectRateItemTooltip'
            }
        },
        animation: {
            easing: 'easeOut',
            //duration: 200
        },
        store: {
            type: 'processDirectRate'
        },
        theme: 'chartTheme',
        axes: [{
            type: 'numeric',
            position: 'bottom',
            fields: 'PasssRate',
            grid: false,
            maximum: 100,
            minimum: 0,
            majorTickSteps: 10,
            renderer: 'onProcessDirectRateAxisLabel'
        }, {
            type: 'category',
            position: 'left',
            fields: 'ProcessName',
            grid: false
        }],
        series: [{
            type: 'bar',
            xField: 'ProcessName',
            yField: 'PasssRate',
            style: {
                opacity: 0.80,
                minGapWidth: 30
            },
            highlightCfg: {
                //fillStyle: '#0050b3',
                strokeStyle: 'black',
                radius: 0
            },
            label: {
                field: 'PasssRate',
                display: 'insideEnd',
                renderer: 'onProcessDirectRateSeriesLabel'
            },
            tooltip: {
                trackMouse: true,
                renderer: 'onProcessDirectRateSeriesTooltip'
            }
        }],
    }],
});