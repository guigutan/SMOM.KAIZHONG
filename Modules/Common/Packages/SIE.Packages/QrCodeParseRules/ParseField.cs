using SIE.ObjectModel;

namespace SIE.Packages.QrCodeParseRules
{
    /// <summary>
    /// 解析栏位
    /// </summary>
    public enum ParseField
	{
		/// <summary>
		/// ReelID
		/// </summary>
		[Label("ReelID")]
		ReelID,

		/// <summary>
		/// 批次号
		/// </summary>
		[Label("批次号")]
		LotCode,

		/// <summary>
		/// 物料编码
		/// </summary>
		[Label("物料编码")]
		ItemCode,

		/// <summary>
		/// 数量
		/// </summary>
		[Label("数量")]
		Qty,

		/// <summary>
		/// ASN单号
		/// </summary>
		[Label("ASN单号")]
		AsnNo,

		/// <summary>
		/// 包装单位
		/// </summary>
		[Label("包装单位")]
		PackUnit,

		/// <summary>
		/// 物料包装规则
		/// </summary>
		[Label("物料包装规则")]
		ItemPkgRule,

		/// <summary>
		/// 物料扩展属性
		/// </summary>
		[Label("物料扩展属性")]
		ItemExtProp,

		/// <summary>
		/// 规格型号
		/// </summary>
		[Label("规格型号")]
		ItemSpecificationModel,

		/// <summary>
		/// 供应商
		/// </summary>
		[Label("供应商")]
		Supplier,

		/// <summary>
		/// 客户
		/// </summary>
		[Label("客户")]
		Customer,

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
		/// 批次属性05
		/// </summary>
		[Label("批次属性05")]
		LotAtt05,

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

		/// <summary>
		/// 项目号
		/// </summary>
		[Label("项目号")]
		ProjectNo,

		/// <summary>
		/// 采购订单号
		/// </summary>
		[Label("采购订单号")]
		PoNo,

		/// <summary>
		/// 采购明细行号
		/// </summary>
		[Label("采购明细行号")]
		PoDtlLineNo,

		/// <summary>
		/// 部门编号
		/// </summary>
		[Label("部门编号")]
		EnterpriseCode,

		/// <summary>
		/// 供应商送货单号
		/// </summary>
		[Label("供应商送货单号")]
		SupplierBillNo,
		
		/// <summary>
		/// 净重
		/// </summary>
		[Label("净重")]
		Weight,

		/// <summary>
		/// 包装数
		/// </summary>
		[Label("包装数")]
		PackedQty,

		/// <summary>
		/// 流水
		/// </summary>
		[Label("流水")]
		Serial,
	}
}
