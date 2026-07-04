/**
 * 员工技能认证管理UI生成器
 * 设置从表历史数据灰色处理
 */
Ext.define('SIE.Web.MES.TeamManagement.SikllAuthentications.SkillAuthUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    /**
     * 生成控件
     * @param aggtMeta 聚合块元数据
     * @param entity 实体
     * @returns 聚合控件
     */
    generateControl: function (aggtMeta, entity) {
        this.setStyle(aggtMeta);
        return this.callParent(arguments);
    },

    /**
     * 设置从表历史数据灰色处理
     * @param aggtMeta 聚合块元数据
     */
    setStyle: function (aggtMeta) {
        aggtMeta.children[0]
        var children = aggtMeta.children;
        if (children.length === 0)
            return;
        Ext.util.CSS.createStyleSheet('.bg_disable{ background:#F0F0F0;}');
        for (var i = 0; i < children.length; i++) {
            var child = children[i];
            if (child.mainBlock.model === 'SIE.Resources.Skills.EmployeeSkill')
                continue;
            child.mainBlock.gridConfig.viewConfig = {
                getRowClass: function (record, index, rowParams, store) {
                    if (record.data.IsHistory)
                        return 'bg_disable';
                }
            };
        }
    }
});