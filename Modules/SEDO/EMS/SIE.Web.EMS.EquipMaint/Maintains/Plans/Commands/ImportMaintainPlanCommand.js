SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.ImportMaintainPlanCommand', {
    extend: 'SIE.Web.EMS.Common.Commands.ImportDataCommonCommand',
    meta: { text: "数据导入", group: "business", iconCls: "icon-Download icon-blue", model: "SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanCheckDataViewModel" },

});