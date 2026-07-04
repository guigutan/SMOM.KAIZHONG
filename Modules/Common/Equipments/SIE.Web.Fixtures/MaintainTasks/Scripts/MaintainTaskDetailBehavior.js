Ext.define('SIE.Web.Fixtures.MaintainTasks.MaintainTaskDetailBehavior', {
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        view.mon(store, 'propertyChanged', me._onTaskDetailChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    _onTaskDetailChanged: function (e) {
        var me = this;
        var entity = e.entity;
        if (e.property === "CheckValue" && entity.getCheckTag() == "0") {
            if (e.value == null)
                entity.setMaintainResult(null);
            if (entity.getMinValue() == null) {
                if (e.value <= entity.getMaxValue()) {
                    entity.setMaintainResult(1);
                } else {
                    entity.setMaintainResult(2);
                }
            }
            if (entity.getMaxValue() == null) {
                if (e.value >= entity.getMinValue()) {
                    entity.setMaintainResult(1);
                } else {
                    entity.setMaintainResult(2);
                }
            }
            if (entity.getMaxValue() != null && entity.getMinValue() != null) {
                if (entity.getMinValue() <= e.value && e.value <= entity.getMaxValue())
                    entity.setMaintainResult(1);
                else
                    entity.setMaintainResult(2);
            }
        }
        if (e.property === "MaintainResult" || e.property === "MaintainTaskId") {
            if (e.property === "MaintainResult" && e.value == null) {
                e.entity._MaintainTask.setPassQty(null);
                e.entity._MaintainTask.setNgQty(null);
            }
            var details = entity._MaintainTask._Details.data.items;
            var noMaintain = 0;
            var isHaveMaintain = 0;
            var hasPass = false;
            var hasNg = false;
            Ext.each(details, function (detail) {
                var result = detail.getMaintainResult();
                if (result == null || result == "null") {
                    noMaintain++;
                } else {
                    isHaveMaintain++;
                }
                if (result == 1) {
                    hasPass = true;
                }
                else if (result == 2) {
                    hasNg = true;
                }
            });

            e.entity._MaintainTask.setHasPass(hasPass);
            e.entity._MaintainTask.setHasNg(hasNg);

            if (isHaveMaintain == 0)//待保养
            {
                e.entity._MaintainTask.setState(5);
                e.entity._MaintainTask.setFinishDate(null);
            }
            if (noMaintain == 0)//保养完成
            {
                e.entity._MaintainTask.setState(15);
                e.entity._MaintainTask.setFinishDate(new Date());
            }
            if (isHaveMaintain > 0 && noMaintain > 0)//部分保养
            {
                e.entity._MaintainTask.setState(10);
                e.entity._MaintainTask.setFinishDate(null);
            }
        }
    }
});