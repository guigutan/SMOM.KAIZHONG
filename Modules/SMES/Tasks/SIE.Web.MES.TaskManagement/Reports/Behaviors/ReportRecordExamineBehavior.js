Ext.define("SIE.Web.MES.TaskManagement.Reports.Behaviors.ReportRecordExamineBehavior", {
    onViewReady: function (view) {
        var entity = Ext.create(view.model);
        var rowData = view.getData();
        if (rowData.length > 0) {
            entity.setNo("合计");
            entity.setQualifiedQty(rowData.reduce(
                (accumulator, currentValue) => accumulator + currentValue.getQualifiedQty(),
                0,
            ));
            rowData.push(entity);
        }

    },
    /**
 * view生命周期函数--数据加载后
 * @param {any} view 逻辑视图
 */
    onDataLoaded: function (view) {
        if (view.getRelations().length > 0) {
            var criter = view.getRelations().first(function (p) { return p._target.isQueryView; });
            if (criter) {
                var storeData = view.getControl().getStore().data;
                var flag = storeData.items.any(function (p) { return p.data.Id == 0; });
                if (storeData.items.length > 0 && !flag) {
                    var model = SIE.getModel(view.model);
                    var entity = new model();
                    entity.setNo("合计");
                    entity.setId(0);



                    //任务数
                    //var dispatchQty = storeData.items.sum(function (p) { return p.data.DispatchQty; });
                    //entity.setDispatchQty(Math.floor(dispatchQty * 1000) / 1000);

                    //报工数
                    var reportQty = storeData.items.sum(function (p) { return p.data.ReportQty; });
                    entity.setReportQty(Math.floor(reportQty * 1000) / 1000);

                    //记录合格数
                    var recordOkQty = storeData.items.sum(function (p) { return p.data.RecordOkQty; });
                    entity.setRecordOkQty(Math.floor(recordOkQty * 1000) / 1000);

                    //记录不合格数
                    var recordNgQty = storeData.items.sum(function (p) { return p.data.RecordNgQty; });
                    entity.setRecordNgQty(Math.floor(recordNgQty * 1000) / 1000);

                    //返工数
                    var reworkQty = storeData.items.sum(function (p) { return p.data.ReworkQty; });
                    entity.setReworkQty(Math.floor(reworkQty * 1000) / 1000);

                    var suspectQty = storeData.items.sum(function (p) { return p.data.SuspectQty; });
                    entity.setSuspectQty(Math.floor(suspectQty * 1000) / 1000); 

                    storeData.add(entity);
                    entity.markSaved();
                }
            }
        }
    }
});