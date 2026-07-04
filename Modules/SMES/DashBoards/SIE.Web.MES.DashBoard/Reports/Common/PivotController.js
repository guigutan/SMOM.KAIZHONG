Ext.define('SIE.Web.MES.DashBoard.Reports.Common.PivotControllerShop', {
    extend: 'SIE.Web.MES.DashBoard.Common.ReportBaseController',
    alias: 'controller.PivotControllerShop',
    exportToPivotXlsx: function () {
        var date = new Date();
        this.doExport({
            type: 'pivotxlsx',
            matrix: this.getView().getMatrix(),
            title: '车间直通率'.t(),
            fileName: '车间直通率'.t() + date.toLocaleDateString() + date.toTimeString().substring(0, 8) + '.xlsx'
        });
    },
    exportTo: function (btn) {
        var date = new Date();
        var cfg = Ext.merge({
            title: '车间直通率'.t(),
            fileName: '车间直通率'.t() + date.toLocaleDateString() + date.toTimeString().substring(0, 8) + '.' + (btn.cfg.ext || btn.cfg.type)
        }, btn.cfg);

        this.doExport(cfg)
    },
    monthLabelRenderer: function (value) {
        if (value.toString().length == 6)
            return value.toString().substr(4, 2) + '月'.t();
        else
            return value.toString().substr(4, 1) + '月'.t();
    },
    dayLabelRenderer: function (value) {
        if (value.toString().length == 8)
            return value.toString().substr(6, 2) + '号'.t();
        else
            return value.toString().substr(6, 1) + '号'.t();
    },
    weekLabelRenderer: function (value) {
        return Ext.String.format('第{0}周'.t(), value);
    },
});

Ext.define('SIE.Web.MES.DashBoard.Reports.Common.PivotControllerWoReach', {
    extend: 'SIE.Web.MES.DashBoard.Reports.Common.PivotControllerShop',
    alias: 'controller.PivotControllerReach',
    exportToPivotXlsx: function () {
        var date = new Date();
        this.doExport({
            type: 'pivotxlsx',
            matrix: this.getView().getMatrix(),
            title: '工单准时达成率'.t(),
            fileName: '工单准时达成率'.t() + date.toLocaleDateString() + date.toTimeString().substring(0, 8) + '.xlsx'
        });
    },
    exportTo: function (btn) {
        var date = new Date();
        var cfg = Ext.merge({
            title: '工单准时达成率'.t(),
            fileName: '工单准时达成率'.t() + date.toLocaleDateString() + date.toTimeString().substring(0, 8) + '.' + (btn.cfg.ext || btn.cfg.type)
        }, btn.cfg);

        this.doExport(cfg)
    },
});

/*
 **产品直通率导出JS
 * @class SIE.Web.MES.DashBoard.Reports.Common.PivotControllerProduct
 */
Ext.define('SIE.Web.MES.DashBoard.Reports.Common.PivotControllerProduct', {
    extend: 'SIE.Web.MES.DashBoard.Reports.Common.PivotControllerShop',
    alias: 'controller.PivotControllerProduct',
    exportToPivotXlsx: function () {
        var date = new Date();
        this.doExport({
            type: 'pivotxlsx',
            matrix: this.getView().getMatrix(),
            title: '产品直通率'.t(),
            fileName: '产品直通率'.t() + date.toLocaleDateString() + date.toTimeString().substring(0, 8) + '.xlsx'
        });
    },
    exportTo: function (btn) {
        var date = new Date();
        var cfg = Ext.merge({
            title: '产品直通率'.t(),
            fileName: '产品直通率'.t() + date.toLocaleDateString() + date.toTimeString().substring(0, 8) + '.' + (btn.cfg.ext || btn.cfg.type)
        }, btn.cfg);

        this.doExport(cfg)
    },
});