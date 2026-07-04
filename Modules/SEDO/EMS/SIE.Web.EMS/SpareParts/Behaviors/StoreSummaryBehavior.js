Ext.define('SIE.Web.EMS.SpareParts.Behaviors.StoreSummaryBehavior', {
    /*
     * view生命周期函数--view生成前
     * @param {*} meta 实体视图元数据
     * @param {*} curEntity 当前操作实体(可空)
     */
    beforeCreate: function (meta, curEntity) {

        var render = {
            renderer: function (value, cell, record) {
                if (!Ext.isEmpty(record.getSafeStock())) {
                    if (record.getSumNumber() < record.getSafeStock()) {
                        cell.tdStyle = "border-right: 1px solid white; background: orange;";
                    }
                }
                return value;
            }
        };

        meta.gridConfig.columns.forEach(function (e) {
            if (e.dataIndex === 'SumNumber') {
                Object.assign(e, render);
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

        var lotChildView = view.findChild('SIE.EMS.SpareParts.StoreSummaryLot');
        var dtlChildView = view.findChild('SIE.EMS.SpareParts.StoreSummaryDetail');
        var tabPanel = lotChildView.getControl().ownerCt.ownerCt;
        var lotTab = lotChildView.getControl().ownerLayout.owner.tab;
        var dtlTab = dtlChildView.getControl().ownerLayout.owner.tab;

        var curEntity = view.getCurrent();
        if (curEntity) {
            if (curEntity.data.ControlMethod == 10) {

                lotTab.hide();
                dtlTab.hide();

                if (tabPanel.items.keys.indexOf(tabPanel.getActiveTab().id) == tabPanel.items.keys.indexOf(lotTab.card.id)
                    || tabPanel.items.keys.indexOf(tabPanel.getActiveTab().id) == tabPanel.items.keys.indexOf(dtlTab.card.id)) {
                    tabPanel.setActiveTab(0);
                }
            }
            else if (curEntity.data.ControlMethod == 20) {

                lotTab.show();
                dtlTab.hide();
                if (tabPanel.items.keys.indexOf(tabPanel.getActiveTab().id) == tabPanel.items.keys.indexOf(dtlTab.card.id))
                    tabPanel.setActiveTab(0);
            }
            else {

                lotTab.hide();
                dtlTab.show();
                if (tabPanel.items.keys.indexOf(tabPanel.getActiveTab().id) == tabPanel.items.keys.indexOf(lotTab.card.id))
                    tabPanel.setActiveTab(0);
            }
        }
        else {
            lotTab.hide();
            dtlTab.hide();
            tabPanel.setActiveTab(0);
        }
    }
});