using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 解析栏位
    /// </summary>
    public enum LotType
	{		
		/// <summary>
		/// 批次号
		/// </summary>
		[Label("批次号")]
		LotCode,
		
		/// <summary>
		/// 生产日期
		/// </summary>
		[Label("生产日期")]
		ProductionDate,

		/// <summary>
		/// 失效日期
		/// </summary>
		[Label("失效日期")]
		InvalidDate,

		/// <summary>
		/// 收货日期
		/// </summary>
		[Label("收货日期")]
		CollectDate,

		/// <summary>
		/// 生产批次
		/// </summary>
		[Label("生产批次")]
		ProductBatch,

		/// <summary>
		/// 复检次数
		/// </summary>
		[Label("复检次数")]
		RecheckCount,

		/// <summary>
		/// 批次属性06
		/// </summary>
		[Label("批次属性06")]
		LotAtt06,

		/// <summary>
		/// 批次属性07
		/// </summary>
		[Label("批次属性07")]
		LotAtt07,

		/// <summary>
		/// 批次属性08
		/// </summary>
		[Label("批次属性08")]
		LotAtt08,

		/// <summary>
		/// 批次属性09
		/// </summary>
		[Label("批次属性09")]
		LotAtt09,

		/// <summary>
		/// 批次属性10
		/// </summary>
		[Label("批次属性10")]
		LotAtt10,

		/// <summary>
		/// 批次属性11
		/// </summary>
		[Label("批次属性11")]
		LotAtt11,

		/// <summary>
		/// 批次属性12
		/// </summary>
		[Label("批次属性12")]
		LotAtt12,		 
	}
}
