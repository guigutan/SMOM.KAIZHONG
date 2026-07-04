Ext.define('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticsPieChart', {
    extend: 'Ext.Panel',
    xtype: 'AndonStatisticsPieChart',
    controller: 'AndonStatisticstPieChartController',
    width: 650,
    items: [
        {
            xtype: 'polar',
            reference: 'chart',
            captions: {
            },
            theme: 'default-gradients',
            width: '100%',
            height: 500,
            insetPadding: 40,
            innerPadding: 20,
            store: {
                fields: ['GroupName', 'AndonNum'],
            },
            legend: {
                docked: 'right'
            },
            interactions: ['rotate'],
            series: [{
                type: 'pie',
                angleField: 'AndonNum',
                label: {
                    field: 'GroupName',
                    calloutLine: {
                        length: 40,
                        width: 3
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