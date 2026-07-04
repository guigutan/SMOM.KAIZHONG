using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.Enums
{
    /// <summary>
    /// 任务方向
    /// </summary>
    public enum JobDirection
    {
        /// <summary>
        /// ERP到中间表
        /// </summary>
        [Label("ERP到中间表")]
        ErpToInf = 0,

        /// <summary>
        /// 中间表到业务表
        /// </summary>
        [Label("中间表到业务表")]
        InfToBusiness = 1,

        /// <summary>
        /// 事务表到中间表
        /// </summary>
        [Label("事务表到中间表")]
        TransToInf = 2,

        /// <summary>
        /// 中间表到ERP
        /// </summary>
        [Label("中间表到ERP")]
        InfToErp = 3,

        /// <summary>
        /// ERP到业务表
        /// </summary>
        [Label("ERP到业务表")]
        ErpToBusiness = 4,
    }
}