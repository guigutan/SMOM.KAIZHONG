using SIE.Domain;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 设备类型
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备类型")]
    public partial class DeviceType : DataEntity
    {
        #region 设备与人员 DevicePur
        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public static readonly IRefIdProperty DevicePurIdProperty = P<DeviceType>.RegisterRefId(e => e.DevicePurId, ReferenceType.Parent);

        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public double DevicePurId
        {
            get { return (double)GetRefId(DevicePurIdProperty); }
            set { SetRefId(DevicePurIdProperty, value); }
        }

        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public static readonly RefEntityProperty<DevicePur> DevicePurProperty = P<DeviceType>.RegisterRef(e => e.DevicePur, DevicePurIdProperty);

        /// <summary>
        /// 设备与人员Id
        /// </summary>
        public DevicePur DevicePur
        {
            get { return GetRefEntity(DevicePurProperty); }
            set { SetRefEntity(DevicePurProperty, value); }
        }
        #endregion

        #region 设备类别 TypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]        
        public static readonly Property<string> TypeCategoryProperty
            = P<DeviceType>.Register(e => e.TypeCategory);
        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory
        {
            get { return GetProperty(TypeCategoryProperty); }
            set { SetProperty(TypeCategoryProperty, value); }
        }

        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty =
            P<DeviceType>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double?)this.GetRefNullableId(EquipTypeIdProperty); }
            set { this.SetRefNullableId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty =
            P<DeviceType>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return this.GetRefEntity(EquipTypeProperty); }
            set { this.SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 类型编码 EquipTypeCode
        /// <summary>
        /// 类型编码
        /// </summary>
        [Label("类型编码")]
        public static readonly Property<string> EquipTypeCodeProperty = P<DeviceType>.RegisterView(e => e.EquipTypeCode, p => p.EquipType.TypeCode);

        /// <summary>
        /// 类型编码
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion

        #region 类型名称 EquipTypeName
        /// <summary>
        /// 类型名称
        /// </summary>
        [Label("类型名称")]
        public static readonly Property<string> EquipTypeNameProperty = P<DeviceType>.RegisterView(e => e.EquipTypeName, p => p.EquipType.TypeName);

        /// <summary>
        /// 类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 设备类型 实体配置
    /// </summary>
    internal class DeviceTypeConfig : EntityConfig<DeviceType>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_DEVICE_TYPE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
