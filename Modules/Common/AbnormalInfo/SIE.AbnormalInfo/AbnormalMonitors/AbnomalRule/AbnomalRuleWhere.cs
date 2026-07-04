using SIE;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常规则数据源条件范围
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("异常规则数据源条件范围")]
	public partial class AbnomalRuleWhere : DataEntity
	{
		#region 层别ID 层别ID
		/// <summary>
		/// 层别ID
		/// </summary>
		[Label("层别ID")]
		[Required]
		public static readonly Property<double> LayerConditionIdProperty = P<AbnomalRuleWhere>.Register(e => e.LayerConditionId);

		/// <summary>
		/// 标识
		/// </summary>
		public double LayerConditionId
		{
			get { return GetProperty(LayerConditionIdProperty); }
			set { SetProperty(LayerConditionIdProperty, value); }
		}
		#endregion

		#region 层别ID 层别ID
		/// <summary>
		/// 层别ID
		/// </summary>
		[Label("层别ID")]
		public static readonly Property<string> LayerNameProperty = P<AbnomalRuleWhere>.Register(e => e.LayerName);

		/// <summary>
		/// 标识
		/// </summary>
		public string LayerName
		{
			get { return GetProperty(LayerNameProperty); }
			set { SetProperty(LayerNameProperty, value); }
		}
		#endregion

		#region 逻辑运算符 LogicOpt
		/// <summary>
		/// 逻辑运算符
		/// </summary>
		[Label("逻辑运算符")]
		public static readonly Property<LogicalOperator?> LogicOptProperty = P<AbnomalRuleWhere>.Register(e => e.LogicOpt);

		/// <summary>
		/// 逻辑运算符
		/// </summary>
		public LogicalOperator? LogicOpt
		{
			get { return GetProperty(LogicOptProperty); }
			set { SetProperty(LogicOptProperty, value); }
		}
		#endregion

		#region 异常判定规则 AbnormalDecisionRule
		/// <summary>
		/// 异常判定规则Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalDecisionRuleIdProperty = P<AbnomalRuleWhere>.RegisterRefId(e => e.AbnormalDecisionRuleId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<AbnormalDecisionRule> AbnormalDecisionRuleProperty = P<AbnomalRuleWhere>.RegisterRef(e => e.AbnormalDecisionRule, AbnormalDecisionRuleIdProperty);

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
	/// 异常规则数据源条件范围 实体配置
	/// </summary>
	internal class AbnomalRuleWhereConfig : EntityConfig<AbnomalRuleWhere>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNOMAL_RULE_WHERE").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}