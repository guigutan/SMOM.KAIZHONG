Ext.define('SIE.Web.EMS.Lubrications.Behaviors.LubricationBehavior', {
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
            if (model.dataIndex === 'LubricationStatus') {
                return false;
            }
            index++;
        });
        var now = new Date();
        //var nowMaxOne = new Date(now.setDate(now.getDate() + 1));  
        for (var i = 0; i < record.length; i++) {
            var date = record[i];
            var status = date.getLubricationStatus();
            var planDate = date.getPlanDate();
            var planDate2 = new Date(planDate.setDate(planDate.getDate() + 1));  
            if ((status === 10 || status === 20) && now > planDate2) {
                grid.getCell(i, index).style.backgroundColor = '#FF0000';
            }
        }
    },
    onShow: function (view) {
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            //获取查询视图
            var conditionView = view.getConditionView();
            //获取查询实体元数据
            var criteria = conditionView.getData();
            //赋值传递过来的维修单
            criteria.setLubricationNo(params.LubricationNo);
            //清空所有时间范围控件的开始结束时间
            var dateRangeCtls = conditionView.getControl().items.items.filter(function (e) { return e.xtype === "dateRange"; })
            if (dateRangeCtls.length > 0) {
                dateRangeCtls.forEach(function (ctl) {
                    ctl.setDataRangValue(null, null);
                });
            }
            //执行查询
            conditionView.tryExecuteQuery();
            params.LubricationNo = "";
        }
    }
});