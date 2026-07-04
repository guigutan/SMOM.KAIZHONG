/**
 * 折线图Store定义
 * @class SIE.Web.MES.DashBoard.Common.LineChartInfo
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.LineChartInfo', {
    extend: 'Ext.data.Store',
    alias: 'store.lineChartInfo',
    fields: ['XDate', 'YData'],
});
//SIE:classEnd
/**
 * 折线图定义
 * @class SIE.Web.MES.DashBoard.Common.LineChart
 * @constructs
 */
Ext.define('SIE.Web.MES.DashBoard.Common.LineChart', {
    extend: 'Ext.Panel',
    xtype: 'lineChart',
    controller: 'reportBaseController',
    //requires: [
    //    'SIE.Web.MES.DashBoard.Common.ReportBaseController',
    //],
    listeners: {
        onDesiredAlarmChange: 'onDesiredAlarmChange',
        onLineChartTitleChange:'onLineChartTitleChange'
    },
    YDesired: null,
    YAlarm: null,
    width: 650,
    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        height: 500,
        //scrollable: true,
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
        captions: {
            title: '直通率趋势图'.t(),
        },
        axes: [{
            type: 'numeric',
            position: 'left',
            grid: false,
            minimum: 0,
            maximum: 120,
            limits: [{
                value: 0,
                line: {
                    strokeStyle: 'red',
                    lineWidth: 2,
                    lineDash: [6, 3],
                    title: {
                        text: '',
                        fontWeight: 'bold',
                        fillStyle: 'red',
                        fontSize: 14
                    }
                }
            }, {
                value: 0,
                line: {
                    strokeStyle: 'green',
                    lineWidth: 2,
                    lineDash: [6, 3],
                    title: {
                        text: '',
                        fontWeight: 'bold',
                        fillStyle: 'green',
                        fontSize: 14
                    }
                }
            }],
            renderer: 'onLineChartAxisLabel'
        }, {
            type: 'category',
            position: 'bottom',
            grid: false,
            label: {
                rotate: {
                    degrees: -45
                }
            }
        }],
        series: [{
            type: 'line',
            xField: 'XDate',
            yField: 'YData',
            style: {
                lineWidth: 2
            },
            marker: {
                radius: 4,
                lineWidth: 2
            },
            label: {
                field: 'YData',
                display: 'over'
            },
            highlight: {
                fillStyle: '#000',
                radius: 5,
                lineWidth: 2,
                strokeStyle: '#fff'
            },
            tooltip: {
                trackMouse: true,
                showDelay: 0,
                dismissDelay: 0,
                hideDelay: 0,
                renderer: 'onLineChartSeriesTooltip'
            }
        }],
        listeners: {
            itemhighlight: 'onLineChartItemHighlight',           
        }
    }
});