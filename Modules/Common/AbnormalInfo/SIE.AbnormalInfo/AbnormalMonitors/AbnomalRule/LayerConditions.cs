using SIE;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常规则层别条件
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("异常规则层别条件")]
	public partial class LayerConditions : DataEntity
	{
		#region 范围条件 IsWhere
		/// <summary>
		/// 范围条件
		/// </summary>
		[Label("范围条件")]
		public static readonly Property<bool> IsWhereProperty = P<LayerConditions>.Register(e => e.IsWhere);

		/// <summary>
		/// 范围条件
		/// </summary>
		public bool IsWhere
		{
			get { return GetProperty(IsWhereProperty); }
			set { SetProperty(IsWhereProperty, value); }
		}
		#endregion

		#region 分组条件 IsGroup
		/// <summary>
		/// 分组条件
		/// </summary>
		[Label("分组条件")]
		public static readonly Property<bool> IsGroupProperty = P<LayerConditions>.Register(e => e.IsGroup);

		/// <summary>
		/// 分组条件
		/// </summary>
		public bool IsGroup
		{
			get { return GetProperty(IsGroupProperty); }
			set { SetProperty(IsGroupProperty, value); }
		}
		#endregion

		#region 计算指标 IsCacul
		/// <summary>
		/// 计算指标
		/// </summary>
		[Label("计算指标")]
		public static readonly Property<bool> IsCaculProperty = P<LayerConditions>.Register(e => e.IsCacul);

		/// <summary>
		/// 计算指标
		/// </summary>
		public bool IsCacul
		{
			get { return GetProperty(IsCaculProperty); }
			set { SetProperty(IsCaculProperty, value); }
		}
		#endregion

		#region 层别名 LayerName
		/// <summary>
		/// 层别名
		/// </summary>
		[Label("层别名")]
		public static readonly Property<string> LayerNameProperty = P<LayerConditions>.Register(e => e.LayerName);

		/// <summary>
		/// 层别名
		/// </summary>
		public string LayerName
		{
			get { return GetProperty(LayerNameProperty); }
			set { SetProperty(LayerNameProperty, value); }
		}
		#endregion

		#region 层别字段 LayerColumn
		/// <summary>
		/// 层别字段
		/// </summary>
		[Label("层别字段")]
		public static readonly Property<string> LayerColumnProperty = P<LayerConditions>.Register(e => e.LayerColumn);

		/// <summary>
		/// 层别字段
		/// </summary>
		public string LayerColumn
		{
			get { return GetProperty(LayerColumnProperty); }
			set { SetProperty(LayerColumnProperty, value); }
		}
		#endregion

		#region 值1 Value1
		/// <summary>
		/// 值1
		/// </summary>
		[Label("值1")]
		public static readonly Property<string> Value1Property = P<LayerConditions>.Register(e => e.Value1);

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
		public static readonly Property<string> Value2Property = P<LayerConditions>.Register(e => e.Value2);

		/// <summary>
		/// 值2
		/// </summary>
		public string Value2
		{
			get { return GetProperty(Value2Property); }
			set { SetProperty(Value2Property, value); }
		}
		#endregion

		#region 编辑类型 FieldProp
		/// <summary>
		/// 编辑类型
		/// </summary>
		[Label("字段属性")]
		public static readonly Property<FieldProp> FieldPropProperty = P<LayerConditions>.Register(e => e.FieldProp);

		/// <summary>
		/// 字段属性
		/// </summary>
		public FieldProp FieldProp
		{
			get { return GetProperty(FieldPropProperty); }
			set { SetProperty(FieldPropProperty, value); }
		}
		#endregion

		#region 字段类型 PropType
		/// <summary>
		/// 字段类型
		/// </summary>
		[Label("字段类型")]
		public static readonly Property<string> PropTypeProperty = P<LayerConditions>.Register(e => e.PropType);

		/// <summary>
		/// 字段类型
		/// </summary>
		public string PropType
		{
			get { return GetProperty(PropTypeProperty); }
			set { SetProperty(PropTypeProperty, value); }
		}
		#endregion

		#region 字段实体名称 PropName
		/// <summary>
		/// 字段名称
		/// </summary>
		[Label("字段名称")]
		public static readonly Property<string> PropNameProperty = P<LayerConditions>.Register(e => e.PropName);

		/// <summary>
		/// 字段名称
		/// </summary>
		public string PropName
		{
			get { return GetProperty(PropNameProperty); }
			set { SetProperty(PropNameProperty, value); }
		}
		#endregion

		#region 字段所在表 PropTabName
		/// <summary>
		/// 字段所在表
		/// </summary>
		[Label("字段所在表")]
		public static readonly Property<string> PropTabNameProperty = P<LayerConditions>.Register(e => e.PropTabName);

		/// <summary>
		/// 字段所在表
		/// </summary>
		public string PropTabName
		{
			get { return GetProperty(PropTabNameProperty); }
			set { SetProperty(PropTabNameProperty, value); }
		}
		#endregion

		#region 显示表名 PropDisTabName
		/// <summary>
		/// 显示表名
		/// </summary>
		[Label("显示表名")]
		public static readonly Property<string> PropDisTabNameProperty = P<LayerConditions>.Register(e => e.PropDisTabName);

		/// <summary>
		/// 显示表名
		/// </summary>
		public string PropDisTabName
		{
			get { return GetProperty(PropDisTabNameProperty); }
			set { SetProperty(PropDisTabNameProperty, value); }
		}
		#endregion

		#region 运算符 Operator
		/// <summary>
		/// 运算符
		/// </summary>
		[Label("")]
		public static readonly Property<Operator?> OperatorProperty = P<LayerConditions>.Register(e => e.Operator);

		/// <summary>
		/// 运算符
		/// </summary>
		public Operator? Operator
		{
			get { return GetProperty(OperatorProperty); }
			set { SetProperty(OperatorProperty, value); }
		}
		#endregion

		#region 异常判定规则 AbnormalDecisionRule
		/// <summary>
		/// 异常判定规则Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalDecisionRuleIdProperty = P<LayerConditions>.RegisterRefId(e => e.AbnormalDecisionRuleId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<AbnormalDecisionRule> AbnormalDecisionRuleProperty = P<LayerConditions>.RegisterRef(e => e.AbnormalDecisionRule, AbnormalDecisionRuleIdProperty);

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
	/// 异常规则层别条件 实体配置
	/// </summary>
	internal class LayerConditionsConfig : EntityConfig<LayerConditions>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNOMAL_RULE_LAYER_CDTS").MapAllPropertiesExcept(LayerConditions.PropNameProperty);
			Meta.Property(LayerConditions.LayerNameProperty).ColumnMeta.HasLength(500);
			Meta.Property(LayerConditions.PropTypeProperty).ColumnMeta.HasLength(200);
			Meta.EnablePhantoms();
		}
	}
}