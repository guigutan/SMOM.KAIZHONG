using SIE.ObjectModel;

namespace SIE.Fixtures.Fixtures.Abnormals
{
    /// <summary>
	/// 类型
	/// </summary>
	public enum AbnormalType
    {
        /// <summary>
        /// 异常现象
        /// </summary>
        [Label("异常现象")]
        Unusual = 5,

        /// <summary>
        /// 故障类型
        /// </summary>
        [Label("故障类型")]
        Fault = 10,
    }
}
