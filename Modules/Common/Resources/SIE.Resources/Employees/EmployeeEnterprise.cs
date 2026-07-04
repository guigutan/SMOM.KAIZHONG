using System;
using SIE.DataAuth;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;

namespace SIE.Resources.Employees
{
	/// <summary>
	/// 员工与企业关系
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("员工与企业关系")]
	[EmployeeAuthAttribute(nameof(EmployeeId), nameof(EnterpriseId))]
	public partial class EmployeeEnterprise : DataEntity
	{
		#region 工厂 Enterprise
		/// <summary>
		/// 工厂Id
		/// </summary>
        [Label("工厂")]
		public static readonly IRefIdProperty EnterpriseIdProperty = P<EmployeeEnterprise>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

		/// <summary>
		/// 工厂Id
		/// </summary>
		public double EnterpriseId
		{
			get { return (double)GetRefId(EnterpriseIdProperty); }
			set { SetRefId(EnterpriseIdProperty, value); }
		}

		/// <summary>
		/// 工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<EmployeeEnterprise>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Enterprise
		{
			get { return GetRefEntity(EnterpriseProperty); }
			set { SetRefEntity(EnterpriseProperty, value); }
		}
		#endregion

		#region 工厂编码 EnterpriseCode
		/// <summary>
		/// 工厂编码
		/// </summary>
		[Label("编码")]
		public static readonly Property<string> EnterpriseCodeProperty = P<EmployeeEnterprise>.RegisterView(e => e.EnterpriseCode, p => p.Enterprise.Code);

		/// <summary>
		/// 工厂编码
		/// </summary>
		public string EnterpriseCode
		{
			get { return this.GetProperty(EnterpriseCodeProperty); }
		}
		#endregion

		#region 资源名称 EnterpriseName
		/// <summary>
		/// 资源名称
		/// </summary>
		[Label("名称")]
		public static readonly Property<string> EnterpriseNameProperty = P<EmployeeEnterprise>.RegisterView(e => e.EnterpriseName, p => p.Enterprise.Name);

		/// <summary>
		/// 资源名称
		/// </summary>
		public string EnterpriseName
		{
			get { return this.GetProperty(EnterpriseNameProperty); }
		}
		#endregion

		#region 员工 Employee
		/// <summary>
		/// 员工Id
		/// </summary>
		[Label("员工")]
		public static readonly IRefIdProperty EmployeeIdProperty = P<EmployeeEnterprise>.RegisterRefId(e => e.EmployeeId, ReferenceType.Parent);

		/// <summary>
		/// 员工Id
		/// </summary>
		public double EmployeeId
		{
			get { return (double)GetRefId(EmployeeIdProperty); }
			set { SetRefId(EmployeeIdProperty, value); }
		}

		/// <summary>
		/// 员工
		/// </summary>
		public static readonly RefEntityProperty<Employee> EmployeeProperty = P<EmployeeEnterprise>.RegisterRef(e => e.Employee, EmployeeIdProperty);

		/// <summary>
		/// 员工
		/// </summary>
		public Employee Employee
		{
			get { return GetRefEntity(EmployeeProperty); }
			set { SetRefEntity(EmployeeProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 员工与企业关系 实体配置
	/// </summary>
	internal class EmployeeEnterpriseConfig : EntityConfig<EmployeeEnterprise>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("RES_EMP_ENTERPRISE").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}