using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 采购对象权限
    /// </summary>
    [ChildEntity, Serializable]
    [Label("采购对象")]
    public partial class DevicePurchaseObjectType : DataEntity
    {
        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType> PurchaseObjectTypeProperty = P<DevicePurchaseObjectType>.Register(e => e.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType PurchaseObjectType
        {
            get { return GetProperty(PurchaseObjectTypeProperty); }
            set { SetProperty(PurchaseObjectTypeProperty, value); }
        }
        #endregion

        #region 设备与人员权限维护 DevicePur
        /// <summary>
        /// 设备与人员权限维护Id
        /// </summary>
        public static readonly IRefIdProperty DevicePurIdProperty = P<DevicePurchaseObjectType>.RegisterRefId(e => e.DevicePurId, ReferenceType.Parent);

        /// <summary>
        /// 设备与人员权限维护Id
        /// </summary>
        public double DevicePurId
        {
            get { return (double)GetRefId(DevicePurIdProperty); }
            set { SetRefId(DevicePurIdProperty, value); }
        }

        /// <summary>
        /// 设备与人员权限维护
        /// </summary>
        public static readonly RefEntityProperty<DevicePur> DevicePurProperty = P<DevicePurchaseObjectType>.RegisterRef(e => e.DevicePur, DevicePurIdProperty);

        /// <summary>
        /// 设备与人员权限维护
        /// </summary>
        public DevicePur DevicePur
        {
            get { return GetRefEntity(DevicePurProperty); }
            set { SetRefEntity(DevicePurProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 采购对象权限 实体配置
    /// </summary>
    internal class DevicePurchaseObjectTypeConfig : EntityConfig<DevicePurchaseObjectType>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_DEV_OBJECT_TYPE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}