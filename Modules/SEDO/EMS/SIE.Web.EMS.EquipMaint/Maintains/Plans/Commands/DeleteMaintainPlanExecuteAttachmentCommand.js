SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.DeleteMaintainPlanExecuteAttachmentCommand', {
    extend: 'SIE.Web.Core.Common.Commands.ImmediateDeleteCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-Delete icon-blue" },
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();
            var item = parentEntity;
            if (item.data.ExeState == 1 || item.data.ExeState == 3) {
                return false;
            }
            return true;
        }
        else {
            return false;
        }
    },
});