SIE.defineCommand('SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
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
    },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', SIE.Web.MES.Routings.Scripts.RoutingBomCommonFun.onRoutingBomEditPropertyChanged, this);
        }
    }
});