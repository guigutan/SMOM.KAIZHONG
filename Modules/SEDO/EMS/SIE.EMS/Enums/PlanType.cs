using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 计划规则
    /// </summary>
    public enum PlanType
	{
		/// <summary>
		/// 基准日期
		/// </summary>
		[Label("基准日期")]
		BaseDate = 20,
		/// <summary>
		/// 完工日期
		/// </summary>
		[Label("完工日期")]
		CompleteDate = 10,
	}
}