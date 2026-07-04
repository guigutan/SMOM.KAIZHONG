using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
	{
		/// <summary>
		/// 创建
		/// </summary>
		[Label("创建")]
		CREATE = 10,
		/// <summary>
		/// 检验录入
		/// </summary>
		[Label("检验录入")]
		INPUT = 20,
		/// <summary>
		/// 提交
		/// </summary>
		[Label("提交")]
		SUBMIT = 30,
	}
}
