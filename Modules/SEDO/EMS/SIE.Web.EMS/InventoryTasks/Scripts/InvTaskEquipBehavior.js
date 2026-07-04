Ext.define('SIE.Web.EMS.InventoryTasks.InvTaskEquipBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var headers = view.gridConfig.columns.filter(function (p) {
            return p.header == "实盘".t() || p.header == "原始".t()
        });
        headers.forEach(function (p) {
            p.columns.forEach(function (item, idx, arr) {
                if (item.readonlyLambda && item.readonlyLambda != "") {
                    let func = view.getFunc(item.readonlyLambda);
                    view.addProChgHandler({ pro: item.dataIndex, effect: 'setReadOnly', lambda: func });
                }
                if (item.cascade && item.cascade.length > 0) {
                    item.cascade.forEach(function (e, i, arrc) {
                        let func = view.getFunc(e);
                        view.addProChgHandler({ pro: item.dataIndex, effect: 'cascade', lambda: func });
                    });
                }
            });
        });
    },
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        //有更新且选择【盘亏】时，清空【实盘XX】字段值
        if ((e.property === 'FirstInventoryResult' || e.property === 'SecondInventoryResult') && e.value === 40) {
            e.entity.setRealManageDeptId(null);
            e.entity.setRealUseDeptId(null);
            e.entity.setAccountUseState(null);
            e.entity.setAccountState(null);
            e.entity.setUserId(null);
            e.entity.setRealWorkShopId(null);
            e.entity.setRealResourceId(null);
            e.entity.setRealWarehouseId(null);
            e.entity.setStorageLocationId(null);
            e.entity.setRealLocation("");
        }
        //有更新且选择【正常、信息变动】时，【实盘XX】字段带出为【原始XX】字段值
        if ((e.property === 'FirstInventoryResult' || e.property === 'SecondInventoryResult') && (e.value === 10 || e.value === 20)) {
            var cur = e.entity.data;
            e.entity.setRealManageDeptId_Display(cur.OldManageDept);
            e.entity.setRealManageDeptId(cur.OldManageDeptId);
            e.entity.setRealUseDeptId_Display(cur.OldUseDeptName);
            e.entity.setRealUseDeptId(cur.OldUseDeptId);
            e.entity.setAccountUseState(cur.OldAccountUseState);
            e.entity.setAccountState(cur.OldAccountState);
            e.entity.setUserId_Display(cur.OldUserName);
            e.entity.setUserId(cur.OldUserId);
            e.entity.setRealWorkShopId_Display(cur.OldWorkShopName);
            e.entity.setRealWorkShopId(cur.OldWorkShopId);
            e.entity.setRealResourceId_Display(cur.OldResourceName);
            e.entity.setRealResourceId(cur.OldResourceId);
            e.entity.setRealWarehouseId_Display(cur.OldWarehouseCode);
            e.entity.setRealWarehouseId(cur.OldWarehouseId);
            e.entity.setStorageLocationId_Display(cur.OldStorageLocationCode);
            e.entity.setStorageLocationId(cur.OldStorageLocationId);
            e.entity.setRealLocation(cur.OldLocation);
        }
        if (e.property === 'FirstInventoryResult') {
            if (e.entity.data.InventoryAssetSource === 10 && e.value === 30) {
                e.entity.setFirstInventoryResult(null);
                SIE.Msg.showMessage("来源为【账内资产】,不能选择盘盈".t());
            }
        }
        if (e.property === 'SecondInventoryResult') {
            if (e.entity.data.InventoryAssetSource === 10 && e.value === 30) {
                e.entity.setSecondInventoryResult(null);
                SIE.Msg.showMessage("来源为【账内资产】,不能选择盘盈".t());
            }
            if (e.entity.data.InventoryAssetSource === 20 && (e.value === 10 || e.value === 20)) {
                e.entity.setSecondInventoryResult(null);
                SIE.Msg.showMessage("来源为【盘盈新增】,只可选【盘盈、盘亏】".t());
            }
        }
    }
});