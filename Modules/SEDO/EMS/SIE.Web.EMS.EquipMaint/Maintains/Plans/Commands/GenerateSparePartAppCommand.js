SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.GenerateSparePartAppCommand', {
    meta: { text: "申请", group: "edit", iconCls: "icon-Check" },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        //view.getData().data.length > 0 && 
        return entity && (entity.getExeState() == 0 || entity.getExeState() == 4);
    },
    execute: function (view) {
        var me = this;
        var datas = [];
        Ext.each(view.getData().data.items, function (data) { datas.push(data.data); });

        var commandInfo = {
            command: "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.GenerateSparePartAppCommand",
            entityType: "SIE.EMS.Maintains.Plans.MaintainPlanSparePartApl",
            parentType: "SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanViewModel",
            moduleName: "设备保养计划维护",
            childModuleName: "",
            commandName: "申请",
        }

        view.execute({
            data: datas,
            logInfo: commandInfo,
            success: function (res) {
                SIE.Msg.showInstantMessage('备件申请成功'.t());
                view.loadChildData(true);
            }
        });
    }
});
