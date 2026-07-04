SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.OpenMaintainsCommand', {
    meta: { text: "跳转", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.Maintains.Plans.MaintainPlan',
            viewGroup: 'PlanExecuteViewGroup',
            title: '跳转'.L10N(),
            isDetail: true,
        });
    }
});