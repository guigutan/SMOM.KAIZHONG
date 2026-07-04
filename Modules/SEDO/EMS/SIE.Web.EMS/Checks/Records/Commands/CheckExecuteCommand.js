SIE.defineCommand('SIE.Web.EMS.Checks.Records.Commands.CheckExecuteCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "点检执行", group: "edit" },
    //是否点击
    _isClick: false,
    canExecute: function (view) {
        var selectModels = view.getSelection();

        if (selectModels.length != 1) {
            return false;
        }

        if (view.getSelection()[0].data.ExeState == 1 || view.getSelection()[0].data.ExeState == 6 || view.getSelection()[0].data.ExeState == 3) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {
        var me = this;
        var sel = view.getSelection()[0].data;

        SIE.AutoUI.getMeta({
            model: 'SIE.Web.EMS.Checks.Plans.ViewModels.SelDepartmentViewModel',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock) {
                    mainBlock = res.mainBlock;
                }
                else {
                    mainBlock = res;
                }

                var listView = SIE.AutoUI.createListView(mainBlock);
                var ui = listView.getControl();

                //查询当前用户，当前时间段，拥有操作权限的点检单，按部门分类显示
                //理论上，一个部门最多在该时间节点只有一张点检单
                SIE.invokeDataQuery({
                    method: 'GetCheckPlanById',
                    params: [sel.Id],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer',
                    token: view.token,
                    success: function (resGetCheckPlan) {
                        if (resGetCheckPlan.Success) {

                            if (resGetCheckPlan.Result == null || resGetCheckPlan.Result.length <= 0) {
                                SIE.Msg.showMessage("当前登陆人无责任部门权限".t());
                                return;
                            }

                            if (resGetCheckPlan.Result.length === 1) {
                                //当只有一张检验单，直接打开页签
                                if ((resGetCheckPlan.Result[0].State === 3 || resGetCheckPlan.Result[0].State === 5) && resGetCheckPlan.Result[0].IfOpenConfirmationTab === true) {
                                    if (resGetCheckPlan.Result[0].IsConfirm) {
                                        //已评分或者待确认则打开确认界面
                                        me.showConfirmationView(resGetCheckPlan.Result[0], view);
                                    }
                                    else {
                                        SIE.Msg.showMessage("当前登陆人设备与人员权限没有点检确认权限".t());
                                    }
                                } else {
                                    //其他状态则打开执行界面
                                    me.showExecutionView(resGetCheckPlan.Result[0], view);
                                }
                            } else {
                                //赋值界面数据
                                resGetCheckPlan.Result.forEach(function (item) {
                                    listView.getData().add(item);
                                });

                                //存在多张检验单，打开点检单选择页面
                                me.showCheckPlanSelectionView(ui, listView, view);
                            }
                        }
                    },
                })
            },
        });
    },
    /**
   * 弹出“点检计划执行”页签效果
   * @method showExecutionView
   * @param  {SelDepartmentViewModel} data 部门选择VM
   * @param  {date} beginDate 点检开始时间
   * @param  {date} endDate 点检结束时间
   * @param  {ListlView} view 生成的view
   */
    showExecutionView: function (data, view) {
        var me = this;

        //判断是否已打开对应的点检执行，如果已打开，则不重新打开，避免重复事件绑定处理
        if (me.tab) {
            var tabPanel = portal.getTabPanel();
            tabPanel.setActiveTab(me.tab);
            return;
        }

        if (me._isClick) {
            return;
        }

        me._isClick = true;

        //初始化选中点检单数据，用以生成点检项目，查询上一次点检小结
        SIE.invokeDataQuery({
            method: 'InitExeCheckPlan',
            params: [data.CheckPlanId, data.EquipAccountId, data.DepartmentId],
            action: 'queryer',
            type: 'SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Success) {
                    CRT.Workbench.addPage({
                        entityType: "SIE.EMS.Checks.Plans.CheckPlan",
                        recordId: data.CheckPlanId,
                        title: "点检执行".t(),
                        viewGroup: "PlanExecuteViewGroup",
                        module: view.module,
                        isDetail: true,
                        ignoreQuery: true,
                        params: {
                            LastCheckSummary: res.Result
                        },
                    });
                }

                me._isClick = false;
            },
            error: function (res) {
                if (!res.Success) {
                    me._isClick = false;
                }
            },
            failure: function (response, opts) {
                //http state
                if ('communication failure' === response.statusText) {
                    SIE.Msg.showWarning('请求时间超时'.t());
                } else if (response.statusText === '') {
                    //do nothing
                }
                else {
                    var res = response.responseJson;
                    if (!res && response.responseText) {
                        res = Ext.decode(response.responseText);
                        SIE.Msg.showError(res.Message);
                    }
                }

                me._isClick = false;
            }
        });
    },
    /**
    * 弹出“点检计划确认”页签效果
    * @method showConfirmationView
    * @param  {SelDepartmentViewModel} data 部门选择VM
    * @param  {date} beginDate 点检开始时间
    * @param  {date} endDate 点检结束时间
    * @param  {ListlView} view 生成的view
    */
    showConfirmationView: function (data, view) {
        var me = this;
        //判断是否已打开对应的点检执行，如果已打开，则不重新打开，避免重复事件绑定处理
        if (me.tab) {
            me.isTabExist = true;
            var tabPanel = portal.getTabPanel();
            tabPanel.setActiveTab(me.tab);
            return;
        } else {
            me.isTabExist = false;
        }

        if (!this.isTabExist) {
            //打开“点检确认”明细页签
            SIE.invokeDataQuery({
                method: 'InitConfirmCheckPlan',
                params: [data.CheckPlanId, data.CheckPlanNo, data.EquipAccountId, data.DepartmentId],
                action: 'queryer',
                type: 'SIE.Web.EMS.Checks.Confirmations.DataQuery.CheckConfirmationQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Success) {
                        CRT.Workbench.addPage({
                            entityType: "SIE.EMS.Checks.Plans.CheckPlan",
                            recordId: data.CheckPlanId,
                            title: "点检确认".t(),
                            module: view.module,
                            viewGroup: "PlanConfirmViewGroup",
                            isDetail: true,
                            ignoreQuery: true,
                            token: view.token,
                            params: {
                                RecordId: data.CheckPlanId,
                                CheckPlanNo: data.CheckPlanNo,
                                DepartmentId: data.DepartmentId,
                                DepartmentName: data.DepartmentName,
                                EquipAccountId: data.EquipAccountId,
                                ConfirmResult: res.Result.ConfirmResult,
                                ConfirmNote: res.Result.ConfirmNote,
                                CheckExeState: res.Result.CheckExeState
                            }
                        });
                    }
                }
            })
        }
    },

    /**
    * 弹出点检单选择页面
    * @method showCheckPlanSelectionView
    * @param  {ctl} ui 界面UI控件
    * @param  {ListlView} view 点检单选择界面view
    * @param  {date} beginDate 点检开始时间
    * @param  {date} endDate 点检结束时间
    * @param  {view} view 主界面view
    */
    showCheckPlanSelectionView: function (ui, listView, view) {
        var me = this;
        var win = SIE.Window.show({
            title: "选择点检单".t(),
            width: 500,
            height: 500,
            items: ui,
            callback: function (btn) {
                if (btn == "确定".t()) {

                    var current = listView.getCurrent();

                    if (current == null) {
                        SIE.Msg.showMessage("请选择点检单".t());
                    }
                    else {
                        //依据执行状态打开对应的页签
                        if ((current.data.State === 3 || current.data.State === 5) && current.data.IfOpenConfirmationTab === true) {
                            if (current.data.IsConfirm) {
                                //已评分或者待确认则打开确认界面
                                me.showConfirmationView(current.data, view);
                            }
                            else {
                                SIE.Msg.showMessage("当前登陆人设备与人员权限没有点检确认权限".t());
                            }
                        } else {
                            //其他状态则打开执行界面
                            me.showExecutionView(current.data, view);
                        }

                        win.close();
                    }                   
                }
            }
        });
    },

});