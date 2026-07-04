SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.UnloadStockCommand', {
    meta: { text: "出库", group: "edit" },
    /**
     * canExecute 是否出库
     * @method canExecute
     * @param {view} view 当前视图
     */
    canExecute: function (view) {
        var curEntity = view.getCurrent();
        if (curEntity == null)
            return false;
        return true;
    },

    /**
    * execute 执行出库
    * @method execute
    * @param {view} view 当前视图
     * @param {source} source
    */
    execute: function (view, source) {
        var me = this;
        me.view = view;
        me.curEntity = view.getCurrent();
        var curData = me.curEntity.getData();
        SIE.AutoUI.getMeta({
            model: view.model,
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                me.detailView = SIE.AutoUI.createDetailView(mainBlock);
                me.curEntity.setUnloadQty(null);
                me.detailView.setData(me.curEntity);
                var ui = me.detailView.getControl();
                var win = SIE.Window.show({
                    title: "出库".t(),
                    width: '40%',
                    height: '35%',
                    layout: 'fit',
                    plain: true,
                    buttonAlign: 'right',
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            me.bindUnloadData(me);
                            var result = me.validateUnloadData(me.data);
                            if (!result)
                                return false;
                            me.unloadData(me);
                        }
                    }
                });
            }
        });
    },

    /**
    * bindUnloadData 执行出库
    * @method bindUnloadData
    * @param {me} me 当前页面
    */
    bindUnloadData: function (me) {
        var detailData = me.detailView.getCurrent().getData();
        me.data = {};
        me.data.UnloadStockVM = detailData;
        me.data.RestUnloadVMList = [];
        me.unloadView = me.view._parent.findChild('SIE.Fixtures.FixtureDemands.ViewModels.FixtureUnloadViewModel');
        if (me.unloadView) {
            me.unloadView.getData().getData().items.forEach(function (item) {
                me.data.RestUnloadVMList.push(item.getData());
            });
        }

        var parentData = me.view.getParent().getData().data;
        if (parentData) {
            me.data.WarehouseId = parentData.WarehouseId;
        }

        var oldDetailView = me.view.getParent().findChild('SIE.Fixtures.FixtureDemands.FixtureDemandDetail');
        if (oldDetailView) {
            me.data.DemandDetail = oldDetailView.getCurrent().getData();
        }
    },

    /**
    * validateUnloadData 验证出库数据是否合法
    * @method validateUnloadData
    * @param {me} me 当前页面
    */
    validateUnloadData: function (data) {
        if (data.UnloadStockVM.UnloadQty <= 0) {
            SIE.Msg.showInstantMessage('出库数量不能小于等于0，请重新填写！'.t());
            return false;
        }

        if (data.UnloadStockVM.UnloadQty > data.UnloadStockVM.Qty) {
            SIE.Msg.showInstantMessage('出库数量不能大于库存合格数，请重新填写！'.t());
            return false;
        }

        if (data.UnloadStockVM.UnloadQty > data.DemandDetail.DemandQty) {
            SIE.Msg.showInstantMessage('出库数量不能大于需求数量，请重新填写！'.t());
            return false;
        }

        return true;
    },

    /**
    * unloadData 执行出库
    * @method unloadData
    * @param {me} me 当前页面
    */
    unloadData: function (me) {
        var indata = {};
        indata.Data = Ext.encode(me.data);

        var commandInfo = {
            command: "SIE.Web.Fixtures.Demands.Commands.UnloadStockCommand",
            entityType: "SIE.Fixtures.FixtureDemands.ViewModels.UnloadStockViewModel",
            parentType: "SIE.Fixtures.FixtureDemands.FixtureDemand",
            moduleName: "工治具需求清单".t(),
            childModuleName: "",
            commandName: "出库".t(),
        }

        me.view.execute({
            data: indata,
            logInfo: commandInfo,
            success: function (res) {
                var unloadInfo = res.Result;
                if (unloadInfo.ErrMsg !== '') {
                    SIE.Msg.showError(unloadInfo.ErrMsg);
                    return;
                }
                else {
                    me.curEntity.setTurnoverToolCode(null);
                    if (me.unloadView) {
                        var unloadControl = me.unloadView.getControl();
                        var store = unloadControl.getStore();
                        store.setData(unloadInfo.RestUnloadVMList);
                        unloadControl.setStore(store);
                        me.unloadView.syncCmdState();
                    }

                    SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun.loadStockInfo(me.view.getParent(), unloadInfo.UnloadStockVMList);
                }
            }
        });
    },
});