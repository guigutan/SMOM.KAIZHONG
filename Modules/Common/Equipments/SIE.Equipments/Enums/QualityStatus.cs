using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 质量状态
    /// </summary>
    public enum QualityStatus
	{
		/// <summary>
		/// 良品
		/// </summary>
		[Label("良品")]
		Good = 10,
		/// <summary>
		/// 不良品
		/// </summary>
		[Label("不良品")]
		Defective = 20,
	}
}