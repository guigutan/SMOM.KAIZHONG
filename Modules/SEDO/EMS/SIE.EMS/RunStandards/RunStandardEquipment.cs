using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.RunStandards
{
	/// <summary>
	/// 维修运行定标设备清单
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("维修运行定标设备清单")]
	public partial class RunStandardEquipment : DataEntity
	{
		#region 设备 EquipAccount
		/// <summary>
		/// 设备Id
		/// </summary>
		[Label("设备")]
		public static readonly IRefIdProperty EquipAccountIdProperty = P<RunStandardEquipment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

		/// <summary>
		/// 设备Id
		/// </summary>
		public double EquipAccountId
		{
			get { return (double)GetRefId(EquipAccountIdProperty); }
			set { SetRefId(EquipAccountIdProperty, value); }
		}

		/// <summary>
		/// 设备
		/// </summary>
		public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<RunStandardEquipment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

		/// <summary>
		/// 设备
		/// </summary>
		public EquipAccountSelect EquipAccount
		{
			get { return GetRefEntity(EquipAccountProperty); }
			set { SetRefEntity(EquipAccountProperty, value); }
		}
		#endregion

		#region 设备运行定标 RunStandard
		/// <summary>
		/// 设备运行定标Id
		/// </summary>
		[Label("设备运行定标")]
		public static readonly IRefIdProperty RunStandardIdProperty = P<RunStandardEquipment>.RegisterRefId(e => e.RunStandardId, ReferenceType.Parent);

		/// <summary>
		/// 设备运行定标Id
		/// </summary>
		public double RunStandardId
		{
			get { return (double)GetRefId(RunStandardIdProperty); }
			set { SetRefId(RunStandardIdProperty, value); }
		}

		/// <summary>
		/// 设备运行定标
		/// </summary>
		public static readonly RefEntityProperty<RunStandard> RunStandardProperty = P<RunStandardEquipment>.RegisterRef(e => e.RunStandard, RunStandardIdProperty);

		/// <summary>
		/// 设备运行定标
		/// </summary>
		public RunStandard RunStandard
		{
			get { return GetRefEntity(RunStandardProperty); }
			set { SetRefEntity(RunStandardProperty, value); }
		}
		#endregion


		#region 视图属性
		#region 设备编码 EquipAccountCode
		/// <summary>
		/// 设备编码
		/// </summary>
		[Label("设备编码")]
		public static readonly Property<string> EquipAccountCodeProperty = P<RunStandardEquipment>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

		/// <summary>
		/// 设备编码
		/// </summary>
		public string EquipAccountCode
		{
			get { return this.GetProperty(EquipAccountCodeProperty); }
		}
		#endregion

		#region 设备名称 EquipAccountName
		/// <summary>
		/// 设备名称
		/// </summary>
		[Label("设备名称")]
		public static readonly Property<string> EquipAccountNameProperty = P<RunStandardEquipment>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipAccountName
		{
			get { return this.GetProperty(EquipAccountNameProperty); }
		}
		#endregion

		#region 技术型号 Specifications
		/// <summary>
		/// 技术型号
		/// </summary>
		[Label("技术型号")]
		public static readonly Property<string> SpecificationsProperty = P<RunStandardEquipment>.RegisterView(e => e.Specifications, p => p.EquipAccount.EquipModel.Specifications);

		/// <summary>
		///技术型号
		/// </summary>
		public string Specifications
		{
			get { return this.GetProperty(SpecificationsProperty); }
		}
		#endregion

		#region 设备类别 EquipTypeCategory
		/// <summary>
		/// 设备类别
		/// </summary>
		[Label("设备类别")]
		public static readonly Property<string> EquipTypeCategoryProperty = P<RunStandardEquipment>.RegisterView(e => e.EquipTypeCategory, p => p.EquipAccount.EquipModel.TypeCategory);

		/// <summary>
		/// 设备类别
		/// </summary>
		public string EquipTypeCategory
		{
			get { return this.GetProperty(EquipTypeCategoryProperty); }
		}
		#endregion

		#region 生产厂家 Manufacturers
		/// <summary>
		/// 生产厂家
		/// </summary>
		[Label("生产厂家")]
		public static readonly Property<string> ManufacturersProperty = P<RunStandardEquipment>.RegisterView(e => e.Manufacturers, p => p.EquipAccount.EquipModel.Manufacturers);

		/// <summary>
		///生产厂家
		/// </summary>
		public string Manufacturers
		{
			get { return this.GetProperty(ManufacturersProperty); }
		}
		#endregion

		#region 使用部门 Manufacturers
		/// <summary>
		/// 使用部门
		/// </summary>
		[Label("使用部门")]
		public static readonly Property<string> UseDepartmentNameProperty = P<RunStandardEquipment>.RegisterView(e => e.UseDepartmentName, p => p.EquipAccount.UseDepartment.Name);

		/// <summary>
		///使用部门
		/// </summary>
		public string UseDepartmentName
		{
			get { return this.GetProperty(UseDepartmentNameProperty); }
		}
		#endregion

		#region 管理状态 UseState
		/// <summary>
		/// 管理状态
		/// </summary>
		[Label("管理状态")]
		public static readonly Property<AccountUseState> UseStateProperty = P<RunStandardEquipment>.RegisterView(e => e.UseState, p => p.EquipAccount.UseState);

		/// <summary>
		///管理状态
		/// </summary>
		public AccountUseState UseState
		{
			get { return this.GetProperty(UseStateProperty); }
		}
		#endregion

		#region 立卡日期 UseState
		/// <summary>
		/// 立卡日期
		/// </summary>
		[Label("立卡日期")]
		public static readonly Property<DateTime?> CardDateProperty = P<RunStandardEquipment>.RegisterView(e => e.CardDate, p => p.EquipAccount.CardDate);

		/// <summary>
		///立卡日期
		/// </summary>
		public DateTime? CardDate
		{
			get { return this.GetProperty(CardDateProperty); }
		}
		#endregion
		#endregion

	}

	/// <summary>
	/// 维修运行定标设备清单 实体配置
	/// </summary>
	internal class RunStandardEquipmentConfig : EntityConfig<RunStandardEquipment>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_RUN_STD_EQP").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}