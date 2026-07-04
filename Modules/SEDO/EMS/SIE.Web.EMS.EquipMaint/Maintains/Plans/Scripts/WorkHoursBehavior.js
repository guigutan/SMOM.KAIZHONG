Ext.define('SIE.Web.EMS.EquipMaint.Maintains.Plans.Scripts.WorkHoursBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view 逻辑视图
     */
    onDataLoaded: function (view) {
        var me = this;
        var current = view.getData();
        view.mon(current, "propertyChanged", me.onPropertyChanged, view);
    },
    /**
     * onEntityPropertyChanged 属性变更事件
     * @param {*} e 参数
     */
    onPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            var entity = e.entity;
            if (e.property == 'BeginDay' || e.property == 'EndDay') {
                if (entity.getBeginDay() != null && entity.getEndDay() != null) {
                    if (entity.getBeginDay() > entity.getEndDay()) {
                        entity.setEndDay();
                        SIE.MessageBox.showMessage("保养结束时间不能小于保养开始时间".t());
                        return;
                    }
                    SIE.invokeDataQuery({
                        method: 'CalculateDate',
                        params: [entity.getBeginDay(), entity.getEndDay()],
                        action: 'queryer',
                        async: false,
                        type: 'SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer',
                        token: this.token,
                        success: function (res) {
                            entity.setWorkHours(res.Result);
                            var data = me.getData().data.items;
                            var sumWorkHours = 0;
                            for (var i = 0; i < data.length; i++) {
                                sumWorkHours += me.getData().data.items[i].getWorkHours();
                            }
                            me._parent.getData().setSumWorkHours(sumWorkHours);
                        }
                    })
                }
            }
        }
    }
});