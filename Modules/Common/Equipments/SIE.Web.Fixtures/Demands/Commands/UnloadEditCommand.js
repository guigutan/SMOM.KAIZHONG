SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.UnloadEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit" },

    /**
     * @override 是否可执行
     * @param {any} view
     */
    canExecute: function (view) {
        var entity = view.getCurrent();
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 1)
            return false;
        if (entity != null && entity.data) {
            return entity.data.IsOld == 0;
        }
        return false;
    },

    /**
    * @override 正在编辑中
    * @param {entity} entity
    */
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },

    /**
    * @onEntityPropertyChanged 属性变更事件
    * @param {e} e
    */
    onEntityPropertyChanged: function (e) {
        var me = this;
        me.view = this.view;
        if (e.property.length > 0) {
            var editData = e.entity.data;
            var data = me.getUiData(this.view, editData);
            if (e.property === 'UnloadQty') {
                SIE.invokeDataQuery({
                    type: "SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer",
                    method: "ValidateUnloadQty",
                    params: [data],
                    async: false,
                    token: this.view.token,
                    callback: function (res) {
                        if (res.Success) {
                            var errMsg = res.Result;
                            if (errMsg !== '') {
                                SIE.Msg.showError(errMsg);
                                return;
                            }
                            else {                                
                                me.updateUnloadStockDatas(data, me.view);
                            }
                        }
                    },
                });
            }                
        }
    },

    /**
    * @getUiData 获取界面数据
    * @param {view} view
    * @returns {}
    */
    getUiData: function (view, editData) {
        var data = {};
        data.RestUnloadVMList = [];
        view.getData().data.items.forEach(function (item) {
            data.RestUnloadVMList.push(item.getData());
        });

        var parentData = view.getParent().getData().data;
        if (parentData) {
            data.WarehouseId = parentData.WarehouseId;
        }

        var detailView = view.getParent().findChild('SIE.Fixtures.FixtureDemands.FixtureDemandDetail');
        if (detailView) {
            data.DemandDetail = detailView.getCurrent().getData();
        }

        var parentData = view.getParent().getData().data;
        if (parentData) {
            data.DemandId = parentData.Id;
        }

        data.FixtureUnloadVM = editData;

        return data;
    },

    /**
     * updateUnloadStockDatas 绑定旧配件更换记录值
     * @param {} data 当前编辑实体数据
     * @param {} view 当前编辑实体
     * @param {} token
     * @returns {}
     */
    updateUnloadStockDatas: function (data, view) {
        SIE.invokeDataQuery({
            type: "SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer",
            method: "GetUnloadInfo",
            params: [data],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Success) {
                    var unloadInfo = res.Result;
                    if (unloadInfo.ErrMsg !== '') {
                        SIE.Msg.showError(unloadInfo.ErrMsg);
                        return;
                    }
                    else {

                        SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun.loadStockInfo(view.getParent(), unloadInfo.UnloadStockVMList);

                        var unloadControl = view.getControl();
                        var store = unloadControl.getStore();
                        store.setData(unloadInfo.RestUnloadVMList);
                        unloadControl.setStore(store);
                    }
                }
            },
        });
    },
});