/**
 * 工艺路线工序属性电子行业扩展
 * @class SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension
 * @constructor
 */
Ext.define('SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension', {
    extend: 'SIE.Web.Tech.Common.Routings.RoutingProcessPropertyInfo',
    /**
     * 属性配置
     * @property {Array} configs
     */
    configs: [{
        xtype: 'checkboxfield',
        fieldLabel: '是否扣料'.L10N(),
        name: 'IsBuckleMaterial',
        bind: '{m.IsBuckleMaterial}',
        hidden: true,
        index: 55
    }, {
        xtype: 'combobox',
        fieldLabel: '起始工序'.L10N(),
        name: 'StartProcess',
        bind: '{m.StartProcess}',
        displayField: 'Name',
        valueField: 'Id',
        hidden: true,
        index: 82
    }, {
        xtype: 'numberfield',
        fieldLabel: '超时时间（分钟）'.L10N(),
        name: 'Overtime',
        bind: '{m.Overtime}',
        minValue: 1,
        allowDecimals: false,
        hidden: true,
        index: 84
    }, {
        xtype: 'checkboxfield',
        fieldLabel: '直通率取值'.L10N(),
        name: 'IsPassRate',
        bind: '{m.IsPassRate}',
        hidden: true,
        index: 86
    }],

    /**
     * 重置控件
     * @method resetControl
     * @for SIE.Web.Elec.Fixture.Fixtures.Demands.Scripts.FixtureDemandControl
     * @param {DesignCanvas} control DesignCanvas
     */
    resetControl: function (control) {
    },

    /**
     * 加载数据
     * @method loadData
     * @for SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension
     * @param {DesignCanvas} control DesignCanvas
     * @param {Object} node 工序节点信息
     * @param {Boolean} isDisable 是否可编辑
     */
    loadData: function (control, node, isDisable) {
        this.setStartProcessData(control, node);
        this.loadPropertyData(control, node, isDisable);
    },

    /**
     * 验证
     * @method validate
     * @for SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension
     * @param {DesignCanvas} canvas 节点信息
     * @param {Array} nodes 节点集合
     * @param {Array} lines 连线集合
     */
    validate: function (canvas, nodes, lines) {
        this.validateBuckleMeterial(canvas, nodes);
        this.validateStartProcess(canvas, nodes);
    },

    /**
     * 序列化
     * @method serialize
     * @for SIE.Web.Elec.Fixture.Fixtures.Demands.Scripts.FixtureDemandControl
     * @param {Object} nodeData 节点信息
     * @param {Object} activity 活动图
     * @param {String} pre 前缀
     */
    serialize: function (nodeData, activity, pre) {
        var overtime = activity[pre + "Overtime"];
        var startProcess = activity[pre + "StartProcess"];
        nodeData.IsBuckleMaterial = activity[pre + "IsBuckleMaterial"];
        nodeData.StartProcess = startProcess === 'null' ? null : startProcess;
        nodeData.Overtime = overtime === 'null' ? null : overtime;
        nodeData.IsPassRate = activity[pre + "IsPassRate"];
    },

    /**
     * 反序列化
     * @method deserialize
     * @for SIE.Web.Elec.Fixture.Fixtures.Demands.Scripts.FixtureDemandControl
     * @param {Object} nodeData 节点信息
     * @param {Object} activity 活动图
     * @param {String} pre 前缀
     */
    deserialize: function (nodeData, activity, pre) {
        var startProcess = !nodeData.StartProcess || nodeData.StartProcess === null || nodeData.StartProcess === 'undefined' ? null : nodeData.StartProcess;
        var overtime = !nodeData.Overtime || nodeData.Overtime === null || nodeData.Overtime === 'undefined' ? null : nodeData.Overtime;
        activity[pre + "StartProcess"] = startProcess;
        activity[pre + "IsBuckleMaterial"] = nodeData.IsBuckleMaterial || false;
        activity[pre + "Overtime"] = overtime;
        activity[pre + "IsPassRate"] = nodeData.IsPassRate || false;
    },

    /**
    * 设置起始工序数据
    * @method setStartProcessData
    * @for SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension
    * @param {Object} node 工序节点信息
    */
    setStartProcessData: function (control, node) {
        var me = control;
        if (!node || node.nodeType !== 'RoutingNode')
            return;
        var startProcessItem = me.items.items.first(function (item) { return item.name === 'StartProcess'; });
        if (!startProcessItem)
            return;
        var datas = me.mainView.layout.designControl.designCanvas.getPreProcessInfo(node);
        var store = Ext.create('Ext.data.Store', {
            data: datas,
        });
        startProcessItem.setStore(store);
    },

    /**
     * 加载工序属性值
     * @method loadPropertyData
     * @for SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension
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
            model.setData({ m: designerData });
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
                    if (item.name === 'IsBuckleMaterial' || item.name === 'StartProcess' || item.name === 'Overtime' || item.name === 'IsPassRate')
                        contorl.setVisible(item, false);
                }
                if (!(designerData.ProcessType === 'Pqc' || designerData.ProcessType === 'Fqc')) {
                    if (item.name === 'IsPassRate')
                        contorl.setVisible(item, false);  //只有检验类型工序显示直通率取值勾选项
                }
            }
        }
    },

    /**
     * 验证扣料工序
     * @method validateBuckleMeterial
     * @for SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension
     * @param {Array} nodes 节点信息
     */
    validateBuckleMeterial: function (control, nodes) {
        ////验证工艺路线只能有一个扣料工序
        var result = nodes.where(function (p) {
            return p.nodeType === 'RoutingNode' && (p.designerData.IsBuckleMaterial === true || p.designerData.IsBuckleMaterial === 'true');
        });
        if (result.length > 1)
            throw new Error(Ext.String.format('只能存在一个扣料工序\r已存在扣料工序[{0}]'.L10N(), result.select(function (p) {
                return p.designerData.Text;
            }).join(';')));
    },

    /**
     * 验证起始工序
     * @method validateStartProcess
     * @for SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension
     * @param {Array} nodes 节点信息
     */
    validateStartProcess: function (control, nodes) {
        var me = control;
        var processNodes = nodes.where(function (p) { return p.nodeType === 'RoutingNode' });
        processNodes.forEach(function (node) {
            //验证起始工序，超时时间
            if (node.designerData.StartProcess) {
                var preNodes = me.getPreProcessInfo(node);
                var existNode = preNodes.first(function (preNode) { return preNode.Id.toString() === node.designerData.StartProcess.toString() });
                if (!existNode)
                    throw new Error(Ext.String.format('工序[{0}]起始工序不存在，请重新选择'.L10N(), node.designerData.Text));
                if (!node.designerData.Overtime)
                    throw new Error(Ext.String.format('工序[{0}]超时时间不能为空'.L10N(), node.designerData.Text));
            }
            if (node.designerData.Overtime) {
                if (node.designerData.Overtime && node.designerData.Overtime <= 0)
                    throw new Error(Ext.String.format('工序[{0}]超时时间不能小于等于0'.L10N(), node.designerData.Text));
                if (!node.designerData.StartProcess)
                    throw new Error(Ext.String.format('工序[{0}]起始工序不能为空'.L10N(), node.designerData.Text));
            }
        });
    },
});
(function () {
    var property = new SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessPropertyExtension();
    //SIE.Web.Tech.Common.Routings.PropertyExt.addProcessProperty(property);
}());