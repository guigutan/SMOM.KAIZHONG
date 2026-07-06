/**
 * SPC明细表行为（Behavior）
 * 功能：根据父实体的"子组大小"(GroupCount)动态隐藏多余的观测值列
 * 例如：GroupCount=5时，只显示子组1~5列，子组6~30列自动隐藏
 */
Ext.define("SIE.Web.MES.SpcFromMesDetails.Behaviors.SpcFromMesDetailBehavior", {
    /**
     * 视图创建前钩子：遍历表格列配置，将序号大于GroupCount的ObservedValue列设为隐藏
     */
    beforeCreate: function (viewmeta, curEntity) {
        var parentRecord = CRT.Context.PageContext.getCurrentRecord();
        var groupCount = parentRecord ? (parentRecord.get('GroupCount') || 0) : 0;
        var gridConfig = viewmeta.gridConfig;
        gridConfig.columns.forEach(function (columnConfig) {
            var match = columnConfig.dataIndex && columnConfig.dataIndex.match(/^ObservedValue(\d+)$/);
            if (match) {
                var index = parseInt(match[1], 10);
                if (index > groupCount) {
                    columnConfig.hidden = true;
                }
            }
        });
    }
});
