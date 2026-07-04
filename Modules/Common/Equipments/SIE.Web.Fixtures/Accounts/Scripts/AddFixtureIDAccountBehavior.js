Ext.define('SIE.Web.Fixtures.Accounts.Scripts.AddFixtureIDAccountBehavior', {
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var store = new view._model();
        store.setTotalQty(1);
        store.setProprietorship(5);
        view.setData(store);
        SIE.invokeDataQuery({
            type: "SIE.Web.Fixtures.Accounts.DataQuery.AccountDataQueryer",
            method: "GetFixtureIDAccountNo",
            params: [],
            async: false,
            token: view.token,
            callback: function (res) {
                store.setCode(res.Result);
            },
        });
        view.mon(store, 'propertyChanged', me._onEntityPropertyChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    _onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === "FixtureEncodeId") {
            var data = e.entity;
            e.entity.setWarehouseId(null);
            e.entity.setWarehouseName("");
            e.entity.setWarehouseId_Display("");
            e.entity.setLocationId(null);
            e.entity.setLocationName("");
            e.entity.setLocationId_Display("");
            e.entity.setAccountState(null);
            if (e.value != null) {
                SIE.invokeDataQuery({
                    method: 'GetIdAccountState',
                    params: [e.value],
                    action: 'queryer',
                    type: 'SIE.Web.Fixtures.Accounts.DataQuery.AccountDataQueryer',
                    token: me.token,
                    success: function (res) {
                        var data = res.Result;
                        e.entity.setAccountState(data);
                    }
                })
            }
        } else if (e.property === "FixtureStorageLocationId") {
            if (e.value != null) {
                SIE.invokeDataQuery({
                    method: 'IsExistAccountLocation',
                    params: [e.value],
                    action: 'queryer',
                    type: 'SIE.Web.Fixtures.Accounts.DataQuery.AccountDataQueryer',
                    token: me.token,
                    success: function (res) {
                        if (res.Result != "" && res.Result != null)
                            SIE.Msg.showInstantMessage(Ext.String.format("当前所选库位已被[{0}]工治具关联占用，请确定是否继续关联此库位。".L10N(),res.Result));
                    }
                })
            }
        }
    }
});