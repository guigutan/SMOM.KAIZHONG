using SIE.ObjectModel;

namespace SIE.EventMessages.QMS.Models
{
	/// <summary>
	/// 改善类别
	/// </summary>
	public enum RecoveryType
    {
		/// <summary>
		/// 质量审核
		/// </summary>
		[Label("质量审核")]
		QA,
		/// <summary>
		/// 异常管理
		/// </summary>
		[Label("异常管理")]
        AbnormalInfo,

        /// <summary>
        /// 改善建议
        /// </summary>
        [Label("单据")]
        InspBill,
    }
}