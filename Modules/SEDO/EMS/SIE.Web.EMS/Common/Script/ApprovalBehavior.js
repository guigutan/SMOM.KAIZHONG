Ext.define('SIE.Web.EMS.Common.Script.ApprovalBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var viewType = 1;  //界面类型（1:润滑记录 2:资源调拨 3:副资源调拨 4:固定资产台账 5:资产领用 6:资产发放  7:备件申请单）
        var ApprovalCmdName = null;
        var CancelCmdName = null;
        if (view.model === "SIE.EMS.Lubrications.Lubrication") {
            viewType = 1;
            ApprovalCmdName = "SIE.Web.EMS.Lubrications.Commands.LubricationApprovalCommand";
        }
        if (view.model === "SIE.EMS.AssetTransfers.AssetTransfer") {
            viewType = 2;
            ApprovalCmdName = "SIE.Web.EMS.AssetTransfers.Commands.ApprovalCommand";
            CancelCmdName = "SIE.Web.EMS.AssetTransfers.Commands.CancelCommand";
        }
        if (view.model === "SIE.EMS.ViceTransfers.ViceTransfer") {
            viewType = 3;
            ApprovalCmdName = "SIE.Web.EMS.ViceTransfers.Commands.ApprovalCommand";
            CancelCmdName = "SIE.Web.EMS.ViceTransfers.Commands.ApprovalCommand";
        }
        if (view.model === "SIE.EMS.FixedAssets.Accounts.FixedAssetsAccount") {
            viewType = 4;
            ApprovalCmdName = "SIE.Web.EMS.FixedAssets.Accounts.Commands.ApproveCommand";
            CancelCmdName = "SIE.Web.EMS.FixedAssets.Accounts.Commands.WithdrawCommand";
        }
        if (view.model === "SIE.EMS.AssetRequisitions.AssetRequisition") {
            viewType = 5;
            ApprovalCmdName = "SIE.Web.EMS.AssetRequisitions.Commands.ApprovalAssetRequisitionCommand";
            CancelCmdName = "SIE.Web.EMS.AssetRequisitions.Commands.CancelAssetRequisitionCommand";
        }
        if (view.model === "SIE.EMS.AssetIssues.AssetIssue") {
            viewType = 6;
            ApprovalCmdName = "SIE.Web.EMS.AssetIssues.Commands.ApprovalAssetIssueCommand";
            CancelCmdName = "SIE.Web.EMS.AssetIssues.Commands.CancelAssetIssueCommand";
        }
        if (view.model === "SIE.EMS.SpareParts.Applys.SparePartApp") {
            viewType = 7;
            ApprovalCmdName = "SIE.Web.EMS.SpareParts.Applys.Commands.AuditAppCommand";
            CancelCmdName = "SIE.Web.EMS.SpareParts.Applys.Commands.CancelAppCommand";
        }

        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [viewType],
            action: 'queryer',
            type: 'SIE.Web.EMS.Common.DataQuery.ApprovalDataQuery',
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