SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.ProcessBomAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green"},
    onItemCreated: function (entity) {
        this.callParent();
        entity.mon(entity, 'propertyChanged', SIE.Web.MES.WoCommonFun.ProcessBomPropertyChanged, this.view)
    },
});