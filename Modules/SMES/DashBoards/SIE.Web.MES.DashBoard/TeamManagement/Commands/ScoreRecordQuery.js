SIE.defineCommand('SIE.Web.MES.DashBoard.TeamManagement.Commands.ScoreRecordQuery', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },
    _view: null,
    execute: function (view) {
        var me = this;
        me._view = view;
        var record = view.getCurrent();
        delete record.data['CriteriaModuleKey'];
        delete record.data['CriteriaType'];
        delete record.data["CriteriaString"];
        var istrue = true;
        view.getControl().items.items.forEach(function (item) {
            if (!item.validate()) {
                istrue = false;
            }
        });
        SIE.invokeDataQuery({
            method: 'GetScoreRecordsVM',
            params: [record.data],
            action: 'queryer',
            type: 'SIE.Web.MES.DashBoard.TeamManagement.ScoreRecordDataQueryer',
            token: view.getToken(),
            success: function (res) {
                var mainView = me._view._relations[0]._target;
                var gridControl = mainView.GridControl;
                var chartControl = mainView.ChartControl;
                if (gridControl) {
                    gridControl.addColumns(me._view.getCurrent().data);
                    var resData = res.Result;
                     mainView.owner.initSeries(resData.chartEmp, resData.chartEmpId, resData.chartData, resData.chartMin, resData.chartMax);
                    gridControl.initStore(resData.gridData);
                }
            }
        });
    }
});