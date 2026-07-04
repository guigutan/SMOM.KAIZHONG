Ext.define('SIE.Web.EMS.EquipRepairs.PlanRepairs.PlanRepairsBehavior', {
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
                SIE.invokeDataQuery({
                    method: 'GetPlanRepairs',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.EquipRepair.PlanRepairs.PlanRepairsDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setNo(info.No);
                            entity.setCreateDate(info.CreateDate);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setBillSourceType(info.BillSourceType);
                        }
                    }
                });
            }
        }
        var cmdExamineName = "SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.ApprovalCommand";
        var cmdCancelName = "SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.CancelCommand";
        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.EMS.EquipRepair.PlanRepairs.PlanRepairsDataQueryer',
            token: view.token,
            success: function (res) {
                var use_approval = res.Result;
                _golble_use_approval = use_approval;
                if (!use_approval) {
                    var cmd = view.getCmdControl(cmdCancelName);
                    if (cmd) {
                        cmd.setHidden(true);
                        view._commands.removeAtKey(cmdCancelName);
                    }
                    var cmdExamine = view.getCmdControl(cmdExamineName);
                    if (cmdExamine) {
                        cmdExamine.setHidden(true);
                        view._commands.removeAtKey(cmdExamineName);
                    }
                    var tabPanels = view._children.find(m => m.model == "SIE.Equipments.WorkFlows.WorkFlowRecord");
                    if (tabPanels) {
                        var tabPanelItems = tabPanels.getControl().up().up().items.items;
                        Ext.each(tabPanelItems, function (item) {
                            if (("审核记录").indexOf(item.title) >= 0) {
                                item.tab.hide();
                            }
                        });
                    }
                }
            }
        });

    }
});