Ext.define('SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun', {
    statics: {
        /**         
     * onEntityPropertyChanged 属性变更事件
     * @param {} e 参数
     * @returns {}
     */
        onEntityPropertyChanged: function (e) {
            var me = this;
            if (e.property.length > 0) {
                var detail = e.entity;
                var data = e.entity.data;
                me.woId = this.view.getParent().getCurrent().getWorkOrderId();
                me.processSegmentId = this.view.getParent().getCurrent().getProcessSegmentId();
                me.token = this.view.token;
                if (e.property === 'FixtureEncodeId')
                    SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun.bindDeck(data, detail, me);
            }
        },

        /**
         * bindProcessSurface 绑定工艺面
         * @param {} data 当前编辑实体数据
         * @param {} detail 当前编辑实体
         * @param {} me 当前视图
         * @returns {}
         */
        bindDeck: function (data, detail, me) {
            SIE.invokeDataQuery({
                type: "SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer",
                method: "GetDeckInfo",
                params: [me.woId, data.FixtureEncodeId],
                async: false,
                token: me.token,
                callback: function (res) {
                    if (res.Success) {
                        var deckInfo = res.Result;
                        detail.setParentProcessSegmentId(me.processSegmentId);
                        if (deckInfo.ErrMsg === '') {
                            detail.setProcessSurface(deckInfo.Deck);
                            if (detail.data.ModelName === '') {
                                detail.setFixtureModelId(deckInfo.FixtureModelId);
                                detail.setFixtureModelId_Display(deckInfo.ModelCode);
                                detail.setModelCode(deckInfo.ModelCode);
                                detail.setModelName(deckInfo.ModelName);
                                detail.setFixtureTypeId(deckInfo.FixtureTypeId);
                                detail.setFixtureTypeId_Display(deckInfo.FixtureTypeCode);
                               

                            }
                        }
                        else {
                            detail.setProcessSurface("");
                            SIE.Msg.showError(deckInfo.ErrMsg);
                            return;
                        }
                    }
                },
            });
        },

        loadStockInfo: function (view, unloadStockVMList) {
            var stockView = view.findChild('SIE.Fixtures.FixtureDemands.ViewModels.UnloadStockViewModel');
            if (stockView) {
                var stockControl = stockView.getControl();
                var store = stockControl.getStore();
                store.setData(unloadStockVMList);
                stockControl.setStore(store);
            }
        },
    }
});