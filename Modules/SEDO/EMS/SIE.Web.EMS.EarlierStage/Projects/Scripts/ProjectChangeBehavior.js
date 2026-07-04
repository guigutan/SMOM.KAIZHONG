Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectChangeBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        me.keyChildView = view._children.first(function (p) { return p.model === "SIE.EMS.EarlierStage.Projects.ProjectChangeKeyItem"; });
        me.memChildView = view._children.first(function (p) { return p.model === "SIE.EMS.EarlierStage.Projects.ProjectChangeMember"; });
        me.workChildView = view._children.first(function (p) { return p.model === "SIE.EMS.EarlierStage.Projects.ProjectChangeWorkItem"; });
        me.susChildView = view._children.first(function (p) {
            return p.model === "SIE.EMS.EarlierStage.Projects.ProjectChange" && (p.viewGroup === "SuspendView" || p.viewGroup === "SuspendReadonlyView");
        });
        me.recChildView = view._children.first(function (p) {
            return p.model === "SIE.EMS.EarlierStage.Projects.ProjectChange" && (p.viewGroup === "RecoveryView" || p.viewGroup === "RecoveryReadonlyView");
        });
        if (entity.data.ProjectStatus > 0)
            me.setChildView(entity.data.ProjectStatus);
        view.ProBehavior = me;
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
        view.mon(view, 'beforeClosewin', me.beforeClosewin, view);
    },
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var qty = view._children.length;
        var tabPanel = view._children[0].getControl().ownerCt.ownerCt;
        for (var i = 1; i < qty; i++) {
            tabPanel.setActiveTab(i);
        }
        tabPanel.setActiveTab(0);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var proBehavior = me.ProBehavior;
        if (e.property === 'ProjectId') {
            proBehavior.setChildInfo(e, me.token);
        }
        if (e.property === 'ProjectStatus') {
            proBehavior.setChildView(e.value);
        }
    },
    onKeyStorePropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            if (e.property === 'BudgetAmount' || e.property === 'ProjectChangeId') {
                var qty = 0;
                me.getData().data.items.forEach(function (p) { qty += p.data.BudgetAmount; });
                var projectChange = me._parent.getCurrent();
                projectChange.setAmount(qty);
            }
        }
    },
    setChildInfo: function (e, token) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetChildInfoByProjectId',
            params: [e.value],
            action: 'queryer',
            type: 'SIE.Web.EMS.EarlierStage.Projects.ProjectDataQueryer',
            token: token,
            success: function (res) {
                if (res.Result) {
                    var info = res.Result;
                    SIE.each(info.Item1, function (model) {
                        model.BudgetId_Display = model.BudgetNo;
                        model.ProjectChangeWorkItemId_Display = model.ProjectChangeWorkItem_Display;
                        model.CreateDate = null;
                        model.UpdateDate = null;
                    });
                    SIE.each(info.Item2, function (model) {
                        model.EmployeeId_Display = model.EmployeeName;
                        model.CreateDate = null;
                        model.UpdateDate = null;
                    });
                    SIE.each(info.Item3, function (model) {
                        model.PrincipalId_Display = model.PrincipalName;
                        model.CreateDate = null;
                        model.UpdateDate = null;
                    });
                    if (me.keyChildView) {
                        var keyStore = me.keyChildView.getControl().getStore();
                        keyStore.setData(info.Item1);
                        me.keyChildView.mon(keyStore, 'propertyChanged', me.onKeyStorePropertyChanged, me.keyChildView);
                    }
                    if (me.memChildView) {
                        var memStore = me.memChildView.getControl().getStore();
                        memStore.setData(info.Item2);
                    }
                    if (me.workChildView) {
                        var workStore = me.workChildView.getControl().getStore();
                        workStore.setData(info.Item3);
                    }
                }
            }
        });
    },
    setChildView: function (value) {
        var me = this;
        if (value === 20) {
            if (me.susChildView) {
                me.susChildView._control.ownerLayout.config.owner.tab.setVisible(false);
            }
            if (me.recChildView) {
                me.recChildView._control.ownerLayout.config.owner.tab.setVisible(true);
            }
        } else {
            if (me.susChildView) {
                me.susChildView._control.ownerLayout.config.owner.tab.setVisible(true);
            }
            if (me.recChildView) {
                me.recChildView._control.ownerLayout.config.owner.tab.setVisible(false);
            }
        }
    },
    beforeClosewin: function (returnObj) {
        var me = this;
        var ismun = false;
        for (var modifiedProperty in me.getData().modified) {
            if (modifiedProperty != "ProjectType_Display")
                ismun = true;
        }
        returnObj.hasData = ismun;
    }
});