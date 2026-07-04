Ext.define('SIE.Web.EMS.FixedAssets.Accounts.Scripts.ApprovalBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        me.view = view;
        me.showDetailTab(view);
        view.mon(view, 'currentChanged', me.currentChanged, me);
    },
    currentChanged: function (config) {
        var me = this;
        me.showDetailTab(me.view);
    },
    showDetailTab: function (view) {

        var equipChildView = view.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetDeviceBill');
        var sparePartChildView = view.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetSparePart');
        var fixtureChildView = view.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetFixtureBill');
        var resumeChildView = view.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetResume');

        var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
        var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
        var sparePartTab = sparePartChildView.getControl().ownerLayout.owner.tab;
        var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
        var resumeTab = resumeChildView.getControl().ownerLayout.owner.tab;

        var curEntity = view.getCurrent();
        if (curEntity) {

            if (curEntity.data.AssetsType == 5) {
                equipTab.show();
                sparePartTab.hide();
                fixtureTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
            }
            if (curEntity.data.AssetsType == 10) {
                equipTab.hide();
                sparePartTab.show();
                fixtureTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(sparePartTab.card.id));
            }
            if (curEntity.data.AssetsType == 15) {
                equipTab.hide();
                sparePartTab.hide();
                fixtureTab.show();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
            }
        }
        else {
            equipTab.hide();
            sparePartTab.hide();
            fixtureTab.hide();
            tabPanel.setActiveTab(tabPanel.items.keys.indexOf(resumeTab.card.id));
        }
    }
});