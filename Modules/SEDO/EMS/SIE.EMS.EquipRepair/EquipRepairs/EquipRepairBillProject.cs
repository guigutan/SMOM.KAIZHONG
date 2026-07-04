using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
	/// <summary>
	/// 设备维修单维修规程
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("设备维修单维修规程")]
	public partial class EquipRepairBillProject : DataEntity
	{
		#region 部位 Part
		/// <summary>
		/// 部位
		/// </summary>
		[Label("部位")]
		public static readonly Property<string> PartProperty = P<EquipRepairBillProject>.Register(e => e.Part);

		/// <summary>
		/// 部位
		/// </summary>
		public string Part
		{
			get { return GetProperty(PartProperty); }
			set { SetProperty(PartProperty, value); }
		}
		#endregion

		#region 项目耗材 Consumable
		/// <summary>
		/// 项目耗材
		/// </summary>
		[Label("项目耗材")]
		public static readonly Property<string> ConsumableProperty = P<EquipRepairBillProject>.Register(e => e.Consumable);

		/// <summary>
		/// 项目耗材
		/// </summary>
		public string Consumable
		{
			get { return GetProperty(ConsumableProperty); }
			set { SetProperty(ConsumableProperty, value); }
		}
		#endregion

		#region 操作方法 Method
		/// <summary>
		/// 操作方法
		/// </summary>
		[Label("操作方法")]
		public static readonly Property<string> MethodProperty = P<EquipRepairBillProject>.Register(e => e.Method);

		/// <summary>
		/// 操作方法
		/// </summary>
		public string Method
		{
			get { return GetProperty(MethodProperty); }
			set { SetProperty(MethodProperty, value); }
		}
		#endregion

		#region 标准 Standard
		/// <summary>
		/// 标准
		/// </summary>
		[Label("标准")]
		public static readonly Property<string> StandardProperty = P<EquipRepairBillProject>.Register(e => e.Standard);

		/// <summary>
		/// 标准
		/// </summary>
		public string Standard
		{
			get { return GetProperty(StandardProperty); }
			set { SetProperty(StandardProperty, value); }
		}
		#endregion

		#region 最小值 MinValue
		/// <summary>
		/// 最小值
		/// </summary>
		[Label("最小值")]
		public static readonly Property<decimal?> MinValueProperty = P<EquipRepairBillProject>.Register(e => e.MinValue);

		/// <summary>
		/// 最小值
		/// </summary>
		public decimal? MinValue
		{
			get { return GetProperty(MinValueProperty); }
			set { SetProperty(MinValueProperty, value); }
		}
		#endregion

		#region 最大值 MaxValue
		/// <summary>
		/// 最大值
		/// </summary>
		[Label("最大值")]
		public static readonly Property<decimal?> MaxValueProperty = P<EquipRepairBillProject>.Register(e => e.MaxValue);

		/// <summary>
		/// 最大值
		/// </summary>
		public decimal? MaxValue
		{
			get { return GetProperty(MaxValueProperty); }
			set { SetProperty(MaxValueProperty, value); }
		}
		#endregion

		#region 单位 Unit
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
		public static readonly Property<string> UnitProperty = P<EquipRepairBillProject>.Register(e => e.Unit);

		/// <summary>
		/// 单位
		/// </summary>
		public string Unit
		{
			get { return GetProperty(UnitProperty); }
			set { SetProperty(UnitProperty, value); }
		}
		#endregion

		#region 用时(分钟) UseTime
		/// <summary>
		/// 用时(分钟)
		/// </summary>
		[Label("用时(分钟)")]
		public static readonly Property<string> UseTimeProperty = P<EquipRepairBillProject>.Register(e => e.UseTime);

		/// <summary>
		/// 用时(分钟)
		/// </summary>
		public string UseTime
		{
			get { return GetProperty(UseTimeProperty); }
			set { SetProperty(UseTimeProperty, value); }
		}
		#endregion

		#region 点检保养项目 ProjectDetail
		/// <summary>
		/// 点检保养项目Id
		/// </summary>
		[Label("点检保养项目")]
		public static readonly IRefIdProperty ProjectDetailIdProperty = P<EquipRepairBillProject>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Normal);

		/// <summary>
		/// 点检保养项目Id
		/// </summary>
		public double ProjectDetailId
		{
			get { return (double)GetRefId(ProjectDetailIdProperty); }
			set { SetRefId(ProjectDetailIdProperty, value); }
		}

		/// <summary>
		/// 点检保养项目
		/// </summary>
		public static readonly RefEntityProperty<ProjectDetail> ProjectDetailProperty = P<EquipRepairBillProject>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

		/// <summary>
		/// 点检保养项目
		/// </summary>
		public ProjectDetail ProjectDetail
		{
			get { return GetRefEntity(ProjectDetailProperty); }
			set { SetRefEntity(ProjectDetailProperty, value); }
		}
		#endregion

		#region 设备维修单 EquipRepairBill
		/// <summary>
		/// 设备维修单Id
		/// </summary>
		[Label("设备维修单")]
		public static readonly IRefIdProperty EquipRepairBillIdProperty = P<EquipRepairBillProject>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Parent);

		/// <summary>
		/// 设备维修单Id
		/// </summary>
		public double EquipRepairBillId
		{
			get { return (double)GetRefId(EquipRepairBillIdProperty); }
			set { SetRefId(EquipRepairBillIdProperty, value); }
		}

		/// <summary>
		/// 设备维修单
		/// </summary>
		public static readonly RefEntityProperty<EquipRepairBill> EquipRepairBillProperty = P<EquipRepairBillProject>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

		/// <summary>
		/// 设备维修单
		/// </summary>
		public EquipRepairBill EquipRepairBill
		{
			get { return GetRefEntity(EquipRepairBillProperty); }
			set { SetRefEntity(EquipRepairBillProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 设备维修单维修规程 实体配置
	/// </summary>
	internal class EquipRepairBillProjectConfig : EntityConfig<EquipRepairBillProject>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_REPR_BILL_PRJ").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}