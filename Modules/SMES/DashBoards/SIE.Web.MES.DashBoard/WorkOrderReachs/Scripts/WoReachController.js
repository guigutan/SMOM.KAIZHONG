Ext.define('SIE.Web.MES.DashBoard.WoReachController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.WoReachController',

    onBarSeriesTooltipRender: function (tooltip, record, item) {
        var fieldIndex = Ext.Array.indexOf(item.series.getYField(), item.field);
        title = item.series.getTitle()[fieldIndex];
        tooltip.setHtml(
            record.get('monthDay') + "<br>" +
            item.series._title[0] + ': ' + record.get(item.series._yField[0]) + "<br>" +
            item.series._title[1] + ': ' + record.get(item.series._yField[1]) + "<br>" +
            item.series._title[2] + ': ' + record.get(item.series._yField[2])
            );
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
    }
});