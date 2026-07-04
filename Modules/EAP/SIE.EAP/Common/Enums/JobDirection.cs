using SIE.ObjectModel;

namespace SIE.EAP.Common.Enums
{
    /// <summary>
    /// 任务方向
    /// </summary>
    public enum JobDirection
    {
        /// <summary>
        /// EAP到中间表
        /// </summary>
        [Label("EAP到中间表")]
        EapToInf = 0,

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
        /// 中间表到EAP
        /// </summary>
        [Label("中间表到EAP")]
        InfToEap = 3,

        /// <summary>
        /// EAP到业务表
        /// </summary>
        [Label("EAP到业务表")]
        EapToBusiness = 4,

        /// <summary>
        /// 业务表到EAP
        /// </summary>
        [Label("业务表到EAP")]
        BusinessToEap = 5,

        /// <summary>
        /// 立库到业务表
        /// </summary>
        [Label("立库到业务表")]
        JiWSToBusiness = 6,

        /// <summary>
        /// 业务表到立库
        /// </summary>
        [Label("业务表到立库")]
        BusinessToJiWS = 7
    }
}