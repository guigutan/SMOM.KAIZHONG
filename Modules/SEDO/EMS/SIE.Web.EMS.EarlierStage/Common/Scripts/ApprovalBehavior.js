Ext.define('SIE.Web.EMS.EarlierStage.Common.Scripts.ApprovalBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var viewType = 0;  //界面类型（0：预算，1：预算变更，2：项目，3：项目变更，4：项目结项）
        var ApprovalCmdName = "SIE.Web.EMS.EarlierStage.Budgets.Commands.ExamineBudgetCommand";
        var CancelCmdName = "SIE.Web.EMS.EarlierStage.Budgets.Commands.CancelBudgetCommand";
        if (view.model === "SIE.EMS.EarlierStage.Budgets.BudgetChange") {
            viewType = 1;
            ApprovalCmdName = "SIE.Web.EMS.EarlierStage.Budgets.Commands.ExamineBudgetChangeCommand";
            CancelCmdName = "SIE.Web.EMS.EarlierStage.Budgets.Commands.CancelBudgetChangeCommand";
        }
        else if (view.model === "SIE.EMS.EarlierStage.Projects.Project") {
            viewType = 2;
            ApprovalCmdName = "SIE.Web.EMS.EarlierStage.Projects.Commands.ExamineProjectCommand";
            CancelCmdName = "SIE.Web.EMS.EarlierStage.Projects.Commands.CancelProjectCommand";
        }
        else if (view.model === "SIE.EMS.EarlierStage.Projects.ProjectChange") {
            viewType = 3;
            ApprovalCmdName = "SIE.Web.EMS.EarlierStage.Projects.Commands.ExamineProjectChangeCommand";
            CancelCmdName = "SIE.Web.EMS.EarlierStage.Projects.Commands.CancelProjectChangeCommand";
        }
        else if (view.model === "SIE.EMS.EarlierStage.Projects.ProjectClose") {
            viewType = 4;
            ApprovalCmdName = "SIE.Web.EMS.EarlierStage.Projects.Commands.ExamineProjectCloseCommand";
            CancelCmdName = "SIE.Web.EMS.EarlierStage.Projects.Commands.CancelProjectCloseCommand";
        }
        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [viewType],
            action: 'queryer',
            type: 'SIE.Web.EMS.EarlierStage.Common.DataQuery.ApprovalDataQueryer',
            token: view.token,
            success: function (res) {
                if (!res.Result) {
                    if (ApprovalCmdName != null) {
                        var approvalCmd = view.getCmdControl(ApprovalCmdName);
                        if (approvalCmd) {
                            approvalCmd.setHidden(true);
                            view._commands.removeAtKey(ApprovalCmdName);
                        }
                    }
                    if (CancelCmdName != null) {
                        var cancelCmd = view.getCmdControl(CancelCmdName);
                        if (cancelCmd) {
                            cancelCmd.setHidden(true);
                            view._commands.removeAtKey(CancelCmdName);
                        }
                    }
                }
            }
        });
    }
});