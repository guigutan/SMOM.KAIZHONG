Ext.define('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticsBarChart', {
    extend: 'Ext.Panel',
    xtype: 'AndonStatisticsBarChart',
    controller: 'AndonStatisticsBarChartController',
    width: 650,
    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        Height: 500,
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
            fields: ['GroupName', 'AndonNum', "AndonTime", "AndonStopNum", "AndonStopLine", "TriggerAccuracy"],

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
            maximun: 100,
            fields: ['AndonNum']
        },
        {
            type: 'category',
            position: 'bottom',
            grid: false,
            fields: ['GroupName']
        }
        ],
        series: [
            {
                type: 'bar',
                axis: 'rigth',
                title: [''.t()],
                xField: ['GroupName'],
                showInLegend: false,
                stacked: false,
                yField: ['AndonNum', "AndonTime", "AndonStopNum", "AndonStopLine", "TriggerAccuracy"],
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
                    field: ['AndonNum', "AndonTime", "AndonStopNum", "AndonStopLine", "TriggerAccuracy"],
                    renderer: Ext.util.Format.numberRenderer('0'),
                    orientation: 'horizontal', //horizontal  vertical
                },
            }
        ]

    }
});