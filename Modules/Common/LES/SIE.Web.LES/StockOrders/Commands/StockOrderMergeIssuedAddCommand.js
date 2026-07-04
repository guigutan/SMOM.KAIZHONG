SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.StockOrderMergeIssuedAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        entity.setState(0);
    },
});