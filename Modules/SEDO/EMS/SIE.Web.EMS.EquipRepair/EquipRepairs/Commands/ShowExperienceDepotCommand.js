SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ShowExperienceDepotCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.EMS.EquipRepair.ExperienceDepots.ExperienceDepot' }
    },
    meta: { text: "查看经验库", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        if (view.getParent()) {
            if (view.getParent().getCurrent()) {
                return !view.getParent().getCurrent().data.reportBtnDisable;
            }
            else
                return false;
        }
        else
            return false;
    },
    execute: function (view, source) {
        var me = this;
        var model = view.model;

        var entity = view.getParent().getCurrent();

        var EquipAccountCode = entity.data.EquipAccountCode;
        var EquipAccountName = entity.data.EquipAccountName;

        var EquipModelCode = entity.data.EquipModelCode;

        var SparePartCode = entity.data.SparePartCode;
        var SparePartName = entity.data.SparePartName;

        if (model) {
            SIE.AutoUI.getMeta({
                model: me.dataParams.targetClassName,
                viewGroup: 'ListView',
                ignoreChild: true,
                ignoreCommands: true,
                isReadonly: true,
                ignoreQuery: false,
                isAggt: true,
                callback: function (block) {
                    var mainBlock;

                    if (block.mainBlock)
                        mainBlock = block.mainBlock;
                    else
                        mainBlock = block;

                    var gridConfig = mainBlock.gridConfig;

                    me._queryBlockProcess(block);
                    me._gridBlockProcess(block);

                    gridConfig.selModel = {
                        selType: 'checkboxmodel',
                        mode: 'SINGLE',
                    };

                    var ui = SIE.AutoUI.generateAggtControl(block);
                    
                    //设置查询条件
                    var queryEntity = ui._view._relations[0]._target._current;
                    queryEntity.setEquipAccountCode(EquipAccountCode);
                    queryEntity.setEquipAccountName(EquipAccountName);

                    queryEntity.setEquipModelCode(EquipModelCode);

                    queryEntity.setSparePartCode(SparePartCode);
                    queryEntity.setSparePartName(SparePartName);

                    me._popupWin(ui, source);
                    ui._view._relations[0]._target.tryExecuteQuery();
                }
            });
        }
    },
    save: function (win) {

        var me = this;
        var selections = me._targetView.getSelection();
        var selDate = selections[0].data;
        var entity = me._ownerView.getData();
        entity.setFaultReason(selDate.FaultReson);
        entity.setFaultCategoryId(selDate.EquipLargeFaultId);
        entity.setFaultCategoryId_Display(selDate.EquipLargeFaultId_Display);
        entity.setFaultPart(selDate.FaultPart);
        entity.setFaultDescriptionId(selDate.FaultDescribeId);
        entity.setFaultDescriptionId_Display(selDate.FaultDescribeId_Display);
        entity.setFaultDescriptionRemark(selDate.FaultDescribeRemark);
        entity.setRepairMethod(selDate.RepairWay);
        entity.setPreventionAdvice(selDate.PreventionAdvice);
        entity.setDeviceAbnormalCode(selDate.FaultCode);
        win.close();
    }
});
