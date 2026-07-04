Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.AbnormalMonitorTasks.WritingAbnormalProcessUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    /**
     * @override 布局重写，添加Gerber显示区
     * @param {any} aggtMeta
     * @param {any} regions
     */
    _layout: function (aggtMeta, regions) {
        var model = regions.main.getView().model;
       var  layout = "";
        if (model ==="SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTask")
        {
            layout = Ext.create("SIE.Web.AbnormalInfo.AnomalyMonitors.AbnormalMonitorTasks.WritingAbnormalProcessLayout");
        }
        else {
            layout = new SIE.autoUI.layouts.Common();
        }
        return layout.layout(regions);
    }
});