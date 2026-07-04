SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.GenerateMaintainPlanCommand', {
    meta: { text: "生成", group: "edit", iconCls: "icon-AddEntity" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        me.view = view;
        var parent = view.getParent().getData().getData();
        if (!parent.PlanBeginDate || !parent.PlanEndDate) {
            SIE.Msg.showInstantMessage('计划开始日期和计划结束日期不可为空!'.t());
            return false;
        }

        if (parent.PlanBeginDate - parent.PlanEndDate > 0) {
            SIE.Msg.showInstantMessage('计划开始时间不能大于计划结束时间!'.t());
            return false;
        }

        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer",
            method: "GetMaintainPlanRecords",
            params: [parent.PlanBeginDate, parent.PlanEndDate, parent.MaintainCycleType],
            async: false,
            token: view.token,
            callback: function (res) {
                console.log(res);
                var entities = [];
                for (var data of res.Result) {                    
                    var entity = new view._model();
                    entity.setYearAndMonth(data.YearAndMonth);
                    entity.setCycle(data.Cycle);
                    entity.setPlanBeginDate(data.PlanBeginDate);
                    entity.setPlanEndDate(data.PlanEndDate);
                    entity.setMaintainType(data.MaintainType);
                    entity.setMaintainTime(parent.MaintainTime);
                    entity.setMaintainCycleType(data.MaintainCycleType);
                    entity.setMaintainTypeInfoId(data.MaintainTypeInfoId);
                    entity.setMaintainTypeInfoId_Display(data.MaintainTypeInfoValue);
                    entity.setWhetherRepair(data.WhetherRepair);
                    entity.markSaved(); // 启用前端排序需要标记已保存
                    entities.push(entity);
                }

                view.getData().setData(entities);
                if (entities.length > 0)
                {
                    view.getData().queryRecords().forEach(function (record) {
                        view.mon(record, "propertyChanged", me.PropertyChanged, me);
                    });
                }
            }
        })
    },
    PropertyChanged: function (arg) {
        var me = this;
        var planEndDate = new Date(arg.entity.data.PlanEndDate.valueOf());
        planEndDate.setHours(planEndDate.getHours() + 24);
        if (arg.property == 'PrecisePlanBeginDate' && arg.value) {
            if (arg.value < arg.entity.data.PlanBeginDate || arg.value >= planEndDate) {                
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
                arg.entity.setPrecisePlanEndDate(null);
                return false;
            }

            if (arg.entity.data.PrecisePlanBeginDate)
                if (arg.value <= arg.entity.data.PrecisePlanBeginDate) {                    
                    arg.entity.setPrecisePlanEndDate(null);
                    return false;
                }

            var endTime = new Date(arg.value.valueOf());
            var startTime = arg.entity.data.PrecisePlanBeginDate;
            if (!Ext.isEmpty(startTime)) {
                var date = endTime.getTime() - startTime.getTime();
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
        var plans = me.view.getData().getData().items;
        var date = null;
        Ext.each(plans, function (item) {
            if (item.data.Cycle == (cycle - 1)) {
                date = item.data.PrecisePlanEndDate;
                return;
            }
        });
        return date;
    }
});
