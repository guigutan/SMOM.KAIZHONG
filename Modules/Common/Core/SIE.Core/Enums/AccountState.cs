using SIE.ObjectModel;

namespace SIE.Core.Enums
{
    /// <summary>
	/// 台账状态
	/// </summary>
	public enum AccountState
    {
        /// <summary>
        /// 运行
        /// </summary>
        [Label("运行")]
        Running = 0,

        /// <summary>
        /// 停机
        /// </summary>
        [Label("停机")]
        Downtime = 1,

        /// <summary>
        /// 故障
        /// </summary>
        [Label("故障")]
        Fault = 2,

        /// <summary>
        /// 带病作业 
        /// </summary>
        [Label("带病作业")]
        RunWithFault = 3,
    }
}
