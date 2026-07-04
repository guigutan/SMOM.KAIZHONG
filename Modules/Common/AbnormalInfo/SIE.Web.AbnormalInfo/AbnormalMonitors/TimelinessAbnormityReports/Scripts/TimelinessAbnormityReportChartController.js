Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.Scripts.TimelinessAbnormityReportChartController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.TimelinessAbnormityReportChartController',

    onSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('taskState') + ': ' + record.get('data') + "个".t());
    },
    onLineSeriesTooltipRender: function (tooltip, record, item) {
        var store = record.store,
            i, complaints = [];

        for (i = 0; i <= item.index; i++) {
            complaints.push(store.getAt(i).get('complaint'));
        }
        tooltip.setHtml('<div style="text-align: center; font-weight: bold">' +
            record.get('cumpercent') + '%</div>' + complaints.join('<br>'));
    },
    onBarSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('complaint') + ': ' +
            record.get('count') + "个".t());
    },
    onAxisLabelRender: function (axis, label, layoutContext) {
        var total = axis.getRange()[1];
        return (label / total * 100).toFixed(0) + '%';
    },
    onItemHighlight: function (chart, newHighlightItem, oldHighlightItem) {
        this.setSeriesLineWidth(newHighlightItem, 4);
        this.setSeriesLineWidth(oldHighlightItem, 2);
    },
    setSeriesLineWidth: function (item, lineWidth) {
        if (item) {
            item.series.setStyle({
                lineWidth: lineWidth
            });
        }
    },
    onLineChartSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('date') + '完成: ' + record.get('count') + "个".t());
    },
});