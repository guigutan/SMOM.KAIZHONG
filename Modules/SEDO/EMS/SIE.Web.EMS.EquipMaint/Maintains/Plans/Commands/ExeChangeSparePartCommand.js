SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.ExeChangeSparePartCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "更换", group: "edit", iconCls: "icon-Check" },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        return view.getData().data.length > 0 && entity && (entity.getExeState() == 0 || entity.getExeState() == 4);
    },
    execute: function (view) {
        var commandInfo = {
            command: "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.ExeChangeSparePartCommand",
            entityType: "SIE.EMS.Maintains.Plans.MaintainPlanSparePart",
            parentType: "SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanViewModel",
            moduleName: "设备保养计划维护",
            childModuleName: "",
            commandName: "更换",
        }

        var datas = [];
        Ext.each(view.getData().data.items, function (data) { datas.push(data.data); });
        view.execute({
            data: datas,
            logInfo: commandInfo,
            success: function (res) {
                SIE.Msg.showInstantMessage('备件更换成功'.t());
                view.loadChildData(true);
            }
        });
    }
});
