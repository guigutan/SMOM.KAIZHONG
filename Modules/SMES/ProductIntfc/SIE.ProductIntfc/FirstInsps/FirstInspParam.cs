using SIE.ObjectModel;

namespace SIE.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 首检报检参数
    /// </summary>
    public enum FirstInspParam
    {
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        WorkOrder,

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        WipResource,

        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        ProductionDate,

        /// <summary>
        /// 检验结果不合格重新报检
        /// </summary>
        [Label("检验结果不合格重新报检")]
        InspNGRework,

        /// <summary>
        /// 任务单
        /// </summary>
        [Label("任务单")]
        DispatchTaskBill
    }
}