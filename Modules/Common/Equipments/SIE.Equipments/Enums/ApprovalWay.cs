using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 审批方式
    /// </summary>
    public enum ApprovalWay
    {
        /// <summary>
		/// 流程审批
		/// </summary>
		[Label("流程审批")]
        WorkFlow=5,

        /// <summary>
        /// 固定审批
        /// </summary>
        [Label("固定审批")]
        Fixed =10


    }
}
