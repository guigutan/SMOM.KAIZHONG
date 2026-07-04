SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.BatchAnalysisSCDetailCommand', {
    meta: { text: "批量分析", group: "edit", iconCls: "icon-EditAdd icon-blue" },
    canExecute: function (view) {
        var selectCurrents = view.getSelection();
        if (selectCurrents == null || selectCurrents.length == 0) {
            return false;
        }
        if (selectCurrents.any(function (item) {
            return item.getState() != 40 || item.getLesStockCountDetailResult() != 10;
        })) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var param = me.view.getCurrent().data;
        SIE.AutoUI.getMeta({
            model: 'SIE.Web.LES.LesStockCounts.ViewModels.LesStockCountDetailAnalysisViewModel',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var selIds = me.view.getSelectionIds();
                var entity = new detailView._model();
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "批量分析".t(),
                    width: 300,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.AnalysisResult == null) {
                                SIE.Msg.showError("分析结果不能为空!".t());
                                return false;
                            }

                            var storeData = me.view.getControl().getStore().data;
                            storeData.items.where(function (p) { return selIds.any(function (a) { return a == p.data.Id; }) }).forEach(function (p) {
                                p.setAnalysisResult(indata.AnalysisResult);
                                p.setResultDesc(indata.ResultDesc);
                            });
                            me.view._parent.syncCmdState(me.view._parent, true);
                        }
                    }
                });
            },
        });
    }
});