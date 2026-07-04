Ext.define('SIE.Web.EMS.Report.SparePartMitReports.Scripts.SparePartMixReportExWarehouseChartController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.SparePartMixReportExWarehouseChartController',

    /**
   * 获取折线图x标签
   * @method onLineChartAxisLabel
   * @param {axis} axis 轴对象
   * @param {label} label 标签
   * @param {layoutContext} layoutContext 内容
   * @return {string} x标签
   */
    onLineChartAxisLabelRender: function (axis, label, layoutContext) {
        return label.toFixed(1);
    },

    /**
   * Chart报表浮动框
   * @param {any} tooltip
   * @param {any} record
   * @param {any} item
   */
    onBarSeriesTooltipRender: function (tooltip, record, item) {
        //var fieldIndex = Ext.Array.indexOf(item.series.getYField(), item.field);
        //title = item.series.getTitle()[fieldIndex];
        //if (record) {
        //    var date = record.get('Date');
        //    var strRate = record.get('PassRate').toFixed(4) * 100;
        //    tooltip.setHtml(
        //        date + "<br>" +
        //        '通过人次: '.t() + record.get('PassQty') + "<br>" +
        //        '不通过人次: '.t() + record.get('NgQty') + "<br>" +
        //        '通过率: '.t() + strRate + '%'
        //    );
        //}
    },

    /**
    * 序列标签
    * @param {any} value
    */
    onSeriesLabelRender: function (value) {
        return value;
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
            var date = record.get('Month')+" ";
            tooltip.setHtml(date + item.series._title + ':' + record.get(item.series._yField));
        }
    },
});