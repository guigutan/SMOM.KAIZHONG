/**
 * 排班表查询命令
 */
SIE.defineCommand('SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleQuery', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },

    /**
     * @property ListLogicView
     * 查询逻辑视图
     */
    view: null,

    /**
     * @property {Boolean}
     * 是否允许查询，反正恶意查询 
     */
    allow: true,

    /**
     * 判断查询方法能否执行
     * @param view 查询逻辑视图
     * @returns 能执行返回true，否则返回false
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        return this.allow && current != null;
    },

    /**
     * 执行查询方法 
     * @param view 查询逻辑视图
     */
    execute: function (view) {
        var me = this;
        try {
            me.allow = false;
            me.view = view;
            var record = view.getCurrent();
            delete record.data['CriteriaModuleKey'];
            delete record.data['CriteriaType'];
            delete record.data["CriteriaString"];
            var istrue = true;
            me.view.getControl().items.items.forEach(function (item) {
                if (!item.validate()) {
                    istrue = false;
                }
            });
            var criteria = record.data;
            if (!me.validateCriteria(criteria)) {
                me.allow = true;
                return;
            }
            SIE.invokeDataQuery({
                method: 'GetShiftScheduleTables',
                params: [criteria],
                action: 'queryer',
                type: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleDataQueryer',
                token: me.view.getToken(),
                success: function (res) {
                    var layout = me.view.mainLayout;
                    if (!layout)
                        return;
                    var scheduleDate = me.view.getCurrent().data.ScheduleDate;
                    layout.setGridPanelData(res.Result, scheduleDate.BeginValue, scheduleDate.EndValue);
                    me.allow = true;
                }
            });
        } catch (e) {
            me.allow = true;
            throw e;
        }
    },

    /**
     * 验证查询条件 
     * @param criteria 查询实体ShiftScheduleTableCriteria
     * @returns 通过返回true，否则返回false
     */
    validateCriteria: function (criteria) {
        if (criteria === null)
            return false;
        if (!criteria.ScheduleDate.BeginValue) {
            SIE.Msg.showMessage('开始日期不能为空'.t());
            return false;
        }
        if (!criteria.ScheduleDate.EndValue) {
            SIE.Msg.showMessage('结束日期不能为空'.t());
            return false;
        }
        if (criteria.ScheduleDate.BeginValue > criteria.ScheduleDate.EndValue) {
            SIE.Msg.showMessage('开始日期不能大于结束日期'.t());
            return false;
        }
        return true;
    }
});