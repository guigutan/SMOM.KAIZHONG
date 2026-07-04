Ext.define('SIE.Web.EMS.IdleArchives.IdleArchivesBehavior', {
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
                    method: 'GetIdleArchive',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.IdleArchives.IdleArchivesDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setNo(info.No);
                            entity.setIdleArchiveType(info.IdleArchiveType);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setApplyDate(info.ApplyDate);
                            entity.setApplicantId(info.ApplicantId);
                            entity.setApplicantId_Display(info.ApplicantName);
                        }
                    }
                });
            }
        }
        var cmdExamineName = "SIE.Web.EMS.IdleArchives.Commands.ApprovalCommand";
        var cmdCancelName = "SIE.Web.EMS.IdleArchives.Commands.CancelCommand";
        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.EMS.IdleArchives.IdleArchivesDataQueryer',
            token: view.token,
            success: function (res) {
                var use_approval = res.Result;
                if (!use_approval) {
                    var cmd = view.getCmdControl(cmdCancelName);
                    if (cmd) {
                        cmd.setHidden(true);
                        view._commands.removeAtKey(cmdCancelName);
                    }
                    var cmdExamine = view.getCmdControl(cmdExamineName);
                    if (cmdExamine) {
                        cmdExamine.setHidden(true);
                        view._commands.removeAtKey(cmdExamineName);
                    }
                    var tabPanelItems = view._children.find(m => m.model == "SIE.Equipments.WorkFlows.WorkFlowRecord").getControl().up().up().
                        items.items;
                    Ext.each(tabPanelItems, function (item) {
                        if (("审核记录").indexOf(item.title) >= 0) {
                            item.tab.hide();
                        }
                    });
                }
            }
        });

        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        ///业务类型、工厂、管理部门、使用部门、固定资产、设备类别
        if (e.property === 'IdleArchiveType' || e.property === "FactoryId" || e.property === "DepartmentId" && e.property === "UseDepartmentId"
            || e.property === 'TypeCategory' || e.property === 'IsAsset') {

            var childrenView = this._children.find(m => m.model == "SIE.EMS.IdleArchives.IdleArchiveDetail");
            if (childrenView) {
                childrenView.getData().data.removeAll();
            }
        }
    }
});