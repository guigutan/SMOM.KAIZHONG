/**
 * 任务统计报表布局 
 * @class SIE.Web.MES.TaskManagement.Reports.Scripts.ReportDispatchTaskLayout
 * @constructs
 */
Ext.define('SIE.Web.MES.TaskManagement.Reports.Scripts.ReportDispatchTaskLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'DispatchReportLayout',
    /**
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        regions.main.getView().getRelations()[0].getTarget().mainLayout = this;
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });
        //表格
        var directRateControl = this.createDispatchReport(this);
        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [{
                region: 'north',
                items: toolbar,
                border: false,
            }, {
                region: 'center',
                layout: 'border',
                xtype: 'panel',
                border: false,
                items: [directRateControl]
            }]
        });
    },

    /**
     * 加载任务统计报表
     * @method loadShopReportData
     * @param {SIE.Web.MES.DashBoard.Reports.ShopFPY.ShopReportViewModelCriteria} criteria 标签控件
     * @param {token} token 新激活子页签
     */
    loadDispatchTaskData: function (criteria, token) {
        var dispatchMe = this;
        _token = token;
        _shopName = "";
        _resourceName = "";
        SIE.invokeDataQuery({
            method: 'GetDispatchReport',
            params: [criteria],
            action: 'queryer',
            async: false,
            type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
            token: token,
            success: function (res) {
                if (res.Success) {
                    //shopShopRateInfos = res.Result;
                    dispatchMe.bindDispatchTaskReport(res.Result);
                }
            }
        });
    },

    /**
     * 绑定产线直通率信息
     * @method bindShopShopRateInfos
     * @param {List<SIE.Web.MES.DashBoard.Reports.ShopFPY.ShopionShopRateInfo>} shopShopRateInfos 新激活子页签
     */
    bindDispatchTaskReport: function (dispatchData) {
        var prodShopRateChart = Ext.getCmp('TaskGridId');
        var store = prodShopRateChart.getStore();
        store.setData(dispatchData);
        prodShopRateChart.setStore(store);

        //遍历产线直通率列表
    }
});
