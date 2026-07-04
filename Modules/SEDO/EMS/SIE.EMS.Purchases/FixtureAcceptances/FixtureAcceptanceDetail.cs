using SIE;
using SIE.Domain;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.Fixtures;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.FixtureAcceptances
{
	/// <summary>
	/// 工治具验收明细
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("工治具验收明细")]
	public partial class FixtureAcceptanceDetail : DataEntity
	{
		#region 单价 Price
		/// <summary>
		/// 单价
		/// </summary>
		[Label("单价")]
		public static readonly Property<decimal> PriceProperty = P<FixtureAcceptanceDetail>.Register(e => e.Price);

		/// <summary>
		/// 单价
		/// </summary>
		public decimal Price
		{
			get { return GetProperty(PriceProperty); }
			set { SetProperty(PriceProperty, value); }
		}
		#endregion

		#region 接收数量 ReceiveQty
		/// <summary>
		/// 接收数量
		/// </summary>
		[Label("接收数量")]
		public static readonly Property<int> ReceiveQtyProperty = P<FixtureAcceptanceDetail>.Register(e => e.ReceiveQty);

		/// <summary>
		/// 接收数量
		/// </summary>
		public int ReceiveQty
		{
			get { return GetProperty(ReceiveQtyProperty); }
			set { SetProperty(ReceiveQtyProperty, value); }
		}
		#endregion

		#region 合格数量 PassQty
		/// <summary>
		/// 合格数量
		/// </summary>
		[Label("合格数量")]
		public static readonly Property<int> PassQtyProperty = P<FixtureAcceptanceDetail>.Register(e => e.PassQty);

		/// <summary>
		/// 合格数量
		/// </summary>
		public int PassQty
		{
			get { return GetProperty(PassQtyProperty); }
			set { SetProperty(PassQtyProperty, value); }
		}
		#endregion

		#region 不合格数量 UnqualifiedQty
		/// <summary>
		/// 不合格数量
		/// </summary>
		[Label("不合格数量")]
		public static readonly Property<int> UnqualifiedQtyProperty = P<FixtureAcceptanceDetail>.Register(e => e.UnqualifiedQty);

		/// <summary>
		/// 不合格数量
		/// </summary>
		public int UnqualifiedQty
		{
			get { return GetProperty(UnqualifiedQtyProperty); }
			set { SetProperty(UnqualifiedQtyProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<FixtureAcceptanceDetail>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 工治具接收明细 FixtureReceiveDetail
		/// <summary>
		/// 工治具接收明细Id
		/// </summary>
		public static readonly IRefIdProperty FixtureReceiveDetailIdProperty = P<FixtureAcceptanceDetail>.RegisterRefId(e => e.FixtureReceiveDetailId, ReferenceType.Normal);

		/// <summary>
		/// 工治具接收明细Id
		/// </summary>
		public double FixtureReceiveDetailId
		{
			get { return (double)GetRefId(FixtureReceiveDetailIdProperty); }
			set { SetRefId(FixtureReceiveDetailIdProperty, value); }
		}

		/// <summary>
		/// 工治具接收明细
		/// </summary>
		public static readonly RefEntityProperty<FixtureReceiveDetail> FixtureReceiveDetailProperty = P<FixtureAcceptanceDetail>.RegisterRef(e => e.FixtureReceiveDetail, FixtureReceiveDetailIdProperty);

		/// <summary>
		/// 工治具接收明细
		/// </summary>
		public FixtureReceiveDetail FixtureReceiveDetail
		{
			get { return GetRefEntity(FixtureReceiveDetailProperty); }
			set { SetRefEntity(FixtureReceiveDetailProperty, value); }
		}
		#endregion

		#region 序列号明细 FixtureAcceptanceSnList
		/// <summary>
		/// 序列号明细
		/// </summary>
		public static readonly ListProperty<EntityList<FixtureAcceptanceSn>> FixtureAcceptanceSnListProperty = P<FixtureAcceptanceDetail>.RegisterList(e => e.FixtureAcceptanceSnList);
		/// <summary>
		/// 序列号明细
		/// </summary>
		public EntityList<FixtureAcceptanceSn> FixtureAcceptanceSnList
		{
			get { return this.GetLazyList(FixtureAcceptanceSnListProperty); }
		}
		#endregion

		#region 验收明细 FixtureAcceptance
		/// <summary>
		/// 验收明细Id
		/// </summary>
		public static readonly IRefIdProperty FixtureAcceptanceIdProperty = P<FixtureAcceptanceDetail>.RegisterRefId(e => e.FixtureAcceptanceId, ReferenceType.Parent);

		/// <summary>
		/// 验收明细Id
		/// </summary>
		public double FixtureAcceptanceId
		{
			get { return (double)GetRefId(FixtureAcceptanceIdProperty); }
			set { SetRefId(FixtureAcceptanceIdProperty, value); }
		}

		/// <summary>
		/// 验收明细
		/// </summary>
		public static readonly RefEntityProperty<FixtureAcceptance> FixtureAcceptanceProperty = P<FixtureAcceptanceDetail>.RegisterRef(e => e.FixtureAcceptance, FixtureAcceptanceIdProperty);

		/// <summary>
		/// 验收明细
		/// </summary>
		public FixtureAcceptance FixtureAcceptance
		{
			get { return GetRefEntity(FixtureAcceptanceProperty); }
			set { SetRefEntity(FixtureAcceptanceProperty, value); }
		}
		#endregion
		#region 采购单号 PurOrderNo
		/// <summary>
		/// 采购单号
		/// </summary>
		[Label("采购单号")]
		public static readonly Property<string> PurOrderNoProperty = P<FixtureAcceptanceDetail>.RegisterView(e => e.PurOrderNo, p => p.FixtureReceiveDetail.PurchaseOrder.OrderNo);

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
		public static readonly Property<string> OrderLineNoProperty = P<FixtureAcceptanceDetail>.RegisterView(e => e.OrderLineNo, p => p.FixtureReceiveDetail.PurchaseOrderItem.LineNo);

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
		public static readonly Property<string> ReceiveWhProperty = P<FixtureAcceptanceDetail>.RegisterView(e => e.ReceiveWh, p => p.FixtureReceiveDetail.Warehouse.Name);

		/// <summary>
		/// 接收仓库
		/// </summary>
		public string ReceiveWh
		{
			get { return this.GetProperty(ReceiveWhProperty); }
			set { SetProperty(ReceiveWhProperty, value); }
		}
		#endregion


		#region 管控方式 ManageMode
		/// <summary>
		/// 管控方式
		/// </summary>
		[Label("管控方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureAcceptanceDetail>.RegisterView(e => e.ManageMode, p => p.FixtureReceiveDetail.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// ManageMode
        /// </summary>
        public ManageMode ManageMode
		{
            get { return this.GetProperty(ManageModeProperty); }
        }
        #endregion

    }

	/// <summary>
	/// 工治具验收明细 实体配置
	/// </summary>
	internal class FixtureAcceptanceDetailConfig : EntityConfig<FixtureAcceptanceDetail>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_FIXT_ACPT_DTL").MapAllProperties();
			Meta.Property(FixtureAcceptanceDetail.RemarkProperty).ColumnMeta.HasLength(2000);
			Meta.EnablePhantoms();
		}
	}
}