using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.LES
{
    /// <summary>
    /// 备料模式推式查询实体
    /// </summary>
    [QueryEntity, Serializable]
	[Label("备料模式推式查询实体")]
	public partial class PrepareItemPushCriteria : BasePrepareItemCriteria
	{
		#region 提前小时 AdvanceHours
		/// <summary>
		/// 提前小时
		/// </summary>
		[Label("提前小时")]
		public static readonly Property<decimal?> AdvanceHoursProperty = P<PrepareItemPushCriteria>.Register(e => e.AdvanceHours);

		/// <summary>
		/// 提前小时
		/// </summary>
		public decimal? AdvanceHours
		{
			get { return GetProperty(AdvanceHoursProperty); }
			set { SetProperty(AdvanceHoursProperty, value); }
		}
		#endregion

		#region 最短满足时间（小时） SatisfactionTime
		/// <summary>
		/// 最短满足时间（小时）
		/// </summary>
		[Label("最短满足时间（小时）")]
		public static readonly Property<decimal?> SatisfactionTimeProperty = P<PrepareItemPushCriteria>.Register(e => e.SatisfactionTime);

		/// <summary>
		/// 最短满足时间（小时）
		/// </summary>
		public decimal? SatisfactionTime
		{
			get { return GetProperty(SatisfactionTimeProperty); }
			set { SetProperty(SatisfactionTimeProperty, value); }
		}
		#endregion

		#region 最小备料时间（小时) PreparationTime
		/// <summary>
		/// 最小备料时间（小时)
		/// </summary>
		[Label("最小备料时间（小时)")]
		public static readonly Property<decimal?> PreparationTimeProperty = P<PrepareItemPushCriteria>.Register(e => e.PreparationTime);

		/// <summary>
		/// 最小备料时间（小时)
		/// </summary>
		public decimal? PreparationTime
		{
			get { return GetProperty(PreparationTimeProperty); }
			set { SetProperty(PreparationTimeProperty, value); }
		}
		#endregion

		#region 生产资源 WipResource
		/// <summary>
		/// 生产资源Id
		/// </summary>
		[Label("生产资源")]
		[Required]
		public static readonly IRefIdProperty WipResourceIdProperty = P<PrepareItemPushCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

		/// <summary>
		/// 生产资源Id
		/// </summary>
		public double? WipResourceId
		{
			get { return (double?)GetRefNullableId(WipResourceIdProperty); }
			set { SetRefNullableId(WipResourceIdProperty, value); }
		}

		/// <summary>
		/// 生产资源
		/// </summary>
		public static readonly RefEntityProperty<WipResource> WipResourceProperty = P<PrepareItemPushCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

		/// <summary>
		/// 生产资源
		/// </summary>
		public WipResource WipResource
		{
			get { return GetRefEntity(WipResourceProperty); }
			set { SetRefEntity(WipResourceProperty, value); }
		}
		#endregion
		/// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<PrepareItemController>().GetPrepareItemPushs(this);
		}
	}
}