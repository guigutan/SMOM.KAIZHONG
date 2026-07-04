/**
 * 工序一次良品/不良品统计报表控件Store定义
 * @class SIE.Web.MES.DashBoard.Common.ProcessStatisticsInfo
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProcessStatisticsInfo', {
    extend: 'Ext.data.Store',
    alias: 'store.ProcessStatisticsInfo',
    fields: ['ProcessName', 'PassQty', 'FailedQty'],
});

/**
 * 工序一次良品/不良品统计报表控件定义
 * @class SIE.Web.MES.DashBoard.Common.ProcessStatisticsChart
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.ProcessStatisticsChart', {
    extend: 'Ext.Panel',
    xtype: 'processStatisticsChart',
    controller: 'reportBaseController',
    requires: [
        //'SIE.Web.MES.DashBoard.Common.ReportBaseController',
        'Ext.chart.theme.Muted',
        //'SIE.Web.MES.DashBoard.Common.ChartTheme'
    ],
    width: 650,
    items: [{
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        height: 600,
        legend: {
            type: 'sprite',
            docked: 'bottom',
            marker: {
                type: 'square'
            },
            border: {
                radius: 0
            }
        },
        store: {
            type: 'ProcessStatisticsInfo'
        },
        theme: 'chartTheme',
        captions: {
            title: '工序一次良品/不良品统计',
        },
        axes: [{
            type: 'numeric',
            //minimum: 0,
            //majorTickSteps: 1,
            //adjustByMajorUnit: true,
            position: 'left',
            grid: false,
            fields: ['PassQty', 'FailedQty'],
            renderer: 'onProcessStatisticsAxisLabel'
        }, {
            type: 'category',
            position: 'bottom',
            fields: 'ProcessName',
            label: {
                //rotate: {
                //    degrees: -45
                //}
            }
        }],
        series: [{
            type: 'bar',
            stacked: true,
            fullStack: true,
            title: ['一次良品', '一次不良品'],
            xField: 'ProcessName',
            yField: ['PassQty', 'FailedQty'],
            style: {
                minGapWidth: 20
            },
            //highlight: {
            //    strokeStyle: 'black',
            //    //fillStyle: '#0050b3'
            //},
            label: {
                field: ['PassQty', 'FailedQty'],
                display: 'insideEnd',
                renderer: 'onProcessStatisticsSeriesLabel'
            },
            tooltip: {
                trackMouse: false,
                renderer: 'onProcessStatisticsBarTip'
            }
        }],
    }],
});