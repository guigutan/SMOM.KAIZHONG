using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.DashboardDemo
{
	/// <summary>
	/// 每日产量
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("每日产量")]
	public partial class DemoWorkOrderLineBoard : DataEntity
	{
		#region 时间 HourDate
		/// <summary>
		/// 时间
		/// </summary>
		[Label("时间")]
		public static readonly Property<DateTime> HourDateProperty = P<DemoWorkOrderLineBoard>.Register(e => e.HourDate);

		/// <summary>
		/// 时间
		/// </summary>
		public DateTime HourDate
		{
			get { return GetProperty(HourDateProperty); }
			set { SetProperty(HourDateProperty, value); }
		}
		#endregion

		#region 产量 Qty
		/// <summary>
		/// 产量
		/// </summary>
		[Label("产量")]
		public static readonly Property<decimal> QtyProperty = P<DemoWorkOrderLineBoard>.Register(e => e.Qty);

		/// <summary>
		/// 产量
		/// </summary>
		public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 每日产量 实体配置
	/// </summary>
	internal class DemoWorkOrderLineBoardConfig : EntityConfig<DemoWorkOrderLineBoard>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("Demo_ApsWorkOrderLineBoard").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}
