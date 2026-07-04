Ext.define('SIE.Web.MES.DashBoard.DashBoardController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.DashBoardController',

    onSeriesTooltipRender: function (tooltip, record, item) {
        var title = item.series.getTitle();

        tooltip.setHtml(title + record.get('monthDay') + ': ' +
            record.get(item.series.getYField()));
    },
    onAxisLabelRender: function (axis, label, layoutContext) {
        var total = axis.getRange()[1];
        return (label / total * 100).toFixed(0) + '%';
    },
    onLineSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('monthDay') + item.series._title + ':' + (record.get(item.series._yField)) * 100 + '%');
    },
    onSeriesLabelRender: function (value) {
        return (value * 100).toFixed(2) + '%';
    },
    lineLabelRender: function (value) {
        return value + '%';
    }
});