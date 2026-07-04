Ext.define('SIE.Web.EMS.Purchases.SparePartAcceptances.SparePartAcceptBehavior', {
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        view.lotView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartAcceptances.SparePartAcceptanceLot"; });
        view.snView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartAcceptances.SparePartAcceptanceSn"; });
        view.mon(view, 'currentChanged', me.currentChanged, view);
    },
    currentChanged: function (config) {
        var me = this;
        var curEntity = me.getCurrent();
        if (!curEntity) {
            return;
        }
        if (me.lotView) {
            let tabPanel = me.lotView.getControl().ownerCt.ownerCt;
            if (tabPanel.getActiveTab().title.indexOf('批次'.t()) > -1 || tabPanel.getActiveTab().title.indexOf('序列'.t()) > -1)
                tabPanel.setActiveTab(0);
            if (curEntity.data.ControlMethod === 20) {
                me.lotView._control.ownerLayout.config.owner.tab.setVisible(true);
            } else {
                me.lotView._control.ownerLayout.config.owner.tab.setVisible(false);
            }
        }
        if (me.snView) {
            let tabPanel = me.snView.getControl().ownerCt.ownerCt;
            if (tabPanel.getActiveTab().title.indexOf('批次'.t()) > -1 || tabPanel.getActiveTab().title.indexOf('序列'.t()) > -1)
                tabPanel.setActiveTab(0);
            if (curEntity.data.ControlMethod === 30) {
                me.snView._control.ownerLayout.config.owner.tab.setVisible(true);
            }
            else {
                me.snView._control.ownerLayout.config.owner.tab.setVisible(false);
            }
        }
    }
});