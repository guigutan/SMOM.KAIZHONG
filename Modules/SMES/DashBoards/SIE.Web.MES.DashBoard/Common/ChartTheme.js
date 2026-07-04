var colors = ['#096dd9', '#FF0000'];
var defectColors = ['#fa8c16', '#303133'];
Ext.define('SIE.Web.MES.DashBoard.Common.ChartTheme', {
    extend: 'Ext.chart.theme.Base',
    alias: 'chart.theme.chartTheme',
    constructor: function (config) {
        this.callParent([Ext.apply({
            colors: colors
        }, config)]);
    }
});
Ext.define('SIE.Web.MES.DashBoard.Common.DefectTheme', {
    extend: 'Ext.chart.theme.Base',
    alias: 'chart.theme.defectTheme',
    constructor: function (config) {
        this.callParent([Ext.apply({
            colors: defectColors
        }, config)]);
    }
});