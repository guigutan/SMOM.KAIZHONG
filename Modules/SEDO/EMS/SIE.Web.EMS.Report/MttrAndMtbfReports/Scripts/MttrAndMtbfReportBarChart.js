Ext.define('SIE.Web.EMS.Report.MttrAndMtbfReports.Scripts.MttrAndMtbfReportBarChart', {
    extend: 'Ext.Panel',
    xtype: 'MttrAndMtbfReportBarChart',
    controller: 'MttrAndMtbfReportBarChartController',
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
            fields: ['Month', 'Count', 'RepairTimeTotal'],
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
            fields: ['Count', 'RepairTimeTotal'],
        }, {
            type: 'category',
            position: 'bottom',
                fields: ['Month'],
                renderer: function (v1, v2) {  return (v2.split('-')[1]+'月').t(); },
            grid: false,
        }],
        series: [{
            type: 'bar',
            axis: 'left',
            title: ['故障次数'.t(), '维修时长(h)'.t()],
            xField: 'Month',
            stacked: false,
            yField: ['Count', 'RepairTimeTotal'],
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
                field: ['Count', 'RepairTimeTotal'],
                renderer: Ext.util.Format.numberRenderer('0'),
                orientation: 'horizontal',
            },
            style: {
                maxBarWidth: 80
            },
        }]
    }
});