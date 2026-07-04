using SIE.ObjectModel;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 委外扣料时点
    /// </summary>
    public enum OutsourcingTimeType
    {
        /// <summary>
        /// 收货
        /// </summary>
        [Label("收货")]
        Receipt = 0,

        /// <summary>
        /// 质检合格
        /// </summary>
        [Label("质检合格")]
        InspectionOK = 1,

        /// <summary>
        /// 质检完成
        /// </summary>
        [Label("质检完成")]
        FinishInspection = 2,
    }
}
