/**
 * 产品工艺路线设计控件
 * @class SIE.Web.MES.ProductRoutingDesignControl
 * @constructor
 */
Ext.define('SIE.Web.MES.ProductRoutingDesignControl', {
    extend: 'Ext.panel.Panel',
    xtype: 'techProductRoutingDesign',

    /**
     * 产品工艺路线主视图
     * @property {ListLogicView} mainView
     */
    mainView: null,

    /**
     * 设计画布
     * @property {DesignCanvas} designCanvas
     */
    designCanvas: null,

    /**
     * 产品工艺路线命令字典  key命令名称 value命令按钮
     * @property {字典} dicCommands
     */
    dicCommands: {},

    /**
     * 产品工艺路线布局
     * @property {ProductRoutingLayout} routingLayout
     */
    routingLayout: null,  //工艺路线布局
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 0,
    border: 0,
    items: [{
        title: null,
        padding: 5,
        border: 0,
        xtype: 'toolbar',
        defaults: {
            xtype: 'button',
            disabled: true
        },
        items: [{
            text: '暂停'.L10N(),
            command: 'pause',
            handler: function () {
                this.up('panel').pause();
            }
        }, {
            text: '设为当前工序'.L10N(),
            command: 'setCurrent',
            handler: function () {
                this.up('panel').setCurrent();
            }
        }, {
            text: '保存'.L10N(),
            command: 'save',
            handler: function () {
                this.up('panel').save();
            }
        }, {
            text: '启用'.L10N(),
            command: 'enabled',
            handler: function () {
                this.up('panel').enabled();
            }
        }
        ]
    }, {
        title: null,
        autoScroll: true,
        flex: 1,
        bodyStyle: {
            backgroundImage: "url('/images/drawtools/dot_bg.jpg')",
            backgroundRepeat: 'repeat'
        },
        html: '<div style="position:absolute; width:100%; height:100%" id = "productRoutingCanvas"></div>'
    }],
    listeners: {
        //选中节点变更事件
        nodeChanged: function (designControl, node) {
        },
        render: function (scop, eOpts) {
            var me = this;
            me.initDicCammands();
            me.createDesignCanvas();  //必须在控件初始化后执行，不然找不到画布 
        }
    },

    /**
     * 控件初始化
     * @method initComponent
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    initComponent: function () {
        var me = this;
        me.designCanvas = new DesignCanvas(me.mainView, 'productRoutingCanvas', null);  //先初始化，这样外部就可以挂nodeChanged事件
        me.callParent();
    },

    /**
     * 初始化命令字典
     * @method initDicCammands
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    initDicCammands: function () {
        var me = this;
        me.items.items[0].items.items.forEach(function (cmdBtn) {
            me.dicCommands[cmdBtn.command] = cmdBtn;
        });
    },

    /**
     * 创建设计画布
     * @method createDesignCanvas
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    createDesignCanvas: function () {
        var me = this;
        me.designCanvas.InitDrawViewControl();
    },

    /**
     * 画工艺路线
     * @method drawRouting
     * @for SIE.Web.MES.ProductRoutingDesignControl
     * @param {String} layout 工艺路线布局
     */
    drawRouting: function (layout) {
        var me = this;
        me.routingLayout = layout;
        me.resetMainBlock();
        me.setDesignCanvasLock();
        me.designCanvas.drawRouting(layout);
    },

    /**
     * 重置画布
     * @method resetMainBlock
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    resetMainBlock: function () {
        var me = this;
        //清除画布内容
        me.designCanvas.clearDrawControl();
    },

    //************************************************命令逻辑*************************************************
    /**
     * 更新命令状态
     * @method updateCommandsStatus
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    updateCommandsStatus: function () {
        //暂停、设为当前工序、保存、启用
        var me = this;
        me.updatePauseStatus();
        me.updateSetCurrentStatus();
        me.updateSaveStatus();
        me.updateEnabledStatus();
        me.setDesignCanvasLock();
    },

    /**
     * 更新暂停命令状态
     * @method updatePauseStatus
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    updatePauseStatus: function () {
        //非暂停情况下可以暂停
        var me = this;
        var layout = me.mainView.layout;
        var canExecute = false;
        if (layout.version && layout.version.IsPause === 0 && layout.version.IsFinish === false)   //0:No   1:Yes
            canExecute = true;
        me.setCommandStatus('pause', canExecute);
    },

    /**
     * 更新设置当前命令状态
     * @method updateSetCurrentStatus
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    updateSetCurrentStatus: function () {
        //设置当前工序是否可执行（暂停并且选中节点不为终止节点才可以设置当前工序）
        var me = this;
        var canExecute = false;
        var layout = me.mainView.layout;
        var selectNode = me.designCanvas.selectNode;
        if (layout.version && layout.version.IsPause === 1 && selectNode) {
            var designerData = selectNode.designerData;
            if (designerData.Type !== 'Completion' && designerData.ProcessState !== 'Current')
                canExecute = true;
        }
        me.setCommandStatus('setCurrent', canExecute);
    },

    /**
     * 更新保存命令状态
     * @method updateSaveStatus
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    updateSaveStatus: function () {
        var me = this;
        var layout = me.mainView.layout;
        var canExecute = false;
        if (layout.version && layout.version.IsPause === 1)
            canExecute = true;
        me.setCommandStatus('save', canExecute);
    },

    /**
     * 更新启用命令状态
     * @method updateEnabledStatus
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    updateEnabledStatus: function () {
        var me = this;
        var layout = me.mainView.layout;
        var canExecute = false;
        if (layout.version && layout.version.IsPause === 1)
            canExecute = true;
        me.setCommandStatus('enabled', canExecute);
    },

    /**
     * 更新命令状态
     * @method setCommandStatus
     * @for SIE.Web.MES.ProductRoutingDesignControl
     * @param {String} cmdName 命令名称
     * @param {Boolean} isDisabled 是否可用
     */
    setCommandStatus: function (cmdName, isDisabled) {
        var me = this;
        var command = me.dicCommands[cmdName];
        if (command)
            command.setDisabled(!isDisabled);
    },

    /**
     * 设置画布锁定
     * @method setDesignCanvasLock
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    setDesignCanvasLock: function () {
        var me = this;
        var layout = me.mainView.layout;
        if (layout.version === null || (layout.version && layout.version.IsPause === 0))   //0:No   1:Yes
            me.designCanvas.setLock(true);
        else
            me.designCanvas.setLock(false);
    },

    /**
     * 暂停
     * @method pause
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    pause: function () {
        var me = this;
        var layout = me.mainView.layout;
        var barcode = '';
        if (layout.barcode)
            barcode = layout.barcode.Sn;
        SIE.Msg.askQuestion(Ext.String.format('产品：{0} 暂停，会影响产品继续生产过站，是否暂停？'.L10N(), barcode), function () {
            var newLayout = me.getRoutingXml();
            layout.invokeDataQuery('PauseProductRouting', [layout.version.Id, me.routingLayout, newLayout], function (result) {
                //TODO刷新是异步的，需同步
                layout.refreshWipProductInfo(layout.barcode);  //重新加载工艺路线信息 
                if (me.mainView.currentNode) {
                    //加载属性
                    layout.propertyControl.nodeChanged(me.mainView.currentNode, function () {
                        return false;
                    });
                }
            });
        });
    },

    /**
     * 设置当前
     * @method setCurrent
     * @for SIE.Web.MES.ProductRoutingDesignControl 
     */
    setCurrent: function () {
        var me = this;
        var layout = me.mainView.layout;
        if (!layout)
            return;
        var selectNode = me.designCanvas.selectNode;
        if (selectNode) {
            me.InitProcessStatus(selectNode, layout.runtimeProduct);
            me.designCanvas.updateNodesColor();
            SIE.Msg.showMessage(Ext.String.format('已设置 {0} 为当前工序'.L10N(), selectNode.designerData.Text));
        }
    },

    /**
     * 初始化工序节点状态
     * @method InitProcessStatus
     * @for SIE.Web.MES.ProductRoutingDesignControl
     * @param {Object} currentNode 当前工序节点
     * @param {product} product 运行时产品
     */
    InitProcessStatus: function (currentNode, product) {
        var me = this;
        if (!currentNode || !product)
            return;
        var design = me.designCanvas.drawControl.generalDesign();
        var dicStatus = {};
        var isCurrentBegin = false;

        var current = product.Routing.Processes.first(function (p) { return p.Index == currentNode.designerData.Index; });
        if (currentNode.nodeType === 'BeginNode') {
            //所有工序都未过站
            design.nodes.select(function (p) { return p.designerData.Index; }).where(function (p) { return p > 0; }).forEach(function (Index) { dicStatus[Index] = 2; });
            currentNode.designerData.ProcessState = 'Current';
            isCurrentBegin = true;
        }
        else {
            //获取运行时工艺路线当前工序节点，循环设置后工序
            var firstProcess = product.Routing.Processes.first(function (p) { return p.Sign === 2; });  //首工序
            //首工序--->当前工序之间的设置为已过站；当前工序之后的设置为未过站
            me.GetProcessStatus(dicStatus, product.Routing.Processes, current.Index, firstProcess, 0, true);
        }
        design.nodes.forEach(function (node) {
            if (node.nodeType === 'RoutingNode') {
                var status = dicStatus[node.designerData.Index];
                if (status === 0)
                    node.designerData.ProcessState = 'Has';
                else if (status === 1)
                    node.designerData.ProcessState = 'Current';
                else
                    node.designerData.ProcessState = 'Not';
            }
            else if (!isCurrentBegin) {
                node.designerData.ProcessState = 'Not';
            }
        });
    },

    /**
     * 获取工序节点状态，（已过站、当前工序、未过站）
     * @method GetProcessStatus
     * @for SIE.Web.MES.ProductRoutingDesignControl
     * @param {字典} dicStatus 工序节点状态字典
     * @param {Array} processList 运行时产品工序集合
     * @param {Number} currentId 当前工序ID
     * @param {Object} process 生产产品工序节点
     * @param {Number} status 0已过站 1当前工序 2未过站
     * @param {Boolean} isFirst 工序是否首工序
     */
    GetProcessStatus: function (dicStatus, processList, currentId, process, status, isFirst) {
        var me = this;
        if (!process) {//工序组不存在工序
            return;
        }
        if (currentId === process.Index) {
            dicStatus[currentId] = 1;
            status = 2;
        }
        else if (isFirst === true) {
            dicStatus[process.Index] = 0;
        }
        for (var key in process.Next) {
            for (var i = 0; i < process.Next[key].length; i++) {
                var nextnextRoutingProcessId = process.Next[key][i];  //工艺路线工序ID
                var nextRoutingProcess = processList.first(function (p) { return p.Id === nextnextRoutingProcessId; });
                var nextProcessId = nextRoutingProcess.Index;
                //if (dicStatus[nextProcessId] != undefined)
                //    continue;
                /*dicStatus[nextProcessId] = status;*/
                if (nextProcessId == undefined) {//下一工序为工序组的时候 nextProcessId为undefined
                    if (dicStatus[nextRoutingProcess.Index] != undefined)
                        continue;
                    dicStatus[nextRoutingProcess.Index] = status;

                } else {
                    if (dicStatus[nextProcessId] != undefined)
                        continue;
                    dicStatus[nextProcessId] = status;
                }

                me.GetProcessStatus(dicStatus, processList, currentId, nextRoutingProcess, status, false);
            }
            if (Object.keys(dicStatus).length === processList.length)
                break;
        }
    },

    /**
     * 保存
     * @method save
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    save: function () {
        var me = this;
        var layout = me.mainView.layout;
        var barcode = '';
        if (layout.barcode)
            barcode = layout.barcode.Sn;
        if (layout.version === null || layout.version === undefined)
            throw Ext.String.format('产品:{0}还未上线'.L10N(), barcode);
        var msg = me.setNodeBoms();
        if (msg) {
            SIE.Msg.showMessage(msg);
            return;
        }
        var newLayout = me.getRoutingXml();
        SIE.Msg.wait("正在保存......".t());
        layout.invokeDataQuery('SaveProductRouting', [layout.version.Id, me.routingLayout, newLayout], function (result) {
            layout.refreshWipProductInfo(layout.barcode);  //重新加载工艺路线信息
            me.updateCommandsStatus();   //更新命令状态
            SIE.Msg.showMessage('保存成功'.L10N());
        });
    },

    /**
     * 启用
     * @method enabled
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    enabled: function () {
        var me = this;
        var layout = me.mainView.layout;
        var barcode = '';
        if (layout.barcode)
            barcode = layout.barcode.Sn;
        if (layout.version === null || layout.version === undefined)
            throw Ext.String.format('产品:{0}还未上线'.L10N(), barcode);

        var msg = me.setNodeBoms();
        if (msg) {
            SIE.Msg.showMessage(msg);
            return;
        }

        var newLayout = me.getRoutingXml();
        SIE.Msg.wait("正在启用......".t());
        layout.invokeDataQuery('EnableProductRouting', [layout.version.Id, me.routingLayout, newLayout], function (result) {
            me.refreshWipProductInfo();
            if (me.mainView.currentNode) {
                //加载属性
                layout.propertyControl.nodeChanged(me.mainView.currentNode, function () {
                    return true;
                });
            }

            SIE.Msg.showMessage(Ext.String.format('产品：{0} 工艺路线启用成功'.L10N(), barcode));
        });
    },

    /**
     * 刷新生产产品信息
     * @method refreshWipProductInfo
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    refreshWipProductInfo: function () {
        var me = this;
        var layout = me.mainView.layout;
        layout.refreshWipProductInfo(layout.barcode);  //重新加载工艺路线信息
        me.updateCommandsStatus();   //更新命令状态
    },

    /**
     * 获取工艺路线布局
     * @method getRoutingXml
     * @for SIE.Web.MES.ProductRoutingDesignControl
     * @return {String} 布局
     */
    getRoutingXml: function () {
        var me = this;
        return Ext.getCmp(me.mainView.routingDrawControlId).getXml();
    },

    /**
     * 设置节点BOM
     * @method setNodeBoms
     * @for SIE.Web.MES.ProductRoutingDesignControl
     */
    setNodeBoms: function () {
        var me = this;

        var design = me.designCanvas.getDesignData();

        var processBomDic = me.mainView.layout.processBomDic;

        for (var item in design.nodes) {
            var node = design.nodes[item];

            if (!node.designerData)
                continue;

            //var processId = node.designerData.ProcessId;
            var activityId = node.id;

            if (processBomDic[activityId]) {
                var bomList = processBomDic[activityId];

                var groupBoms = bomList.groupBy(function (p) { return p.ItemId; });

                groupBoms.forEach(function (groupBom) {
                    var bom = groupBom[0];
                    if (groupBom.length > 1)
                        return Ext.String.format('工序[{0}]存在相同物料[{1}]的BOM'.L10N(), node.designerData.Text, bom.Code);
                });

                var processBoms = [];

                bomList.forEach(function (bom) {
                    processBoms.push({
                        ItemId: bom.ItemId,
                        Qty: bom.Qty,
                        IsBuckleMaterial: bom.IsBuckleMaterial,
                        Point: bom.Point,
                        WorkStepId: bom.WorkStepId,
                        IsAttachment: bom.IsAttachment,
                        IsExternal: bom.IsExternal,
                        IsSingleLabel: bom.IsSingleLabel,
                        IsRepeat: bom.IsRepeat,
                        HasBarcodeRule: bom.HasBarcodeRule,
                        MainMaterialId: bom.MainMaterialId
                    });
                });

                node.designerData.ProcessBoms = processBoms;
            }
        }
    }
});