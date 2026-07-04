Ext.define('SIE.Web.Core.Behaviors.GridClickOnceToClickBehavior',
    {
        beforeCreate: function (meta) {
            var config = meta.gridConfig;

            if (!Ext.isEmpty(config.plugins)) {
                var cellConfig = config.plugins.first(function (p) { return p.ptype == "cellediting" });
                if (cellConfig) {
                    cellConfig.clicksToEdit = 1; //改为一次点击触发编辑
                }
            }
        },
    });