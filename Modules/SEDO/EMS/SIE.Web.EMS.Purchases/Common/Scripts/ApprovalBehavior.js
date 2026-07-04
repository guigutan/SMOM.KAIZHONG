Ext.define('SIE.Web.EMS.Purchases.Common.ApprovalBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var viewType = 0;  //界面类型（0：采购申请，1：采购订单，2：付款计划，3：设备开箱验收，4：备件验收,5:工治具验收,6:安装调试）
        var ApprovalCmdName = "SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.ExaminePurRequireCommand";
        var CancelCmdName = "SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.CancelPurRequireCommand";

        if (view.model === "SIE.EMS.Purchases.PurchaseOrders.PurchaseOrder") {
            viewType = 1;
            ApprovalCmdName = "SIE.Web.EMS.Purchases.PurchaseOrders.Commands.ExaminePurOrderCommand";
            CancelCmdName = "SIE.Web.EMS.Purchases.PurchaseOrders.Commands.CancelPurOrderCommand";
        } else if (view.model === "SIE.EMS.Purchases.PaymentPlans.PaymentPlan") {
            viewType = 2;
            ApprovalCmdName = "SIE.Web.EMS.Purchases.PaymentPlans.Commands.ExaminePaymentPlanCommand";
            CancelCmdName = "SIE.Web.EMS.Purchases.PaymentPlans.Commands.CancelPaymentPlanCommand";
        }
        else if (view.model === "SIE.EMS.Purchases.EquipmentAcceptances.EquipmentAcceptance") {
            viewType = 3;
            ApprovalCmdName = "SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.ExamineEquipAcceptCommand";
            CancelCmdName = "SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.CancelEquipAcceptCommand";
        }
        else if (view.model === "SIE.EMS.Purchases.SparePartAcceptances.SparePartAcceptance") {
            viewType = 4;
            ApprovalCmdName = "SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.ExamineSparePartAcceptCommand";
            CancelCmdName = "SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.CancelSparePartAcceptanceCommand";
        }
        else if (view.model === "SIE.EMS.Purchases.FixtureAcceptances.FixtureAcceptance") {
            viewType = 5;
            ApprovalCmdName = "SIE.Web.EMS.Purchases.FixtureAcceptances.Commands.ExamineFixtureAcceptanceCommand";
            CancelCmdName = "SIE.Web.EMS.Purchases.FixtureAcceptances.Commands.CancelFixtureAcceptanceCommand";
        }
        else if (view.model === "SIE.EMS.Purchases.EquipmentSetups.EquipmentSetup") {
            viewType = 6;
            ApprovalCmdName = "SIE.Web.EMS.Purchases.EquipmentSetups.Commands.ExamineEquipmentSetupCommand";
            CancelCmdName = "SIE.Web.EMS.Purchases.EquipmentSetups.Commands.CancelEquipmentSetupCommand";
        }
        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [viewType],
            action: 'queryer',
            type: 'SIE.Web.EMS.Purchases.Common.DataQuery.PurchaseDataQueryer',
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