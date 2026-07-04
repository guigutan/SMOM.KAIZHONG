using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Traces.Common
{
    /// <summary>
    /// 工序维修记录
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("工序维修记录")]
	public partial class ProductRepairTraceViewModel : ViewModel
    {
		#region 维修类型 RepairType
		/// <summary>
		/// 维修类型
		/// </summary>
		[Label("维修类型")]
		public static readonly Property<string> RepairTypeProperty = P<ProductRepairTraceViewModel>.Register(e => e.RepairType);

		/// <summary>
		/// 维修类型
		/// </summary>
		public string RepairType
		{
			get { return GetProperty(RepairTypeProperty); }
			set { SetProperty(RepairTypeProperty, value); }
		}
		#endregion

		#region 维修人 RepairBy
		/// <summary>
		/// 维修人
		/// </summary>
		[Label("维修人")]
		public static readonly Property<string> RepairByProperty = P<ProductRepairTraceViewModel>.Register(e => e.RepairBy);

		/// <summary>
		/// 维修人
		/// </summary>
		public string RepairBy
		{
			get { return GetProperty(RepairByProperty); }
			set { SetProperty(RepairByProperty, value); }
		}
		#endregion

		#region 维修时间 RepairTime
		/// <summary>
		/// 维修时间
		/// </summary>
		[Label("维修时间")]
		public static readonly Property<DateTime?> RepairTimeProperty = P<ProductRepairTraceViewModel>.Register(e => e.RepairTime);

		/// <summary>
		/// 维修时间
		/// </summary>
		public DateTime? RepairTime
		{
			get { return GetProperty(RepairTimeProperty); }
			set { SetProperty(RepairTimeProperty, value); }
		}
		#endregion

		#region 维修工序 RepairProcess
		/// <summary>
		/// 维修工序
		/// </summary>
		[Label("维修工序")]
		public static readonly Property<string> RepairProcessProperty = P<ProductRepairTraceViewModel>.Register(e => e.RepairProcess);

		/// <summary>
		/// 维修工序
		/// </summary>
		public string RepairProcess
		{
			get { return GetProperty(RepairProcessProperty); }
			set { SetProperty(RepairProcessProperty, value); }
		}
		#endregion

		#region 维修工位 RepairStation
		/// <summary>
		/// 维修工位
		/// </summary>
		[Label("维修工位")]
		public static readonly Property<string> RepairStationProperty = P<ProductRepairTraceViewModel>.Register(e => e.RepairStation);

		/// <summary>
		/// 维修工位
		/// </summary>
		public string RepairStation
		{
			get { return GetProperty(RepairStationProperty); }
			set { SetProperty(RepairStationProperty, value); }
		}
		#endregion

		#region 缺陷描述 DefectDes
		/// <summary>
		/// 缺陷描述
		/// </summary>
		[Label("缺陷描述")]
		public static readonly Property<string> DefectDesProperty = P<ProductRepairTraceViewModel>.Register(e => e.DefectDes);

		/// <summary>
		/// 缺陷描述
		/// </summary>
		public string DefectDes
		{
			get { return GetProperty(DefectDesProperty); }
			set { SetProperty(DefectDesProperty, value); }
		}
		#endregion

		#region 缺陷备注 DefectRemark
		/// <summary>
		/// 缺陷备注
		/// </summary>
		[Label("缺陷备注")]
		public static readonly Property<string> DefectRemarkProperty = P<ProductRepairTraceViewModel>.Register(e => e.DefectRemark);

		/// <summary>
		/// 缺陷备注
		/// </summary>
		public string DefectRemark
		{
			get { return GetProperty(DefectRemarkProperty); }
			set { SetProperty(DefectRemarkProperty, value); }
		}
		#endregion
	}
}