/**
 * 工艺路线工序子页签扩展静态类
 * @for SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension
 */
Ext.define('SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension', {
    statics: {
        /**
         * 子页签控件配置类集合
         * @property {Array} ctlConfigs
         */
        ctlConfigs: [],

        /**
         * 子页签控件类集合
         * @property {Array} controls
         */
        controls: [],

        /**
         * 获取子页签控件配置
         * @method getChildControlConfig
         * @for SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension
         */
        getChildControlConfig: function () {
            return SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.ctlConfigs.orderBy(function (config) {
                return config.index;
            });
        },

        /**
         * 添加子页签控件配置
         * @method addChildControlConfig
         * @for SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension
         */
        addChildControlConfig: function (ctlConfig) {
            SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.ctlConfigs.push(ctlConfig);
        }
    },
}); 