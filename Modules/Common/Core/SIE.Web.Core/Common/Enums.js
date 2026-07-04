
/**
 * 处理状态
 */
Ext.define('SIE.Core.Enums.ProcessStatus', {
    statics: {
		/// <summary>
		/// 待发起
		/// </summary>
		ToDo : 0,
		/// <summary>
		/// 已发起
		/// </summary>
		Doing : 1,
		/// <summary>
		/// 完成
		/// </summary>
		Done : 2,
		/// <summary>
		/// 终止
		/// </summary>
		Termination : 3,
		/// <summary>
		/// 驳回
		/// </summary>
		Reject : 4,
    }
});