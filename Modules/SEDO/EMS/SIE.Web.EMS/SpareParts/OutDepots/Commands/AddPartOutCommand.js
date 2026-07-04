SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.AddPartOutCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        var parent = view._parent;

        //if (view.getSelection() == null || view.getSelection().length == 0) {
        //    return false;
        //}

        if (parent == null) {

            return false;
        } else if (parent.getCurrent() == null) {
            return false;
        }
        else if (parent.getCurrent().data != null && parent.getCurrent().data.INV_ORG_ID == null) {
            return false;
        }
        if (parent.getCurrent().getOutDepotState() != 0) {
            return false;
        }
        return true;
    },
});