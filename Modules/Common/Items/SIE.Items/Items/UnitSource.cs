using SIE.ObjectModel;

namespace SIE.Items.Items
{
    /// <summary>
    /// 单位来源
    /// </summary>
    public enum UnitSource
	{
		/// <summary>
		/// 手工添加
		/// </summary>
		[Label("手工添加")]
		Manaul,

		/// <summary>
		/// 基准单位
		/// </summary>
		[Label("基准单位")]
		BaseUnit,

		/// <summary>
		/// ERP同步
		/// </summary>
		[Label("ERP同步")]
		ERP,

        /// <summary>
        /// 接口添加
        /// </summary>
        [Label("接口添加")]
		Interface
	}
}