using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.LES
{
	/// <summary>
	/// 备料模式推式
	/// </summary>
	[RootEntity, Serializable]
	[ConditionQueryType(typeof(PrepareItemPushCriteria))]
	[Label("备料模式推式")]
	public partial class PrepareItemPush : BasePrepareItem
	{
		#region 提前小时 AdvanceHours
		/// <summary>
		/// 提前小时
		/// </summary>
		[Label("提前小时")]
		[MinValue(0)]
		public static readonly Property<decimal?> AdvanceHoursProperty = P<PrepareItemPush>.Register(e => e.AdvanceHours);

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
		[MinValue(0)]
		public static readonly Property<decimal?> SatisfactionTimeProperty = P<PrepareItemPush>.Register(e => e.SatisfactionTime);

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
		[MinValue(0)]
		public static readonly Property<decimal?> PreparationTimeProperty = P<PrepareItemPush>.Register(e => e.PreparationTime);

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
		public static readonly IRefIdProperty WipResourceIdProperty = P<PrepareItemPush>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<WipResource> WipResourceProperty = P<PrepareItemPush>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

		/// <summary>
		/// 生产资源
		/// </summary>
		public WipResource WipResource
		{
			get { return GetRefEntity(WipResourceProperty); }
			set { SetRefEntity(WipResourceProperty, value); }
		}
		#endregion

		#region 生产资源编码 WipResourceCode
		/// <summary>
		/// 生产资源编码
		/// </summary>
		[Label("生产资源编码")]
        public static readonly Property<string> NameProperty = P<PrepareItemPush>.RegisterView(e => e.WipResourceCode, p => p.WipResource.Code);

		/// <summary>
		/// 生产资源编码
		/// </summary>
		public string WipResourceCode
		{
            get { return this.GetProperty(NameProperty); }
        }
		#endregion

		#region 视图属性

		#region 资源名称 WipResourceNameView
		/// <summary>
		/// 资源名称
		/// </summary>
		[Label("资源名称")]
		public static readonly Property<string> WipResourceNameViewProperty = P<PrepareItemPush>.RegisterView(e => e.WipResourceNameView, p => p.WipResource.Name);

		/// <summary>
		/// 资源名称
		/// </summary>
		public string WipResourceNameView
		{
			get { return this.GetProperty(WipResourceNameViewProperty); }
		}
		#endregion

		#endregion

	}

    /// <summary>
    /// 备料模式推式 实体配置
    /// </summary>
    internal class PrepareItemPushConfig : EntityConfig<PrepareItemPush>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("PREPARE_ITEM").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}