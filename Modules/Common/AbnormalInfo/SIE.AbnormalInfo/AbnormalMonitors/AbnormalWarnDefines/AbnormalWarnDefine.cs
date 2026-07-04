using SIE;
using SIE.AbnormalInfo.AbnormalMonitors.Configs;
using SIE.Common.Configs;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常预警定义
	/// </summary>
	[RootEntity, Serializable]
	[ConditionQueryType(typeof(AbnormalWarnDefineCriteria))]
	[EntityWithConfig(typeof(PushUpgradeRuleTimeConfig))]
	[EntityWithConfig(typeof(WarnDefineForRedCardConfig))]
	[EntityWithConfig(typeof(WarnDefineForSpcConfig))]
	[Label("异常预警定义")]
	[DisplayMember(nameof(Code))]
	public partial class AbnormalWarnDefine : DataEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Label("编码")]
		[Required]
		[NotDuplicate]
		public static readonly Property<string> CodeProperty = P<AbnormalWarnDefine>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 名称 Name
		/// <summary>
		/// 名称
		/// </summary>
		[Label("名称")]
		[Required]
		public static readonly Property<string> NameProperty = P<AbnormalWarnDefine>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 责任人Id集合 JoinEmployeeIds
		/// <summary>
		/// 责任人Id集合
		/// </summary>
		[Label("责任人")]
		[MaxLength(500)]
		[Required]
		public static readonly Property<string> JoinEmployeeIdsProperty = P<AbnormalWarnDefine>.Register(e => e.JoinEmployeeIds);

		/// <summary>
		/// 责任人Id集合
		/// </summary>
		public string JoinEmployeeIds
		{
			get { return this.GetProperty(JoinEmployeeIdsProperty); }
			set { this.SetProperty(JoinEmployeeIdsProperty, value); }
		}
		#endregion

		#region 责任人名称集合(不映射数据库) JoinEmployeeNames
		/// <summary>
		/// 责任人名称集合(不映射数据库)
		/// </summary>
		[Label("责任人")]
		public static readonly Property<string> JoinEmployeeNamesProperty = P<AbnormalWarnDefine>.Register(e => e.JoinEmployeeNames);

		/// <summary>
		///责任人名称集合(不映射数据库)
		/// </summary>
		public string JoinEmployeeNames
		{
			get { return this.GetProperty(JoinEmployeeNamesProperty); }
			set { this.SetProperty(JoinEmployeeNamesProperty, value); }
		}
		#endregion

		#region 推送模块 PushModule
		/// <summary>
		/// 推送模块Id
		/// </summary>
		public static readonly IRefIdProperty PushModuleIdProperty = P<AbnormalWarnDefine>.RegisterRefId(e => e.PushModuleId, ReferenceType.Normal);

		/// <summary>
		/// 推送模块Id
		/// </summary>
		public double? PushModuleId
		{
			get { return (double?)GetRefNullableId(PushModuleIdProperty); }
			set { SetRefNullableId(PushModuleIdProperty, value); }
		}

		/// <summary>
		/// 推送模块
		/// </summary>
		public static readonly RefEntityProperty<Pusher> PushModuleProperty = P<AbnormalWarnDefine>.RegisterRef(e => e.PushModule, PushModuleIdProperty);

		/// <summary>
		/// 推送模块
		/// </summary>
		public Pusher PushModule
		{
			get { return GetRefEntity(PushModuleProperty); }
			set { SetRefEntity(PushModuleProperty, value); }
		}
		#endregion

		#region 推送升级机制 UpgradeRuleList
		/// <summary>
		/// 推送升级机制
		/// </summary>
		public static readonly ListProperty<EntityList<PushUpgradeRule>> UpgradeRuleListProperty = P<AbnormalWarnDefine>.RegisterList(e => e.UpgradeRuleList);
		/// <summary>
		/// 推送升级机制
		/// </summary>
		public EntityList<PushUpgradeRule> UpgradeRuleList
		{
			get { return this.GetLazyList(UpgradeRuleListProperty); }
		}
		#endregion
	}

	/// <summary>
	/// 异常预警定义 实体配置
	/// </summary>
	internal class AbnormalWarnDefineConfig : EntityConfig<AbnormalWarnDefine>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNORMAL_WARN_DEFINE").MapAllPropertiesExcept(AbnormalWarnDefine.JoinEmployeeNamesProperty);
			Meta.Property(AbnormalWarnDefine.JoinEmployeeIdsProperty).ColumnMeta.HasLength(1500);
			Meta.EnablePhantoms();
		}
	}
}