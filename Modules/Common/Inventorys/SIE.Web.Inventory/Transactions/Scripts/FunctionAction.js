Ext.define('SIE.Web.Inventory.FunctionAction', {
    statics: {
        getTransaction: function (entity, token) {

            SIE.invokeDataQuery({
                method: 'GetTransaction',
                action: 'queryer',
                params: [entity.getOrderType()],
                type: 'SIE.Web.Inventory.Transactions.DataQueryer.FunctionDataQuery',
                token: token,
                success: function success(res) {
                    if (res.Result) {
                        entity.setTransactionId_Display(res.Result.Name);
                        entity.setTransactionId(res.Result.Id);
                    }
                }
            });
        }
    }
});