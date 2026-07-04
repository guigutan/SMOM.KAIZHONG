Ext.define('SIE.Web.RedCardManagment.RedCards.Behaviors.ProductRetroactiveCriteriaBehavior',
    {
        /**
        * view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            var me = this;
            var entity = view.getCurrent();
            var params = CRT.Context.PageContext.getParams();
            entity.setRedCardId(params.RedCardId);
            if (params.BatchList)entity.setBatchList(params.BatchList.join(","));
            if (params.SnList)entity.setSnList(params.SnList.join(","));
        },
    });