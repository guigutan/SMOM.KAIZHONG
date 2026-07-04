Ext.define('SIE.Web.EMS.EarlierStage.Budgets.BudgetBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.action = params.action;
            if (params.action === 0) {
                entity.setBudgetNo(params.BudgetNo);
                entity.setYear(params.Year);
                entity.setApprovalStatus(params.ApprovalStatus);
                entity.setCurrency(params.Currency);
                entity.setAmountUnit(params.Currency);
                entity.setBudgeGrade(params.BudgeGrade);
            }
        }
        view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
        view.mon(view, 'beforeClosewin', me.beforeClosewin, view);
    },
    onEntityPropertyChanged: function (e) {
        if (e.property === 'Currency') {
            e.entity.setAmountUnit(e.value);
        }
    },
    /**
     * 主视图关闭事件
     * @param {any} returnObj
     */
    beforeClosewin: function (returnObj) {
        var me = this;
        var ismun = false;
        for (var modifiedProperty in me.getData().modified) {
            if (modifiedProperty != "InvestClass_Display" && modifiedProperty != "BudgetClass_Display")
                ismun = true;
        }
        returnObj.hasData = ismun;
    }
});