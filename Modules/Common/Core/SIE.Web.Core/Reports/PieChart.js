/**
 * 饼图定义
 * @class SIE.Web.Core.Reports.PieChart
 * @constructs
 */
Ext.define('SIE.Web.Core.Reports.PieChart', {
    extend: 'Ext.Panel',
    xtype: 'pie-basic',
    width: '100%',
    layout: 'fit',
    border: false,
    controller: 'PieControllerRateBase',

    items: [{
        xtype: 'polar',
        reference: 'chart',
        captions: {
            credits: {
                align: 'left'
            }
        },
        theme: 'default-gradients',
        width: '100%',
        height: 420,
        legend: {
            docked: 'right'
        },
        interactions: ['rotate'],
        series: [{
            type: 'pie',
            angleField: 'data1',
            label: {
                field: 'os',
                calloutLine: {
                    length: 60,
                    width: 3
                    // specifying 'color' is also possible here
                }
            },
            highlight: true,
            tooltip: {
                trackMouse: true,
                renderer: 'onSeriesTooltipRender'
            }
        }]
    }]
});