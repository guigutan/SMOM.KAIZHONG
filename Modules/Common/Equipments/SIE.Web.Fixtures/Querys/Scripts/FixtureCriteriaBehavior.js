Ext.define('SIE.Web.Fixtures.Querys.Scripts.FixtureCriteriaBehavior',
    {
        /**
        * onViewReady 视图加载完成
        * @param {*} view 当前视图
        */
        onViewReady: function (view) {
            var me = this;
            var woComboId = view.getControl().items.items[3].id;
            var conditionControl = Ext.getCmp(woComboId);
            conditionControl.view = view;
            conditionControl.mon(conditionControl, 'change', this.change, conditionControl);
        },
        change: function (combo, newValue, oldValue, eOpts) {
            var me = this;
            var resData = me.up('container').SIEView.getCurrent();
            resData.setDeck(null);
            if (newValue) {
                SIE.invokeDataQuery({
                    type: "SIE.Web.Fixtures.Querys.DataQuery.FixtureQueryDataQueryer",
                    method: "GetWorkOrderDeck",
                    params: [newValue],
                    async: false,
                    token: me.view.token,
                    callback: function (res) {
                        if (res.Success) {
                            resData.setDeck(res.Result);
                        }
                    }
                });
            }            
        },
    });