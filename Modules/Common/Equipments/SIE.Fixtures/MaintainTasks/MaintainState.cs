using SIE.ObjectModel;

namespace SIE.Fixtures.MaintainTasks
{
    /// <summary>
	/// 保养状态
	/// </summary>
	public enum MaintainState
    {
        /// <summary>
        /// 待保养
        /// </summary>
        [Label("待保养")]
        Wait = 5,

        /// <summary>
        /// 部分保养
        /// </summary>
        [Label("部分保养")]
        Part = 10,

        /// <summary>
        /// 保养完成
        /// </summary>
        [Label("保养完成")]
        Finish = 15,
    }
}
