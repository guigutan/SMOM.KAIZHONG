SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.GenerateCheckPlanCommand', {
    meta: { text: "生成", group: "edit", iconCls: "icon-AddEntity" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var parent = view.getParent().getData().getData();
        //1.0 数据生成前验证
        if (!me.dataValidate(parent)) return false;
        //2.0 设置点检计划
        me.generateCheckPlan(parent,view);
    },
    generateCheckPlan: function (data,view) {
        var entities = [];
        for (var date = new Date(data.BeginDate) ; date - data.EndDate <= 0;) {

            var entity = {};
            entity.EquipAccountId = data.EquipAccountId;
            entity.EquipAccountId_Display = data.EquipAccountId_Display;
            entity.CheckCycleType = data.CheckCycleType;
            entity.CheckDate = new Date(date.toDateString());//引用会有问题，这里new一下
            entity.WhetherAcrossDay = data.WhetherAcrossDay;

            var beginDate = new Date(date);
            var endDate = new Date(date);
            beginDate.setHours(data.CheckBeginDate.getHours());
            beginDate.setMinutes(data.CheckBeginDate.getMinutes());
            endDate.setHours(data.CheckEndDate.getHours());
            endDate.setMinutes(data.CheckEndDate.getMinutes());
            if (data.WhetherAcrossDay == 1) {
                endDate.setDate(endDate.getDate() + 1);
            }

            entity.CheckBeginDate = beginDate;
            entity.CheckEndDate = endDate;

            entities.push(entity);

            date.setDate(date.getDate() + 1);
        }
        view.getData().setData(entities);
    },
    dataValidate: function (data) {
        var flag = true;
        if (!data.EquipAccountId || !data.BeginDate || !data.EndDate || data.CheckCycleType == null || !data.CheckBeginDate || !data.CheckEndDate) {
            SIE.Msg.showInstantMessage('表单栏位不可为空!'.t());
            flag= false;
        }
        if (data.BeginDate - data.EndDate > 0) {
            SIE.Msg.showInstantMessage('计划开始时间不能大于计划结束时间!'.t());
            flag = false;
        }

        if (data.WhetherAcrossDay == 0 && data.CheckBeginDate - data.CheckEndDate > 0) {
            SIE.Msg.showInstantMessage('在不跨日的情况下，点检开始时间不能大于点检结束时间!'.t());
            flag = false;
        }
        return flag;
    }
});
