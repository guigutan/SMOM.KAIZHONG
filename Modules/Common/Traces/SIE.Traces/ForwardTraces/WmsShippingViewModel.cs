using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Traces.ForwardTraces
{
    /// <summary>
    /// Wms发运单追溯
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("发运信息")]
	public partial class WmsShippingViewModel : ViewModel
    {

        #region 发货数量 ShipQty
        /// <summary>
        /// 发货数量
        /// </summary>
        [Label("发货数量")]
		public static readonly Property<decimal?> ShipQtyProperty = P<WmsShippingViewModel>.Register(e => e.ShipQty);

		/// <summary>
		/// 发货数量
		/// </summary>
		public decimal? ShipQty
		{
			get { return GetProperty(ShipQtyProperty); }
			set { SetProperty(ShipQtyProperty, value); }
		}
		#endregion

		#region 发运单 ShippingOrderNo
		/// <summary>
		/// 发运单
		/// </summary>
		[Label("发运单")]
		public static readonly Property<string> ShippingOrderNoProperty = P<WmsShippingViewModel>.Register(e => e.ShippingOrderNo);

		/// <summary>
		/// 发运单
		/// </summary>
		public string ShippingOrderNo
		{
			get { return GetProperty(ShippingOrderNoProperty); }
			set { SetProperty(ShippingOrderNoProperty, value); }
		}
		#endregion

		#region 接收人 ReceiveByName
		/// <summary>
		/// 接收人
		/// </summary>
		[Label("接收人")]
		public static readonly Property<string> ReceiveByNameProperty = P<WmsShippingViewModel>.Register(e => e.ReceiveByName);

		/// <summary>
		/// 接收人
		/// </summary>
		public string ReceiveByName
		{
			get { return GetProperty(ReceiveByNameProperty); }
			set { SetProperty(ReceiveByNameProperty, value); }
		}
		#endregion

		#region 接收时间 ReceiveTime
		/// <summary>
		/// 接收时间
		/// </summary>
		[Label("接收时间")]
		public static readonly Property<DateTime?> ReceiveTimeProperty = P<WmsShippingViewModel>.Register(e => e.ReceiveTime);

		/// <summary>
		/// 接收时间
		/// </summary>
		public DateTime? ReceiveTime
		{
			get { return GetProperty(ReceiveTimeProperty); }
			set { SetProperty(ReceiveTimeProperty, value); }
		}
		#endregion

		#region 工厂名称 FactoryName
		/// <summary>
		/// 工厂名称
		/// </summary>
		[Label("工厂名称")]
		public static readonly Property<string> FactoryNameProperty = P<WmsShippingViewModel>.Register(e => e.FactoryName);

		/// <summary>
		/// 工厂名称
		/// </summary>
		public string FactoryName
		{
			get { return GetProperty(FactoryNameProperty); }
			set { SetProperty(FactoryNameProperty, value); }
		}
		#endregion

		#region 车间名称 WorkShopName
		/// <summary>
		/// 车间名称
		/// </summary>
		[Label("车间名称")]
		public static readonly Property<string> WorkShopNameProperty = P<WmsShippingViewModel>.Register(e => e.WorkShopName);

		/// <summary>
		/// 车间名称
		/// </summary>
		public string WorkShopName
		{
			get { return GetProperty(WorkShopNameProperty); }
			set { SetProperty(WorkShopNameProperty, value); }
		}
		#endregion

		#region 生产资源名称 ResourceName
		/// <summary>
		/// 生产资源名称
		/// </summary>
		[Label("生产资源名称")]
		public static readonly Property<string> ResourceNameProperty = P<WmsShippingViewModel>.Register(e => e.ResourceName);

		/// <summary>
		/// 生产资源名称
		/// </summary>
		public string ResourceName
		{
			get { return GetProperty(ResourceNameProperty); }
			set { SetProperty(ResourceNameProperty, value); }
		}
		#endregion

		#region 工单 WorkOrderNo
		/// <summary>
		/// 工单
		/// </summary>
		[Label("工单")]
		public static readonly Property<string> WorkOrderNoProperty = P<WmsShippingViewModel>.Register(e => e.WorkOrderNo);

		/// <summary>
		/// 工单
		/// </summary>
		public string WorkOrderNo
		{
			get { return GetProperty(WorkOrderNoProperty); }
			set { SetProperty(WorkOrderNoProperty, value); }
		}
		#endregion
	}
}