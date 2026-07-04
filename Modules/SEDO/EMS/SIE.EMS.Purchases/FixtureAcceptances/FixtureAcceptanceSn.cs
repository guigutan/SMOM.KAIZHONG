using SIE;
using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.FixtureAcceptances
{
	/// <summary>
	/// 序列号明细
	/// </summary>
	[ChildEntity, Serializable]
	[Label("序列号明细")]
	[DisplayMember(nameof(Sn))]
	public partial class FixtureAcceptanceSn : DataEntity
	{
		#region 序列号编码 Sn
		/// <summary>
		/// 序列号编码
		/// </summary>
		[Label("序列号编码")]
		public static readonly Property<string> SnProperty = P<FixtureAcceptanceSn>.Register(e => e.Sn);

		/// <summary>
		/// 序列号编码
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
		#endregion

		#region 原厂序列号 OriginalSn
		/// <summary>
		/// 原厂序列号
		/// </summary>
		[Label("原厂序列号")]
		public static readonly Property<string> OriginalSnProperty = P<FixtureAcceptanceSn>.Register(e => e.OriginalSn);

		/// <summary>
		/// 原厂序列号
		/// </summary>
		public string OriginalSn
		{
			get { return GetProperty(OriginalSnProperty); }
			set { SetProperty(OriginalSnProperty, value); }
		}
		#endregion

		#region 生产日期 ProductionDate
		/// <summary>
		/// 生产日期
		/// </summary>
		[Label("生产日期")]
		public static readonly Property<DateTime?> ProductionDateProperty = P<FixtureAcceptanceSn>.Register(e => e.ProductionDate);

		/// <summary>
		/// 生产日期
		/// </summary>
		public DateTime? ProductionDate
		{
			get { return GetProperty(ProductionDateProperty); }
			set { SetProperty(ProductionDateProperty, value); }
		}
		#endregion

		#region 生产厂家 Maker
		/// <summary>
		/// 生产厂家
		/// </summary>
		[Label("生产厂家")]
		public static readonly Property<string> MakerProperty = P<FixtureAcceptanceSn>.Register(e => e.Maker);

		/// <summary>
		/// 生产厂家
		/// </summary>
		public string Maker
		{
			get { return GetProperty(MakerProperty); }
			set { SetProperty(MakerProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<FixtureAcceptanceSn>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 验收结果 InspectionResult
		/// <summary>
		/// 验收结果
		/// </summary>
		[Label("检验结果")]
		public static readonly Property<InspectionResult?> InspectionResultProperty = P<FixtureAcceptanceSn>.Register(e => e.InspectionResult);

		/// <summary>
		/// 验收结果
		/// </summary>
		public InspectionResult? InspectionResult
		{
			get { return GetProperty(InspectionResultProperty); }
			set { SetProperty(InspectionResultProperty, value); }
		}
		#endregion

		#region 序列号明细 FixtureAcceptanceDetail
		/// <summary>
		/// 序列号明细Id
		/// </summary>
		public static readonly IRefIdProperty FixtureAcceptanceDetailIdProperty = P<FixtureAcceptanceSn>.RegisterRefId(e => e.FixtureAcceptanceDetailId, ReferenceType.Parent);

		/// <summary>
		/// 序列号明细Id
		/// </summary>
		public double FixtureAcceptanceDetailId
		{
			get { return (double)GetRefId(FixtureAcceptanceDetailIdProperty); }
			set { SetRefId(FixtureAcceptanceDetailIdProperty, value); }
		}

		/// <summary>
		/// 序列号明细
		/// </summary>
		public static readonly RefEntityProperty<FixtureAcceptanceDetail> FixtureAcceptanceDetailProperty = P<FixtureAcceptanceSn>.RegisterRef(e => e.FixtureAcceptanceDetail, FixtureAcceptanceDetailIdProperty);

		/// <summary>
		/// 序列号明细
		/// </summary>
		public FixtureAcceptanceDetail FixtureAcceptanceDetail
		{
			get { return GetRefEntity(FixtureAcceptanceDetailProperty); }
			set { SetRefEntity(FixtureAcceptanceDetailProperty, value); }
		}
		#endregion

		#region 采购单号 PurOrderNo
		/// <summary>
		/// 采购单号
		/// </summary>
		[Label("采购单号")]
		public static readonly Property<string> PurOrderNoProperty = P<FixtureAcceptanceSn>.RegisterView(e => e.PurOrderNo, p => p.FixtureAcceptanceDetail.FixtureReceiveDetail.PurchaseOrder.OrderNo);

		/// <summary>
		/// 采购单号
		/// </summary>
		public string PurOrderNo
		{
			get { return this.GetProperty(PurOrderNoProperty); }
			set { SetProperty(PurOrderNoProperty, value); }
		}
		#endregion



		#region 采购单行号 OrderLineNo
		/// <summary>
		/// 采购单行号
		/// </summary>
		[Label("采购单行号")]
		public static readonly Property<string> OrderLineNoProperty = P<FixtureAcceptanceSn>.RegisterView(e => e.OrderLineNo, p => p.FixtureAcceptanceDetail.FixtureReceiveDetail.PurchaseOrderItem.LineNo);

		/// <summary>
		/// 采购单行号
		/// </summary>
		public string OrderLineNo
		{
			get { return this.GetProperty(OrderLineNoProperty); }
			set { SetProperty(OrderLineNoProperty, value); }
		}
		#endregion


		#region 接收仓库 ReceiveWh
		/// <summary>
		/// 行号
		/// </summary>
		[Label("接收仓库")]
		public static readonly Property<string> ReceiveWhProperty = P<FixtureAcceptanceSn>.RegisterView(e => e.ReceiveWh, p => p.FixtureAcceptanceDetail.FixtureReceiveDetail.Warehouse.Name);

		/// <summary>
		/// 接收仓库
		/// </summary>
		public string ReceiveWh
		{
			get { return this.GetProperty(ReceiveWhProperty); }
			set { SetProperty(ReceiveWhProperty, value); }
		}
		#endregion


		#region 单价 Price
		/// <summary>
		/// 单价
		/// </summary>
		[Label("单价")]
		public static readonly Property<decimal> PriceProperty = P<FixtureAcceptanceSn>.RegisterView(e => e.Price, p => p.FixtureAcceptanceDetail.Price);

		/// <summary>
		/// 单价
		/// </summary>
		public decimal Price
		{
			get { return this.GetProperty(PriceProperty); }
			set { SetProperty(PriceProperty, value); }
		}
		#endregion

	}

	/// <summary>
	/// 序列号明细 实体配置
	/// </summary>
	internal class FixtureAcceptanceSnConfig : EntityConfig<FixtureAcceptanceSn>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_FIXT_ACPT_SN").MapAllProperties();
			Meta.Property(FixtureAcceptanceSn.RemarkProperty).ColumnMeta.HasLength(2000);
			Meta.EnablePhantoms();
		}
	}
}