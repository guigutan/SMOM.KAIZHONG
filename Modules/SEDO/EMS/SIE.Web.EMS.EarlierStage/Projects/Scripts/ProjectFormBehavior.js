Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectFormBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.action = params.action;
            if (params.action === 0) {
                SIE.invokeDataQuery({
                    method: 'GetNewProject',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.EarlierStage.Projects.ProjectDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setCode(info.Code);
                            entity.setYear(info.Year);
                            entity.setProjectStatus(info.ProjectStatus);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setPlanType(info.PlanType);
                            entity.setProjectDate(info.ProjectDate);
                            entity.setPrincipalId(info.PrincipalId);
                            var userInfo = CRT.Context.GlobalContext.getContext('userInfo');
                            entity.setPrincipalId_Display(userInfo.Name);
                        }
                    }
                });
            }
        }
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'FactoryId' || e.property === 'DepartmentId' || e.property === 'PlanType') {
            var childView = me._children.first(function (p) { return p.model === "SIE.EMS.EarlierStage.Projects.ProjectKeyItem"; });
            if (childView) {
                var store = childView.getData();
                store.removeAll();
                childView.setData(store);
            }
        }
    }
});