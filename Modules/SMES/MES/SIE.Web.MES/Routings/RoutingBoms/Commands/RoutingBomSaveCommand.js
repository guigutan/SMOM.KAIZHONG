SIE.defineCommand('SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        if (view.getCurrent() == null)
            return false;
        if (view.getCurrent()._RoutingBomDetailList == null)
            return true;
        var data = view.getCurrent()._RoutingBomDetailList.data;
        if (data == null || data.length == 0)
            return true;
        return false;
    }
});