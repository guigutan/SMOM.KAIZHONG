using SIE.ObjectModel;

namespace SIE.EMS.Budgets
{
    /// <summary>
    /// 预算等级
    /// </summary>
    public enum BudgeGrade
	{
		/// <summary>
		/// 一级
		/// </summary>
		[Label("一级")]
		FirstLevel = 10,
		/// <summary>
		/// 二级
		/// </summary>
		[Label("二级")]
		SecondLevel = 20,
	}
}