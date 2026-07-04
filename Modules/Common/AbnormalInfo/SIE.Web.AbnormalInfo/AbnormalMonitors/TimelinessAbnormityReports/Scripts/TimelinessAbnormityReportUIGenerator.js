Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.Scripts.TimelinessAbnormityReportUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    /**
    * @override 布局重写
    * @param {any} aggtMeta
    * @param {any} regions
    */
    _layout: function (aggtMeta, regions) {
        if (regions.main.getView().model == "SIE.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.TimelinessAbnormityReportsViewModel") {
            var layout = null;
            layout = Ext.create("SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.Scripts.TimelinessAbnormityReportLayout");
            return layout.layout(regions);
        } 
        else
            return this.callParent(arguments);
    }
});