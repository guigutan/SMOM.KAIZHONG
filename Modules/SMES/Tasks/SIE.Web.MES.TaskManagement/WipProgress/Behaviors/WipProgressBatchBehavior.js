Ext.define('SIE.Web.MES.TaskManagement.WipProgress.Behaviors.WipProgressBatchBehavior', {
    /**
    * view生命周期函数--view生成前
    * @param {*} meta 实体视图元数据
    * @param {*} curEntity 当前操作实体(可空)
    */
    beforeCreate: function (meta, curEntity) {
        //debugger;
        if (meta && meta.gridConfig && meta.gridConfig.columns) {
            meta.gridConfig["features"] = [{ ftype: "summary", dock: 'bottom' }];
            var summaryFields = ['Qty', /*'InProcessQty', */'ReportQty', 'PreReportQty'];
            var columns = meta.gridConfig.columns;
            for (var i = 0; i < columns.length; i++) {
                if (columns[i]["dataIndex"] == 'BatchNo') {
                    columns[i]["summaryRenderer"] = function (value, summaryData, dataIndex) {
                        return "合计";
                    }
                }
                else if (summaryFields.includes(columns[i]["dataIndex"])) //汇总字段
                    columns[i]["summaryType"] = "sum";
            }
        }
    },
    /**
    * view生命周期函数--view聚合后
    * @param {*} view 生成的view
    */
    onViewReady: function (view) {
        var me = this;
        this.view = view;
        var entity = view.getCurrent();
        var queryView = me.view._relations[0];

        var params = CRT.Context.PageContext.getParams();
        if (params && queryView) {

            var queryData = queryView._target._current;
            queryData.setWorkOrderNo(params.WorkOrderNo);
            queryData.setProcessId(params.ProcessId);
            queryData.setProcessCode(params.ProcessCode);
            queryData.setProcessName(params.ProcessName);
            queryData.setBatchNo(params.BatchNo);
            queryData.setPreProcessCode(params.PreProcessCode);
            queryView._target.tryExecuteQuery();
        }

    }

});