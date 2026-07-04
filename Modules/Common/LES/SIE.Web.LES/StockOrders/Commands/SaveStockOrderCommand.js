SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.SaveStockOrderCommand', {
    meta: { text: "保存", group: "edit", iconCls: "iconfont icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.FormSave',
    canExecute: function (view) {
        let cur = view.getCurrent();
        if (cur == null || !cur.isDirty()) {
            return false;
        }

        return view.getData().isDirty();
    },
    onSaved: function (view, res) {
        let me = this;
        let current = view.getCurrent();
        current.markSaved();
        SIE.invokeDataQuery({
            method: 'GetStockOrder',
            params: [current.data.Id],
            action: 'queryer',
            type: 'SIE.Web.LES.StockOrders.DataQueryer.StockOrderQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result && res.Result == 10) {
                    current.setStockState(SIE.LES.StockOrder.StockState.Audit.value);
                    current.markSaved();
                }
            }
        });

        this.callParent(arguments);
    },
});