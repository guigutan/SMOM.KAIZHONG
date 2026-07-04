using SIE.ObjectModel;

namespace SIE.Fixtures.MaintainTasks
{
    /// <summary>
	/// 保养触发条件
	/// </summary>
	public enum MaintainType
    {
        /// <summary>
        /// 上线定期保养
        /// </summary>
        [Label("上线定期保养")]
        Regular = 5,

        /// <summary>
        /// 入库保养
        /// </summary>
        [Label("入库保养")]
        InStorage = 10,

        /// <summary>
        /// 常规保养
        /// </summary>
        [Label("常规保养")]
        Common = 15,

        /// <summary>
        /// 出库保养
        /// </summary>
        [Label("出库保养")]
        ToStorage = 20,
    }
}
