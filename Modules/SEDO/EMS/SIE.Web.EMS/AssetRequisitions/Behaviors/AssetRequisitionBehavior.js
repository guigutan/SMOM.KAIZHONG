Ext.define('SIE.Web.EMS.AssetRequisitions.Behaviors.AssetRequisitionBehavior', {

    /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
    onCreated: function (view) {

        SIE.invokeDataQuery({
            method: 'GetApprovalConfigValue',
            params: [],
            async: false,
            action: 'queryer',
            type: 'SIE.Web.EMS.AssetRequisitions.DataQueryer.AssetRequisitionDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Success) {
                    var configValue = res.Result.data.items[0].data;
                    CRT.Context.PageContext.setContext("AssetRequisitionConfig", configValue);
                }
            }
        });
    },
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
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

        var equipChildView = view.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionFixture');
        var externalChildView = view.findChild('SIE.EMS.AssetRequisitions.AssetRequisition');
        var attachmentChildView = view.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionAttachment');

        var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
        var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
        var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
        var externalTab = externalChildView.getControl().ownerLayout.owner.tab;
        var attachmentTab = attachmentChildView.getControl().ownerLayout.owner.tab;

        var approvalChildView = view.findChild('SIE.Equipments.WorkFlows.WorkFlowRecord');
        var approvalTab = approvalChildView.getControl().ownerLayout.owner.tab;

        var configValue = CRT.Context.PageContext.getContext('AssetRequisitionConfig');
        if (!configValue.EnableAudit) {
            approvalTab.hide();
        }

        var curEntity = view.getCurrent();
        if (curEntity) {

            if (curEntity.data.AssetObject == 10) {
                equipTab.show();
                fixtureTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
            }
            if (curEntity.data.AssetObject == 20) {
                equipTab.hide();
                fixtureTab.show();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
            }

            if (curEntity.data.External) {
                externalTab.show();
            }
            else {
                externalTab.hide();
            }
        }
        else {
            equipTab.hide();
            fixtureTab.hide();
            externalTab.hide();
            tabPanel.setActiveTab(tabPanel.items.keys.indexOf(attachmentTab.card.id));
        }
    }
});