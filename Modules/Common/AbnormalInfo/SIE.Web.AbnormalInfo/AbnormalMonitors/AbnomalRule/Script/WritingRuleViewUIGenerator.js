Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.WritingRuleViewUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    /**
     * @override 布局重写，添加Gerber显示区
     * @param {any} aggtMeta
     * @param {any} regions
     */
    _layout: function (aggtMeta, regions) {
        var model = regions.main.getView().model;
       var  layout = "";
        if (model ==="SIE.AbnormalInfo.AbnormalMonitors.IndicatorCondition")
        {
            layout = Ext.create("SIE.Web.AbnormalInfo.AnomalyMonitors.IndicatorConditionLayout");
        }
        else if (model == 'SIE.AbnormalInfo.AbnormalMonitors.AbnormalDecisionRule') {
            layout = Ext.create("SIE.Web.AbnormalInfo.AnomalyMonitors.WritingRuleLayout");
        }
        else if (model === "SIE.AbnormalInfo.AbnormalMonitors.AbnomalRuleWhere")
        {
            layout = Ext.create("SIE.Web.AbnormalInfo.AnomalyMonitors.WhereConditionLayout");
        }
        else {
            layout = new SIE.autoUI.layouts.Common();
        }
        return layout.layout(regions);
    }
});