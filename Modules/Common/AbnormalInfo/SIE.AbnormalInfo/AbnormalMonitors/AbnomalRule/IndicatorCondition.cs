using SIE;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 规则计算指标条件
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("规则计算指标条件")]
	public partial class IndicatorCondition : DataEntity
	{
		#region 标识 Code
		/// <summary>
		/// 标识
		/// </summary>
		[Label("标识")]
		public static readonly Property<string> CodeProperty = P<IndicatorCondition>.Register(e => e.Code);

		/// <summary>
		/// 标识
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 层别ID 层别ID
		/// <summary>
		/// 层别ID
		/// </summary>
		[Label("层别ID")]
		[Required]
		public static readonly Property<double> LayerConditionIdProperty = P<IndicatorCondition>.Register(e => e.LayerConditionId);

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
		[MaxLength(1000)]
		public static readonly Property<string> LayerNameProperty = P<IndicatorCondition>.Register(e => e.LayerName);

		/// <summary>
		/// 标识
		/// </summary>
		public string LayerName
		{
			get { return GetProperty(LayerNameProperty); }
			set { SetProperty(LayerNameProperty, value); }
		}
		#endregion

		#region 指标取值 IndicatorValue
		/// <summary>
		/// 指标取值
		/// </summary>
		[Label("指标取值")]
		public static readonly Property<IndicatorValue> IndicatorValueProperty = P<IndicatorCondition>.Register(e => e.IndicatorValue);

		/// <summary>
		/// 指标取值
		/// </summary>
		public IndicatorValue IndicatorValue
		{
			get { return GetProperty(IndicatorValueProperty); }
			set { SetProperty(IndicatorValueProperty, value); }
		}
		#endregion

		#region 异常判定规则 AbnormalDecisionRule
		/// <summary>
		/// 异常判定规则Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalDecisionRuleIdProperty = P<IndicatorCondition>.RegisterRefId(e => e.AbnormalDecisionRuleId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<AbnormalDecisionRule> AbnormalDecisionRuleProperty = P<IndicatorCondition>.RegisterRef(e => e.AbnormalDecisionRule, AbnormalDecisionRuleIdProperty);

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
	/// 规则计算指标条件 实体配置
	/// </summary>
	internal class IndicatorConditionConfig : EntityConfig<IndicatorCondition>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
            
            Meta.MapTable("ABNORMAL_RULE_IND_CDTS").MapAllProperties();
            Meta.Property(IndicatorCondition.LayerNameProperty).ColumnMeta.HasLength(3000);
            Meta.EnablePhantoms();
		}
	}
}