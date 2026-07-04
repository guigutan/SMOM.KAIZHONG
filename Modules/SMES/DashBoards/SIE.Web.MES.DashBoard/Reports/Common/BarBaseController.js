Ext.define('SIE.Web.MES.DashBoard.Reports.Common.BarBaseController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.barBaseController',
    onAxisLabelRender: function (axis, label, layoutContext) {
        return layoutContext.renderer(label) + '%';
    },
    onSeriesLabelRender: function (v) {
        if (v.toString().indexOf('.') != -1)
            return v.toFixed(0)+'%';
        else return v + '%';
    },
    onSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('ProcessName') + ': ' +
            record.get(item.field).toFixed(0) + '%');
    }
});