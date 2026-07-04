using SIE.ObjectModel;

namespace SIE.Packages.QrCodeParseRules
{
    /// <summary>
    /// 截取方式
    /// </summary>
    public enum InterceptWay
	{
		/// <summary>
		/// 截取位
		/// </summary>
		[Label("截取位")]
		InterceptDigit,

		/// <summary>
		/// 分隔符
		/// </summary>
		[Label("分隔符")]
		Separator,
	}
}
