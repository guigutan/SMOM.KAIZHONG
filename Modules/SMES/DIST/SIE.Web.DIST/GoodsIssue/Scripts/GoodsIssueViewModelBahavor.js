Ext.define('SIE.Web.DIST.GoodsIssueViewModelBahavor', {
    /**
     * view生命周期函数--view准备完成
     * @param {DetailView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        view.token = params.token;
        view.goodsIssueId = params.goodsIssueId;
        var model = new SIE.Web.DIST.GoodsIssueViewModel(); 
        model.ownerView = view;
        model.token = view.token;
        view.setData(model);
        me.initModel(view.token, model, view.goodsIssueId);
        SIE.Web.DIST.GoodsIssueCommonFun.initChildrenData(view, view.goodsIssueId);
        Ext.ComponentQuery.query('textfield[name=GoodsIssueSn]')[0].focus(true, 100);
        model.children = view.getChildren();
        model.markSaved();
    },

    initModel: function (token, model, goodsIssueId) {
        SIE.invokeDataQuery({
            type: "SIE.Web.DIST.GoodsIssueDataQueryer",
            method: "GetGoodsIssue",
            token: token,
            params: [goodsIssueId],
            success: function (res) {
                if (res.Result && res.Result.data.items.length > 0) {
                    var goodsIssue = res.Result.data.items[0].data;
                    model.setId(goodsIssue.Id);
                    model.setGoodsIssueId(goodsIssue.Id);
                    model.setWorkOrderNo(goodsIssue.WorkOrderNo);
                    model.setItemCode(goodsIssue.ItemCode);
                    model.setItemName(goodsIssue.ItemName);
                    model.setItemModel(goodsIssue.ItemModel);
                    model.setItemId(goodsIssue.ItemId);
                    model.setGoodsQty(goodsIssue.Qty);
                    model.setRemainQty(goodsIssue.RemainderQty);
                    model.setDistributionQty(goodsIssue.DistributionQty);
                    model.setDefectQty(goodsIssue.DefectQty);
                    model.setUnitName(goodsIssue.UnitName);
                    model.data.ItemLabelList = [];
                    if (goodsIssue.RemainderQty <= 0)
                        model.setTips(Ext.String.format("工单[{0}]物料数量已全部配送完成".t(), goodsIssue.WorkOrderNo));
                    else
                        model.setTips("请扫描配送周转箱".t());
                    model.setQty(0);
                }
            }
        });
    },
});