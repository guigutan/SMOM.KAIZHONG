SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.AddProjectKeyItemCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        entity.setFactoryId(entity._Project.getFactoryId());
        entity.setDepartmentId(entity._Project.getDepartmentId());
        entity.setPlanType(entity._Project.getPlanType());
        entity.setBudgetAmount(0);
        entity.setWorkStatus(10);
    }
});