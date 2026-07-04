using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipModels
{
    /// <summary>
    /// 设备型号维护
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Code))]
    [Label("设备型号维护")]
    [ConditionQueryType(typeof(EquipModelCriteria))]
    public class EquipModel : SIE.Core.Equipments.EquipModel
    {
        #region 电子行业基础数据
        #region 轨道类型 RailType
        /// <summary>
        /// 轨道类型
        /// </summary>
        [Label("轨道类型")]
        public static readonly Property<OrbitType?> RailTypeProperty = P<EquipModel>.Register(e => e.RailType);

        /// <summary>
        /// 轨道类型
        /// </summary>
        public OrbitType? RailType
        {
            get { return GetProperty(RailTypeProperty); }
            set { SetProperty(RailTypeProperty, value); }
        }
        #endregion

        #region 是否Feeder绑定 FeederBinding
        /// <summary>
        /// 是否Feeder绑定
        /// </summary>
        [Label("是否Feeder绑定")]
        public static readonly Property<YesNo?> FeederBindingProperty = P<EquipModel>.Register(e => e.FeederBinding);

        /// <summary>
        /// 是否维护Feeder绑定
        /// </summary>
        public YesNo? FeederBinding
        {
            get { return GetProperty(FeederBindingProperty); }
            set { SetProperty(FeederBindingProperty, value); }
        }
        #endregion

        #region 启用站位防错 FeederLocFailSafe
        /// <summary>
        /// 启用站位防错
        /// </summary>
        [Label("启用站位防错")]
        public static readonly Property<State?> FeederLocFailSafeProperty = P<EquipModel>.Register(e => e.FeederLocFailSafe);

        /// <summary>
        /// 启用站位防错
        /// </summary>
        public State? FeederLocFailSafe
        {
            get { return GetProperty(FeederLocFailSafeProperty); }
            set { SetProperty(FeederLocFailSafeProperty, value); }
        }
        #endregion

        #region 启用Feeder防错 FeederBarcodeFailSafe
        /// <summary>
        /// 启用Feeder防错
        /// </summary>
        [Label("启用Feeder防错")]
        public static readonly Property<State?> FeederBarcodeFailSafeProperty = P<EquipModel>.Register(e => e.FeederBarcodeFailSafe);

        /// <summary>
        /// 启用Feeder防错
        /// </summary>
        public State? FeederBarcodeFailSafe
        {
            get { return GetProperty(FeederBarcodeFailSafeProperty); }
            set { SetProperty(FeederBarcodeFailSafeProperty, value); }
        }
        #endregion

        #region 虚拟设备 VirtualDevice
        /// <summary>
        /// 虚拟设备
        /// </summary>
        [Label("虚拟设备")]
        public static readonly Property<YesNo?> VirtualDeviceProperty = P<EquipModel>.Register(e => e.VirtualDevice);

        /// <summary>
        /// 虚拟设备
        /// </summary>
        public YesNo? VirtualDevice
        {
            get { return GetProperty(VirtualDeviceProperty); }
            set { SetProperty(VirtualDeviceProperty, value); }
        }
        #endregion

        #region 禁用 IsDisabled
        /// <summary>
        /// 禁用
        /// </summary>
        [Label("禁用")]
        public static readonly Property<YesNo?> IsDisabledProperty = P<EquipModel>.Register(e => e.IsDisabled);

        /// <summary>
        /// 禁用
        /// </summary>
        public YesNo? IsDisabled
        {
            get { return GetProperty(IsDisabledProperty); }
            set { SetProperty(IsDisabledProperty, value); }
        }
        #endregion
                
        #region 老化方式 AgingType
        /// <summary>
        /// 老化方式
        /// </summary>
        [Label("老化方式")]
        public static readonly Property<AgingMode?> AgingTypeProperty = P<EquipModel>.Register(e => e.AgingType);

        /// <summary>
        /// 老化方式
        /// </summary>
        public AgingMode? AgingType
        {
            get { return this.GetProperty(AgingTypeProperty); }
            set { this.SetProperty(AgingTypeProperty, value); }
        }
        #endregion

        #region 产品生产模式 ProductionType
        /// <summary>
        /// 产品生产模式
        /// </summary>
        [Label("产品生产模式")]
        public static readonly Property<ProductionMode?> ProductionTypeProperty = P<EquipModel>.Register(e => e.ProductionType);

        /// <summary>
        /// 产品生产模式
        /// </summary>
        public ProductionMode? ProductionType
        {
            get { return this.GetProperty(ProductionTypeProperty); }
            set { this.SetProperty(ProductionTypeProperty, value); }
        }
        #endregion
        #endregion

        #region 位置列表 LocationList
        /// <summary>
        /// 位置列表
        /// </summary>
        [Label("位置列表")]
        public static readonly ListProperty<EntityList<EquipModelLocation>> LocationListProperty = P<EquipModel>.RegisterList(e => e.LocationList);

        /// <summary>
        /// 位置列表
        /// </summary>
        public EntityList<EquipModelLocation> LocationList
        {
            get { return this.GetLazyList(LocationListProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 设备型号维护 实体配置
    /// </summary>
    internal class EquipModelConfig : EntityConfig<EquipModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_MODEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}