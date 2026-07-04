SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Projects.Commands.ExportXls', {
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    myview: {}, // 当前视图对象
    fieldNames: [],//导出的数据
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var dataCount = view.getData().data.items.length;
        //数据存在时允许导出
        if (dataCount === 0) {
            SIE.Msg.showInstantMessage('没有需要导出的数据！'.t());
            return false;
        }
        var parentType = "SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanViewModel";

        SIE.Core.Scripts.Signature.otherCheckIsNeedToSignByParentType("导出", parentType, view, function () {
            SIE.ExportExcelHelper.exportXls(view.gridConfig, view.getData().getData().items, view.label.t());
        });
    },
});