Ext.define('SIE.Web.Common.NumberRules.ItemCategoryLevelMethod', {
    extend: 'SIE.control.PagingLookUpMethod',

    //选择分类层级下拉框时，自动带出分类类型。
    setValue: function (value) {
        var me = this.control;
        if (value !== null && value.data) {
            if (value.data.Type == 0)
                me.up().context.record.set("Type", "0");
            if (value.data.Type == 1)
                me.up().context.record.set("Type", "1");
            if (value.data.Type == 2)
                me.up().context.record.set("Type", "2");
        }
        this.callParent(arguments);
    },

    /**
     * 生成请求参数据
     * @param {any} dsp
     */
    _searchByDSPfilter: function (dsp) {
        var me = this.control;
        var rec = this._getContainerRecord();
        var parentNode = rec.parentNode;
        var data = parentNode.data;
        if (data.root)
            data.LevelId = -1;
        var filter = {
            Parameters: {
                EntityType: this._getSIEView().model,
                Entity: data,
                DataSourceProperty: dsp
            }
        };
        return filter;
    },
});