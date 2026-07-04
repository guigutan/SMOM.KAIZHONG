using SIE;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 推送对象
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("推送对象")]
	public partial class PushTarget : DataEntity
	{
		#region 对象Id TargetId
		/// <summary>
		/// 对象Id
		/// </summary>
		[Label("对象Id")]
		public static readonly Property<double> TargetIdProperty = P<PushTarget>.Register(e => e.TargetId);

		/// <summary>
		/// 对象编码
		/// </summary>
		public double TargetId
		{
			get { return GetProperty(TargetIdProperty); }
			set { SetProperty(TargetIdProperty, value); }
		}
		#endregion

		#region 对象编码 TargetCode
		/// <summary>
		/// 对象编码
		/// </summary>
		[Label("对象编码")]
		public static readonly Property<string> TargetCodeProperty = P<PushTarget>.Register(e => e.TargetCode);

		/// <summary>
		/// 对象编码
		/// </summary>
		public string TargetCode
		{
			get { return GetProperty(TargetCodeProperty); }
			set { SetProperty(TargetCodeProperty, value); }
		}
		#endregion

		#region 对象名称 TargetName
		/// <summary>
		/// 对象名称
		/// </summary>
		[Label("对象名称")]
		public static readonly Property<string> TargetNameProperty = P<PushTarget>.Register(e => e.TargetName);

		/// <summary>
		/// 对象名称
		/// </summary>
		public string TargetName
		{
			get { return GetProperty(TargetNameProperty); }
			set { SetProperty(TargetNameProperty, value); }
		}
		#endregion

		#region 对象类型 TargetType
		/// <summary>
		/// 对象类型
		/// </summary>
		[Label("对象类型")]
		public static readonly Property<PushTargetEnum> TargetTypeProperty = P<PushTarget>.Register(e => e.TargetType);

		/// <summary>
		/// 对象类型
		/// </summary>
		public PushTargetEnum TargetType
		{
			get { return GetProperty(TargetTypeProperty); }
			set { SetProperty(TargetTypeProperty, value); }
		}
		#endregion

		#region 推送升级机制 PushUpgradeRule
		/// <summary>
		/// 推送升级机制Id
		/// </summary>
		public static readonly IRefIdProperty PushUpgradeRuleIdProperty = P<PushTarget>.RegisterRefId(e => e.PushUpgradeRuleId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<PushUpgradeRule> PushUpgradeRuleProperty = P<PushTarget>.RegisterRef(e => e.PushUpgradeRule, PushUpgradeRuleIdProperty);

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
	/// 推送对象 实体配置
	/// </summary>
	internal class PushTargetConfig : EntityConfig<PushTarget>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNORMAL_PUSH_TARGET").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}