Ext.define('SIE.Web.AbnormalInfo.Reports.Scripts.AbnormalInfoReportController', {
    extend: 'SIE.Web.Core.Reports.PivotController',
    alias: 'controller.AbnormalInfoReportController',

    /**
    * 导出
    * @method exportTo
    * @param {object} btn btn
    */
    exportTo: function (btn) {
        var date = new Date();
        var cfg = Ext.merge({
            title: '异常信息报表'.t(),
            fileName: '异常信息报表'.t() + date.toLocaleDateString() + date.toTimeString().substring(0, 8) + '.' + (btn.cfg.ext || btn.cfg.type)
        }, btn.cfg);

        this.doExport(cfg);
    },

    //自定义统计方法
    aggregateCustom: function (records, measure, matrix, rowGroupKey, colGroupKey) {
        if (Ext.isEmpty(records))
            return 0;
        if (records[0].get('ReportInfo') == "异常关闭率") {
            //合格率。根据日期查找并计算总的异常发生数和总的异常关闭数，然后得出关闭率
            var dates = records.select(function (p) { return p.data.Date; });
            var currentDimensionDatas = matrix.store.queryRecordsBy(function (p) { return dates.contains(p.data.Date); });
            var totalSum = currentDimensionDatas.filter(function (p) { return p.data.ReportInfo == "异常发生数" }).sum(function (p) { return p.get(measure); });
            var totalClose = currentDimensionDatas.filter(function (p) { return p.data.ReportInfo == "异常关闭数" }).sum(function (p) { return p.get(measure); });
            var val = SIE.Web.Core.CommonFuns.round(totalClose / totalSum * 100, 2);//取两位小数
            return val + '%'; //关闭率添加%
        }
        else {
            var val = records.sum(function (p) { return p.get(measure) });
            return val;
        }
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
            '异常发生数: ' + record.get('totalQty') + "<br>" +
            '异常关闭数: ' + record.get('closeQty') + "<br>" +
            '异常关闭率: ' + record.get('closeRate')
        );
    },
});