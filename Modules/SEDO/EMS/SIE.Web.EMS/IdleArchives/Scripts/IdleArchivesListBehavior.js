Ext.define('SIE.Web.EMS.IdleArchives.IdleArchivesListBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
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
    },
    onShow: function (view) {
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            //获取查询视图
            var conditionView = view.getConditionView();
            //获取查询实体元数据
            var criteria = conditionView.getData();
            //赋值传递过来的调拨单号

            criteria.setNo(params.IdleArchiveNo);
            //清空所有时间范围控件的开始结束时间
            var dateRangeCtls = conditionView.getControl().items.items.filter(function (e) { return e.xtype === "dateRange"; })
            if (dateRangeCtls.length > 0) {
                dateRangeCtls.forEach(function (ctl) {
                    ctl.setDataRangValue(null, null);
                });
            }
            //执行查询
            conditionView.tryExecuteQuery();
        }
    }
});