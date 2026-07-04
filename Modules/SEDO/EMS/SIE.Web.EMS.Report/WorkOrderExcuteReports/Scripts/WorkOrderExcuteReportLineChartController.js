Ext.define('SIE.Web.EMS.Report.WorkOrderExcuteReports.Scripts.WorkOrderExcuteReportLineChartController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.WorkOrderExcuteReportLineChartController',

    /**
   * 获取折线图x标签
   * @method onLineChartAxisLabel
   * @param {axis} axis 轴对象
   * @param {label} label 标签
   * @param {layoutContext} layoutContext 内容
   * @return {string} x标签
   */
    onLineChartAxisLabelRender: function (axis, label, layoutContext) {
         return layoutContext.renderer(label * 100) + '%';
    },

    /**
   * Chart报表浮动框
   * @param {any} tooltip
   * @param {any} record
   * @param {any} item
   */
    onBarSeriesTooltipRender: function (tooltip, record, item) {
        var fieldIndex = Ext.Array.indexOf(item.series.getYField(), item.field);
        title = item.series.getTitle()[fieldIndex];
        tooltip.setHtml(
            record.get('Month') + "<br>" +
            '工单总数: ' + record.get('WorkOrderQty') + "<br>" +
            '工单完成数: ' + record.get('CompleteQty') + "<br>" +
            '工单完成率: ' + record.get('CompleteRate')
        );
    },

    /**
    * 序列标签
    * @param {any} value
    */
    onSeriesLabelRender: function (value) {
        return (value * 100).toFixed(2) + '%';
    },

    /**
     * 折线图浮点浮动框
     * @method onLineChartSeriesTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 折线图浮点浮动框
     */
    onLineChartSeriesTooltipRender: function (tooltip, record, item) {
        if (record) {
            tooltip.setHtml(record.get('Month') + item.series._title + ':' + (record.get(item.series._yField)) * 100 + '%');
        }
    },
});