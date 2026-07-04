SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.EditEquipMaintainPlanCommand', {
    meta: { text: "修改保养计划", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    items: null,
    canExecute: function (view) {
        return view.getCurrent();
    },
    execute: function (view, source) {
        var me = this;
        var equipData = view.getCurrent().data;
        SIE.AutoUI.getMeta({
            model: 'SIE.EMS.Maintains.Plans.MaintainPlan',
            module: 'SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanViewModel,SIE.EMS',
            viewGroup: 'EditMaintainPlan',
            isDetail: true,
            isAggt: true,
            async: false,
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                var model = SIE.getModel('SIE.EMS.Maintains.Plans.MaintainPlan');
                var entity = new model();
                entity.token = view.token;
                entity.setEquipMaintainType(0);
                entity.setEquipAccountCode(equipData.EquipAccountCode);
                entity.setEquipAccountName(equipData.EquipAccountName);
                entity.setMachineNo(equipData.EquipAccountName);
                var entities = [];
                SIE.invokeDataQuery({
                    type: "SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer",
                    method: "GetEditMaintainPlanRecords",
                    params: [equipData.EquipAccountCode],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        
                        if (res.Result.length > 0){
                            entity.setPlanBeginDate(res.Result[0].PlanBeginDate);
                            entity.setPlanEndDate(res.Result[res.Result.length-1].PlanEndDate);
                            entity.setMaintainTime(res.Result[0].MaintainTime);
                            for (var i = 0; i < res.Result.length; i++) {
                                var data = res.Result[i];
                                var plan = {};
                                plan.Id = data.Id;
                                plan.EquipAccountId = data.EquipAccountId;
                                plan.YearAndMonth = data.YearAndMonth;
                                plan.Cycle = data.Cycle;
                                plan.PlanBeginDate = new Date(data.PlanBeginDate);
                                plan.PlanEndDate = new Date(data.PlanEndDate);
                                plan.MaintainType = data.MaintainType;
                                plan.PrecisePlanBeginDate = data.PrecisePlanBeginDate == null ? null : new Date(data.PrecisePlanBeginDate);
                                plan.PrecisePlanEndDate = data.PrecisePlanBeginDate == null ? null : new Date(data.PrecisePlanEndDate);
                                plan.MaintainTime = data.MaintainTime;
                                plan.IntervalTime = data.IntervalTime;
                                plan.LastPrecisePlanEndTime = data.LastPrecisePlanEndTime;
                                entities.push(plan);
                            }
                        }
                        me.view = detailView._view;
                        detailView._view._setDefaultValue(entity);
                        detailView._view.setData(entity);

                        detailView._view._children[0].getControl().store.setData(entities);
                        detailView._view._children[0].getControl().store.data.each(function (item) {
                            item.commit();
                        });
                        if (entities.length > 0) {
                            detailView._view._children[0].getControl().store.queryRecords().forEach(function (record) {
                                view.mon(record, "propertyChanged", me.PropertyChanged, me);
                            });
                        }

                        detailView._view._children[0].syncCmdState();
                        var win = SIE.Window.show({
                            title: '修改保养计划'.t(),
                            width: '55%',
                            height: '70%',
                            items: detailView.getControl(),
                            callback: function (btn) {
                                if (btn === "确定".t()) {
                                    me.save(view, win);
                                    return false;
                                }
                            }
                        });
                    }
                });
            }
        });
    },
    PropertyChanged: function (arg) {
        var me = this;

        var planEndDate = new Date(arg.entity.data.PlanEndDate.valueOf());
        planEndDate.setHours(planEndDate.getHours() + 24);
        if (arg.property == 'PrecisePlanBeginDate' && arg.value) {
            if (arg.value < arg.entity.data.PlanBeginDate || arg.value >= planEndDate) {
                //SIE.Msg.showWarning("指定计划开始时间需在计划开始日期和结束日期之间！".t());
                arg.entity.setPrecisePlanBeginDate(null);
                return false;
            }

            //获取上一条记录的结束时间
            var LastPrecisePlanEndTime = me.getLastPrecisePlanEndDate(arg.entity.data.Cycle);
            if (arg.entity.data.IntervalTime > 1 && LastPrecisePlanEndTime != null) {
                //计算最快可以进行下一次保养的开始时间
                var nextStartDate = new Date(LastPrecisePlanEndTime.valueOf());
                nextStartDate.setHours(nextStartDate.getHours() + arg.entity.data.IntervalTime * 24);
                if (arg.value <= nextStartDate) {
                    //SIE.Msg.showWarning("指定计划开始时间离上周指定保养结束时间未超过"+ arg.entity.data.IntervalTime +"天，请重新维护！".t());
                    arg.entity.setPrecisePlanBeginDate(null);
                    return false;
                }
            }

            var startTime = new Date(arg.value.valueOf());
            var maintainTime = arg.entity.data.MaintainTime;
            if (!Ext.isEmpty(maintainTime)) {
                var endDate = startTime.setHours(startTime.getHours() + maintainTime);
                arg.entity.setPrecisePlanEndDate(new Date(endDate));
            }
        }
        if (arg.property == 'PrecisePlanEndDate' && arg.value) {
            if (arg.value < arg.entity.data.PlanBeginDate || arg.value >= planEndDate) {
                //SIE.Msg.showWarning("指定计划结束时间需在计划开始日期和结束日期之间！".t());
                arg.entity.setPrecisePlanEndDate(null);
                return false;
            }

            if (arg.entity.data.PrecisePlanBeginDate)
                if (arg.value <= arg.entity.data.PrecisePlanBeginDate) {
                    //SIE.Msg.showWarning("指定计划结束时间须大于指定计划开始时间！".t());
                    arg.entity.setPrecisePlanEndDate(null);
                    return false;
                }

            var endTime = new Date(arg.value.valueOf());
            var startTime = arg.entity.data.PrecisePlanBeginDate;
            if (!Ext.isEmpty(startTime)) {
                var date = endTime.getTime() - startTime.getTime();
                console.log(date);
                var leave1 = date / (3600 * 1000);
                var maintainTime = Math.floor(leave1);
                arg.entity.setMaintainTime(maintainTime);
            }
        }

        if (arg.property == 'MaintainTime' && arg.value) {
            var maintainTime = arg.value;
            if (arg.entity.data.PrecisePlanBeginDate != null) {
                var startTime = new Date(arg.entity.data.PrecisePlanBeginDate.valueOf());
                if (!Ext.isEmpty(startTime)) {
                    var endDate = startTime.setHours(startTime.getHours() + maintainTime);
                    arg.entity.setPrecisePlanEndDate(new Date(endDate));
                }
            }
        }
    },
    getLastPrecisePlanEndDate: function (cycle) {
        var me = this;
        var plans = me.view._children[0].getData().getData().items;
        var date = null;
        Ext.each(plans, function (item) {
            if (item.data.Cycle == (cycle - 1)) {
                date = item.data.PrecisePlanEndDate;
                return;
            }
        });
        return date;
    },
    save: function (view, win, ui) {
        var me = this;
        var plans = me.view._children[0].getData().getData().items;
        var planArr = [];
        Ext.each(plans, function (item) {
            planArr.push(item.data);
        });
        console.log(planArr);
        if (planArr.length == 0) {
            SIE.Msg.showWarning("该设备没有可更新的保养计划！".t());
            return false;
        }

        var indata = {};
        indata.Data = Ext.encode(planArr);
        view.execute({
            data: indata,
            command: "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.EditEquipMaintainPlanCommand",
            success: function (res) { //回调
                var conditionView = view.getConditionView();
                conditionView.getCommands().items[0].execute(conditionView);
                win.close();
                SIE.Msg.showInstantMessage('保存成功'.t());
                view.reloadData();
            }
        });
    }
});