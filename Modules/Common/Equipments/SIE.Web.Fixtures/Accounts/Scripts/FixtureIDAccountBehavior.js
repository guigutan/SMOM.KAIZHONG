Ext.define('SIE.Web.Fixtures.Accounts.Scripts.FixtureIDAccountBehavior', {
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
     */
    onViewReady: function (view) {

        var me = this;
        me.view = view;
        me.showFeederTab(view);
        view.mon(view, 'currentChanged', me.currentChanged, me);
    },
    currentChanged: function (config) {
        var me = this;
        me.showFeederTab(me.view); 
    },
    showFeederTab: function (view)
    {
        var feederChildView = view.findChild('SIE.Fixtures.Fixtures.Accounts.FixtureAccountTool');
        if (feederChildView) {
            var tabPanel = feederChildView.getControl().ownerCt.ownerCt;
            var feederTab = feederChildView.getControl().ownerLayout.owner.tab;

            var curEntity = view.getCurrent();
            if (curEntity) {
                //行业属性为电子时则显示Feeder页签
                if (curEntity.data.IndustryProperties == 20) {
                    feederTab.show();
                }
                else {
                    feederTab.hide();
                    if (tabPanel.getActiveTab().title.indexOf('feeder') > -1)
                        tabPanel.setActiveTab(0);
                }
            }
            else {
                feederTab.hide();
                tabPanel.setActiveTab(0);
            }
        }
    }
});