Ext.define('SIE.Web.MES.WorkOrders.CreateWorkOrderBehavior', {
    extend: 'SIE.Web.MES.WorkOrders.BaseWorkOrderDetailBehavior',

    /**
     * 原工单ID
     */
    oldWorkOrderId: null,

    /**
     * 初始化工单数据
     * @param {any} view
     */
    initWorkOrderData: function (view, workOrder) {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        var action = params.action; //0新增、1复制新增、2修改、3查看
        me.oldWorkOrderId = params.oldWorkOrderId;
        if (action === 0) {
            me.initNewWorkOrder(workOrder, view.token);
            //me.setBatchQty(view);
        } else
            me.initCopyWorkOrder(workOrder, view);
        view.mon(workOrder, 'propertyChanged', SIE.Web.MES.WoCommonFun.WorkOrderPropertyChanged, view);
        me.initTemplate(view);
        me.initLoadevent(view);
        if (action === 1)
            SIE.Web.MES.WoCommonFun.initProductChanged(workOrder);
    },

    /**
     * 初始化新建工单数据
     * @param {any} workOrder
     * @param {any} token
     */
    initNewWorkOrder: function (workOrder, token) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetNewWorkOrder',
            action: 'queryer',
            token: token,
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            success: function success(res) {
                var data = res.Result.data.getAt(0).data;
                me.setNewWorkOrderData(workOrder, data);
            }
        });
    },

    /**
     * 设置新增工单的数据
     * @param {any} workOrder 工单
     * @param {any} data 工单数据
     */
    setNewWorkOrderData: function (workOrder, data) {
        workOrder.setPanelQty(data.PanelQty);
        workOrder.setPanelPrintQty(data.PanelPrintQty);
        workOrder.setSource(data.Source);
        workOrder.setState(data.State);
        workOrder.setKitType(data.KitType);
        workOrder.setType(data.Type);
        workOrder.setMakerId(data.MakerId);
        workOrder.setMakeDate(data.MakeDate);
        workOrder.setTemplate(data.Template);
        workOrder.setNo(data.No);
        workOrder.setPlanBeginDate(data.PlanBeginDate);
        workOrder.setPlanEndDate(data.PlanEndDate);
        workOrder.setPlanQty(0);
        workOrder.setOrderQty(0);
        workOrder.setId(data.Id);
    },

    /**
     * 初始化复制的工单数据
     * @param {any} workOrder
     * @param {any} token
     */
    initCopyWorkOrder: function (workOrder, view) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetCopyWorkOrderInfo',
            action: 'queryer',
            params: [workOrder.data, me.oldWorkOrderId],
            token: view.token,
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            success: function success(res) {
                var resultData = res.Result;
                if (!resultData) return;
                //设置工单数据
                me.setCopyWorkOrderData(workOrder, resultData.CopyWorkOrder);
                //设置批次数量
                var batModel = "SIE.Core.WorkOrders.WoWipBatch";
                var batchView = view._children.first(function (p) { return p.model == batModel; });
                if (batchView) {
                    //me.activeTabItem("SIE.Core.WorkOrders.WoWipBatch", true);
                    //var inputId = batchView._control.items.items[0].getInputId();
                    //document.getElementById(inputId).value = resultData.BatchQty;
                    batchView.getData().setQty(resultData.BatchQty);
                }

                //设置打印模板
                var templateChildView = me.getChildView('SIE.Core.Items.LabelPrintTemplate');
                var templateEntity = new templateChildView._model();
                templateEntity.setNumberRuleId_Display(resultData.PrintTemplate.RuleCode);
                templateEntity.setLabelTemplateId_Display(resultData.PrintTemplate.LabelFileName);
                templateEntity.setPackingTemplateId_Display(resultData.PrintTemplate.PackingFileName);
                templateEntity.setNumberRuleId(resultData.PrintTemplate.NumberRuleId);
                templateEntity.setLabelTemplateId(resultData.PrintTemplate.LabelTemplateId);
                templateEntity.setPackingTemplateId(resultData.PrintTemplate.PackingTemplateId);
                templateChildView.setData(templateEntity);
            }
        });
    },

    /**
     * 设置复制工单的数据
     * @param {any} workOrder 工单
     * @param {any} data 工单数据
     */
    setCopyWorkOrderData: function (workOrder, wo) {
        workOrder.setPanelQty(wo.PanelQty);
        workOrder.setPanelPrintQty(wo.PanelPrintQty);
        workOrder.setNo(wo.No);
        workOrder.setMakerId(wo.MakerId);
        workOrder.setMakeDate(wo.MakeDate);
        workOrder.setTemplateId(wo.TemplateId);
        workOrder.setPlanBeginDate(wo.PlanBeginDate);
        workOrder.setPlanEndDate(wo.PlanEndDate);
        workOrder.setId(wo.Id);
        workOrder.setSource(0);
        workOrder.setState(0);
        workOrder.setIsPause(0);
        workOrder.setPrintedQty(0);
        workOrder.setVersionId(0);
        workOrder.setFinishQty(0);
        workOrder.setOnlineQty(0);
        //workOrder.setStorageQty(0);
        workOrder.setIsCommonMode(0);
        workOrder.setIsMainMaterial(0);
        workOrder.setActuFinishDate(wo.ActuFinishDate);
        workOrder.setActuStartDate(wo.ActuStartDate);
        workOrder.setProcessTechOrderCode('');
        workOrder.setPlanNo('');
        workOrder.setProportion(0);
    },

    /**
     * 设置复制打印模板的数据
     * @param {any} templateEntity 打印模板
     * @param {any} data 打印模板数据
     */
    setPrintTemplateData: function (templateEntity, data) {
        templateEntity.setNumberRuleId(data.NumberRuleId);
        templateEntity.setLabelTemplateId(data.LabelTemplateId);
        templateEntity.setPackingTemplateId(data.PackingTemplateId);
        templateEntity.setNumberRuleId_Display(data.RuleCode);
        templateEntity.setLabelTemplateId_Display(data.LabelFileName);
        templateEntity.setPackingTemplateId_Display(data.PackingFileName);
    },

    /**
     * 设置批次数量默认值
     * @param targetView 工单视图      
     */
    setBatchQty: function (view) {
        var me = this;
        var batModel = "SIE.Core.WorkOrders.WoWipBatch";
        var batchView = view._children.first(function (p) { return p.model == batModel; });
        /*me.activeTabItem("SIE.Core.WorkOrders.WoWipBatch", true);*/
        var store = SIE.data.Utils.createStore({
            model: batModel,
            storeConfig: {
                proxy: Ext.clone(SIE.getModel(batModel).proxyConfig)
            },
            remoteSort: true
        });
        var bM = SIE.getModel(batModel);
        var newEntity = new bM();
        store.add(newEntity);
        store.getData().items[0].setQty(1);
        view._current[batchView.getAssociateKey()] = store;
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
        /*me.activeTabItem(batModel, true);*/
        var bM = SIE.getModel(batModel);
        var newEntity = new bM();
        store.add(newEntity);
        view._current[tempView.getAssociateKey()] = store;
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
    }
});