Ext.define('SIE.Web.Andon.AndonStatisticsReports.Scripts.AndonStatisticstPieChartController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.AndonStatisticstPieChartController',

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
    * 序列标签
    * @param {any} value
    */
    onSeriesLabelRender: function (value) {
        return value;
    },

    onDataRender: function (v) {
        return v + '%';
    },

    onSeriesTooltipRender: function (tooltip, record, item) {
        debugger;
        tooltip.setHtml(record.get('GroupName') + ': ' + record.get(item.field) + '%');
        
    }
});