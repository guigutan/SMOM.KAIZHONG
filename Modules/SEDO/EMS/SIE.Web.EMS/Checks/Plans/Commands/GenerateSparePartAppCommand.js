SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.GenerateSparePartAppCommand', {
    meta: { text: "申请", group: "edit", iconCls: "icon-Check" },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        return (entity.getExeState() == 0 || entity.getExeState() == 4);
    },
    execute: function (view) {
        var commandInfo = {
            command: "SIE.Web.EMS.Checks.Plans.Commands.GenerateSparePartAppCommand",
            entityType: "SIE.EMS.Checks.Plans.CheckPlanSparePartApl",
            parentType: "SIE.EMS.Checks.Plans.ViewModels.CheckPlanViewModel",
            moduleName: "点检计划维护",
            childModuleName: "",
            commandName: "申请",
        }
        var datas = [];
        Ext.each(view.getData().data.items, function (data) { datas.push(data.data); });
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
