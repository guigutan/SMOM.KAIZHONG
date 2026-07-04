SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.DeleteCheckSparePartAplCommand', {
    extend: 'SIE.Web.Core.Common.Commands.ImmediateDeleteCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length <= 0) {
            return false;
        }

        if (view.getSelection().any(function (p) { return p.data.IsApply == true; }))
            return false;

        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.getExeState() == 0 || entity.getExeState() == 4;
        }

        return true;
    },
})