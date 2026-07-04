Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectCloseBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        view.childView = view._children.first(function (p) { return p.model === "SIE.EMS.EarlierStage.Projects.ProjectClose"; });
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var child = me.childView.getData();
        if (child) {
            if (e.property === 'ContentAndBasis')
                child.setContentAndBasis(e.entity.data.ContentAndBasis);
            if (e.property === 'GoalAndBenefit')
                child.setGoalAndBenefit(e.entity.data.GoalAndBenefit);
            if (e.property === 'ActualAmount')
                child.setActualAmount(e.entity.data.ActualAmount);
            if (e.property === 'LaborCost')
                child.setLaborCost(e.entity.data.LaborCost);
        }
    }
});