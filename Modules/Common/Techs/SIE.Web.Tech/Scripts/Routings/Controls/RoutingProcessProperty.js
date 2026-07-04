/**
 * 工艺路线工序标准属性
 * @class SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
 * @constructor
 */
Ext.define('SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty', {
    extend: 'SIE.Web.Tech.Common.Routings.RoutingProcessPropertyInfo',
    /**
     * 属性配置
     * @property {Array} configs
     */
    configs: [{
        xtype: 'displayfield',
        fieldLabel: '名称'.L10N(),
        name: 'Text',
        bind: '{m.Text}',
        hidden: true,
        index: 10
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '是否可选'.L10N(),
        name: 'IsOptional',
        bind: '{m.IsOptional}',
        hidden: true,
        index: 20,
        listeners: [{
            change: function (control, newValue, oldValue, eOpts) {
                var node = document.getElementById(control.ownerCt.viewModel.data.id);
                if (node) {
                    var isOptionalImg = node.getElementsByTagName("img")[1];
                    if (isOptionalImg) {
                        isOptionalImg.style.display = newValue === true ? "block" : "none";
                    }
                }
            }
        }]
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '重复过站'.L10N(),
        name: 'IsRepeat',
        bind: '{m.IsRepeat}',
        hidden: true,
        index: 30,
        listeners: [{
            change: function (control, newValue, oldValue, eOpts) {
                var node = document.getElementById(control.ownerCt.viewModel.data.id);
                if (node) {
                    var isRepeatImg = node.getElementsByTagName("img")[2];
                    if (isRepeatImg) {
                        isRepeatImg.style.display = newValue === true ? "block" : "none";
                    }
                }
            }
        }]
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '创建SKU'.L10N(),
        name: 'CreateSku',
        bind: '{m.CreateSku}',
        hidden: true,
        index: 40
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '是否计产'.L10N(),
        name: 'IsCalculate',
        bind: '{m.IsCalculate}',
        hidden: true,
        index: 40
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '是否生成工序任务单'.L10N(),
        name: 'IsGenerateTask',
        bind: '{m.IsGenerateTask}',
        hidden: true,
        index: 50,
        listeners: [{
            change: function (control, newValue, oldValue, eOpts) {
                if (control.ownerCt != null && control.ownerCt.items != null && control.ownerCt.items.items != null) {
                    var isRequirementTaskchk = control.ownerCt.items.items.find(m => m.name == "IsRequirementTask");
                    if (isRequirementTaskchk != null) {
                        isRequirementTaskchk.setDisabled(newValue);
                        isRequirementTaskchk.setValue(newValue)
                    }
                }
            }
        }]
    },
    {
        xtype: 'checkboxfield',
        fieldLabel: '是否需求任务清单'.L10N(),
        name: 'IsRequirementTask',
        bind: '{m.IsRequirementTask}',
        hidden: true,
        index: 52
    },

    {
        xtype: 'numberfield',
        fieldLabel: '最大过站次数'.L10N(),
        name: 'MaxPassNum',
        bind: '{m.MaxPassNum}',
        minValue: 1,
        allowDecimals: false,
        hidden: true,
        index: 55
    }, {
        xtype: 'combobox',
        fieldLabel: '正常胜制'.L10N(),
        name: 'NormalVictory',
        bind: '{m.NormalVictory}',
        displayField: 'Display',
        valueField: 'Id',
        hidden: true,
        index: 60
    }, {
        xtype: 'combobox',
        fieldLabel: '维修胜制'.L10N(),
        name: 'RepairVictory',
        bind: '{m.RepairVictory}',
        displayField: 'Display',
        valueField: 'Id',
        hidden: true,
        index: 70
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '加严'.L10N(),
        name: 'IsStricter',
        bind: '{m.IsStricter}',
        hidden: true,
        index: 80
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '绑定'.L10N(),
        name: 'IsBinding',
        bind: '{m.IsBinding}',
        hidden: true,
        index: 90
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '解绑'.L10N(),
        name: 'IsUnBinding',
        bind: '{m.IsUnBinding}',
        hidden: true,
        index: 100
    }, {
        xtype: 'numberfield',
        fieldLabel: '显示网格',
        name: 'ShowGrid',
        bind: '{m.ShowGrid}',
        hidden: true,
        index: 110
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '启用入站'.L10N(),
        name: 'EnableMoveInControl',
        bind: '{m.EnableMoveInControl}',
        hidden: true,
        index: 120
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '是否委外'.L10N(),
        name: 'IsOutsourcing',
        bind: '{m.IsOutsourcing}',
        hidden: true,
        index: 120,
    },
    //{
    //xtype: 'combobox',
    //fieldLabel: '工序交接'.L10N(),
    //name: 'TransferType',
    //bind: '{m.TransferType}',
    //displayField: 'Display',
    //valueField: 'Id',
    //hidden: true,
    //index: 130
    //}, 
    {
        xtype: 'checkboxfield',
        fieldLabel: '是否下工序入站'.L10N(),
        name: 'IsNextMoveIn',
        bind: '{m.IsNextMoveIn}',
        displayField: 'Display',
        valueField: 'Id',
        hidden: true,
        index: 130
    }
    ],

    /**
     * 重置控件
     * @method resetControl
     * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
     * @param {DesignCanvas} control DesignCanvas
     */
    resetControl: function (control) {
    },

    /**
     * 加载数据
     * @method loadData
     * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
     * @param {Object} node 工序节点信息
     */
    loadData: function (control, node, isDisable) {
        this.loadVictoryStandards(control, node);
        this.loadTransferTypes(control, node);
        this.loadPropertyData(control, node, isDisable);
    },

    /**
     * 验证
     * @method validate
     * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
     * @param {DesignCanvas} canvas 节点信息
     * @param {Array} nodes 节点集合
     * @param {Array} lines 连线集合
     */
    validate: function (canvas, nodes, lines) {
        var me = this;
        var routingNode = nodes.where(function (p) {
            return p.nodeType === 'RoutingNode';
        }).orderBy(function (p) {
            return p.designerData.Index;
        });
        var firstProcessId = routingNode.first().designerData.ProcessId;
        var endProcessId = routingNode.last().designerData.ProcessId;
        nodes.forEach(function (node) {
            //验证工序属性 
            me.validateProperty(node, firstProcessId, endProcessId);
        });
        //验证SKU
        me.validateSku(nodes);
    },

    /**
     * 验证工序属性
     * @method validateProperty
     * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
     * @param {Object} node 节点信息
     * @param {Number} firstProcessId 首工序id
     * @param {Number} endProcessId 末工序id
     */
    validateProperty: function (node, firstProcessId, endProcessId) {
        var me = this;
        //开始/结束节点不能设置可选工序
        if (node.designerData.IsSelected === true || node.designerData.IsSelected === 'true') {
            //if (node.designerData.ProcessId === firstProcessId)
            //    throw new Error(Ext.String.format('工序[{0}]是开始工序，不可以为可选工序'.L10N(), node.designerData.Text));
            //else
            if (node.designerData.ProcessId === endProcessId)
                throw new Error(Ext.String.format('工序[{0}]是结束工序，不可以为可选工序'.L10N(), node.designerData.Text));
        }
        if (node.nodeType !== 'RoutingNode')
            return;
        //非检验工序不能配置胜制方案
        if (node.designerData.ProcessType !== "Pqc" && node.designerData.ProcessType !== "Fqc") {
            if (node.designerData.NormalVictory > 0)
                throw new Error(Ext.String.format('工序[{0}]非检验类型工序，不允许配置正常胜制方案'.L10N(), node.designerData.Text));
            if (node.designerData.RepairVictory > 0)
                throw new Error(Ext.String.format('工序[{0}]非检验类型工序，不允许配置维修胜制方案'.L10N(), node.designerData.Text));
        }
    },

    /**
     * 验证是否创建SKU
     * @method validateSku
     * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
     * @param {Array} nodes 节点信息
     */
    validateSku: function (nodes) {
        var allNodes = [];

        var nodedataArry = nodes.where(function (p) {
            return p.nodeType === 'RoutingNode' && !p.designerData.IsGroup;
        })
        if (nodedataArry && nodedataArry.length) {
            nodedataArry.forEach(itemData => {
                allNodes.push(itemData);
            });
        }

        var groupNodes = nodes.where(function (p) {
            return p.designerData.NodeType === 'RoutingGroupNode';
        })
        if (groupNodes && groupNodes.length) {
            groupNodes.forEach(itemData => {
                itemData.groupDesignerData.forEach(itemDesignerData => {
                    allNodes.push(itemDesignerData);
                });
            });
        }

        var skuNodes = allNodes.where(function (p) {
            return p.nodeType === 'RoutingNode' && (p.designerData.CreateSku === true || p.designerData.CreateSku === 'true');
        });
        //if (skuNodes.length > 1) //创建sku节点数
        //    throw new Error(Ext.String.format('只能存在一个Sku\n已存在Sku工序[{0}]'.L10N(), skuNodes.select(function (p) {
        //        return p.designerData.Text;
        //    }).join(';')));
        var packingNodes = allNodes.where(function (p) {
            return p.designerData.ProcessType === 'Packing' || p.designerData.ProcessType === 'BatchPacking';
        });

        if (packingNodes.length > 0 && skuNodes.length === 0) {
            //存在包装节点但是没有设置创建sku
            throw new Error('存在包装工序，必须存在一个Sku'.L10N());
        }

        if (packingNodes.length < 1) return; //创建sku必须在第一个包装工序之前或者该包装工序

        //var minIndex = packingNodes.min(function (p) {
        //    return p.designerData.Index;
        //});

        //if (skuNodes[0].designerData.Index > minIndex) {
        //    var firstPackingNode = packingNodes.first(function (p) {
        //        return p.designerData.Index === minIndex;
        //    });
        //    throw new Error(Ext.String.format('存在包装工序，必须在第一个包装工序[{0}]前创建Sku'.L10N(), firstPackingNode.designerData.Text));
        //}
    },

    /**
     * 序列化
     * @method serialize
     * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
     * @param {Object} nodeData 节点信息
     * @param {Object} activity 活动图
     * @param {String} pre 前缀
     */
    serialize: function (nodeData, activity, pre) {
        var normalVictory = activity[pre + "NormalVictory"];
        var repairVictory = activity[pre + "RepairVictory"];
        var maxPassNum = activity[pre + "MaxPassNum"];
        var enableMoveInControl = activity[pre + "EnableMoveInControl"];
        var IsOutsourcing = activity[pre + "IsOutsourcing"];
        var TransferType = activity[pre + "TransferType"];
        nodeData.Text = activity[pre + "Text"];
        nodeData.IsSelected = activity[pre + "IsSelected"];
        nodeData.IsRepeat = activity[pre + "IsRepeat"];
        nodeData.CreateSku = activity[pre + "CreateSku"];
        nodeData.IsCalculate = activity[pre + "IsCalculate"];
        nodeData.IsOptional = activity[pre + "IsOptional"];
        nodeData.IsGenerateTask = activity[pre + "IsGenerateTask"];

        nodeData.IsRequirementTask = activity[pre + "IsRequirementTask"];

        nodeData.NormalVictory = normalVictory === 'null' ? "" : normalVictory;
        nodeData.RepairVictory = repairVictory === 'null' ? "" : repairVictory;
        nodeData.IsStricter = activity[pre + "IsStricter"];
        nodeData.IsBinding = activity[pre + "IsBinding"];
        nodeData.IsUnBinding = activity[pre + "IsUnBinding"];
        nodeData.MaxPassNum = maxPassNum === 'null' ? null : maxPassNum;

        nodeData.IsNextMoveIn = activity[pre + "IsNextMoveIn"];

        nodeData.IsOutsourcing = IsOutsourcing === 'null' ? false : IsOutsourcing;
        nodeData.EnableMoveInControl = enableMoveInControl === 'null' ? null : enableMoveInControl;
        nodeData.TransferType = TransferType === 'null' || TransferType === 'undefined' || TransferType === -1 ? null : TransferType;
    },

    /**
     * 反序列化
     * @method deserialize
     * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
     * @param {Object} nodeData 节点信息
     * @param {Object} activity 活动图
     * @param {String} pre 前缀
     */
    deserialize: function (nodeData, activity, pre) {
        var normalVictory = !nodeData.NormalVictory || nodeData.NormalVictory === null || nodeData.NormalVictory === 'undefined' ? null : nodeData.NormalVictory;
        var repairVictory = !nodeData.RepairVictory || nodeData.RepairVictory === null || nodeData.RepairVictory === 'undefined' ? null : nodeData.RepairVictory;
        var maxPassNum = !nodeData.MaxPassNum || nodeData.MaxPassNum === null || nodeData.MaxPassNum === 'undefined' ? null : nodeData.MaxPassNum;
        var enableMoveInControl = !nodeData.EnableMoveInControl || nodeData.EnableMoveInControl === null
            || nodeData.EnableMoveInControl === 'undefined' ? null : nodeData.EnableMoveInControl;
        var IsOutsourcing = !nodeData.IsOutsourcing || nodeData.IsOutsourcing === null
            || nodeData.IsOutsourcing === 'undefined' ? false : nodeData.IsOutsourcing;

        activity[pre + "Text"] = nodeData.Text;
        activity[pre + "IsSelected"] = nodeData.IsSelected || false;
        activity[pre + "IsRepeat"] = nodeData.IsRepeat || false;
        activity[pre + "CreateSku"] = nodeData.CreateSku || false;
        activity[pre + "IsCalculate"] = nodeData.IsCalculate || false;
        activity[pre + "IsOptional"] = nodeData.IsOptional || false;
        activity[pre + "IsGenerateTask"] = nodeData.IsGenerateTask || false;
        activity[pre + "IsRequirementTask"] = nodeData.IsRequirementTask || false;

        activity[pre + "NormalVictory"] = normalVictory;
        activity[pre + "RepairVictory"] = repairVictory;
        activity[pre + "IsStricter"] = nodeData.IsStricter || false;
        activity[pre + "IsBinding"] = nodeData.IsBinding || false;
        activity[pre + "IsUnBinding"] = nodeData.IsUnBinding || false;
        activity[pre + "MaxPassNum"] = maxPassNum;
        activity[pre + "EnableMoveInControl"] = enableMoveInControl;
        activity[pre + "IsOutsourcing"] = IsOutsourcing;
        activity[pre + "IsNextMoveIn"] = nodeData.IsNextMoveIn || false;
        activity[pre + "TransferType"] = nodeData.TransferType === null || nodeData.TransferType === 'undefined' || nodeData.TransferType === -1 ? null : nodeData.TransferType;
    },

    /**
    * 加载胜制方案
    * @method loadVictoryStandards
    * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
    */
    loadVictoryStandards: function (control, node) {
        var me = control;
        if (!node || node.nodeType !== 'RoutingNode')
            return;
        //检验类型工序采集加载
        if (node.designerData.ProcessType === "Pqc" || node.designerData.ProcessType === "Fqc") {
            var startProcessItems = me.items.items.where(function (item) { return item.name === 'NormalVictory' || item.name == 'RepairVictory'; });
            if (startProcessItems.length === 0)
                return;
            SIE.invokeDataQuery({
                method: 'GetVictoryStandards',
                action: 'queryer',
                type: 'SIE.Web.Tech.Routings.TechDataQueryer',
                token: me.mainView.token,
                success: function (res) {
                    var store = Ext.create('Ext.data.Store', {
                        data: res.Result,
                    });
                    startProcessItems.forEach(function (cbo) {
                        cbo.setStore(store);
                    });
                }
            });
        }
    },
    /**
    * 加载工序交接类型
    * @method loadTransferTypes
    * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
    */
    loadTransferTypes: function (control, node) {
        var me = control;
        if (!node || node.nodeType !== 'RoutingNode')
            return;
        var trProcessItems = me.items.items.where(function (item) { return item.name === 'TransferType'; });
        if (trProcessItems.length === 0)
            return;
        SIE.invokeDataQuery({
            method: 'GetTransferTypes',
            action: 'queryer',
            type: 'SIE.Web.Tech.Routings.TechDataQueryer',
            token: me.mainView.token,
            success: function (res) {
                var store = Ext.create('Ext.data.Store', {
                    data: res.Result,
                });
                trProcessItems.forEach(function (cbo) {
                    cbo.setStore(store);
                });
            }
        });

    },
    /**
     * 加载工序属性值
     * @method loadPropertyData
     * @for SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty
     * @param {Object} node 工序节点信息
     * @param {回调} disableCallback 工序属性是否可编辑
     */
    loadPropertyData: function (contorl, node, isDisable) {
        //1、未选中节点或者选中非工艺节点（开始结束之外的节点）：隐藏属性控件
        //2、选中工艺节点，设置属性值并根据工艺路线是否已发布，已发布的不能编辑
        var me = this; //当前属性控件
        var propertyNames = me.configs.select(function (item) { return item.name; });
        if (!propertyNames || propertyNames.length == 0)
            return;
        var model = contorl.getViewModel();
        var designerData = {};
        if (node !== undefined && node.nodeType === 'RoutingNode') {
            designerData = node.designerData;
            model.setData({ m: designerData, id: node.id });
            for (var i = 0; i < contorl.items.items.length; i++) {
                var item = contorl.items.items[i];
                if (!propertyNames.contains(item.name) || !designerData.hasOwnProperty(item.name))
                    continue;
                var value = designerData[item.name];
                if (value && value != null && value != 'undefined')
                    model.set('m.' + item.name, value);
                contorl.setVisible(item, true);
                contorl.setDisable(item, isDisable);
                if (contorl.isBatchProcess(designerData.ProcessType)) {
                    if (item.name === 'IsRepeat' || item.name === 'IsBinding' || item.name === 'IsUnBinding')
                        contorl.setVisible(item, false);
                    if (item.name === 'IsNextMoveIn') {
                        contorl.setVisible(item, true);
                    }
                }
                else {
                    if (item.name === 'IsNextMoveIn') {
                        contorl.setVisible(item, false);
                    }
                }
                if (designerData.ProcessType === 'Fix' || designerData.ProcessType === 'BatchFix') {
                    if (item.name === 'IsGenerateTask')
                        contorl.setVisible(item, false);  //维修和批次维修不显示是否生成任务单属性
                }
                if (designerData.ProcessType === 'Fix' || designerData.ProcessType === 'BatchFix') {
                    if (item.name === 'IsRequirementTask')
                        contorl.setVisible(item, false);  //维修和批次维修不显示是否需求任务清单
                }

                if (!(designerData.ProcessType === 'Pqc' || designerData.ProcessType === 'Fqc')) {
                    if (item.name === 'NormalVictory' || item.name === 'RepairVictory' || item.name === 'IsStricter')
                        contorl.setVisible(item, false);  //非检验工序，局数和合格局数不可见
                }
                if (item.name === 'IsOutsourcing') {
                    for (var k = 0; k < contorl.items.items.length; k++) {
                        var enableMoveInControl = contorl.items.items[k];
                        if (enableMoveInControl.name == "EnableMoveInControl") {
                            if (value === 'true') {
                                enableMoveInControl.setReadOnly(true);
                            } else {
                                enableMoveInControl.setReadOnly(false);
                            }
                        }
                    }
                }
            }
        }
    },
});
(function () {
    var property = new SIE.Web.Tech.Scripts.Routings.RoutingProcessProperty();
    SIE.Web.Tech.Common.Routings.PropertyExt.addProcessProperty(property);
}());