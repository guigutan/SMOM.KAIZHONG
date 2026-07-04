Ext.define('SIE.Web.EMS.MeteringEquipment.Common.Behaviors.ApprovalBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var viewType = 1;  //界面类型（1：计量设备定检）
        var cmdName = "";
        if (view.model === "SIE.EMS.MeteringEquipment.Calibrations.Calibration") {
            viewType = 1;
            cmdName = "SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.AuditCommand";
        }

        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [viewType],
            action: 'queryer',
            type: 'SIE.Web.EMS.MeteringEquipment.Common.DataQuery.ApprovalDataQuery',
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