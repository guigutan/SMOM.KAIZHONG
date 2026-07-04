using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 监控实体数据源查询数据列
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("监控实体表关系")]
	public partial class AbnormalRuleTabRelation : DataEntity
	{
		#region 表名 TabName
		/// <summary>
		/// 表名
		/// </summary>
		[Label("表名")]
		public static readonly Property<string> TabNameProperty = P<AbnormalRuleTabRelation>.Register(e => e.TabName);

		/// <summary>
		/// 表名
		/// </summary>
		public string TabName
		{
			get { return GetProperty(TabNameProperty); }
			set { SetProperty(TabNameProperty, value); }
		}
		#endregion

		#region 字段类型 TabType
		/// <summary>
		/// 字段类型
		/// </summary>
		[Label("字段类型")]
		public static readonly Property<string> TabTypeProperty = P<AbnormalRuleTabRelation>.Register(e => e.TabType);

		/// <summary>
		/// 字段类型
		/// </summary>
		public string TabType
		{
			get { return GetProperty(TabTypeProperty); }
			set { SetProperty(TabTypeProperty, value); }
		}
		#endregion

		#region 父级表名 parentTabName
		/// <summary>
		/// 父级表名
		/// </summary>
		[Label("父级表名")]
		public static readonly Property<string> parentTabNameProperty = P<AbnormalRuleTabRelation>.Register(e => e.parentTabName);

		/// <summary>
		/// 父级表名
		/// </summary>
		public string parentTabName
		{
			get { return GetProperty(parentTabNameProperty); }
			set { SetProperty(parentTabNameProperty, value); }
		}
		#endregion

		#region  RefPColumnName
		/// <summary>
		/// 引用父级实体，父级字段名,一对多
		/// </summary>
		[Label("引用父级实体，父级字段名")]
		public static readonly Property<string> RefPColumnNameProperty = P<AbnormalRuleTabRelation>.Register(e => e.RefPColumnName);

		/// <summary>
		/// 父级关联字段名
		/// </summary>
		public string RefPColumnName
		{
			get { return GetProperty(RefPColumnNameProperty); }
			set { SetProperty(RefPColumnNameProperty, value); }
		}
		#endregion

		#region  SuperRefColumnName
		/// <summary>
		/// 上级引用当前表，当前表字段名，一对一
		/// </summary>
		[Label("上级引用当前表，当前表字段名")]
		public static readonly Property<string> SuperRefColumnNameProperty = P<AbnormalRuleTabRelation>.Register(e => e.SuperRefColumnName);

		/// <summary>
		/// 上级引用当前表，当前表字段名
		/// </summary>
		public string SuperRefColumnName
		{
			get { return GetProperty(SuperRefColumnNameProperty); }
			set { SetProperty(SuperRefColumnNameProperty, value); }
		}
		#endregion
		#region 异常判定规则 AbnormalDecisionRule
		/// <summary>
		/// 异常判定规则Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalDecisionRuleIdProperty = P<AbnormalRuleTabRelation>.RegisterRefId(e => e.AbnormalDecisionRuleId, ReferenceType.Parent);

		/// <summary>
		/// 异常判定规则Id
		/// </summary>
		public double AbnormalDecisionRuleId
		{
			get { return (double)GetRefId(AbnormalDecisionRuleIdProperty); }
			set { SetRefId(AbnormalDecisionRuleIdProperty, value); }
		}

		/// <summary>
		/// 异常判定规则
		/// </summary>
		public static readonly RefEntityProperty<AbnormalDecisionRule> AbnormalDecisionRuleProperty = P<AbnormalRuleTabRelation>.RegisterRef(e => e.AbnormalDecisionRule, AbnormalDecisionRuleIdProperty);

		/// <summary>
		/// 异常判定规则
		/// </summary>
		public AbnormalDecisionRule AbnormalDecisionRule
		{
			get { return GetRefEntity(AbnormalDecisionRuleProperty); }
			set { SetRefEntity(AbnormalDecisionRuleProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 监控实体表关系查询数据列 实体配置
	/// </summary>
	internal class AbnormalRuleTabRelationConfig : EntityConfig<AbnormalRuleTabRelation>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNOMAL_RULE_TAB_RLT").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}