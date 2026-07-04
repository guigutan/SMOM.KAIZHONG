using SIE.Common.Configs;
using SIE.Domain;
using SIE.Fixtures.Enums;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.Models.Config;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Models
{
    /// <summary>
    /// 工治具型号
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureModelCriteria))]
    [EntityWithConfig(typeof(FixturesModelsConfig))]
    [Label("工治具型号")]
    [DisplayMember(nameof(Code))]
    public partial class FixtureModel : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FixtureModel()
        {
            this.IsFeeder = false;
        }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<FixtureModel>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<FixtureModel>.Register(e => e.Name);

        /// <summary>
        ///名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 最大使用次数 MaxUseNum
        /// <summary>
        /// 最大使用次数
        /// </summary>
        [Label("最大使用次数")]
        public static readonly Property<int> MaxUseNumProperty = P<FixtureModel>.Register(e => e.MaxUseNum);

        /// <summary>
        /// 最大使用次数
        /// </summary>
        public int MaxUseNum
        {
            get { return GetProperty(MaxUseNumProperty); }
            set { SetProperty(MaxUseNumProperty, value); }
        }
        #endregion

        #region 最大使用小时数 MaxUseHour
        /// <summary>
        /// 最大使用小时数
        /// </summary>
        [Label("最大使用小时数")]
        public static readonly Property<decimal> MaxUseHourProperty = P<FixtureModel>.Register(e => e.MaxUseHour);

        /// <summary>
        /// 最大使用小时数
        /// </summary>
        public decimal MaxUseHour
        {
            get { return GetProperty(MaxUseHourProperty); }
            set { SetProperty(MaxUseHourProperty, value); }
        }
        #endregion

        #region 保养标准(次数) MaintainNum
        /// <summary>
        /// 保养标准(次数)
        /// </summary>
        [Label("保养标准(次数)")]
        public static readonly Property<int> MaintainNumProperty = P<FixtureModel>.Register(e => e.MaintainNum);

        /// <summary>
        /// 保养标准(次数)
        /// </summary>
        public int MaintainNum
        {
            get { return GetProperty(MaintainNumProperty); }
            set { SetProperty(MaintainNumProperty, value); }
        }
        #endregion

        #region 预警值(次数) WarnNum
        /// <summary>
        /// 预警值(次数)
        /// </summary>
        [Label("预警值(次数)")]
        public static readonly Property<int> WarnNumProperty = P<FixtureModel>.Register(e => e.WarnNum);

        /// <summary>
        /// 预警值(次数)
        /// </summary>
        public int WarnNum
        {
            get { return GetProperty(WarnNumProperty); }
            set { SetProperty(WarnNumProperty, value); }
        }
        #endregion

        #region 保养标准(小时) MaintainHour
        /// <summary>
        /// 保养标准(小时)
        /// </summary>
        [Label("保养标准(小时)")]
        public static readonly Property<decimal> MaintainHourProperty = P<FixtureModel>.Register(e => e.MaintainHour);

        /// <summary>
        /// 保养标准(小时)
        /// </summary>
        public decimal MaintainHour
        {
            get { return GetProperty(MaintainHourProperty); }
            set { SetProperty(MaintainHourProperty, value); }
        }
        #endregion

        #region 预警值(小时) WarnHour
        /// <summary>
        /// 预警值(小时)
        /// </summary>
        [Label("预警值(小时)")]
        public static readonly Property<decimal> WarnHourProperty = P<FixtureModel>.Register(e => e.WarnHour);

        /// <summary>
        /// 预警值(小时)
        /// </summary>
        public decimal WarnHour
        {
            get { return GetProperty(WarnHourProperty); }
            set { SetProperty(WarnHourProperty, value); }
        }
        #endregion

        #region 上线定期保养标准(小时) OnlineHour
        /// <summary>
        /// 上线定期保养标准(小时)
        /// </summary>
        [Label("上线定期保养标准(小时)")]
        public static readonly Property<decimal> OnlineHourProperty = P<FixtureModel>.Register(e => e.OnlineHour);

        /// <summary>
        /// 上线定期保养标准(小时)
        /// </summary>
        public decimal OnlineHour
        {
            get { return GetProperty(OnlineHourProperty); }
            set { SetProperty(OnlineHourProperty, value); }
        }
        #endregion

        #region 保养强制执行 MaintainEnforce
        /// <summary>
        /// 保养强制执行
        /// </summary>
        [Label("保养强制执行")]
        public static readonly Property<bool> MaintainEnforceProperty = P<FixtureModel>.Register(e => e.MaintainEnforce);

        /// <summary>
        /// 保养强制执行
        /// </summary>
        public bool MaintainEnforce
        {
            get { return GetProperty(MaintainEnforceProperty); }
            set { SetProperty(MaintainEnforceProperty, value); }
        }
        #endregion

        #region 上料管理 LoadingManage
        /// <summary>
        /// 上料管理
        /// </summary>
        [Label("上料管理")]
        public static readonly Property<YesNo> LoadingManageProperty = P<FixtureModel>.Register(e => e.LoadingManage);

        /// <summary>
        /// 上料管理
        /// </summary>
        public YesNo LoadingManage
        {
            get { return GetProperty(LoadingManageProperty); }
            set { SetProperty(LoadingManageProperty, value); }
        }
        #endregion

        #region 固定储位 FixedStorage
        /// <summary>
        /// 固定储位
        /// </summary>
        [Label("固定储位")]
        public static readonly Property<YesNo> FixedStorageProperty = P<FixtureModel>.Register(e => e.FixedStorage);

        /// <summary>
        /// 固定储位
        /// </summary>
        public YesNo FixedStorage
        {
            get { return GetProperty(FixedStorageProperty); }
            set { SetProperty(FixedStorageProperty, value); }
        }
        #endregion

        #region 绑定产品 BindProduct
        /// <summary>
        /// 绑定产品
        /// </summary>
        [Label("绑定产品")]
        public static readonly Property<YesNo> BindProductProperty = P<FixtureModel>.Register(e => e.BindProduct);

        /// <summary>
        /// 绑定产品
        /// </summary>
        public YesNo BindProduct
        {
            get { return GetProperty(BindProductProperty); }
            set { SetProperty(BindProductProperty, value); }
        }
        #endregion

        #region 绑定设备 BindEquip
        /// <summary>
        /// 绑定设备
        /// </summary>
        [Label("绑定设备")]
        public static readonly Property<YesNo> BindEquipProperty = P<FixtureModel>.Register(e => e.BindEquip);

        /// <summary>
        /// 绑定设备
        /// </summary>
        public YesNo BindEquip
        {
            get { return GetProperty(BindEquipProperty); }
            set { SetProperty(BindEquipProperty, value); }
        }
        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Required]
        [Label("管理方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureModel>.Register(e => e.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return GetProperty(ManageModeProperty); }
            set { SetProperty(ManageModeProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        public static readonly IRefIdProperty UnitIdProperty = P<FixtureModel>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId
        {
            get { return (double)GetRefId(UnitIdProperty); }
            set { SetRefId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<FixtureModel>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return GetRefEntity(UnitProperty); }
            set { SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureModel>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具类型Id
        /// </summary>
        public double? FixtureTypeId
        {
            get { return (double?)this.GetRefNullableId(FixtureTypeIdProperty); }
            set { this.SetRefNullableId(FixtureTypeIdProperty, value); }
        }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public static readonly RefEntityProperty<SIE.Fixtures.FixtureTypes.FixtureType> FixtureTypeProperty =
            P<FixtureModel>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public SIE.Fixtures.FixtureTypes.FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        #region 槽位类型 SlotType
        /// <summary>
        /// 槽位类型
        /// </summary>
        [Label("槽位类型")]
        public static readonly Property<SlotType?> SlotTypeProperty = P<FixtureModel>.Register(e => e.SlotType);

        /// <summary>
        /// 槽位类型
        /// </summary>
        public SlotType? SlotType
        {
            get { return GetProperty(SlotTypeProperty); }
            set { SetProperty(SlotTypeProperty, value); }
        }
        #endregion


        #region 行业属性 IndustryProperties
        /// <summary>
        /// 行业属性
        /// </summary>
        [Label("行业属性")]
        [Required]
        public static readonly Property<IndustryProperties> IndustryPropertiesProperty = P<FixtureModel>.Register(e => e.IndustryProperties);

        /// <summary>
        /// 行业属性
        /// </summary>
        public IndustryProperties IndustryProperties
        {
            get { return this.GetProperty(IndustryPropertiesProperty); }
            set { this.SetProperty(IndustryPropertiesProperty, value); }
        }
        #endregion


        #region 是否飞达 IsFeeder
        /// <summary>
        /// 是否飞达
        /// </summary>
        [Label("Feeder")]
        public static readonly Property<bool> IsFeederProperty = P<FixtureModel>.Register(e => e.IsFeeder);

        /// <summary>
        /// 是否飞达
        /// </summary>
        public bool IsFeeder
        {
            get { return this.GetProperty(IsFeederProperty); }
            set { this.SetProperty(IsFeederProperty, value); }
        }
        #endregion

        #region 刮刀 IsScraper
        /// <summary>
        /// 是否刮刀
        /// </summary>
        [Label("刮刀")]
        public static readonly Property<bool> IsScraperProperty = P<FixtureModel>.Register(e => e.IsScraper);

        /// <summary>
        /// 是否刮刀
        /// </summary>
        public bool IsScraper
        {
            get { return this.GetProperty(IsScraperProperty); }
            set { this.SetProperty(IsScraperProperty, value); }
        }
        #endregion

        #region 钢网 IsSteelNet
        /// <summary>
        /// 钢网
        /// </summary>
        [Label("钢网")]
        public static readonly Property<bool> IsSteelNetProperty = P<FixtureModel>.Register(e => e.IsSteelNet);

        /// <summary>
        /// 钢网
        /// </summary>
        public bool IsSteelNet
        {
            get { return this.GetProperty(IsSteelNetProperty); }
            set { this.SetProperty(IsSteelNetProperty, value); }
        }
        #endregion



        #region 设备清单列表 EquipmentList
        /// <summary>
        /// 设备清单列表
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureModelEquipDetail>> EquipmentListProperty = P<FixtureModel>.RegisterList(e => e.EquipmentList);
        /// <summary>
        /// 设备清单列表
        /// </summary>
        public EntityList<FixtureModelEquipDetail> EquipmentList
        {
            get { return this.GetLazyList(EquipmentListProperty); }
        }
        #endregion

        #region 保养项目列表 MaintainProjectList
        /// <summary>
        /// 保养项目列表
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureModelMaintainProject>> MaintainProjectListProperty = P<FixtureModel>.RegisterList(e => e.MaintainProjectList);
        /// <summary>
        /// 保养项目列表
        /// </summary>
        public EntityList<FixtureModelMaintainProject> MaintainProjectList
        {
            get { return this.GetLazyList(MaintainProjectListProperty); }
        }
        #endregion

        #region 视图属性
        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<FixtureModel>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 工治具类型 FixtureTypeName
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeNameProperty = P<FixtureModel>.RegisterView(e => e.FixtureTypeName, p => p.FixtureType.Name);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypeName
        {
            get { return this.GetProperty(FixtureTypeNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 工治具型号 实体配置
    /// </summary>
    internal class FixtureModelConfig : EntityConfig<FixtureModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXTURE_MODEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}