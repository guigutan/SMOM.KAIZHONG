Ext.define('SIE.Web.MES.QTimes.Scripts.Common', {
    statics: {
        // 非包装单体工序
        singleProcessList: [
            0, //检验
            10, // 维修
            13, // 返工
            15, // 装配
            20, // 包装
            22, // 老化
        ],
        // 非包装批次工序
        batchProcessList: [
            25, // 批次装配
            30, // 批次检验
            35, // 批次维修
            40, // 批次包装
        ],
        ProcessIsMoveIn: function (processId, token) {
            var isIn = false;
            SIE.invokeDataQuery({
                method: 'ProcessIsMoveIn',
                params: [processId],
                action: 'queryer',
                type: 'SIE.Web.MES.QTimes.DataQueryers.QTDataQueryer',
                async:false,
                token: token,
                success: function (res) {
                    isIn = res.Result;
                }
            });
            return isIn;
        },
    }
});
