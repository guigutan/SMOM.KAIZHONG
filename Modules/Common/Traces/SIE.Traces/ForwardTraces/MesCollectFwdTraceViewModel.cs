using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Traces.ForwardTraces
{
    /// <summary>
    /// 过程追溯
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("过程追溯-采集记录")]
	public partial class MesCollectFwdTraceViewModel : ViewModel
    {
		#region 采集工序 CollectProcess
		/// <summary>
		/// 采集工序
		/// </summary>
		[Label("采集工序")]
		public static readonly Property<string> CollectProcessProperty = P<MesCollectFwdTraceViewModel>.Register(e => e.CollectProcess);

		/// <summary>
		/// 采集工序
		/// </summary>
		public string CollectProcess
		{
			get { return GetProperty(CollectProcessProperty); }
			set { SetProperty(CollectProcessProperty, value); }
		}
		#endregion

		#region 采集工位 CollectStation
		/// <summary>
		/// 采集工位
		/// </summary>
		[Label("采集工位")]
		public static readonly Property<string> CollectStationProperty = P<MesCollectFwdTraceViewModel>.Register(e => e.CollectStation);

		/// <summary>
		/// 采集工位
		/// </summary>
		public string CollectStation
		{
			get { return GetProperty(CollectStationProperty); }
			set { SetProperty(CollectStationProperty, value); }
		}
		#endregion		

		#region 使用数量 UsedQty
		/// <summary>
		/// 使用数量
		/// </summary>
		[Label("使用数量")]
		public static readonly Property<decimal?> UsedQtyProperty = P<MesCollectFwdTraceViewModel>.Register(e => e.UsedQty);

		/// <summary>
		/// 使用数量
		/// </summary>
		public decimal? UsedQty
		{
			get { return GetProperty(UsedQtyProperty); }
			set { SetProperty(UsedQtyProperty, value); }
		}
		#endregion

		#region 采集时间 CollectTime
		/// <summary>
		/// 采集时间
		/// </summary>
		[Label("采集时间")]
		public static readonly Property<DateTime?> CollectTimeProperty = P<MesCollectFwdTraceViewModel>.Register(e => e.CollectTime);

		/// <summary>
		/// 采集时间
		/// </summary>
		public DateTime? CollectTime
		{
			get { return GetProperty(CollectTimeProperty); }
			set { SetProperty(CollectTimeProperty, value); }
		}
		#endregion

		#region 操作人 CollectBy
		/// <summary>
		/// 操作人
		/// </summary>
		[Label("操作人")]
		public static readonly Property<string> CollectByProperty = P<MesCollectFwdTraceViewModel>.Register(e => e.CollectBy);

		/// <summary>
		/// 操作人
		/// </summary>
		public string CollectBy
		{
			get { return GetProperty(CollectByProperty); }
			set { SetProperty(CollectByProperty, value); }
		}
		#endregion
	}
}