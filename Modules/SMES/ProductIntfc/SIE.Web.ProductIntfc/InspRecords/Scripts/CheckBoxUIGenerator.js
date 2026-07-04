Ext.define('SIE.Web.ProductIntfc.InspRecords.Scripts.CheckBoxUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    /**
     * 生成控件
     * @param aggtMeta 聚合块元数据
     * @param entity 实体
     * @returns 聚合控件
     */
    generateControl: function (aggtMeta, entity) {
        this.addCheckBox(aggtMeta);
        return this.callParent(arguments);
    },

    /**
     * 设置从表历史数据灰色处理
     * @param aggtMeta 聚合块元数据
     */
    addCheckBox: function (aggtMeta) {
        var firstChildren = aggtMeta.children[0];
        if (!firstChildren)
            return;
        if (firstChildren.mainBlock.model != 'SIE.ProductIntfc.InspRecords.InspBarcode')
            return;
        var gridConfig = firstChildren.mainBlock.gridConfig;
        gridConfig.selModel = {
            injectCheckbox: 0, //checkbox位于哪一列，默认值为0
            selType: 'checkboxmodel', //checkbox
            checkOnly: true, //只能通过checkbox选择
            mode: 'MULTI'   //是否多选
        };
    }
});