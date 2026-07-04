Ext.define("SIE.Web.MES.WorkOrderArchives.Behaviors.ItemShortBehavior", {
    beforeCreate: function (viewmeta, curEntity) {
        var gridConfig = viewmeta.gridConfig;
        gridConfig.columns.forEach(function (columnConfig) {
            //指定列
            if (columnConfig.dataIndex === 'ShortQty') {
                //数据列配置
                var config = {
                    //绘制函数
                    renderer: function (value, cellmeta, record, rowIndex, colIndex, store, view) {//设置列样式
                        if (record.data.ShortQty != null && record.data.ShortQty > 0) {
                            //设置单元格样式
                            cellmeta.style = "color:#F5171A";
                        }
                        return value;
                    }
                };
                Ext.merge(columnConfig, config);
            }
        });
    },
});
