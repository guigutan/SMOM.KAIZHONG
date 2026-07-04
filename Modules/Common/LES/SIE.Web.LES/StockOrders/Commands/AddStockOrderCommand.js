SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.AddStockOrderCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    CRT.Workbench.addPage({
                        entityType: me.view.model,
                        recordId: entity.getId(),
                        title: me.getEditViewTitle(entity),
                        params: {
                            No: data.No,
                            StockState: SIE.LES.StockOrder.StockState.Created.value,
                            BillSource: SIE.LES.StockOrder.BillSource.Manual.value,
                            TriggerMode: data.TriggerMode,
                        },
                        isDetail: true
                    })
                }
            }, me.view);
        }
    },
});