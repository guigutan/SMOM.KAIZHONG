SIE.defineCommand('SIE.Web.EMS.InventoryPlans.Commands.SaveInventoryPlanCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaved: function (view, res) {
        SIE.Msg.showInstantMessage('保存成功'.t());
        var current = view.getCurrent();
        if (current != undefined) {
            current.markSaved();
        }
        else {
            view.reloadData();
        }
        view.syncCmdState(view, false);
        this.saveRange(current, view);
    },
    saveRange: function (plan, planView) {
        if (plan.getInventoryAssetObject() == 10) {
            planView.findChild("SIE.EMS.InventoryPlans.InventoryPlanEquipment").loadChildData(true);
        }
        else if (plan.getInventoryAssetObject() == 20) {
            planView.findChild("SIE.EMS.InventoryPlans.InventoryPlanSparePart").loadChildData(true);
        }
        else if (plan.getInventoryAssetObject() == 30) {
            planView.findChild("SIE.EMS.InventoryPlans.InventoryPlanFixture").loadChildData(true);
        }


    }
});