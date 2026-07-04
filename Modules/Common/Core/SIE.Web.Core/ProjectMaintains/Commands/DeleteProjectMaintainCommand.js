SIE.defineCommand('SIE.Web.Core.ProjectMaintains.Commands.DeleteProjectMaintainCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            var curdata = view.getCurrent();
            if (curdata == null || (curdata.data.State != 0 && view.getData().isDirty()))
                return false;
        }
        else {
            var sel = view.getSelection();
            for (var i = 0 ; i < sel.length; i++) {
                var item = sel[i].data;
                if (item.State != 0)
                    return false;
            }
        }

        return true;
    }
});