Ext.define('SIE.Web.Fixtures.Accounts.Scripts.FixtureAccountModelLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'FixtureAccountModelLayout',
    _isRunning: false,
    _token: null,
    _criteria: null,

    /**
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;        
        this._token = regions.main._view.token;
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();

        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });

        var fixtureIDAccountListView = me.createFixtureIDAccountListView();
        var fixtureCodeAccountListView = me.createFixtureCodeAccountListView(me);
        
        this._fixtureIDAccountListView = fixtureIDAccountListView;
        this._fixtureCodeAccountListView = fixtureCodeAccountListView;

        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [{
                region: 'north',
                items: toolbar,
                border: false,
            }, {
                region: 'center',
                layout: 'vbox',
                xtype: 'panel',
                id: "fixtureAccountMainPanel",
                items: [
                    {
                        xtype: "tabpanel",
                        id: "tabpanelCenter",
                        height: 240,
                        region: "north",
                        layout: 'fit',
                        width: "100%",
                        flex: 1,
                        items: [fixtureIDAccountListView.getControl() , fixtureCodeAccountListView.getControl()]
                    }
                ]
            }]
        });
    },

    /**创建ID类工治具台账 */
    createFixtureIDAccountListView: function () {
        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.Fixtures.Fixtures.Accounts.FixtureIDAccount',
            token: this._token,
            module: "SIE.Fixtures.Fixtures.Accounts.FixtureAccountModel,SIE.Fixtures",
            ignoreCommands: false,
            isDetail: false,
            ignoreQuery: false,
            async: false,
            callback: function (res) {
                meta = res;
            }
        });
        
        return SIE.AutoUI.generateAggtControl(meta);
    },
    /**创建编码类工治具台账 */
    createFixtureCodeAccountListView: function () {
        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.Fixtures.Fixtures.Accounts.FixtureCodeAccount',
            token: this._token,
            module: "SIE.Fixtures.Fixtures.Accounts.FixtureAccountModel,SIE.Fixtures",
            ignoreCommands: false,
            isDetail: false,
            ignoreQuery: false,
            async: false,
            callback: function (res) {
                meta = res;
            }
        });

        return SIE.AutoUI.generateAggtControl(meta);
    }
});