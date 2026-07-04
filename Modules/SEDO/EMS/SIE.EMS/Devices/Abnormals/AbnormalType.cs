using SIE.ObjectModel;

namespace SIE.EMS.Devices.Abnormals
{
    /// <summary>
	/// 类型
	/// </summary>
	public enum AbnormalType
    {
        /// <summary>
        /// 故障现象
        /// </summary>
        [Label("故障现象")]
        Unusual = 5,

        /// <summary>
        /// 故障描述
        /// </summary>
        [Label("故障描述")]
        Fault = 10,
    }
}
