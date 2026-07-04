/**
 * 工艺路线工序属性扩展静态类
 * @class SIE.Web.Tech.Common.Routings.PropertyExt
 * @constructor
 */
Ext.define('SIE.Web.Tech.Common.Routings.PropertyExt', {
    statics: {
        /**
         * 属性定义类集合
         * @property {Array} properties
         */
        properties: [],

       /**
        * 获取属性配置
        * @method getPropertyConfigs
        * @for SIE.Web.Tech.Common.Routings.PropertyExt
        */
        getPropertyConfigs: function () {
            var configs = [];
            SIE.Web.Tech.Common.Routings.PropertyExt.properties.forEach(function (propertity) {
                propertity.configs.forEach(function (config) {
                    configs.push(config);
                });
            });
            return configs.orderBy(function (config) {
                return config.index;
            });
        },

       /**
        * 添加工序属性
        * @method addProcessProperty
        * @for SIE.Web.Tech.Common.Routings.PropertyExt
        * @param {SIE.Web.Tech.Common.Routings.RoutingProcessPropertyInfo} property 属性配置
        */
        addProcessProperty: function (property) {
            SIE.Web.Tech.Common.Routings.PropertyExt.properties.push(property);
        }
    },
});

/**
 * 工艺路线工序属性信息
 * @class SIE.Web.Tech.Common.Routings.RoutingProcessPropertyInfo
 * @constructor
 */
Ext.define('SIE.Web.Tech.Common.Routings.RoutingProcessPropertyInfo', {
    
    /**
     * 属性配置
     * @property {Array} configs
     */
    configs: [],

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
     * @for SIE.Web.Elec.Fixture.Fixtures.Demands.Scripts.FixtureDemandControl
     * @param {DesignCanvas} control DesignCanvas
     * @param {Object} node 工序节点信息
     * @param {Boolean} isDisable 是否可编辑
     */
    loadData: function (control, node, isDisable) {
    },

    /**
     * 验证
     * @method validate
     * @for SIE.Web.Tech.Common.Routings.RoutingProcessPropertyInfo
     * @param {DesignCanvas} canvas 节点信息
     * @param {Array} nodes 节点集合
     * @param {Array} lines 连线集合
     */
    validate: function (canvas, nodes, lines) {
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
    }
});