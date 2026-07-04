using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.InventoryTasks
{
	/// <summary>
	/// 工治具盘点任务盘点人
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("工治具盘点任务盘点人")]
	public partial class InventoryTaskFixtureCounter : DataEntity
	{
		#region 初盘 First
		/// <summary>
		/// 初盘
		/// </summary>
		[Label("初盘")]
		public static readonly Property<bool> FirstProperty = P<InventoryTaskFixtureCounter>.Register(e => e.First);

		/// <summary>
		/// 初盘
		/// </summary>
		public bool First
		{
			get { return GetProperty(FirstProperty); }
			set { SetProperty(FirstProperty, value); }
		}
		#endregion

		#region 复盘 Second
		/// <summary>
		/// 复盘
		/// </summary>
		[Label("复盘")]
		public static readonly Property<bool> SecondProperty = P<InventoryTaskFixtureCounter>.Register(e => e.Second);

		/// <summary>
		/// 复盘
		/// </summary>
		public bool Second
		{
			get { return GetProperty(SecondProperty); }
			set { SetProperty(SecondProperty, value); }
		}
		#endregion

		#region 员工 Employee
		/// <summary>
		/// 员工Id
		/// </summary>
		[Label("员工")]
		public static readonly IRefIdProperty EmployeeIdProperty = P<InventoryTaskFixtureCounter>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Employee> EmployeeProperty = P<InventoryTaskFixtureCounter>.RegisterRef(e => e.Employee, EmployeeIdProperty);

		/// <summary>
		/// 员工
		/// </summary>
		public Employee Employee
		{
			get { return GetRefEntity(EmployeeProperty); }
			set { SetRefEntity(EmployeeProperty, value); }
		}
		#endregion

		#region 盘点任务 InventoryTask
		/// <summary>
		/// 盘点任务Id
		/// </summary>
		[Label("盘点任务")]
		public static readonly IRefIdProperty InventoryTaskIdProperty = P<InventoryTaskFixtureCounter>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Parent);

		/// <summary>
		/// 盘点任务Id
		/// </summary>
		public double InventoryTaskId
		{
			get { return (double)GetRefId(InventoryTaskIdProperty); }
			set { SetRefId(InventoryTaskIdProperty, value); }
		}

		/// <summary>
		/// 盘点任务
		/// </summary>
		public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty = P<InventoryTaskFixtureCounter>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

		/// <summary>
		/// 盘点任务
		/// </summary>
		public InventoryTask InventoryTask
		{
			get { return GetRefEntity(InventoryTaskProperty); }
			set { SetRefEntity(InventoryTaskProperty, value); }
		}
		#endregion

		#region 是否只读 IsReadOnly
		/// <summary>
		/// 是否只读
		/// </summary>
		[Label("是否只读")]
		public static readonly Property<bool> IsReadOnlyProperty = P<InventoryTaskFixtureCounter>.Register(e => e.IsReadOnly);

		/// <summary>
		/// 是否只读
		/// </summary>
		public bool IsReadOnly
		{
			get { return this.GetProperty(IsReadOnlyProperty); }
			set { this.SetProperty(IsReadOnlyProperty, value); }
		}
		#endregion


		#region 视图属性
		#region 工号 EmployeeCode
		/// <summary>
		/// 工号
		/// </summary>
		[Label("工号")]
		public static readonly Property<string> EmployeeCodeProperty = P<InventoryTaskFixtureCounter>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

		/// <summary>
		/// 工号
		/// </summary>
		public string EmployeeCode
		{
			get { return this.GetProperty(EmployeeCodeProperty); }
		}
		#endregion

		#region 姓名 Name
		/// <summary>
		/// 姓名
		/// </summary>
		[Label("姓名")]
		public static readonly Property<string> NameProperty = P<InventoryTaskFixtureCounter>.RegisterView(e => e.Name, p => p.Employee.Name);

		/// <summary>
		/// 姓名
		/// </summary>
		public string Name
		{
			get { return this.GetProperty(NameProperty); }
		}
		#endregion
		#endregion
	}

	/// <summary>
	/// 盘点任务盘点人 实体配置
	/// </summary>
	internal class InventoryTaskFixtureCounterConfig : EntityConfig<InventoryTaskFixtureCounter>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_INV_TASK_FIX_CNT").MapAllPropertiesExcept(InventoryTaskFixtureCounter.IsReadOnlyProperty);
			Meta.EnablePhantoms();
		}
	}
}