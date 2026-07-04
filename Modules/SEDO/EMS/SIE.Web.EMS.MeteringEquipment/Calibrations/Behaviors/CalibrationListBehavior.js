Ext.define('SIE.Web.EMS.MeteringEquipment.Calibrations.Behaviors.CalibrationListBehavior',
    {
        onShow: function (view) {
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                //获取查询视图
                var conditionView = view.getConditionView();
                //获取查询实体元数据
                var criteria = conditionView.getData();
                //赋值传递过来的维修单
                criteria.setInspectionNo(params.InspectionNo);
                //清空所有时间范围控件的开始结束时间
                var dateRangeCtls = conditionView.getControl().items.items.filter(function (e) { return e.xtype === "dateRange"; })
                if (dateRangeCtls.length > 0) {
                    dateRangeCtls.forEach(function (ctl) {
                        ctl.setDataRangValue(null, null);
                    });
                }
                //执行查询
                conditionView.tryExecuteQuery();
                params.InspectionNo = "";
            }
        }
    });
