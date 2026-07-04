SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.SCDetailImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入Excel", group: "business", iconCls: "icon-Upload icon-blue" },

    canExecute: function (view) {
        if (view.getData().isDirty()) return false;
        var stockCount = view.getParent().getCurrent();
        if (stockCount == null) return false;
        if (stockCount != null) {
            //SIE.Warehouses.CountState.Audit.value(审批)=10
            //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
            //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
            if (stockCount.getState() !== 10 &&
                stockCount.getState() !== 30 &&
                stockCount.getState() !== 40)
                return false;
        }

        return true;
    },
    _importSuccess: function (view) {
        var me = this;
        view.reloadData();
        var curParent = view.getParent().getCurrent();
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
            method: 'GetLesStockCountState',
            token: me.view.token,
            params: [curParent.getId()],
            success: function (res) {
                if (res.Result != null) {
                    curParent.setState(res.Result);
                    curParent.markSaved();
                }
            }
        });
    },
});