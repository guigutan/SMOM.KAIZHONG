using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.AlarmStates
{
	/// <summary>
	/// 设备物联标签
	/// </summary>
	[RootEntity, Serializable]	
	[Label("设备物联标签")]
	public partial class EquipmentTag : DataEntity
	{
		#region TAG全称 FullName
		/// <summary>
		/// TAG全称
		/// </summary>
		[Label("TAG全称")]
		public static readonly Property<string> FullNameProperty = P<EquipmentTag>.Register(e => e.FullName);

		/// <summary>
		/// TAG全称
		/// </summary>
		public string FullName
		{
			get { return GetProperty(FullNameProperty); }
			set { SetProperty(FullNameProperty, value); }
		}
		#endregion

		#region 描述 Description
		/// <summary>
		/// 描述
		/// </summary>
		[Label("描述")]
		public static readonly Property<string> DescriptionProperty = P<EquipmentTag>.Register(e => e.Description);

		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			get { return GetProperty(DescriptionProperty); }
			set { SetProperty(DescriptionProperty, value); }
		}
		#endregion

		#region 最大值 MaxValue
		/// <summary>
		/// 最大值
		/// </summary>
		[Label("最大值")]
		public static readonly Property<decimal?> MaxValueProperty = P<EquipmentTag>.Register(e => e.MaxValue);

		/// <summary>
		/// 最大值
		/// </summary>
		public decimal? MaxValue
		{
			get { return GetProperty(MaxValueProperty); }
			set { SetProperty(MaxValueProperty, value); }
		}
		#endregion

		#region 最小值 MinValue
		/// <summary>
		/// 最小值
		/// </summary>
		[Label("最小值")]
		public static readonly Property<decimal?> MinValueProperty = P<EquipmentTag>.Register(e => e.MinValue);

		/// <summary>
		/// 最小值
		/// </summary>
		public decimal? MinValue
		{
			get { return GetProperty(MinValueProperty); }
			set { SetProperty(MinValueProperty, value); }
		}
		#endregion

		#region 设备 EquipAccount
		/// <summary>
		/// 设备Id
		/// </summary>
		[Label("设备")]
		public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipmentTag>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<EquipmentTag>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

		/// <summary>
		/// 设备
		/// </summary>
		public EquipAccountSelect EquipAccount
		{
			get { return GetRefEntity(EquipAccountProperty); }
			set { SetRefEntity(EquipAccountProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 设备物联标签 实体配置
	/// </summary>
	internal class EquipmentTagConfig : EntityConfig<EquipmentTag>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_EQP_TAG").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}