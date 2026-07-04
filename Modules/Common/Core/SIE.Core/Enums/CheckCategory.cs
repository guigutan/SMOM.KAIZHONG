using SIE.ObjectModel;

namespace SIE.Core.Enums
{
	/// <summary>
	/// 检验类别
	/// </summary>
	public enum CheckCategory
	{
		/// <summary>
		/// 内检
		/// </summary>
		[Label("内检")]
		InnerCheck,

		/// <summary>
		/// 外校
		/// </summary>
		[Label("外检")]
		OutCheck,
	}
}