SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Records.Commands.MaintainExecuteCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "保养执行", group: "edit" },
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
        var position = sel.Position;

        SIE.AutoUI.getMeta({
            model: 'SIE.Web.EMS.EquipMaint.Maintains.Plans.ViewModels.SelDepartmentViewModel',
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

                //查询当前用户，当前时间段，拥有操作权限的保养单，按部门分类显示
                //理论上，一个部门最多在该时间节点只有一张保养单
                SIE.invokeDataQuery({
                    method: 'GetMaintainPlansById',
                    params: [sel.Id],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {

                            if (res.Result == null || res.Result.length <= 0) {
                                SIE.MessageBox.showMessage("当前节点没有保养单数据".t());
                                return;
                            }
                            if (res.Result.length === 1) {
                                //当只有一张检验单，直接打开页签
                                if ((res.Result[0].State === 3 || res.Result[0].State === 5) && res.Result[0].IfOpenConfirmationTab === true) {
                                    if (res.Result[0].IsConfirm) {
                                        //已评分或者待确认则打开确认界面
                                        me.showConfirmationView(res.Result[0], view);
                                    }
                                    else {
                                        SIE.Msg.showMessage("当前登陆人设备与人员权限没有保养确认权限".t());
                                    }
                                } else {
                                    //其他状态则打开执行界面
                                    me.showExecutionView(res.Result[0], view);
                                }
                            } else {
                                //赋值界面数据
                                res.Result.forEach(function (item) {
                                    listView.getData().add(item);
                                });

                                //存在多张检验单，打开点检单选择页面
                                me.showMaintainPlanSelectionView(ui, listView, view);
                            }
                        }
                    },
                })
            },
        });
    },

    /**
    * 弹出“保养计划确认”页签效果
    * @method showConfirmationView
    * @param  {SelDepartmentViewModel} data 部门选择VM
    * @param  {ListlView} view 生成的view
    */
    showConfirmationView: function (data, view) {
        debugger
        var me = this;
        var meta = null;
        var sel = view.getSelection()[0].data;
        var position = sel.Position; //岗位


        //判断是否已打开对应的保养确认页签，如果已打开，则不重新打开，避免重复事件绑定处理
        if (me.tab) {
            me.isTabExist = true;
            var tabPanel = portal.getTabPanel();
            tabPanel.setActiveTab(me.tab);
            return;
        } else {
            me.isTabExist = false;
        }

        if (!this.isTabExist) {
            //打开“保养确认”明细页签
            SIE.invokeDataQuery({
                method: 'InitConfirmMaintainPlan',
                params: [data.MaintainPlanId, data.MaintainPlanNo, data.EquipAccountId, data.DepartmentId],
                action: 'queryer',
                type: 'SIE.Web.EMS.EquipMaint.Maintains.Confirmations.DataQuery.MaintainConfirmationQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Success) {
                        CRT.Workbench.addPage({
                            entityType: "SIE.EMS.Maintains.Plans.MaintainPlan",
                            recordId: data.MaintainPlanId,
                            title: "保养确认".t(),
                            module: view.module,
                            viewGroup: "PlanConfirmViewGroup",
                            isDetail: true,
                            ignoreQuery: true,
                            token: view.token,
                            params: {
                                RecordId: data.MaintainPlanId,
                                MaintainPlanNo: data.MaintainPlanNo,
                                DepartmentId: data.DepartmentId,
                                DepartmentName: data.DepartmentName,
                                EquipAccountId: data.EquipAccountId,
                                UpMaintainSummary: res.Result.UpMaintainSummary,
                                ConfirmResult: res.Result.ConfirmResult,
                                ConfirmNote: res.Result.ConfirmNote,
                                PrecisePlanBeginDate: res.Result.PrecisePlanBeginDate,
                                PrecisePlanEndDate: res.Result.PrecisePlanEndDate,
                                MaintianExeState: res.Result.MaintianExeState,
                            }
                        });
                    }
                }
            })
        }
    },

    /**
    * 弹出“保养计划执行”页签效果
    * @method showExecutionView
    * @param  {SelDepartmentViewModel} data 部门选择VM
    * @param  {date} beginDate 保养开始时间
    * @param  {date} endDate 保养结束时间
    * @param  {ListlView} view 生成的view
    */
    showExecutionView: function (data, view) {
        var me = this;
        var meta = null;
        var sel = view.getSelection()[0].data;
        var position = sel.Position;


        //判断是否已打开对应的保养执行页签，如果已打开，则不重新打开，避免重复事件绑定处理
        if (me.tab) {
            me.isTabExist = true;
            var tabPanel = portal.getTabPanel();
            tabPanel.setActiveTab(me.tab);
            return;
        } else {
            me.isTabExist = false;
        }

        if (!this.isTabExist) {
            //初始化选中保养单数据，用以生成保养项目，查询上一次保养小结
            SIE.invokeDataQuery({
                method: 'InitExeMaintainPlan',
                params: [data.MaintainPlanId, data.EquipAccountId, data.DepartmentId],
                action: 'queryer',
                type: 'SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Success) {

                        var precisePlanBeginDate = sel.PlanBeginDate;
                        var precisePlanEndDate = sel.PlanEndDate;

                        ////判断指定日期是否为空
                        if (res.Result.PrecisePlanBeginDate != null && res.Result.PrecisePlanEndDate != null) {
                            precisePlanBeginDate = res.Result.PrecisePlanBeginDate;
                            precisePlanEndDate = res.Result.PrecisePlanEndDate;
                        }
                        CRT.Workbench.addPage({
                            entityType: "SIE.EMS.Maintains.Plans.MaintainPlan",
                            recordId: data.MaintainPlanId,
                            title: "保养执行".t(),
                            viewGroup: "PlanExecuteViewGroup",
                            module: view.module,
                            isDetail: true,
                            ignoreQuery: true,
                            params: {
                                PrecisePlanBeginDate: precisePlanBeginDate,
                                PrecisePlanEndDate: precisePlanEndDate,
                                UpMaintainSummary: res.Result.UpMaintainSummary,
                            },
                        });
                    }
                }
            });
        }
    },

    /**
    * 弹出保养单选择页面
    * @method showMaintainPlanSelectionView
    * @param  {ctl} ui 界面UI控件
    * @param  {ListlView} view 保养单选择界面view
    * @param  {date} beginDate 保养开始时间
    * @param  {date} endDate 保养结束时间
    * @param  {view} view 主界面view
    */
    showMaintainPlanSelectionView: function (ui, listView, view) {
        var me = this;
        var win = SIE.Window.show({
            title: "选择保养单".t(),
            width: 500,
            height: 500,
            items: ui,
            callback: function (btn) {
                if (btn == "确定".t()) {

                    var current = listView.getCurrent();

                    if (current == null) {
                        SIE.Msg.showMessage("请选择保养单".t());
                    }
                    else {
                        //依据执行状态打开对应的页签
                        if ((current.data.State === 3 || current.data.State === 5) && current.data.IfOpenConfirmationTab === true) {
                            if (current.data.IsConfirm) {
                                //已评分或者待确认则打开确认界面
                                me.showConfirmationView(current.data, view);
                            }
                            else {
                                SIE.Msg.showMessage("当前登陆人设备与人员权限没有保养确认权限".t());
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