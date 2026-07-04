Ext.define('SIE.Web.Fixtures.Accounts.Scripts.FixtureAccountModelBehavior', {
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
     */
    onViewReady: function (view) {
        var tabpanel = Ext.getCmp("tabpanelCenter");
        tabpanel.items.items[0].tab.text = "ID类工治具台账".t();
        tabpanel.items.items[1].tab.text = "编码类工治具台账".t();
    }
});