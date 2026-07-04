Ext.define('SIE.Web.DIST.GoodsIssueCommonFun', {
    statics: {
        //加载子列表数据
        initChildrenData: function (mainView, goodId) {
            var childView = mainView._children.first(function (p) { return p.model === "SIE.DIST.DistributionBill"; });
            var childProValView = mainView._children.first(function (p) { return p.model === "SIE.Web.Items.ViewModels.PropertyValueViewModel"; });
            SIE.invokeDataQuery({
                method: 'GetDistributionInfo',
                params: [goodId],
                action: 'queryer',
                type: 'SIE.Web.DIST.GoodsIssueDataQueryer',
                token: mainView.token,
                success: function (res) {
                    var info = res.Result;
                    if (info) {
                        if (childView) {
                            var child = childView.getControl();
                            var store = child.getStore();
                            store.setData(info.BillList);
                            child.setStore(store);
                        }

                        if (childProValView) {
                            var childProVal = childProValView.getControl();
                            var store = childProVal.getStore();
                            store.setData(info.PropertyValueVMs);
                            childProVal.setStore(store);
                        }
                    }
                }
            });
        },
        //初始化数据
        initDistributionData: function (entity) {
            entity.setBarcode("");
            entity.setQty(0);
            entity.setTurnoverBox("");
            entity.data.ItemLabelList.removeAll();
            Ext.ComponentQuery.query('textfield[name=GoodsIssueSn]')[0].focus(true, 100);
        },
        //设置提示栏的字体颜色
        setTipsCtlColor: function (color) {
            var tipsComp = Ext.ComponentQuery.query("textfield[name=GoodsIssueTips]")[0];
            tipsComp.setFieldStyle("color:" + color);
        }
    }
});