Ext.define('SIE.Web.Core.Reports.PivotController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.PivotControllerRateBase',

    /**
    * 导出
    * @method exportTo
    * @param {object} config config
    */
    doExport: function (config) {
        this.getView().saveDocumentAs(config).then(null, this.onError);
    },

    //===================================== 年月日标签 ==========================================
    /**
     * 获取年标签事件
     * @method yearLabelRenderer
     * @param {object} value y轴值
     * @return {string} 年标签
     */
    yearLabelRenderer: function (value) {
        return value + '年'.t();
    },

    /**
    * 获取月标签事件
    * @method monthLabelRenderer
    * @param {object} value y轴值
    * @return {string} 月标签
    */
    monthLabelRenderer: function (value) {
        if (value.toString().length == 6)
            return value.toString().substr(4, 2) + '月'.t();
        else
            return value.toString().substr(4, 1) + '月'.t();
    },

    /**
     * 获取日标签事件
     * @method dayLabelRenderer
     * @param {object} value y轴值
     * @return {string} 日标签
     */
    dayLabelRenderer: function (value) {
        if (value.toString().length == 8)
            return value.toString().substr(6, 2) + '号'.t();
        else
            return value.toString().substr(6, 1) + '号'.t();
    },


    /**
     * 获取周标签事件
     * @method weekLabelRenderer
     * @param {object} value y轴值
     * @return {string} 周标签
     */
    weekLabelRenderer: function (value) {
        return '第'.t() + value + '周'.t();
    },
    //===================================== 年月日标签 ======================================= end
    /**
     * 修改配置前处理
     * @param {any} matrix
     * @param {any} config
     * @param {any} eOpts
     */
    onPivotBeforereConfigure: function (matrix, config, eOpts) {
        config.topAxis.sort(function (a, b) {
            return a.sortIndex - b.sortIndex;
        });//按时间顺序 
    },

    //自定义统计方法
    aggregateCustom: function (records, measure, matrix, rowGroupKey, colGroupKey) {
        if (Ext.isEmpty(records))
            return 0;
        if (records[0].get('ReportInfo') == "合格率".t()) {
            //合格率。根据日期查找并计算总的检验总批次和总的合格批次，然后得出合格率
            var dates = records.select(function (p) { return p.data.Date; });
            var currentDimensionDatas = matrix.store.queryRecordsBy(function (p) { return dates.contains(p.data.Date); });
            var totalSum = currentDimensionDatas.filter(function (p) { return p.data.ReportInfo == "检验总批次".t() }).sum(function (p) { return p.get(measure); });
            var totalPass = currentDimensionDatas.filter(function (p) { return p.data.ReportInfo == "合格批次".t() }).sum(function (p) { return p.get(measure); });
            var val = SIE.Web.Core.CommonFuns.round(totalPass / totalSum * 100, 2);//取两位小数
            return val + '%'; //合格率添加%

        }
        else {
            var val = records.sum(function (p) { return p.get(measure) });
            return val;
        }
    },

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
     * 折线图浮点浮动框
     * @method onLineChartSeriesTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 折线图浮点浮动框
     */
    onLineChartSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('monthDay') + item.series._title + ':' + (record.get(item.series._yField)) * 100 + '%');
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
            record.get('monthDay') + "<br>" +
            '总批次数: ' + record.get('totalQty') + "<br>" +
            '合格批次数: ' + record.get('passQty') + "<br>" +
            '批次合格率: ' + record.get('passRate')
        );
    },

    /**
    * 序列标签
    * @param {any} value
    */
    onSeriesLabelRender: function (value) {
        return (value * 100).toFixed(2) + '%';
    },
});
