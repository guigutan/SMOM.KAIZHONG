using SIE.ObjectModel;

namespace SIE.Fixtures.Repairs
{
    /// <summary>
	/// 单据状态
	/// </summary>
	public enum RepairState
    {
        /// <summary>
        /// 待维修
        /// </summary>
        [Label("待维修")]
        Wait = 5,

        /// <summary>
        /// 部分维修
        /// </summary>
        [Label("部分维修")]
        Part = 10,

        /// <summary>
        /// 维修完成
        /// </summary>
        [Label("维修完成")]
        Finish = 15,
    }
}
