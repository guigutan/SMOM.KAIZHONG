Ext.define('SIE.Web.Core.Reports.PivotExporter', {
    extend: 'Ext.pivot.plugin.Exporter',
    alias: "plugin.nototalpivotexporter",
    prepareData: function () {
        //默认的导出方法会导出汇总行Grand Toal
        //重写方法。准备数据前，删除汇总行。（导出数据时，不导出汇总Grand Total数据）
        var curMatrix = this.cmp.getMatrix();
        const curTotals = curMatrix.totals;
        if (!Ext.isEmpty(curTotals)) {
            curTotals.splice(0, curTotals.length);
        }
        return this.callParent(arguments); //调用基类方法
    }
});