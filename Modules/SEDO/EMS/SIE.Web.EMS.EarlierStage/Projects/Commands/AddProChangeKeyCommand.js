SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.AddProChangeKeyCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        entity.setFactoryId(entity._ProjectChange.getFactoryId());
        entity.setDepartmentId(entity._ProjectChange.getDepartmentId());
        entity.setPlanType(entity._ProjectChange.getPlanType());
    }
});