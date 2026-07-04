using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceIOTParas.Details
{
    /// <summary>
    /// 设备清单
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("设备清单")]
	public class FacilityDetail : DataEntity
	{
		#region 设备物联参数 DeviceIOTPara
		/// <summary>
		/// 设备物联参数Id
		/// </summary>
		[Label("设备物联参数")]
		public static readonly IRefIdProperty DeviceIOTParaIdProperty =
			P<FacilityDetail>.RegisterRefId(e => e.DeviceIOTParaId, ReferenceType.Parent);

		/// <summary>
		/// 设备物联参数Id
		/// </summary>
		public double DeviceIOTParaId
		{
			get { return (double)this.GetRefId(DeviceIOTParaIdProperty); }
			set { this.SetRefId(DeviceIOTParaIdProperty, value); }
		}

		/// <summary>
		/// 设备物联参数
		/// </summary>
		public static readonly RefEntityProperty<DeviceIOTPara> DeviceIOTParaProperty =
			P<FacilityDetail>.RegisterRef(e => e.DeviceIOTPara, DeviceIOTParaIdProperty);

		/// <summary>
		/// 设备物联参数
		/// </summary>
		public DeviceIOTPara DeviceIOTPara
		{
			get { return this.GetRefEntity(DeviceIOTParaProperty); }
			set { this.SetRefEntity(DeviceIOTParaProperty, value); }
		}
        #endregion

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
		[NotDuplicate]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<FacilityDetail>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

		/// <summary>
		/// 设备台账Id
		/// </summary>
		public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<FacilityDetail>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

		#region 设备名称 EquipmentName
		/// <summary>
		/// 设备名称
		/// </summary>
		[Label("设备名称")]
		public static readonly Property<string> EquipmentNameProperty = P<FacilityDetail>.RegisterView(e => e.EquipmentName, p => p.EquipAccount.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipmentName
		{
			get { return GetProperty(EquipmentNameProperty); }
		}
		#endregion

		#region 设备型号 UnitType
		/// <summary>
		/// 设备型号
		/// </summary>
		[Label("设备型号")]
		public static readonly Property<string> UnitTypeProperty = P<FacilityDetail>.Register(e => e.UnitType);

		/// <summary>
		/// 设备型号
		/// </summary>
		public string UnitType
		{
			get { return GetProperty(UnitTypeProperty); }
			set { SetProperty(UnitTypeProperty, value); }
		}
		#endregion

		#region 型号名称 ModelName
		/// <summary>
		/// 型号名称
		/// </summary>
		[Label("型号名称")]
		public static readonly Property<string> ModelNameProperty = P<FacilityDetail>.Register(e => e.ModelName);

		/// <summary>
		/// 型号名称
		/// </summary>
		public string ModelName
		{
			get { return GetProperty(ModelNameProperty); }
			set { SetProperty(ModelNameProperty, value); }
		}
		#endregion

		#region 设备类型 DeviceType
		/// <summary>
		/// 设备类型
		/// </summary>
		[Label("设备类型")]
		public static readonly Property<string> DeviceTypeProperty = P<FacilityDetail>.Register(e => e.DeviceType);

		/// <summary>
		/// 设备类型
		/// </summary>
		public string DeviceType
		{
			get { return GetProperty(DeviceTypeProperty); }
			set { SetProperty(DeviceTypeProperty, value); }
		}
		#endregion

		#region 车间 Workshop
		/// <summary>
		/// 车间
		/// </summary>
		[Label("车间")]
		public static readonly Property<string> WorkshopProperty = P<FacilityDetail>.Register(e => e.Workshop);

		/// <summary>
		/// 车间
		/// </summary>
		public string Workshop
		{
			get { return GetProperty(WorkshopProperty); }
			set { SetProperty(WorkshopProperty, value); }
		}
		#endregion

		#region 产线 ProductLine
		/// <summary>
		/// 产线
		/// </summary>
		[Label("产线")]
		public static readonly Property<string> ProductLineProperty = P<FacilityDetail>.Register(e => e.ProductLine);

		/// <summary>
		/// 产线
		/// </summary>
		public string ProductLine
		{
			get { return GetProperty(ProductLineProperty); }
			set { SetProperty(ProductLineProperty, value); }
		}
		#endregion

		#region 工序 Process
		/// <summary>
		/// 工序
		/// </summary>
		[Label("工序")]
		public static readonly Property<string> ProcessProperty = P<FacilityDetail>.Register(e => e.Process);

		/// <summary>
		/// 工序
		/// </summary>
		public string Process
		{
			get { return GetProperty(ProcessProperty); }
			set { SetProperty(ProcessProperty, value); }
		}
		#endregion

		#region 位置 Local
		/// <summary>
		/// 位置
		/// </summary>
		[Label("位置")]
		public static readonly Property<string> LocalProperty = P<FacilityDetail>.Register(e => e.Local);

		/// <summary>
		/// 位置
		/// </summary>
		public string Local
		{
			get { return GetProperty(LocalProperty); }
			set { SetProperty(LocalProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 设备清单 实体配置
	/// </summary>
	internal class FacilityDetailConfig : EntityConfig<FacilityDetail>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_FACILITY_DETAIL").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}
