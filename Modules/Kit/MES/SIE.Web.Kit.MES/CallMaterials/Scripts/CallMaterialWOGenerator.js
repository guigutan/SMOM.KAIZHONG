Ext.define('SIE.Web.Kit.MES.CallMaterials.Scripts.CallMaterialWOGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    /**
     * 生成控件
     * @param aggtMeta 聚合块元数据
     * @param entity 实体
     * @returns 聚合控件
     */
    generateControl: function (aggtMeta, entity) {
        this.setRowStyle(aggtMeta);
        return this.callParent(arguments);
    },

    /**
     * 设置从表历史数据灰色处理
     * @param aggtMeta 聚合块元数据
     */
    setRowStyle: function (aggtMeta) {
        var main = aggtMeta;
        if (!main)
            return;
        if (main.mainBlock.model != 'SIE.Kit.MES.CallMaterials.CallMaterialWorkOrder')
            return;
        var gridConfig = main.mainBlock.gridConfig;
        gridConfig.viewConfig = {
            getRowClass: function (record, index, rowParams, store) {
                if (record.data.IsSendedFail)
                    return 'icon-red';
            }
        };
    },
    _layout: function (aggtMeta, regions) {
        /// <summary>
        /// 对所有区域进行布局。
        /// </summary>
        /// <param name="aggtMeta" type="SIE.Web.ClientMetaModel.ClientAggtMeta"></param>
        /// <param name="regions" type="SIE.autoUI.Regions"></param>
        /// <returns type="Ext.Component" />
        var layout = null;
        if (aggtMeta.layoutClass) {
            layout = Ext.create(aggtMeta.layoutClass);
        }
        else {
            layout = new SIE.Web.Kit.MES.CallMaterials.Scripts.CallMaterialWorkOrderLayout();
        }

        var res = layout.layout(regions);

        return res;
    }
});