Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectWorkItemBehavior', {
    /**
     * view生成前
     * @param {any} view
     */
    onViewReady: function (view) {
        var me = this;
        var grid = view._control.getView();
        grid.mon(grid, 'refresh', me.OnRefresh);
    },
    /**
     * 数据刷新
     * @param {any} grid
     * @param {any} record
     */
    OnRefresh: function (grid, record) {
        if (record.length <= 0)
            return;
        var index = 0;
        SIE.each(grid.getColumnManager().columns, function (model) {
            if (model.dataIndex === 'WorkStatus') {
                return false;
            }
            index++;
        });
        var now = new Date();
        for (var i = 0; i < record.length; i++) {
            var date = record[i];
            var status = date.getWorkStatus();
            if (status === 10 || status === 20) {
                var plantEnd;
                if (date.$className === "SIE.EMS.EarlierStage.Projects.ProjectKeyItemPlan") {
                    plantEnd = date.getPlanEnd();
                } else {
                    plantEnd = date.getPlantEnd();
                }
                if (plantEnd != null && now > plantEnd) {
                    grid.getCell(i, index).style.backgroundColor = '#FF0000';
                }
            }
        }
    },
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getData();
        if (entity) {
            view.mon(view.getData(), 'propertyChanged', me._onEntityPropertyChanged, me);
        }
    },

    /**
     * 属性变更事件
     * @param {any} e
     */
    _onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === "PlanStart") {
            var planstart = e.entity._ProjectKeyItem.data.PlanStart;
            if (planstart > e.value) {
                SIE.Msg.showInstantMessage("计划开始时间早于事项开始时间".t());
            }
        }

        if (e.property === "ActualStart" || e.property === "ActaulEnd") {
            var actualStart = e.entity.data.ActualStart;
            var actaulEnd = e.entity.data.ActaulEnd;
            if (actualStart != null && actaulEnd != null)
                if (actualStart > actaulEnd) {
                    if (e.property === "ActualStart") {
                        e.entity.setActualStart("");
                    }
                    if (e.property === "ActaulEnd") {
                        e.entity.setActaulEnd("");
                    }
                    SIE.Msg.showInstantMessage("实际开始时间不能晚于实际结束时间".t());
                }
        }
    }
});