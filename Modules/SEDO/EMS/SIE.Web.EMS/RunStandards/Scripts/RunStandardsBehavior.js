Ext.define('SIE.Web.EMS.RunStandards.RunStandardsBehavior', {
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
                    method: 'GetRunStandard',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.RunStandards.RunStandardsDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setNo(info.No);
                            entity.setCreateDate(info.CreateDate);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setCreateBy(info.CreateBy);
                            entity.data.CreateBy_Display = info.CreateName;
                        }
                    }
                });
            }
        }
        var cmdExamineName = "SIE.Web.EMS.RunStandards.Commands.ApprovalCommand";
        var cmdCancelName = "SIE.Web.EMS.RunStandards.Commands.CancelCommand";
        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.EMS.RunStandards.RunStandardsDataQueryer',
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
                    var tabPanels = view._children.find(m => m.model == "SIE.Equipments.WorkFlows.WorkFlowRecord");
                    if (tabPanels) {
                        var tabPanelItems = tabPanels.getControl().up().up().items.items;
                        Ext.each(tabPanelItems, function (item) {
                            if (("审核记录").indexOf(item.title) >= 0) {
                                item.tab.hide();
                            }
                        });
                    }
                }
            }
        });
        if (entity)
            view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
    },
    _onEntityPropertyChanged: function (e) {
        var entity = e.entity;
        if (e.property.length > 0 && e.property === "EquipModelId") {
            var child = e.entity.belongsView.findChild("SIE.EMS.RunStandards.RunStandardEquipment");
            child.getData().removeAll();
        }

    }
});