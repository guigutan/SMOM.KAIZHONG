using SIE;
using SIE.AbnormalInfo.Common;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using Newtonsoft.Json;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常判定规则
	/// </summary>
	[RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig), "异常判定规则编码配置项", "异常判定规则编码配置规则")]
	[EntityWithConfig(typeof(IndicatorNoConfig), " 指标运算条件编码配置项", "指标运算条件编码配置规则")]
	[CriteriaQuery]
	[Label("异常判定规则")]
	[DisplayMember(nameof(Code))]
	public partial class AbnormalDecisionRule : DataEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<AbnormalDecisionRule>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 异常规则 RuleName
		/// <summary>
		/// 异常规则
		/// </summary>
		[Label("异常规则")]
		[Required]
		public static readonly Property<string> RuleNameProperty = P<AbnormalDecisionRule>.Register(e => e.RuleName);

		/// <summary>
		/// 异常规则
		/// </summary>
		public string RuleName
		{
			get { return GetProperty(RuleNameProperty); }
			set { SetProperty(RuleNameProperty, value); }
		}
		#endregion

		#region SQL脚本 DisPlaySelect
		/// <summary>
		/// SQL脚本
		/// </summary>
		[Label("SQL脚本")]
		[MaxLength(3000)]
		public static readonly Property<string> DisPlaySelectProperty = P<AbnormalDecisionRule>.Register(e => e.DisPlaySelect);

		/// <summary>
		/// SQL脚本
		/// </summary>
		public string DisPlaySelect
		{
			get { return GetProperty(DisPlaySelectProperty); }
			set { SetProperty(DisPlaySelectProperty, value); }
		}
		#endregion

		#region SQL脚本 IsSQL
		/// <summary>
		///SQL脚本
		/// </summary>
		[Label("启用SQL脚本")]
		public static readonly Property<bool> IsSQLProperty = P<AbnormalDecisionRule>.Register(e => e.IsSQL);

		/// <summary>
		/// SQL脚本
		/// </summary>
		public bool IsSQL
		{
			get { return GetProperty(IsSQLProperty); }
			set { SetProperty(IsSQLProperty, value); }
		}
		#endregion

		#region 指标条件列表 IndicatorCondtionList
		/// <summary>
		/// 异常规则层别条件列表
		/// </summary>
		public static readonly ListProperty<EntityList<IndicatorCondition>> IndicatorCondtionListProperty = P<AbnormalDecisionRule>.RegisterList(e => e.IndicatorCondtionList);
		/// <summary>
		/// 异常规则层别条件列表
		/// </summary>
		public EntityList<IndicatorCondition> IndicatorCondtionList
		{
			get { return this.GetLazyList(IndicatorCondtionListProperty); }
		}
		#endregion

		#region 层别条件列表 LayerConditionsList
		/// <summary>
		/// 层别条件列表
		/// </summary>
		public static readonly ListProperty<EntityList<LayerConditions>> LayerConditionsListProperty = P<AbnormalDecisionRule>.RegisterList(e => e.LayerConditionsList);
		/// <summary>
		/// 层别条件列表
		/// </summary>
		public EntityList<LayerConditions> LayerConditionsList
		{
			get { return this.GetLazyList(LayerConditionsListProperty); }
		}
		#endregion

		#region 监控显示列 WhereList
		/// <summary>
		/// 监控显示列
		/// </summary>
		public static readonly ListProperty<EntityList<AbnomalRuleWhere>> WhereListProperty = P<AbnormalDecisionRule>.RegisterList(e => e.WhereList);
		/// <summary>
		/// 监控显示列
		/// </summary>
		public EntityList<AbnomalRuleWhere> WhereList
		{
			get { return this.GetLazyList(WhereListProperty); }
		}
		#endregion

		#region 监控实体表关系 TabRelationList
		/// <summary>
		/// 监控显示列
		/// </summary>
		public static readonly ListProperty<EntityList<AbnormalRuleTabRelation>> TabRelationListProperty = P<AbnormalDecisionRule>.RegisterList(e => e.TabRelationList);
		/// <summary>
		/// 监控显示列
		/// </summary>
		public EntityList<AbnormalRuleTabRelation> TabRelationList
		{
			get { return this.GetLazyList(TabRelationListProperty); }
		}
		#endregion

		#region 表关系 TabRelations
		/// <summary>
		/// 表关系
		/// </summary>
		[Label("表关系")]
		public static readonly Property<string> TabRelationsProperty = P<AbnormalDecisionRule>.Register(e => e.TabRelations);

		/// <summary>
		/// 异常类型
		/// </summary>
		public string TabRelations
		{
			get { return GetProperty(TabRelationsProperty); }
			set { SetProperty(TabRelationsProperty, value); }
		}
		#endregion

		#region 异常来源 AbnomalSource
		/// <summary>
		/// 异常来源Id
		/// </summary>
		public static readonly IRefIdProperty AbnomalSourceIdProperty = P<AbnormalDecisionRule>.RegisterRefId(e => e.AbnomalSourceId, ReferenceType.Normal);

		/// <summary>
		/// 异常来源Id
		/// </summary>
		public double AbnomalSourceId
		{
			get { return (double)GetRefId(AbnomalSourceIdProperty); }
			set { SetRefId(AbnomalSourceIdProperty, value); }
		}

		/// <summary>
		/// 异常来源
		/// </summary>
		public static readonly RefEntityProperty<AbnormalSource> AbnomalSourceProperty = P<AbnormalDecisionRule>.RegisterRef(e => e.AbnomalSource, AbnomalSourceIdProperty);

		/// <summary>
		/// 异常来源
		/// </summary>
		public AbnormalSource AbnomalSource
		{
			get { return GetRefEntity(AbnomalSourceProperty); }
			set { SetRefEntity(AbnomalSourceProperty, value); }
		}
		#endregion

		#region 异常类型 AbnormalType
		/// <summary>
		/// 异常类型
		/// </summary>
		[Label("")]
		public static readonly Property<AbnomalType> AbnormalTypeProperty = P<AbnormalDecisionRule>.Register(e => e.AbnormalType);

		/// <summary>
		/// 异常类型
		/// </summary>
		public AbnomalType AbnormalType
		{
			get { return GetProperty(AbnormalTypeProperty); }
			set { SetProperty(AbnormalTypeProperty, value); }
		}
		#endregion

		#region 指标运算

		#region 指标名 IndicatorName
		/// <summary>
		/// 指标名
		/// </summary>
		[Label("指标名")]
		public static readonly Property<string> IndicatorNameProperty = P<AbnormalDecisionRule>.Register(e => e.IndicatorName);

		/// <summary>
		/// 指标名
		/// </summary>
		public string IndicatorName
		{
			get { return GetProperty(IndicatorNameProperty); }
			set { SetProperty(IndicatorNameProperty, value); }
		}
		#endregion

		#region 指标运算表达式 IndicatorOperation
		/// <summary>
		/// 指标运算
		/// </summary>
		[Label("指标运算")]
		public static readonly Property<string> IndicatorOperationProperty = P<AbnormalDecisionRule>.Register(e => e.IndicatorOperation);

		/// <summary>
		/// 指标运算
		/// </summary>
		public string IndicatorOperation
		{
			get { return GetProperty(IndicatorOperationProperty); }
			set { SetProperty(IndicatorOperationProperty, value); }
		}
		#endregion

		#region 运算符 Operator
		/// <summary>
		/// 运算符
		/// </summary>
		[Label("")]
		public static readonly Property<Operator?> OperatorProperty = P<AbnormalDecisionRule>.Register(e => e.Operator);

		/// <summary>
		/// 运算符
		/// </summary>
		public Operator? Operator
		{
			get { return GetProperty(OperatorProperty); }
			set { SetProperty(OperatorProperty, value); }
		}
		#endregion



		#region 值1 Value1
		/// <summary>
		/// 值1
		/// </summary>
		[Label("值1")]
		public static readonly Property<string> Value1Property = P<AbnormalDecisionRule>.Register(e => e.Value1);

		/// <summary>
		/// 值1
		/// </summary>
		public string Value1
		{
			get { return GetProperty(Value1Property); }
			set { SetProperty(Value1Property, value); }
		}
		#endregion

		#region 值2 Value2
		/// <summary>
		/// 值2
		/// </summary>
		[Label("值2")]
		public static readonly Property<string> Value2Property = P<AbnormalDecisionRule>.Register(e => e.Value2);

		/// <summary>
		/// 值2
		/// </summary>
		public string Value2
		{
			get { return GetProperty(Value2Property); }
			set { SetProperty(Value2Property, value); }
		}
		#endregion

		#region 单位 IndicatorUnit
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
		public static readonly Property<IndicatorUnit> IndicatorUnitProperty = P<AbnormalDecisionRule>.Register(e => e.IndicatorUnit);

		/// <summary>
		/// 单位
		/// </summary>
		public IndicatorUnit IndicatorUnit
		{
			get { return GetProperty(IndicatorUnitProperty); }
			set { SetProperty(IndicatorUnitProperty, value); }
		}
		#endregion

		#endregion


		#region 注册视图属性(关联实体属性平铺显示，一般用于Web)

		#region 来源名称 MonitorName
		/// <summary>
		/// 来源名称
		/// </summary>
		[Label("来源名称")]
		public static readonly Property<string> MonitorNameProperty = P<AbnormalDecisionRule>.RegisterView(e => e.MonitorName, p => p.AbnomalSource.MonitorName);

		/// <summary>
		/// 产品名称
		/// </summary>
		public string MonitorName
		{
			get { return this.GetProperty(MonitorNameProperty); }
		}
		#endregion

		#region 监控类型 MonitorType
		/// <summary>
		/// 监控类型
		/// </summary>
		[Label("监控类型")]
		public static readonly Property<string> MonitorTypeProperty = P<AbnormalDecisionRule>.RegisterView(e => e.MonitorType, p => p.AbnomalSource.AbnormalEntityMetadata.Type);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string MonitorType
		{
			get { return this.GetProperty(MonitorTypeProperty); }
		}
		#endregion

		#region 监控实体表名 MonitorTabName
		/// <summary>
		/// 监控实体表名
		/// </summary>
		[Label("监控实体表名")]
		public static readonly Property<string> MonitorTabNameProperty = P<AbnormalDecisionRule>.RegisterView(e => e.MonitorTabName, p => p.AbnomalSource.AbnormalEntityMetadata.TableName);

		/// <summary>
		/// 监控实体表名
		/// </summary>
		public string MonitorTabName
		{
			get { return this.GetProperty(MonitorTabNameProperty); }
		}
		#endregion

		#endregion
	}

	/// <summary>
	/// 异常判定规则 实体配置
	/// </summary>
	internal class AbnormalDecisionRuleConfig : EntityConfig<AbnormalDecisionRule>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNORMAL_DECISION_RULE").MapAllPropertiesExcept(AbnormalDecisionRule.TabRelationsProperty);
			Meta.Property(AbnormalDecisionRule.DisPlaySelectProperty).ColumnMeta.HasLength(3000);
			Meta.EnablePhantoms();
		}
	}
}