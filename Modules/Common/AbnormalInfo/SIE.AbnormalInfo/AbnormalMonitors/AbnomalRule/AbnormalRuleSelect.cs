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
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("监控实体数据源查询数据列")]
	public partial class AbnormalRuleSelect : DataEntity
	{
		#region 显示列名 ColumnName
		/// <summary>
		/// 显示列名
		/// </summary>
		[Label("显示列名")]
		public static readonly Property<string> ColumnNameProperty = P<AbnormalRuleSelect>.Register(e => e.ColumnName);

		/// <summary>
		/// 显示列名
		/// </summary>
		public string ColumnName
		{
			get { return GetProperty(ColumnNameProperty); }
			set { SetProperty(ColumnNameProperty, value); }
		}
		#endregion

		#region 显示列 Column
		/// <summary>
		/// 显示列
		/// </summary>
		[Label("显示列")]
		public static readonly Property<string> ColumnProperty = P<AbnormalRuleSelect>.Register(e => e.Column);

		/// <summary>
		/// 显示列
		/// </summary>
		public string Column
		{
			get { return GetProperty(ColumnProperty); }
			set { SetProperty(ColumnProperty, value); }
		}
		#endregion

		#region  TabName
		/// <summary>
		/// 
		/// </summary>
		[Label("")]
		public static readonly Property<string> TabNameProperty = P<AbnormalRuleSelect>.Register(e => e.TabName);

		/// <summary>
		/// 
		/// </summary>
		public string TabName
		{
			get { return GetProperty(TabNameProperty); }
			set { SetProperty(TabNameProperty, value); }
		}
		#endregion

		#region 异常判定规则 AbnormalDecisionRule
		/// <summary>
		/// 异常判定规则Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalDecisionRuleIdProperty = P<AbnormalRuleSelect>.RegisterRefId(e => e.AbnormalDecisionRuleId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<AbnormalDecisionRule> AbnormalDecisionRuleProperty = P<AbnormalRuleSelect>.RegisterRef(e => e.AbnormalDecisionRule, AbnormalDecisionRuleIdProperty);

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
	/// 监控实体数据源查询数据列 实体配置
	/// </summary>
	internal class AbnormalRuleSelectConfig : EntityConfig<AbnormalRuleSelect>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNOMAL_RULE_SLT").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}