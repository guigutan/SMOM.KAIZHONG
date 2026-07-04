Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectKeyItemCriteriaBehavior', {
    /**
    * view聚合后
    * @param {*} view 生成的view
    */
    onViewReady: function (view) {
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            entity.setNo(params.ProjectCode);
            entity.setCreateDate(null);
            view.tryExecuteQuery();
        }
    }
});