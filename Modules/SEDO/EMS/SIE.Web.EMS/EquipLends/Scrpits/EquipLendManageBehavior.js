Ext.define("SIE.Web.EMS.EquipLends.Scrpits.EquipLendManageBehavior", {
    onViewReady: function (view) {
        SIE.invokeDataQuery({
            method: 'IsNeedExamine',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.EMS.EquipLends.DataQueryers.EquipLendDataQueryer',
            token: view.token,
            success: function (res) {
                var isNeedExamine = res.Result;
                if (!isNeedExamine) {
                    // 隐藏审核记录页签
                    view.findChild("SIE.EMS.EquipLends.EquipLendExamineRecord").getControl().ownerLayout.owner.tab.hide();
                    // 隐藏审核按钮
                    var examineCmd = "SIE.Web.EMS.EquipLends.Commands.EquipLendExamineCommand";
                    let cmd = view.getCmdControl(examineCmd);
                    if (cmd) {
                        cmd.setHidden(true);
                        view._commands.removeAtKey(examineCmd);
                    }
                }
            }
        })
    },
})

