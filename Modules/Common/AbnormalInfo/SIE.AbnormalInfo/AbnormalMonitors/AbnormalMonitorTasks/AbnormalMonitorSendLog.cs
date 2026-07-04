using SIE;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常预警推送日志
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("异常预警推送日志")]
	public partial class AbnormalMonitorSendLog : DataEntity
	{
		#region 推送人 JoinPushTargetNames
		/// <summary>
		/// 推送人
		/// </summary>
		[Label("推送人")]
		[MaxLength(1000)]
		public static readonly Property<string> JoinPushTargetNamesProperty = P<AbnormalMonitorSendLog>.Register(e => e.JoinPushTargetNames);

		/// <summary>
		/// 推送人
		/// </summary>
		public string JoinPushTargetNames
		{
			get { return GetProperty(JoinPushTargetNamesProperty); }
			set { SetProperty(JoinPushTargetNamesProperty, value); }
		}
		#endregion

		#region 推送内容 PushContent
		/// <summary>
		/// 推送内容
		/// </summary>
		[Label("推送内容")]
		[MaxLength(1000)]
		public static readonly Property<string> PushContentProperty = P<AbnormalMonitorSendLog>.Register(e => e.PushContent);

		/// <summary>
		/// 推送内容
		/// </summary>
		public string PushContent
		{
			get { return GetProperty(PushContentProperty); }
			set { SetProperty(PushContentProperty, value); }
		}
		#endregion

		#region 异常监控任务 AbnormalMonitorTask
		/// <summary>
		/// 异常监控任务Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalMonitorTaskIdProperty = P<AbnormalMonitorSendLog>.RegisterRefId(e => e.AbnormalMonitorTaskId, ReferenceType.Normal);

		/// <summary>
		/// 异常监控任务Id
		/// </summary>
		public double AbnormalMonitorTaskId
		{
			get { return (double)GetRefId(AbnormalMonitorTaskIdProperty); }
			set { SetRefId(AbnormalMonitorTaskIdProperty, value); }
		}

		/// <summary>
		/// 异常监控任务
		/// </summary>
		public static readonly RefEntityProperty<AbnormalMonitorTask> AbnormalMonitorTaskProperty = P<AbnormalMonitorSendLog>.RegisterRef(e => e.AbnormalMonitorTask, AbnormalMonitorTaskIdProperty);

		/// <summary>
		/// 异常监控任务
		/// </summary>
		public AbnormalMonitorTask AbnormalMonitorTask
		{
			get { return GetRefEntity(AbnormalMonitorTaskProperty); }
			set { SetRefEntity(AbnormalMonitorTaskProperty, value); }
		}
		#endregion

		#region 异常定义 AbnormalDefine
		/// <summary>
		/// 异常定义Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalDefineIdProperty = P<AbnormalMonitorSendLog>.RegisterRefId(e => e.AbnormalDefineId, ReferenceType.Normal);

		/// <summary>
		/// 异常定义Id
		/// </summary>
		public double? AbnormalDefineId
		{
			get { return (double?)GetRefNullableId(AbnormalDefineIdProperty); }
			set { SetRefNullableId(AbnormalDefineIdProperty, value); }
		}

		/// <summary>
		/// 异常定义
		/// </summary>
		public static readonly RefEntityProperty<AbnormalDefine> AbnormalDefineProperty = P<AbnormalMonitorSendLog>.RegisterRef(e => e.AbnormalDefine, AbnormalDefineIdProperty);

		/// <summary>
		/// 异常定义
		/// </summary>
		public AbnormalDefine AbnormalDefine
		{
			get { return GetRefEntity(AbnormalDefineProperty); }
			set { SetRefEntity(AbnormalDefineProperty, value); }
		}
		#endregion

		#region 异常预警定义 AbnormalWarnDefine
		/// <summary>
		/// 异常预警定义Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalWarnDefineIdProperty = P<AbnormalMonitorSendLog>.RegisterRefId(e => e.AbnormalWarnDefineId, ReferenceType.Normal);

		/// <summary>
		/// 异常预警定义Id
		/// </summary>
		public double AbnormalWarnDefineId
		{
			get { return (double)GetRefId(AbnormalWarnDefineIdProperty); }
			set { SetRefId(AbnormalWarnDefineIdProperty, value); }
		}

		/// <summary>
		/// 异常预警定义
		/// </summary>
		public static readonly RefEntityProperty<AbnormalWarnDefine> AbnormalWarnDefineProperty = P<AbnormalMonitorSendLog>.RegisterRef(e => e.AbnormalWarnDefine, AbnormalWarnDefineIdProperty);

		/// <summary>
		/// 异常预警定义
		/// </summary>
		public AbnormalWarnDefine AbnormalWarnDefine
		{
			get { return GetRefEntity(AbnormalWarnDefineProperty); }
			set { SetRefEntity(AbnormalWarnDefineProperty, value); }
		}
		#endregion

		#region 推送方式 Pusher
		/// <summary>
		/// 推送方式Id
		/// </summary>
		public static readonly IRefIdProperty PusherIdProperty = P<AbnormalMonitorSendLog>.RegisterRefId(e => e.PusherId, ReferenceType.Normal);

		/// <summary>
		/// 推送方式Id
		/// </summary>
		public double PusherId
		{
			get { return (double)GetRefId(PusherIdProperty); }
			set { SetRefId(PusherIdProperty, value); }
		}

		/// <summary>
		/// 推送方式
		/// </summary>
		public static readonly RefEntityProperty<Pusher> PusherProperty = P<AbnormalMonitorSendLog>.RegisterRef(e => e.Pusher, PusherIdProperty);

		/// <summary>
		/// 推送方式
		/// </summary>
		public Pusher Pusher
		{
			get { return GetRefEntity(PusherProperty); }
			set { SetRefEntity(PusherProperty, value); }
		}
		#endregion

		#region 推送升级机制 PushUpgradeRule
		/// <summary>
		/// 推送升级机制Id
		/// </summary>
		public static readonly IRefIdProperty PushUpgradeRuleIdProperty = P<AbnormalMonitorSendLog>.RegisterRefId(e => e.PushUpgradeRuleId, ReferenceType.Normal);

		/// <summary>
		/// 推送升级机制Id
		/// </summary>
		public double PushUpgradeRuleId
		{
			get { return (double)GetRefId(PushUpgradeRuleIdProperty); }
			set { SetRefId(PushUpgradeRuleIdProperty, value); }
		}

		/// <summary>
		/// 推送升级机制
		/// </summary>
		public static readonly RefEntityProperty<PushUpgradeRule> PushUpgradeRuleProperty = P<AbnormalMonitorSendLog>.RegisterRef(e => e.PushUpgradeRule, PushUpgradeRuleIdProperty);

		/// <summary>
		/// 推送升级机制
		/// </summary>
		public PushUpgradeRule PushUpgradeRule
		{
			get { return GetRefEntity(PushUpgradeRuleProperty); }
			set { SetRefEntity(PushUpgradeRuleProperty, value); }
		}
		#endregion
		
	}

	/// <summary>
	/// 异常预警推送日志 实体配置
	/// </summary>
	internal class AbnormalMonitorSendLogConfig : EntityConfig<AbnormalMonitorSendLog>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNORMAL_MONITOR_SEND_LOG").MapAllProperties();
			Meta.Property(AbnormalMonitorSendLog.JoinPushTargetNamesProperty).ColumnMeta.HasLength(3000);
			Meta.Property(AbnormalMonitorSendLog.PushContentProperty).ColumnMeta.HasLength(3000);
			Meta.EnablePhantoms();
		}
	}
}