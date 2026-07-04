Ext.define('SIE.Web.Fixtures.Demands.Scripts.FixtureDemandBehavior',
    {
        /**
       * onViewReady 视图加载完成
       * @param {*} view 当前视图
        * _golble_use_approval 用于记录是否启用流程审批
       */
        onViewReady: function (view) {
            var me = this;
            me.mainView = view;
            var cmdExamineName = "SIE.Web.Fixtures.Demands.Commands.ExamineDemandCommand";
            var cmdSubmitName = "SIE.Web.Fixtures.Demands.Commands.SubmitDemandsCommand";
            var cmdEditName = "SIE.Web.Fixtures.Demands.Commands.EditDemandCommand";
            SIE.invokeDataQuery({
                method: 'GetEnableApproval',
                params: [],
                action: 'queryer',
                type: 'SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer',
                token: view.token,
                success: function (res) {
                    _golble_use_approval = res.Result;
                    if (_golble_use_approval != 10) {
                        var cmd = view.getCmdControl(cmdExamineName);
                        if (cmd) {
                            cmd.setHidden(true);
                            view._commands.removeAtKey(cmdExamineName);
                        }
                        var cmdSubmit = view.getCmdControl(cmdSubmitName);
                        if (cmdSubmit) {
                            cmdSubmit.setHidden(true);
                            view._commands.removeAtKey(cmdSubmitName);
                        }
                        var cmdEdit = view.getCmdControl(cmdEditName);
                        if (cmdEdit) {
                            cmdEdit.setHidden(true);
                            view._commands.removeAtKey(cmdEditName);
                        }
                    }
                    if (_golble_use_approval == null)//无审核
                    {
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
        }
    });