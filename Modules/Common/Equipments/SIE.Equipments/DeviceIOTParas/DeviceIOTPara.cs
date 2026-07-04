using SIE.Common.Configs;
using SIE.Domain;
using SIE.Equipments.DeviceIOTParas.ConfigValues;
using SIE.Equipments.DeviceIOTParas.Criterias;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceIOTParas
{   
	/// <summary>
    /// 设备物联参数
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DeviceIOTParaCirteria))]	
	[EntityWithConfig(typeof(EquipMDCConfig))]	
	[Label("设备物联参数")]
    public class DeviceIOTPara : DataEntity
	{
		#region 物联模型编码 Code
		/// <summary>
		/// 物联模型编码
		/// </summary>
		[NotDuplicate]
		[Required]
		[Label("物联模型编码")]
		public static readonly Property<string> CodeProperty = P<DeviceIOTPara>.Register(e => e.Code);

		/// <summary>
		/// 物联模型编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 物联模型名称 Name
		/// <summary>
		/// 物联模型名称
		/// </summary>
		[Label("物联模型名称")]
		[Required]
		public static readonly Property<string> NameProperty = P<DeviceIOTPara>.Register(e => e.Name);

		/// <summary>
		/// 物联模型名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<DeviceIOTPara>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<DeviceIOTPara>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
		public static readonly Property<string> ModelNameProperty = P<DeviceIOTPara>.Register(e => e.ModelName);

		/// <summary>
		/// 型号名称
		/// </summary>
		public string ModelName
		{
			get { return GetProperty(ModelNameProperty); }
			set { SetProperty(ModelNameProperty, value); }
		}
        #endregion

        #region 设备类别 DeviceType
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
		public static readonly Property<string> DeviceTypeProperty = P<DeviceIOTPara>.Register(e => e.DeviceType);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string DeviceType
		{
			get { return GetProperty(DeviceTypeProperty); }
			set { SetProperty(DeviceTypeProperty, value); }
		}
		#endregion

		#region 物料参数 PhysicalUnion
		/// <summary>
		/// 物料参数
		/// </summary>
		public static readonly ListProperty<EntityList<PhysicalUnion>> PhysicalUnionProperty = P<DeviceIOTPara>.RegisterList(e => e.PhysicalUnion);
		/// <summary>
		/// 物料参数
		/// </summary>
		public EntityList<PhysicalUnion> PhysicalUnion
		{
			get { return this.GetLazyList(PhysicalUnionProperty); }
		}
		#endregion

		#region 设备清单 FacilityDetail
		/// <summary>
		/// 设备清单
		/// </summary>
		public static readonly ListProperty<EntityList<FacilityDetail>> FacilityDetailProperty = P<DeviceIOTPara>.RegisterList(e => e.FacilityDetail);
		/// <summary>
		/// 设备清单
		/// </summary>
		public EntityList<FacilityDetail> FacilityDetail
		{
			get { return this.GetLazyList(FacilityDetailProperty); }
		}
		#endregion

	}
	/// <summary>
	/// 设备物联参数 实体配置
	/// </summary>
	internal class DeviceIOTParametersConfig : EntityConfig<DeviceIOTPara>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_DEVICE_IOT_PARA").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}
