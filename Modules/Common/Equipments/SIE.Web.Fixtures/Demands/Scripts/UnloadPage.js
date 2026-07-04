Ext.define('SIE.Web.Fixtures.Demands.Scripts.UnloadPage', {
    extend: 'SIE.Page',
    beforeLoad: function (args) {
        this.isCustomize = true;
    },

    /**
     * onLoad 加载控件
     * @method onLoad
     */
    onLoad: function () {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.No = params.No;
            me.WorkShopName = params.WorkShopName;
            me.ResourceName = params.ResourceName;
            me.WorkOrderNo = params.WorkOrderNo;
            me.ProductCode = params.ProductCode;
            me.FixDemandId = params.FixDemandId;
            me.token = params.token;
        }

        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.Fixtures.FixtureDemands.ViewModels.FixtureDemandViewModel',
            module: 'SIE.Fixtures.FixtureDemands.FixtureDemand,SIE.Fixtures',
            isDetail: true,
            isAggt: true,
            ignoreQuery: true,
            async: false,
            callback: function (res) {
                meta = res;
                me.removeFixtureUnloadLayout(meta);
                //生成主次智能转产视图控件
                me.generateMainControl(me, meta);
                //加载工治具需求清单相关数据
                me.loadDemandRelateInfo(me);

                me.ui.getControl().setRegion('center');
                me.ui.getControl().items.items[0].setHeight('15%');
                me.ui.getControl().items.items[1].setHeight('35%');
                //创建出库明细面板
                me.form = me.createPanel(me);
                //创建容器
                me.createContainer(me);
            }
        });
    },

    /**
     * removeFixtureUnloadLayout 拆分出库明细页签控件
     * @method removeFixtureUnloadLayout
     * @param {meta} meta 元数据
     */
    removeFixtureUnloadLayout: function (meta) {
        var me = this;
        //出库明细
        me.fixtureUnloadLayout = Ext.Array.findBy(meta.children, function (item) {
            if (item.associatedProperty == 'SIE.Fixtures.FixtureDemands.ViewModels.FixtureUnloadViewModel' && item.mainBlock.viewGroup == 'ListView') { return true; }
        });

        //移除出库明细页签
        if (me.fixtureUnloadLayout) {
            var posIndex = meta.children.indexOf(me.fixtureUnloadLayout);
            meta.children.splice(posIndex, 1);
        }
    },

    /**
     * generateMainControl 生成主次智能转产视图控件
     * @method generateMainControl
     * @param {me} me 当前页面
     */
    generateMainControl: function (me, meta) {
        var ui = SIE.AutoUI.generateAggtControl(meta);
        var uiBottom = SIE.AutoUI.generateAggtControl(me.fixtureUnloadLayout);
        uiBottom.getView()._setParent(ui.getView());
        uiBottom.getView().childLayoutType = 1;
        uiBottom.getView()._childProperty = me.fixtureUnloadLayout.childProperty;
        me.ui = ui;
        me.ui.getView().uiBottom = uiBottom;
        me.uiBottom = uiBottom;

        me.detailView = me.ui.getView().findChild('SIE.Fixtures.FixtureDemands.FixtureDemandDetail');
        if (me.detailView) {
            var detailControl = me.detailView.getControl();
            //绑定点击事件
            detailControl.mon(detailControl, 'cellclick', me.onControlCellClick, me);
        }
    },

    /**
     * loadDemandRelateInfo 加载工治具需求清单相关数据
     * @method loadDemandRelateInfo
     * @param {me} me 当前页面
     */
    loadDemandRelateInfo: function (me) {
        var entity = Ext.create(me.ui.getView().model);
        entity.setNo(me.No);
        entity.setWorkShopName(me.WorkShopName);
        entity.setResourceName(me.ResourceName);
        entity.setWorkOrderNo(me.WorkOrderNo);
        entity.setProductCode(me.ProductCode);
        entity.setId(me.FixDemandId);
        me.ui.getView().setData(entity);
        me.ui.getView().getData().dirty = false;
        me.ui.getView().getCurrent().phantom = false;
        var entity = me.ui.getView().getCurrent();
        me.ui.getView().mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.ui.getView());
        me.unloadView = me.ui.getView().findChild('SIE.Fixtures.FixtureDemands.ViewModels.FixtureUnloadViewModel');
        if (me.detailView)
            me.detailView.loadData();
        if (me.unloadView)
            me.unloadView.loadData();
    },

    /**
    * createPanel 创建面板
    * @method createPanel
    * @param {me} me 当前页面
    */
    createPanel: function (me) {
        return Ext.create('Ext.form.Panel', {
            layout: {
                type: 'vbox',
                pack: 'start',
                align: 'stretch'
            },
            items: [{
                flex: 2,
                layout: 'fit',
                items: me.ui.getControl()
            }, {
                xtype: 'splitter'   // A splitter between the two child items
            }, {
                flex: 1,
                layout: 'fit',
                title: '出库明细'.t(),
                items: me.uiBottom.getControl()
            }]
        });
    },

    /**
    * createContainer 创建容器
    * @method createContainer
    * @param {me} me 当前页面
    */
    createContainer: function (me) {
        return Ext.create('Ext.container.Viewport', {
            layout: {
                type: 'border'
            },
            border: 0,
            defaults: {
                layout: 'fit'
            },
            view: me.ui.getView(),
            items: {
                region: 'center',
                items: me.form
            },
            renderTo: Ext.getBody()
        });
    },

    /**
    * 选中数据变更处理事件
    * @param {} selection
    * @returns {}
    */
    onControlCellClick: function (g, row, col, record, tr, rowindex) {
        var me = this;
        if (!record.data)
            return;
        var data = record.data;
        //if (me.oldRowIndex == rowindex)
        //    return;
        //else
        //    me.oldRowIndex = rowindex;

        if (!me.detailView)
            return;

        var warehouseId = null;
        var current = me.ui.getView().getCurrent();
        if (current)
            warehouseId = current.getWarehouseId();

        var unloadList = [];
        if (me.unloadView) {
            me.unloadView.getData().getData().items.forEach(function (item) {
                unloadList.push(item.getData());
            });
        }
        //查询
        SIE.invokeDataQuery({
            type: "SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer",
            method: "GetUnloadStockVMs",
            params: [warehouseId, data, unloadList],
            async: false,
            token: me.detailView.token,
            callback: function (res) {
                if (res.Success) {
                    var unloadStockVMs = res.Result;
                    if (me.detailView) {
                        SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun.loadStockInfo(me.ui.getView(), unloadStockVMs);
                    }
                }
            }
        });
    },

    /**
        * onEntityPropertyChanged 属性变更事件
        * @param {*} e 参数
        */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            var demand = e.entity.data;
            me.demand = demand;
            if (e.property === 'WarehouseId') {
                var data = {};
                var detailView = me.findChild('SIE.Fixtures.FixtureDemands.FixtureDemandDetail');
                if (!detailView)
                    return;
                if (detailView) {
                    var current = detailView.getCurrent();
                    if (!current)
                        return;
                    if (current)
                        data.DemandDetail = current.getData();
                }

                data.RestUnloadVMList = [];
                var stockView = me.findChild('SIE.Fixtures.FixtureDemands.ViewModels.UnloadStockViewModel');
                if (stockView) {
                    stockView.getData().data.items.forEach(function (item) {
                        data.RestUnloadVMList.push(item.getData());
                    });
                }

                data.WarehouseId = me.demand.WarehouseId;

                var indata = {};
                indata.Data = Ext.encode(data);

                SIE.invokeDataQuery({
                    type: "SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer",
                    method: "GetUnloadInfo",
                    params: [data],
                    async: false,
                    token: me.token,
                    callback: function (res) {
                        if (res.Success) {
                            var unloadInfo = res.Result;
                            if (unloadInfo.ErrMsg !== '') {
                                SIE.Msg.showError(unloadInfo.ErrMsg);
                                return;
                            }
                            else {

                                SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun.loadStockInfo(me, unloadInfo.UnloadStockVMList);
                            }
                        }
                    },
                });
            }
        }
    },
});