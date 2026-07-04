Ext.define('SIE.Web.MES.DashBoard.Reports.Common.LineBaseController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.LineBaseController',
    onAxisLabelRender: function (axis, label, layoutContext) {
        return layoutContext.renderer(label) + '%';
    },
    onSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('XDate') + ': ' + record.get('YData') + '%');
    },
    onDesiredTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml('目标值'.t() + ': ' + record.get('YDesired') + '%');
    },
    onAlarmTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml('预警值'.t() + ': ' + record.get('YAlarm') + '%');
    },
    onItemHighlight: function (chart, newHighlightItem, oldHighlightItem) {
        this.setSeriesLineWidth(newHighlightItem, 4);
        this.setSeriesLineWidth(oldHighlightItem, 2);
    },  
    lineLabelRender: function (value) {
        return value + '%';
    }
});