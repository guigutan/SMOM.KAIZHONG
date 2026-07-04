SIE.defineCommand('SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomDetailEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', SIE.Web.MES.Routings.Scripts.RoutingBomCommonFun.onRoutingBomDetailEditPropertyChanged, this);
        }
    }
});