SIE.defineCommand('SIE.Web.DIST.SubmitCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        var current = view._current;
        return current && current.data.TurnoverBox;
    },

    execute: function (view) {
        var me = this;
        var modelData = view.getData();
        var curView = view;
        view.execute({
            data: modelData.data,
            success: function (res) {
                var entity = res.Result;
                if (entity.Error != "") {
                    modelData.setTips(entity.Error);
                    SIE.Web.DIST.GoodsIssueCommonFun.setTipsCtlColor("red");
                }
                else {
                    SIE.Web.DIST.GoodsIssueCommonFun.setTipsCtlColor("#008000");
                    modelData.setTips(entity.Tips);
                    modelData.setDistributionQty(entity.DistributionQty);
                    modelData.setRemainQty(entity.RemainQty);
                    SIE.Web.DIST.GoodsIssueCommonFun.initDistributionData(modelData);
                    //重新加载明细数据
                    SIE.Web.DIST.GoodsIssueCommonFun.initChildrenData(curView, modelData.data.GoodsIssueId);
                    modelData.markSaved();
                }
            }
        });
    }
});