using SIE;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.Fixtures.Enums;
using SIE.Fixtures.FixtureTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Models
{
    /// <summary>
    /// 工治具编码
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureEncodeCriteria))]
    [DisplayMember(nameof(Code))]
    [EntityWithConfig(typeof(NoConfig), "工治具编码配置项", "工治具编码编号生成规则")]
    [EntityWithConfig(typeof(SIE.Fixtures.Models.Config.FixtureEncodeConfig))]
    [Label("工治具编码")]
    public partial class FixtureEncode : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<FixtureEncode>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 工治具编码保养项目列表 FixtureEncodeMaintainProjectList
        /// <summary>
        /// 工治具编码保养项目列表
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureEncodeMaintainProject>> FixtureEncodeMaintainProjectListProperty = P<FixtureEncode>.RegisterList(e => e.FixtureEncodeMaintainProjectList);
        /// <summary>
        /// 工治具编码保养项目列表
        /// </summary>
        public EntityList<FixtureEncodeMaintainProject> FixtureEncodeMaintainProjectList
        {
            get { return this.GetLazyList(FixtureEncodeMaintainProjectListProperty); }
        }
        #endregion

        #region 工治具编码（产品清单）列表 FixtureEncodeProductDetailList
        /// <summary>
        /// 工治具编码（产品清单）列表
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureEncodeProductDetail>> FixtureEncodeProductDetailListProperty = P<FixtureEncode>.RegisterList(e => e.FixtureEncodeProductDetailList);
        /// <summary>
        /// 工治具编码（产品清单）列表
        /// </summary>
        public EntityList<FixtureEncodeProductDetail> FixtureEncodeProductDetailList
        {
            get { return this.GetLazyList(FixtureEncodeProductDetailListProperty); }
        }
        #endregion

        #region 工治具编码（存储位置列表 FixtureEncodeStorageLocationList
        /// <summary>
        /// 工治具编码（存储位置列表
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureEncodeStorageLocation>> FixtureEncodeStorageLocationListProperty = P<FixtureEncode>.RegisterList(e => e.FixtureEncodeStorageLocationList);
        /// <summary>
        /// 工治具编码（存储位置列表
        /// </summary>
        public EntityList<FixtureEncodeStorageLocation> FixtureEncodeStorageLocationList
        {
            get { return this.GetLazyList(FixtureEncodeStorageLocationListProperty); }
        }
        #endregion

        #region  FixtureModel
        /// <summary>
        /// Id
        /// </summary>
        public static readonly IRefIdProperty FixtureModelIdProperty = P<FixtureEncode>.RegisterRefId(e => e.FixtureModelId, ReferenceType.Normal);

        /// <summary>
        /// Id
        /// </summary>
        public double FixtureModelId
        {
            get { return (double)GetRefId(FixtureModelIdProperty); }
            set { SetRefId(FixtureModelIdProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RefEntityProperty<FixtureModel> FixtureModelProperty = P<FixtureEncode>.RegisterRef(e => e.FixtureModel, FixtureModelIdProperty);

        /// <summary>
        /// 工治具型号
        /// </summary>
        public FixtureModel FixtureModel
        {
            get { return GetRefEntity(FixtureModelProperty); }
            set { SetRefEntity(FixtureModelProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureEncode>.RegisterView(e => e.ModelCode, p => p.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureEncode>.RegisterView(e => e.ModelName, p => p.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 工治具类型Id FixtureTypeId
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型Id")]
        public static readonly Property<double> FixtureTypeIdProperty = P<FixtureEncode>.RegisterView(e => e.FixtureTypeId, p => p.FixtureModel.FixtureTypeId);

        /// <summary>
        /// 工治具类型Id
        /// </summary>
        public double FixtureTypeId
        {
            get { return this.GetProperty(FixtureTypeIdProperty); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<FixtureEncode>.RegisterView(e => e.FixtureType, p => p.FixtureModel.FixtureType.Name);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
        }
        #endregion

        #region 工治具类型编码 FixtureTypeCode
        /// <summary>
        /// 工治具类型编码
        /// </summary>
        [Label("工治具类型编码")]
        public static readonly Property<string> FixtureTypeCodeProperty = P<FixtureEncode>.RegisterView(e => e.FixtureTypeCode, p => p.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型编码
        /// </summary>
        public string FixtureTypeCode
        {
            get { return this.GetProperty(FixtureTypeCodeProperty); }
        }
        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Label("管理方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureEncode>.RegisterView(e => e.ManageMode, p => p.FixtureModel.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
        }
        #endregion

        #region 槽位类型 SlotType
        /// <summary>
        /// 槽位类型
        /// </summary>
        [Label("槽位类型")]
        public static readonly Property<SlotType> SlotTypeProperty = P<FixtureEncode>.RegisterView(e => e.SlotType, p => p.FixtureModel.SlotType);

        /// <summary>
        /// 管理方式
        /// </summary>
        public SlotType SlotType
        {
            get { return this.GetProperty(SlotTypeProperty); }
        }
        #endregion

        #region 固定储位 FixedStorage
        /// <summary>
        /// 固定储位
        /// </summary>
        [Label("固定储位")]
        public static readonly Property<YesNo> FixedStorageProperty = P<FixtureEncode>.RegisterView(e => e.FixedStorage, p => p.FixtureModel.FixedStorage);

        /// <summary>
        /// 固定储位
        /// </summary>
        public YesNo FixedStorage
        {
            get { return this.GetProperty(FixedStorageProperty); }
        }
        #endregion


        #region 免检 Exemption
        /// <summary>
        /// 免检
        /// </summary>
        [Label("免检")]
        public static readonly Property<bool> ExemptionProperty = P<FixtureEncode>.Register(e => e.Exemption);

        /// <summary>
        /// 免检
        /// </summary>
        public bool Exemption
        {
            get { return this.GetProperty(ExemptionProperty); }
            set { this.SetProperty(ExemptionProperty, value); }
        }
        #endregion


        #region 上料管理 LoadingManage
        /// <summary>
        /// 上料管理
        /// </summary>
        [Label("上料管理")]
        public static readonly Property<YesNo> LoadingManageProperty = P<FixtureEncode>.RegisterView(e => e.LoadingManage, p => p.FixtureModel.LoadingManage);

        /// <summary>
        /// 上料管理
        /// </summary>
        public YesNo LoadingManage
        {
            get { return this.GetProperty(LoadingManageProperty); }
        }
        #endregion

        #region 绑定产品 BindProduct
        /// <summary>
        /// 绑定产品
        /// </summary>
        [Label("绑定产品")]
        public static readonly Property<YesNo> BindProductProperty = P<FixtureEncode>.RegisterView(e => e.BindProduct, p => p.FixtureModel.BindProduct);

        /// <summary>
        /// 绑定产品
        /// </summary>
        public YesNo BindProduct
        {
            get { return this.GetProperty(BindProductProperty); }
        }
        #endregion

        #region 绑定设备 BindEquip
        /// <summary>
        /// 绑定设备
        /// </summary>
        [Label("绑定设备")]
        public static readonly Property<YesNo> BindEquipProperty = P<FixtureEncode>.RegisterView(e => e.BindEquip, p => p.FixtureModel.BindEquip);

        /// <summary>
        /// 绑定设备
        /// </summary>
        public YesNo BindEquip
        {
            get { return this.GetProperty(BindEquipProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<FixtureEncode>.RegisterView(e => e.UnitName, p => p.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 最大使用次数 MaxUseNum
        /// <summary>
        /// 最大使用次数
        /// </summary>
        [Label("最大使用次数")]
        public static readonly Property<int> MaxUseNumProperty = P<FixtureEncode>.RegisterView(e => e.MaxUseNum, p => p.FixtureModel.MaxUseNum);

        /// <summary>
        /// 最大使用次数
        /// </summary>
        public int MaxUseNum
        {
            get { return this.GetProperty(MaxUseNumProperty); }
        }
        #endregion

        #region 最大使用小时数 MaxUseHour
        /// <summary>
        /// 最大使用小时数
        /// </summary>
        [Label("最大使用小时数")]
        public static readonly Property<int> MaxUseHourProperty = P<FixtureEncode>.RegisterView(e => e.MaxUseHour, p => p.FixtureModel.MaxUseHour);

        /// <summary>
        /// 最大使用小时数
        /// </summary>
        public int MaxUseHour
        {
            get { return this.GetProperty(MaxUseHourProperty); }
        }
        #endregion

        #region 保养标准（次数） MaintainNum
        /// <summary>
        /// 保养标准（次数）
        /// </summary>
        [Label("保养标准（次数）")]
        public static readonly Property<int> MaintainNumProperty = P<FixtureEncode>.RegisterView(e => e.MaintainNum, p => p.FixtureModel.MaintainNum);

        /// <summary>
        /// 保养标准（次数）
        /// </summary>
        public int MaintainNum
        {
            get { return this.GetProperty(MaintainNumProperty); }
        }
        #endregion

        #region 保养标准（小时） MaintainHour
        /// <summary>
        /// 保养标准（小时）
        /// </summary>
        [Label("保养标准（小时）")]
        public static readonly Property<decimal> MaintainHourProperty = P<FixtureEncode>.RegisterView(e => e.MaintainHour, p => p.FixtureModel.MaintainHour);

        /// <summary>
        /// 保养标准（小时）
        /// </summary>
        public decimal MaintainHour
        {
            get { return this.GetProperty(MaintainHourProperty); }
        }
        #endregion

        #region 预警值（次数） WarnNum
        /// <summary>
        /// 预警值（次数）
        /// </summary>
        [Label("预警值（次数）")]
        public static readonly Property<int> WarnNumProperty = P<FixtureEncode>.RegisterView(e => e.WarnNum, p => p.FixtureModel.WarnNum);

        /// <summary>
        /// 预警值（次数）
        /// </summary>
        public int WarnNum
        {
            get { return this.GetProperty(WarnNumProperty); }
        }
        #endregion

        #region 预警值（小时） WarnHour
        /// <summary>
        /// 预警值（小时）
        /// </summary>
        [Label("预警值（小时）")]
        public static readonly Property<decimal> WarnHourProperty = P<FixtureEncode>.RegisterView(e => e.WarnHour, p => p.FixtureModel.WarnHour);

        /// <summary>
        /// 预警值（小时）
        /// </summary>
        public decimal WarnHour
        {
            get { return this.GetProperty(WarnHourProperty); }
        }
        #endregion

        #region 上线定期保养标准(小时) OnlineHour
        /// <summary>
        /// 上线定期保养标准(小时)
        /// </summary>
        [Label("上线定期保养标准(小时)")]
        public static readonly Property<decimal> OnlineHourProperty = P<FixtureEncode>.RegisterView(e => e.OnlineHour, p => p.FixtureModel.OnlineHour);

        /// <summary>
        /// 上线定期保养标准(小时)
        /// </summary>
        public decimal OnlineHour
        {
            get { return this.GetProperty(OnlineHourProperty); }
        }
        #endregion

        #region 保养强制执行 MaintainEnforce
        /// <summary>
        /// 保养强制执行
        /// </summary>
        [Label("保养强制执行")]
        public static readonly Property<bool> MaintainEnforceProperty = P<FixtureEncode>.RegisterView(e => e.MaintainEnforce, p => p.FixtureModel.MaintainEnforce);

        /// <summary>
        /// 保养强制执行
        /// </summary>
        public bool MaintainEnforce
        {
            get { return this.GetProperty(MaintainEnforceProperty); }
        }
        #endregion


        #region 单位ID(视图属性) UnitId
        /// <summary>
        /// 单位ID(视图属性)
        /// </summary>
        [Label("单位ID")]
        public static readonly Property<double> UnitIdProperty = P<FixtureEncode>.RegisterView(e => e.UnitId, p => p.FixtureModel.UnitId);

        /// <summary>
        /// 单位ID(视图属性)
        /// </summary>
        public double UnitId
        {
            get { return this.GetProperty(UnitIdProperty); }
        }
        #endregion


        #endregion

        #region 行业属性 IndustryProperties
        /// <summary>
        /// 行业属性
        /// </summary>
        [Label("行业属性")]
        public static readonly Property<IndustryProperties> IndustryPropertiesProperty = P<FixtureEncode>.RegisterView(e => e.IndustryProperties, p => p.FixtureModel.IndustryProperties);

        /// <summary>
        /// 行业属性
        /// </summary>
        public IndustryProperties IndustryProperties
        {
            get { return this.GetProperty(IndustryPropertiesProperty); }
        }
        #endregion


        #region 总数 TotalNum
        /// <summary>
        /// 总数
        /// </summary>
        [Label("总数")]
        public static readonly Property<int> TotalNumProperty = P<FixtureEncode>.Register(e => e.TotalNum);

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalNum
        {
            get { return this.GetProperty(TotalNumProperty); }
            set { this.SetProperty(TotalNumProperty, value); }
        }
        #endregion


        #region 可用数 CanUseNum
        /// <summary>
        /// 可用数
        /// </summary>
        [Label("可用数")]
        public static readonly Property<int> CanUseNumProperty = P<FixtureEncode>.Register(e => e.CanUseNum);

        /// <summary>
        /// 可用数
        /// </summary>
        public int CanUseNum
        {
            get { return this.GetProperty(CanUseNumProperty); }
            set { this.SetProperty(CanUseNumProperty, value); }
        }
        #endregion

        #region 报废数 ScrapNum
        /// <summary>
        /// 报废数
        /// </summary>
        [Label("报废数")]
        public static readonly Property<int> ScrapNumProperty = P<FixtureEncode>.Register(e => e.ScrapNum);

        /// <summary>
        /// 报废数
        /// </summary>
        public int ScrapNum
        {
            get { return this.GetProperty(ScrapNumProperty); }
            set { this.SetProperty(ScrapNumProperty, value); }
        }
        #endregion

        #region 在库数 WarehouseNum
        /// <summary>
        /// 在库数
        /// </summary>
        [Label("在库数")]
        public static readonly Property<int> InWarehouseNumProperty = P<FixtureEncode>.Register(e => e.InWarehouseNum);

        /// <summary>
        /// 在库数
        /// </summary>
        public int InWarehouseNum
        {
            get { return this.GetProperty(InWarehouseNumProperty); }
            set { this.SetProperty(InWarehouseNumProperty, value); }
        }
        #endregion


        #region 新增待入库数 AcceptedInWHNum
        /// <summary>
        /// 待验收入库
        /// </summary>
        [Label("新增待入库数")]
        public static readonly Property<int> AcceptedInWHNumProperty = P<FixtureEncode>.Register(e => e.AcceptedInWHNum);

        /// <summary>
        /// 新增待入库数
        /// </summary>
        public int AcceptedInWHNum
        {
            get { return this.GetProperty(AcceptedInWHNumProperty); }
            set { this.SetProperty(AcceptedInWHNumProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 工治具编码 实体配置
    /// </summary>
    internal class FixtureEncodeConfig : EntityConfig<FixtureEncode>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXTURE_ENCODE")
                .MapAllPropertiesExcept(FixtureEncode.TotalNumProperty,
                FixtureEncode.CanUseNumProperty,
                FixtureEncode.AcceptedInWHNumProperty,
                FixtureEncode.InWarehouseNumProperty,
                FixtureEncode.ScrapNumProperty);
            Meta.EnablePhantoms();
        }
    }
}