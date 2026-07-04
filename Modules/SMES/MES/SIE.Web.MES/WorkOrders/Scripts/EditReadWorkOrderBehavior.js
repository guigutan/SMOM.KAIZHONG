SIE.defineCommand('SIE.Web.MES.WorkOrders.EditReadWorkOrderBehavior', {
    extend: 'SIE.Web.MES.WorkOrders.BaseWorkOrderDetailBehavior',

    /**
     * 初始化工单数据
     * @param {any} view
     */
    initWorkOrderData: function (view, workOrder) {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        if (params.action === 2) {                 //0新增、1复制新增、2修改、3查看
            view.mon(workOrder, 'propertyChanged', SIE.Web.MES.WoCommonFun.WorkOrderPropertyChanged, view);
            me.getProductIsBatch(workOrder, view);
            workOrder.loaded = true;
        }
        me.initTemplate(view);
        me.initLoadevent(view);
        me.mainView.getCurrent().markSaved();
    },

    /**
     * 获取产品批次信息
     * @param {any} woentity
     * @param {any} detailView
     */
    getProductIsBatch: function (woentity, detailView) {
        SIE.invokeDataQuery({
            method: 'IsSingleProduct',
            params: [woentity.data.ProductId],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            token: detailView.token,
            success: function (res) {
                var batchChild = detailView._children.first(function (p) { return p.model == 'SIE.Core.WorkOrders.WoWipBatch'; });
                if (!batchChild) return;
                if (woentity.data.CreateBy == null) {
                    if (batchChild._control && batchChild._control.items && batchChild._control.items.items.length > 0) {
                        //ar inputId = batchChild._control.items.items[0].inputId;
                        //document.getElementById(inputId).value = 1;
                        batchChild._control.items.items[0].value = 1;
                    }
                    batchChild.getData().setQty(1);
                }
                if (batchChild._control && batchChild._control.items && batchChild._control.items.items.length > 0)
                    batchChild._control.items.items[0].setDisabled(res.Result);
            }
        });
    },

    /**
     * 给主视图绑定附件子的类型，并设置默认数据（框架只有在点击页签的时候才会给主视图绑定附加的子）
     * @param targetView 工单视图             
     */
    initTemplate: function (view) {
        var me = this;
        var batModel = "SIE.Core.Items.LabelPrintTemplate";
        var store = SIE.data.Utils.createStore({
            model: batModel,
            storeConfig: {
                proxy: Ext.clone(SIE.getModel(batModel).proxyConfig)
            },
            remoteSort: true
        });
        var tempView = me.getChildView(batModel);
        view._current[tempView.getAssociateKey()] = store;
        tempView.setData(store);
    },

    /**
     * 注册子页签的数据加载后事件
     * @param view 工单视图
     */
    initLoadevent: function (view) {
        var me = this;
        var processBomView = me.getChildView('SIE.MES.WorkOrders.WorkOrderProcessBom');
        if (processBomView._children.length > 0) {
            view.mon(processBomView._children[0].getControl(), 'storechange', me.initTabDataBySelect, me);
        }
        
        var bomView = me.getChildView('SIE.MES.WorkOrders.WorkOrderBom');
        if (bomView._children.length > 0) {
            view.mon(bomView._children[0].getControl(), 'storechange', me.initBomTabDataBySelect, me);
        }

        var proView = me.getChildView('SIE.Web.Items.ViewModels.PropertyValueViewModel');
        if (proView) {
            var gridPanel = proView.getControl();
            proView.loadData();
           //gridPanel.mon(proView.getData(), 'load', me.setProValue, proView);
        }

        var tempView = me.getChildView('SIE.Core.Items.LabelPrintTemplate');
        if (tempView) {
            var tempPanel = tempView.getControl();
            tempPanel.mon(tempView.getData(), 'load', me.setTempValue, tempView);
        }
        processBomView.getData().mon(processBomView.getData(), 'propertyChanged', SIE.Web.MES.WoCommonFun.ProcessBomPropertyChanged, processBomView);
    },

    /**
     * 初始化工单属性值数据
     * @param {any} store 数据源
     * @param {any} records 行数据
     * @param {any} successful 是否成功
     * @param {any} operation 操作类型
     * @param {any} eOpts 参数
     */
    //setProValue: function (store, records, successful, operation, eOpts) {
    //    var me = this;
    //    var wo = me._parent;
    //    SIE.invokeDataQuery({
    //        method: 'GetPropertyValueViewModel',
    //        params: [wo.getData().data.Id],
    //        action: 'queryer',
    //        type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
    //        token: wo.token,
    //        success: function (res) {
    //            if (res.Result.data && res.Result.data.items.length > 0) {
    //                res.Result.data.items.forEach(function (p) {
    //                    p.data.Values = p.data.Value.split(',');
    //                    p.data.DefinitionValueId = p.data.Value;
    //                    p.data.DefinitionId_Display = p.data.DefinitionName;
    //                });
    //                me.getData().add(res.Result.data.items);
    //                me._parent.getData().markSaved();
    //            }
    //        }
    //    });

    //    SIE.invokeDataQuery({
    //        method: 'GetProcessBomPropertyValueViewModel',
    //        params: [wo.getData().data.Id],
    //        action: 'queryer',
    //        type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
    //        token: wo.token,
    //        success: function (res) {
    //            wo.processBomPropertyList = [];
    //            if (res.Result.data && res.Result.data.items.length > 0) {
    //                res.Result.data.items.forEach(function (p) {
    //                    p.data.Values = p.data.Value.split(',');
    //                    p.data.DefinitionValueId = p.data.Value;
    //                    p.data.DefinitionId_Display = p.data.DefinitionName;
    //                });
    //                if (wo._children[2].getData().data.items.length > 0) {
    //                    wo.processBomPropertyList = res.Result.data.items;
    //                }
    //            }
    //        }
    //    });

    //    SIE.invokeDataQuery({
    //        method: 'GetBomPropertyValueViewModel',
    //        params: [wo.getData().data.Id],
    //        action: 'queryer',
    //        type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
    //        token: wo.token,
    //        success: function (res) {
    //            wo.bomPropertyList = [];
    //            if (res.Result.data && res.Result.data.items.length > 0) {
    //                res.Result.data.items.forEach(function (p) {
    //                    p.data.Values = p.data.Value.split(',');
    //                    p.data.DefinitionValueId = p.data.Value;
    //                    p.data.DefinitionId_Display = p.data.DefinitionName;
    //                });
    //                if (wo._children[1].getData().data.items.length > 0) {
    //                    wo.bomPropertyList = res.Result.data.items;
    //                }
    //            }
    //        }
    //    });
    //},

    /**
     * 重新设置打印模板页签的数据
     * @param {any} store 数据源
     * @param {any} records 行数据
     * @param {any} successful 是否成功
     * @param {any} operation 操作类型
     * @param {any} eOpts 参数
     */
    setTempValue: function (store, records, successful, operation, eOpts) {
        var me = this;
        var wo = me._parent;
        SIE.invokeDataQuery({
            method: 'GetTemplate',
            params: [wo.getData().data.Id],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            token: wo.token,
            success: function (res) {
                if (res.Result.data && res.Result.data.items.length > 0) {
                    var tempData = res.Result.data.items[0];
                    me.setData(tempData);
                }
            }
        });
    },
});