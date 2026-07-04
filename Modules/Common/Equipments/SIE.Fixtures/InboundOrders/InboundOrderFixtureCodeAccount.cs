using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.InboundOrders
{
	/// <summary>
	/// 工治具入库-编码类入库明细
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("工治具入库-编码类入库明细")]
	public partial class InboundOrderFixtureCodeAccount : DataEntity
	{
		#region 数量 Qty
		/// <summary>
		/// 数量
		/// </summary>
		[Label("数量")]
		public static readonly Property<decimal> QtyProperty = P<InboundOrderFixtureCodeAccount>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 库位 StorageLocation
		/// <summary>
		/// 库位Id
		/// </summary>
		public static readonly IRefIdProperty StorageLocationIdProperty = P<InboundOrderFixtureCodeAccount>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

		/// <summary>
		/// 库位Id
		/// </summary>
		public double? StorageLocationId
		{
			get { return (double?)GetRefNullableId(StorageLocationIdProperty); }
			set { SetRefNullableId(StorageLocationIdProperty, value); }
		}

		/// <summary>
		/// 库位
		/// </summary>
		public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<InboundOrderFixtureCodeAccount>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

		/// <summary>
		/// 库位
		/// </summary>
		public StorageLocation StorageLocation
		{
			get { return GetRefEntity(StorageLocationProperty); }
			set { SetRefEntity(StorageLocationProperty, value); }
		}
		#endregion

		#region  InboundOrder
		/// <summary>
		/// Id
		/// </summary>
		public static readonly IRefIdProperty InboundOrderIdProperty = P<InboundOrderFixtureCodeAccount>.RegisterRefId(e => e.InboundOrderId, ReferenceType.Parent);

		/// <summary>
		/// Id
		/// </summary>
		public double InboundOrderId
		{
			get { return (double)GetRefId(InboundOrderIdProperty); }
			set { SetRefId(InboundOrderIdProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly RefEntityProperty<InboundOrder> InboundOrderProperty = P<InboundOrderFixtureCodeAccount>.RegisterRef(e => e.InboundOrder, InboundOrderIdProperty);

		/// <summary>
		/// 
		/// </summary>
		public InboundOrder InboundOrder
		{
			get { return GetRefEntity(InboundOrderProperty); }
			set { SetRefEntity(InboundOrderProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 工治具入库-编码类入库明细 实体配置
	/// </summary>
	internal class InboundOrderFixtureCodeAccountConfig : EntityConfig<InboundOrderFixtureCodeAccount>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_FIXTURE_IN_Code").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}