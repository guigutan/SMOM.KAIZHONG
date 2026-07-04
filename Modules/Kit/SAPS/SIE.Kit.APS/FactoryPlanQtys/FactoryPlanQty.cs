using SIE;
using SIE.DataAuth;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.FactoryPlanQtys
{
	/// <summary>
	/// 工厂计划数配置
	/// </summary>
	[RootEntity, Serializable]
	[ConditionQueryType(typeof(FactoryPlanQtyCriteria))]
	[EntityDataAuthAttribute(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	[Label("工厂计划数配置")]
	public partial class FactoryPlanQty : DataEntity
	{
		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<FactoryPlanQty>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

		/// <summary>
		/// 工厂Id 
		/// </summary>
		public double? FactoryId
		{
			get { return (double?)GetRefNullableId(FactoryIdProperty); }
			set { SetRefNullableId(FactoryIdProperty, value); }
		}

		/// <summary>
		/// 工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<FactoryPlanQty>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
		{
			get { return GetRefEntity(FactoryProperty); }
			set { SetRefEntity(FactoryProperty, value); }
		}
		#endregion

		#region 日计划数 WorkCeil
		/// <summary>
		/// 日计划数
		/// </summary>
		[Label("日计划数")]
		public static readonly Property<decimal> WorkCeilProperty = P<FactoryPlanQty>.Register(e => e.WorkCeil);

		/// <summary>
		/// 日计划数
		/// </summary>
		public decimal WorkCeil
		{
			get { return GetProperty(WorkCeilProperty); }
			set { SetProperty(WorkCeilProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 工厂计划数配置 实体配置
	/// </summary>
	internal class FactoryPlanQtyConfig : EntityConfig<FactoryPlanQty>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("APS_FACTORY_PLAN_QTY").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}