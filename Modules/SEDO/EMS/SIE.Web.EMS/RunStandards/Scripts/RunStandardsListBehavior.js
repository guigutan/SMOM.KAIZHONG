Ext.define('SIE.Web.EMS.RunStandards.RunStandardsListBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var cmdExamineName = "SIE.Web.EMS.RunStandards.Commands.ApprovalCommand";
        var cmdCancelName = "SIE.Web.EMS.RunStandards.Commands.CancelCommand";
        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.EMS.RunStandards.RunStandardsDataQueryer',
            token: view.token,
            success: function (res) {
                var use_approval = res.Result;
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

    },
});