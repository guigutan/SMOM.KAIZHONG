Ext.define('SIE.Web.EMS.AssetIssues.Behaviors.AssetIssueBehavior', {
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
            type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Success) {
                    var configValue = res.Result.data.items[0].data;
                    CRT.Context.PageContext.setContext("AssetIssueConfig", configValue);
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

        var equipChildView = view.findChild('SIE.EMS.AssetIssues.AssetIssueEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetIssues.AssetIssueFixture');
        var externalChildView = view.findChild('SIE.EMS.AssetIssues.AssetIssue');
        var wfRecordChildView = view.findChild('SIE.Equipments.WorkFlows.WorkFlowRecord');

        var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
        var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
        var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
        var externalTab = externalChildView.getControl().ownerLayout.owner.tab;

        var workFlowTab = wfRecordChildView.getControl().ownerLayout.owner.tab;

        var configValue = CRT.Context.PageContext.getContext('AssetIssueConfig');
        if (!configValue.EnableAudit) {
            workFlowTab.hide();
            workFlowTab = null;
        }

        var curEntity = view.getCurrent();
        if (curEntity) {

            equipChildView.getControl().up().up().up().setVisible(true);
            if (curEntity.data.AssetObject == 10) {
                equipTab.show();
                fixtureTab.hide();
                equipChildView.getControl().setVisible(true);
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
            }
            if (curEntity.data.AssetObject == 20) {
                equipTab.hide();
                fixtureTab.show();
                fixtureChildView.getControl().setVisible(true);
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
            }

            if (curEntity.data.External) {
                externalTab.show();
                externalChildView.getControl().setVisible(true);
            }
            else {
                externalTab.hide();
            }
        }
        else {

            equipTab.hide();
            fixtureTab.hide();
            externalTab.hide();
            equipChildView.getControl().setVisible(false);
            fixtureChildView.getControl().setVisible(false);
            externalChildView.getControl().setVisible(false);

            if (workFlowTab) {
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(workFlowTab.card.id));
            }
            else {
                equipChildView.getControl().up().up().up().setVisible(false);
            }
        }
    }
});