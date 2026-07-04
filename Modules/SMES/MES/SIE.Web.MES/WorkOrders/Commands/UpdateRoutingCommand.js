/**
 * 修改工单工艺路线命令UpdateRoutingCommand
 * @class SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand
 * @constructor
 */
SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "修改工艺路线", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    /**
     * 主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 工单
     * @property {WorkOrder} workOrder
     */
    workOrder: null,

    /**
     * 工单工艺路线修改保存执行中（false/true)
     * @property {isConfirmExcuting} isConfirmExcuting
     */
    isConfirmExcuting: false,

    /**
     * 是否可执行
     * 非关闭或者完工状态的工单暂停后可以修改
     * @method canExecute
     * @for SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand
     * @param {ListLogicalView} view 列表逻辑视图
     * @return {Boolean} 能执行返回true，否则返回false
     */
    canExecute: function (view) {
        var wo = view.getCurrent();
        if (!wo || !wo.data)
            return false;
        return wo.data.IsPause === 1 && (wo.data.State === 0 || wo.data.State === 1);
    },

    /**
     * 执行
     * @method execute
     * @for SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand
     * @param {ListLogicalView} view 列表逻辑视图
     * @param {Object} source 参数
     */
    execute: function (view, source) {
        var me = this;

        //工单工艺路线修改执行中值设置为false
        me.isConfirmExcuting = false;
        me.workOrder = view.getCurrent();

        var res = me.validateWorkOrder(me.workOrder);

        if (res) {
            SIE.Msg.showError(res);
            return;
        }

        me.mainView = view;
        me.mainView.layout = me;
        var layout = me.initRoutingDesingControl();
        me.isExistPackingUnit(me.workOrder.getId());
        me.showRoutingDesignWindow(layout, me.commit);
    },

    /**
     * 判断工单是否存在包装关系
     * @method isExistPackingUnit
     * @for SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand
     * @param {Number} workOrderId 工单ID
     */
    isExistPackingUnit: function (workOrderId) {
        var me = this;
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.WorkOrders.WorkOrderDataQueryer",
            method: "IsExistPackingUnit",
            token: me.mainView.token,
            params: [workOrderId],
            success: function (res) {
                if (res.Success) {
                    me.workOrder.isExistPackingUnit = res.Result;
                }
            }
        });
    },

    /**
     * 确定命令回调
     * @method commit
     * @for SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand 
     * @param {SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand} cmd 当前命令
     */
    commit: function (cmd, win) {
        var me = cmd;

        //if (me.isConfirmExcuting) {
        //    alert('工单工艺路线修改保存执行中');
        //    return;
        //}

        me.isConfirmExcuting = true;

        if (!me.designControl.routingLayout) {
            me.isConfirmExcuting = false;
            return;
        }

        var newLayout = me.designControl.getRoutingXml();

        //工艺路线没变更
        if (me.designControl.routingLayout === newLayout)
        {
            me.isConfirmExcuting = false;
            return;
        }

        if (me.designControl.designCanvas.validateDesignData() === false)
        {
            me.isConfirmExcuting = false;
            throw new Error('验证失败'.t());
        }
            
        me.validatePacking(me.designControl.designCanvas.getDesignData());
        var arg = { WorkOrderId: me.workOrder.getId(), WorkOrderNo: me.workOrder.getNo(),  Layout: newLayout };
        cmd.mainView.execute({
            command: Ext.getClassName(cmd),
            data: arg,
            success: function (res) {
                //刷新工单数据
                me.mainView.loadData();
                win.close();
                SIE.Msg.showInstantMessage('保存成功'.t());
            }
        });
    },

    validatePacking: function (designDate) {
        var me = this;
        var existPackingProcess = designDate.nodes.any(function (p) { return p.designerData.ProcessType === 'Packing' || p.designerData.ProcessType === 'BatchPacking'; });
        if (me.workOrder.isExistPackingUnit === false && existPackingProcess === true) {
            me.isConfirmExcuting = false;

            //工单未维护包装规则，工艺路线不能有包装工序
            throw new Error('工单未维护包装规则，工艺路线不能有包装工序'.t());
        }
    },

    /**
     * 验证工单
     * @method validateWorkOrder
     * @for SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand 
     * @param {WorkOrder} workOrder 工单
     * @returns {String/undefined} 不通过返回异常信息，通过不返回
     */
    validateWorkOrder: function (workOrder) {
        if (!workOrder)
            return '工单为空'.L10N();
        if (workOrder.data.IsPause !== 1)
            return '暂停工单才能修改工艺路线'.L10N();
        if (workOrder.data.State !== 0 && workOrder.data.State !== 1)
            return '只有发放或者生产中的工单才能修改工艺路线'.L10N();
    },

    /**
     * 加载工单工艺路线布局
     * @method loadWorkOrderRoutingLayout
     * @for SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand
     * @param {WorkOrder} workOrder 工单
     */
    loadWorkOrderRoutingLayout: function (workOrder) {
        var me = this;
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.WorkOrders.WorkOrderDataQueryer",
            method: "GetRoutingLayout",
            token: me.mainView.token,
            params: [workOrder.get('LayoutId')],
            success: function (res) {
                if (!res.Success)
                    return;
                me.designControl.drawRouting(res.Result);
            }
        });
    },

    /**
     * 初始化工单工艺路线编辑界面
     * @method initRoutingDesingControl
     * @for SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand
     * @returns {container} 控件
     */
    initRoutingDesingControl: function () {
        var me = this;
        me.processControl = Ext.create('SIE.Tech.ProcessTreeControl', {
            mainView: me.mainView
        });
        me.designControl = Ext.create('SIE.Web.MES.WorkOrders.UpdateRoutingControl', {
            mainView: me.mainView
        });
        var items = SIE.Web.Tech.Common.Routings.PropertyExt.getPropertyConfigs();
        me.propertyControl = Ext.create('SIE.Tech.PropertyControl', {
            mainView: me.mainView,
            items: items
        });
        me.registerEvent();
        return Ext.widget('container', {
            layout: 'border',
            that: me,
            items: [{
                region: 'center',
                layout: 'fit',
                items: me.designControl
            }, {
                region: 'west',
                width: 250,
                minWidth: 200,
                maxWidth: 300,
                border: 0,
                layout: 'fit',
                autoScroll: true,
                split: true,
                collapsible: true,
                title: '工序信息'.L10N(),
                items: me.processControl
            }, {
                region: 'east',
                width: 250,
                minWidth: 200,
                maxWidth: 300,
                border: 0,
                collapsible: true,
                split: true,
                layout: 'fit',
                title: '流程属性'.L10N(),
                items: me.propertyControl
            }]
        });
    },

    /**
     * 显示工单工艺路线编辑界面
     * @method showRoutingDesignWindow
     * @param {container} layout 控件
     * @param {回调} commit 确认回调函数
     */
    showRoutingDesignWindow: function (layout, commit) {
        var me = this;
        var win = SIE.Window.show({
            title: '修改 工单工艺路线'.t(),
            items: layout,
            width: '70%',
            height: '90%',
            callback: function (btn) {
                if (btn === '确定'.t()) {
                    try {
                        commit(me, win);
                    } catch (e) {
                        var msg = e.message;
                        if (msg && msg !== '验证失败'.t()) {
                            SIE.Msg.showError(msg);
                        }
                        return false;
                    }
                    return false;
                }
            },
            listeners: {
                /**
                 * Window渲染后事件，加载工单工艺路线
                 * @method afterrender
                 * @for Window
                 * @param {Ext.window.Window} scrop Window
                 * @param {Object} eOpts 参数
                 */
                afterrender: function (scrop, eOpts) {
                    me.loadWorkOrderRoutingLayout(me.workOrder);
                }
            }
        });
    },

    //***********************事件相关*******************
    /**
     * 注册事件
     * @method registerEvent
     */
    registerEvent: function () {
        var me = this;
        var canvas = me.designControl.designCanvas;
        canvas.mon(canvas, 'nodeChanged', me.nodeChanged, me);
    },

    /**
     * 节点变更事件
     * @method nodeChanged
     * @for SIE.Tech.layouts.RoutingLayout
     * @param {ListLogicalView} mainView 父主视图
     * @param {Object} node 节点信息
     */
    nodeChanged: function (mainView, node) {
        var layout = mainView.layout;
        //加载属性 
       //layout.propertyControl.nodeChanged(node, false);
        //if (node)
        //    this.designControl.designCanvas.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 1) 0px 0px 0px 2px', '45px', '118px');

        var isSelectedInner = false;
        if (node) {
            if (!node.designerData || (node.designerData && node.designerData.NodeType !== "RoutingGroupNode")) {
                layout.propertyControl.nodeChanged(node, false);
            } else {
                //工序组需要重新取里面的Node
                var clickInnerDom = document.elementFromPoint(this.designControl.designCanvas.mouseEvent.x, this.designControl.designCanvas.mouseEvent.y);
                var blockDom = null;
                if (clickInnerDom && clickInnerDom.className === "node") {
                    blockDom = clickInnerDom;
                }
                if (clickInnerDom && clickInnerDom.className !== "node") {
                    blockDom = clickInnerDom.parentNode;
                }

                if (blockDom && blockDom.id != node.id) {
                    var groupNodes = this.designControl.designCanvas.drawControl.drawTools.getNode(node.id);//取出组的数据
                    var currentNodeData = null;
                    groupNodes.groupDesignerData.forEach(function (nodeData) {

                        if (nodeData.id == blockDom.id) {
                            currentNodeData = nodeData;
                            return;
                        }
                    });
                    if (currentNodeData) {
                        layout.propertyControl.nodeChanged(currentNodeData, false);
                        isSelectedInner = true
                        this.designControl.designCanvas.updateNodeBoxShadow(currentNodeData, 'rgba(15, 124, 245, 1) 0px 0px 0px 1px', '45px', '118px');
                    }
                }

            }
        }
        if (!isSelectedInner) {//选中内部的时候不需要再调用
            this.designControl.designCanvas.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 1) 0px 0px 0px 2px', '45px', '118px');
        }
    }
});
