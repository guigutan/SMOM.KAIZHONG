using SIE.ObjectModel;

namespace SIE.Equipments.Abnormal
{
    /// <summary>
    /// 异常停线类别
    /// </summary>
    public enum ExceptionStopType
    {
        /// <summary>
		/// 正常
		/// </summary>
		[Label("正常")]
        Normal = 0,

        /// <summary>
        /// 停线
        /// </summary>
        [Label("停线")]
        StopLine = 1,

        /// <summary>
        /// 保养
        /// </summary>
        [Label("保养")]
        Maintain = 2,
    }

    /// <summary>
    /// 异常停线来源
    /// </summary>
    public enum ExceptionStopSourceType
    {
        /// <summary>
        /// 自定义
        /// </summary>
        [Label("自建")]
        UICreate = 0,

        /// <summary>
        /// 安灯异常停线
        /// </summary>
        [Label("安灯异常停线")]
        AlertLight = 1,

        /// <summary>
        /// 预警
        /// </summary>
        [Label("预警")]
        Alerter,

        /// <summary>
        /// 不合格审核
        /// </summary>
        [Label("不合格审核")]
        FailedAudit
    }
}
