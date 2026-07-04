using SIE.ObjectModel;

namespace SIE.Wpf.MES.Controls.Messager
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
		/// 正常
		/// </summary>
		[Label("正常")]
        Normal,
        /// <summary>
		/// 错误
		/// </summary>
		[Label("错误")]
        Error,
        /// <summary>
		/// 成功
		/// </summary>
		[Label("成功")]
        Success,
    }
}
