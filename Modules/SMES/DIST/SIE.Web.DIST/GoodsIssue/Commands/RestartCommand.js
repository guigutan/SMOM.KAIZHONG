SIE.defineCommand('SIE.Web.DIST.RestartCommand', {
    meta: { text: "重新开始", group: "edit", iconCls: "icon-Reload icon-blue" },
    execute: function (view) {      
        var modelData = view.getData();      
        SIE.Web.DIST.GoodsIssueCommonFun.initDistributionData(modelData);
        SIE.Web.DIST.GoodsIssueCommonFun.initChildrenData(view, modelData.data.GoodsIssueId);
        SIE.Web.DIST.GoodsIssueCommonFun.setTipsCtlColor("#008000");        
        modelData.setTips("请扫描配送周转箱".t());
    }
});