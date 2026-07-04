SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.AddPurOrderCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    showView: function (editEntity) {
        var me = this;
        me.addPage({
            entityType: me.view.model,
            recordId: editEntity.getId(),
            title: me.getEditViewTitle(editEntity),
            isDetail: true,
            isNew: true,
            params: {
                action: 0
            }
        });
    }
});