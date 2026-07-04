Ext.define('SIE.Web.EMS.SpecialEquipment.Common.Behaviors.ApprovalBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var viewType = 1;  //界面类型（1：特种设备定检）
        var cmdName = "";
        if (view.model === "SIE.EMS.SpecialEquipment.RegularInspections.RegularInspection") {
            viewType = 1;
            cmdName = "SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.AuditCommand";
        }

        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [viewType],
            action: 'queryer',
            type: 'SIE.Web.EMS.SpecialEquipment.Common.DataQuery.ApprovalDataQuery',
            token: view.token,
            success: function (res) {
                if (!res.Result) {
                    var cmd = view.getCmdControl(cmdName);
                    if (cmd) {
                        cmd.setHidden(true);
                        view._commands.removeAtKey(cmdName);
                    }
                }
            }
        });
    }
});