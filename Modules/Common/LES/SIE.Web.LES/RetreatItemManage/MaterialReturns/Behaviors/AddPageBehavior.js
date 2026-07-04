Ext.define('SIE.Web.LES.RetreatItemManage.MaterialReturns.Behaviors.AddPageBehavior', {
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
            
                SIE.invokeDataQuery({
                    method: 'GetNewMaterialReturns',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.LES.RetreatItemManage.MaterialReturns.MaterialReturnsDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            //var info = res.Result.data.items[0].data;
                            //entity.setNO(info.NO);
                        }
                    }
                });
    },
});