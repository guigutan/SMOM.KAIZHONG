Ext.define('SIE.Web.MES.DashBoard.ShopReportController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.ShopReportController',

    onSeriesTooltipRender: function (tooltip, record, item) {
        var title = item.series.getTitle();

        tooltip.setHtml(title + record.get('monthDay') + '直通率'.t() + ': ' +
            record.get(item.series.getYField())+'%');
    },

    onLineSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('monthDay') + item.series._title + ':' + (record.get(item.series._yField)) * 100 + '%');
    },
    onAxisLabelRender: function (axis, label, layoutContext) {
        var total = axis.getRange()[1];
        return (label / total * 100).toFixed(0) + '%';
    },
    onSeriesLabelRender: function (value) {
        return (value * 100).toFixed(2) + '%';
    },
    lineLabelRender: function (value) {
        return value + '%';
    },
    onStandardSeriesTooltipRender: function (tooltip, record, item) {
        var title = item.series.getTitle();
        tooltip.setHtml(title + ': ' +
            record.get(item.series.getYField()) + '%');
    }
});