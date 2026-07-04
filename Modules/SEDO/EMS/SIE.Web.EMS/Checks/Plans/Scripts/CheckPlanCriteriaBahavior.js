Ext.define('SIE.Web.EMS.Checks.Plans.Scripts.CheckPlanCriteriaBahavior', {
    onViewReady: function (view) {
        var me = this;
        view.getCurrent().setMonth(new Date());
    }
})