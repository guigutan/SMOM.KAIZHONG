SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.DeleteCheckPlanExecuteAttachmentCommand', {
    extend: 'SIE.Web.Core.Common.Commands.ImmediateDeleteCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-Delete icon-blue" },
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();
            var item = parentEntity;
            if (item.data.ExeState == 1) {
                return false;
            }
            return true;
        }
        else {
            return false;
        }
    },
});