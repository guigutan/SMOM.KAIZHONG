using SIE.ObjectModel;

namespace SIE.EMS.EquipRepairs.Enums
{
    /// <summary>
    /// 维修状态
    /// </summary>
    public enum EquipRepairState
    {
        /// <summary>
		/// 报修
		/// </summary>
		[Label("报修")]
        ApplyRepair = 0,

        /// <summary>
        /// 待维修
        /// </summary>
        [Label("待维修")]
        WaitRepair = 1,

        /// <summary>
        /// 维修中
        /// </summary>
        [Label("维修中")]
        Repairing = 2,

        /// <summary>
        /// 待确认
        /// </summary>
        [Label("待确认")]
        WaitConfirm = 3,

        /// <summary>
        /// 待评分
        /// </summary>
        [Label("待评分")]
        WaitScore = 4,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Completed = 5,

        /// <summary>
        /// 暂停中
        /// </summary>
        [Label("暂停中")]
        Suspending = 6,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel = 7,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Closed = 8,
    }
}
