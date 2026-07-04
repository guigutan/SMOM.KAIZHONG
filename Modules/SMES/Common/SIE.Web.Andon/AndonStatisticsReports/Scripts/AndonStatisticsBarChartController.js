Ext.define('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticsBarChartController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.AndonStatisticsBarChartController',

    /**
   * 获取折线图x标签
   * @method onLineChartAxisLabel
   * @param {axis} axis 轴对象
   * @param {label} label 标签
   * @param {layoutContext} layoutContext 内容
   * @return {string} x标签
   */
    onLineChartAxisLabelRender: function (axis, label, layoutContext) {
        return layoutContext.renderer(label);
    },

    /**
   * Chart报表浮动框
   * @param {any} tooltip
   * @param {any} record
   * @param {any} item
   */
    onBarSeriesTooltipRender: function (tooltip, record, item) {
        var fieldIndex = Ext.Array.indexOf(item.series.getYField(), item.field);
        if (record) {
            var date = record.get('GroupName');
            tooltip.setHtml(
                date + "<br>" +
                '安灯次数: '.t() + record.get('AndonNum') + "<br>" +
                '安灯时长: '.t() + record.get('AndonTime') + "<br>" +
                '停线次数: '.t() + record.get('AndonStopNum') + "<br>" +
                '停线时长: '.t() + record.get('AndonStopLine') + "<br>" +
                '安灯名称变更率: '.t() + record.get('TriggerAccuracy') + "%<br>" 
            );
        }
    },

    /**
    * 序列标签
    * @param {any} value
    */
    onSeriesLabelRender: function (value) {
        return value;
    },
});