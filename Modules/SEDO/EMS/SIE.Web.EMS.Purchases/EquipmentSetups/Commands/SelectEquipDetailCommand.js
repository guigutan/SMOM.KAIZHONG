SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.SelectEquipDetailCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.EMS.Purchases.EquipmentSetups.ViewModels.SelEquipDetailViewModel' }
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            ignoreCommands: true,
            ignoreChild: true,
            isAggt: true,
            token: view.token,
            model: "SIE.EMS.Purchases.EquipmentSetups.ViewModels.SelEquipDetailViewModel",
            callback: function (res) {
                var blocks = res;
                me._queryBlockProcess(blocks);
                me._gridBlockProcess(blocks);
                var eqView = SIE.AutoUI.generateAggtControl(res);
                var ui = eqView.getControl();
                eqView._view._relations[0]._target.tryExecuteQuery();
                SIE.Window.show({
                    title: "选择设备台账".t(),
                    width: 950,
                    height: 500,
                    items: ui,
                    id: "SelectEquipDetailCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var datas = eqView._view.getSelection();
                            if (datas.length === 0)
                                return true;
                            var childData = view.getData();
                            SIE.each(datas, function (item) {
                                let model = SIE.getModel('SIE.EMS.Purchases.EquipmentSetups.EquipmentDetail');
                                let entity = new model();
                                entity.setEquipAccountId_Display(item.getCode());
                                entity.setEquipAccountId(item.getId());
                                entity.setEquipAccountName(item.getName());
                                entity.setEquipAccountAlias(item.getAlias());
                                entity.setEquipAccountModelCode(item.getEquipAccountModelCode());
                                entity.setEquipAccountModelName(item.getEquipAccountModelName());
                                entity.setSpecifications(item.getSpecifications());
                                entity.setManageDepartmentName(item.getManageDepartmentName());
                                entity.setUseDepartmentName(item.getUseDepartmentName());
                                entity.setManufacturer(item.getManufacturer());
                                entity.setOriginalSerialNumber(item.getOriginalSerialNumber());
                                entity.setWarrantyPeriod(item.getWarrantyPeriod());
                                childData.add(entity);
                            });
                        }
                    }
                });
            }
        });
    }
});