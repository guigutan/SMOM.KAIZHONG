using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 物料中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("物料中间表")]
    public partial class ItemInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ItemInf>.Register(e => e.Code);

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
        [MaxLength(1000)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ItemInf>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(1000)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<ItemInf>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 图号 DrawingNo
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawingNoProperty = P<ItemInf>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return GetProperty(DrawingNoProperty); }
            set { SetProperty(DrawingNoProperty, value); }
        }
        #endregion

        #region 图号版本 Version
        /// <summary>
        /// 图号版本
        /// </summary>
        [Label("图号版本")]
        public static readonly Property<string> VersionProperty = P<ItemInf>.Register(e => e.Version);

        /// <summary>
        /// 图号版本
        /// </summary>
        public string Version
        {
            get { return GetProperty(VersionProperty); }
            set { SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 基准机型 BaseModel
        /// <summary>
        /// 基准机型
        /// </summary>
        [Label("基准机型")]
        public static readonly Property<string> BaseModelProperty = P<ItemInf>.Register(e => e.BaseModel);

        /// <summary>
        /// 基准机型
        /// </summary>
        public string BaseModel
        {
            get { return GetProperty(BaseModelProperty); }
            set { SetProperty(BaseModelProperty, value); }
        }
        #endregion

        #region 责任人 Person
        /// <summary>
        /// 责任人
        /// </summary>
        [Label("责任人")]
        public static readonly Property<string> PersonProperty = P<ItemInf>.Register(e => e.Person);

        /// <summary>
        /// 责任人
        /// </summary>
        public string Person
        {
            get { return GetProperty(PersonProperty); }
            set { SetProperty(PersonProperty, value); }
        }
        #endregion

        #region MRP控制者 MrpPerson
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpPersonProperty = P<ItemInf>.Register(e => e.MrpPerson);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpPerson
        {
            get { return GetProperty(MrpPersonProperty); }
            set { SetProperty(MrpPersonProperty, value); }
        }
        #endregion

        #region 采购员 PurchasingAgent
        /// <summary>
        /// 采购员
        /// </summary>
        [Label("采购员")]
        public static readonly Property<string> PurchasingAgentProperty = P<ItemInf>.Register(e => e.PurchasingAgent);

        /// <summary>
        /// 采购员
        /// </summary>
        public string PurchasingAgent
        {
            get { return GetProperty(PurchasingAgentProperty); }
            set { SetProperty(PurchasingAgentProperty, value); }
        }
        #endregion

        #region 上偏差 UpperWeight
        /// <summary>
        /// 上偏差
        /// </summary>
        [Label("上偏差")]
        public static readonly Property<decimal> UpperWeightProperty = P<ItemInf>.Register(e => e.UpperWeight);

        /// <summary>
        /// 上偏差
        /// </summary>
        public decimal UpperWeight
        {
            get { return GetProperty(UpperWeightProperty); }
            set { SetProperty(UpperWeightProperty, value); }
        }
        #endregion

        #region 下偏差 LowerWeight
        /// <summary>
        /// 下偏差
        /// </summary>
        [Label("下偏差")]
        public static readonly Property<decimal> LowerWeightProperty = P<ItemInf>.Register(e => e.LowerWeight);

        /// <summary>
        /// 下偏差
        /// </summary>
        public decimal LowerWeight
        {
            get { return GetProperty(LowerWeightProperty); }
            set { SetProperty(LowerWeightProperty, value); }
        }
        #endregion

        #region 最小包装数 MinPackingQty
        /// <summary>
        /// 最小包装数
        /// </summary>
        [Label("最小包装数")]
        public static readonly Property<decimal?> MinPackingQtyProperty = P<ItemInf>.Register(e => e.MinPackingQty);

        /// <summary>
        /// 最小包装数
        /// </summary>
        public decimal? MinPackingQty
        {
            get { return GetProperty(MinPackingQtyProperty); }
            set { SetProperty(MinPackingQtyProperty, value); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [MaxLength(1000)]
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<ItemInf>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return GetProperty(SpecificationModelProperty); }
            set { SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 英文描述 EnglishDescription
        /// <summary>
        /// 英文描述
        /// </summary>
        [Label("英文描述")]
        [MaxLength(1000)]
        public static readonly Property<string> EnglishDescriptionProperty = P<ItemInf>.Register(e => e.EnglishDescription);

        /// <summary>
        /// 英文描述
        /// </summary>
        public string EnglishDescription
        {
            get { return GetProperty(EnglishDescriptionProperty); }
            set { SetProperty(EnglishDescriptionProperty, value); }
        }
        #endregion

        #region 物料简称 ShortDescription
        /// <summary>
        /// 物料简称
        /// </summary>
        [Label("物料简称")]
        public static readonly Property<string> ShortDescriptionProperty = P<ItemInf>.Register(e => e.ShortDescription);

        /// <summary>
        /// 物料简称
        /// </summary>
        public string ShortDescription
        {
            get { return GetProperty(ShortDescriptionProperty); }
            set { SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 长 Length
        /// <summary>
        /// 长
        /// </summary>
        [Label("长")]
        public static readonly Property<decimal?> LengthProperty = P<ItemInf>.Register(e => e.Length);

        /// <summary>
        /// 长
        /// </summary>
        public decimal? Length
        {
            get { return GetProperty(LengthProperty); }
            set { SetProperty(LengthProperty, value); }
        }
        #endregion

        #region 宽 Width
        /// <summary>
        /// 宽
        /// </summary>
        [Label("宽")]
        public static readonly Property<decimal?> WidthProperty = P<ItemInf>.Register(e => e.Width);

        /// <summary>
        /// 宽
        /// </summary>
        public decimal? Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 高 Height
        /// <summary>
        /// 高
        /// </summary>
        [Label("高")]
        public static readonly Property<decimal?> HeightProperty = P<ItemInf>.Register(e => e.Height);

        /// <summary>
        /// 高
        /// </summary>
        public decimal? Height
        {
            get { return GetProperty(HeightProperty); }
            set { SetProperty(HeightProperty, value); }
        }
        #endregion

        #region 体积 Volume
        /// <summary>
        /// 体积
        /// </summary>
        [Label("体积")]
        public static readonly Property<decimal?> VolumeProperty = P<ItemInf>.Register(e => e.Volume);

        /// <summary>
        /// 体积
        /// </summary>
        public decimal? Volume
        {
            get { return GetProperty(VolumeProperty); }
            set { SetProperty(VolumeProperty, value); }
        }
        #endregion

        #region 重量 Weight
        /// <summary>
        /// 重量
        /// </summary>
        [Label("重量")]
        public static readonly Property<decimal?> WeightProperty = P<ItemInf>.Register(e => e.Weight);

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight
        {
            get { return GetProperty(WeightProperty); }
            set { SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 采购提前期 PurchaseLeadTime
        /// <summary>
        /// 采购提前期
        /// </summary>
        [Label("采购提前期")]
        public static readonly Property<int> PurchaseLeadTimeProperty = P<ItemInf>.Register(e => e.PurchaseLeadTime);

        /// <summary>
        /// 采购提前期
        /// </summary>
        public int PurchaseLeadTime
        {
            get { return GetProperty(PurchaseLeadTimeProperty); }
            set { SetProperty(PurchaseLeadTimeProperty, value); }
        }
        #endregion

        #region 单位精度 Precision
        /// <summary>
        /// 单位精度
        /// </summary>
        [Label("单位精度")]
        public static readonly Property<int?> PrecisionProperty = P<ItemInf>.Register(e => e.Precision);

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? Precision
        {
            get { return GetProperty(PrecisionProperty); }
            set { SetProperty(PrecisionProperty, value); }
        }
        #endregion

        #region 商品条码 GoodsBarcode
        /// <summary>
        /// 商品条码
        /// </summary>
        [Label("商品条码")]
        public static readonly Property<string> GoodsBarcodeProperty = P<ItemInf>.Register(e => e.GoodsBarcode);

        /// <summary>
        /// 商品条码
        /// </summary>
        public string GoodsBarcode
        {
            get { return GetProperty(GoodsBarcodeProperty); }
            set { SetProperty(GoodsBarcodeProperty, value); }
        }
        #endregion

        #region 物料分类编码 ItemCategoryCode
        /// <summary>
        /// 物料分类编码
        /// </summary>
        [Label("物料分类编码")]
        public static readonly Property<string> ItemCategoryCodeProperty = P<ItemInf>.Register(e => e.ItemCategoryCode);

        /// <summary>
        /// 物料分类编码
        /// </summary>
        public string ItemCategoryCode
        {
            get { return GetProperty(ItemCategoryCodeProperty); }
            set { SetProperty(ItemCategoryCodeProperty, value); }
        }
        #endregion

        #region 物料消耗类型 ConsumeMode
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        [Label("物料消耗类型")]
        public static readonly Property<string> ConsumeModeProperty = P<ItemInf>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public string ConsumeMode
        {
            get { return GetProperty(ConsumeModeProperty); }
            set { SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 来源类型 ItemSourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<string> ItemSourceTypeProperty = P<ItemInf>.Register(e => e.ItemSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public string ItemSourceType
        {
            get { return GetProperty(ItemSourceTypeProperty); }
            set { SetProperty(ItemSourceTypeProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<ItemInf>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { this.SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> ItemTypeProperty = P<ItemInf>.Register(e => e.ItemType);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
            set { this.SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 物料类型 ItemTypeEbs
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<int> ItemTypeEbsProperty = P<ItemInf>.Register(e => e.ItemTypeEbs);

        /// <summary>
        /// 物料类型
        /// </summary>
        public int ItemTypeEbs
        {
            get { return this.GetProperty(ItemTypeEbsProperty); }
            set { this.SetProperty(ItemTypeEbsProperty, value); }
        }
        #endregion
            

        //#region 是否批次管控 IsLotControl
        ///// <summary>
        ///// 是否批次管控
        ///// </summary>
        //[Label("是否批次管控")]
        //public static readonly Property<string> IsLotControlProperty = P<ItemInf>.Register(e => e.IsLotControl);

        ///// <summary>
        ///// 是否批次管控
        ///// </summary>
        //public string IsLotControl
        //{
        //    get { return this.GetProperty(IsLotControlProperty); }
        //    set { this.SetProperty(IsLotControlProperty, value); }
        //}
        //#endregion

        #region 保质期(天) ShelfLife
        /// <summary>
        /// 保质期(天)
        /// </summary>
        [Label("保质期(天)")]
        [MinValue(0)]
        public static readonly Property<decimal?> ShelfLifeProperty = P<ItemInf>.Register(e => e.ShelfLife);

        /// <summary>
        /// 保质期(天)
        /// </summary>
        public decimal? ShelfLife
        {
            get { return GetProperty(ShelfLifeProperty); }
            set { SetProperty(ShelfLifeProperty, value); }
        }
        #endregion

        #region 批次管理 IsBatch
        /// <summary>
        /// 批次管理
        /// </summary>
        [Label("批次管理")]
        public static readonly Property<bool?> IsBatchProperty = P<ItemInf>.Register(e => e.IsBatch);

        /// <summary>
        /// 批次管理
        /// </summary>
        public bool? IsBatch
        {
            get { return GetProperty(IsBatchProperty); }
            set { SetProperty(IsBatchProperty, value); }
        }
        #endregion

        #region 序列号管理 IsSerialNumber
        /// <summary>
        /// 序列号管理
        /// </summary>
        [Label("序列号管理")]
        public static readonly Property<bool?> IsSerialNumberProperty = P<ItemInf>.Register(e => e.IsSerialNumber);

        /// <summary>
        /// 序列号管理
        /// </summary>
        public bool? IsSerialNumber
        {
            get { return GetProperty(IsSerialNumberProperty); }
            set { SetProperty(IsSerialNumberProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料中间表 实体配置
    /// </summary>
    internal class ItemInfConfig : EntityConfig<ItemInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_ITEM").MapAllProperties();
            Meta.Property(ItemInf.NameProperty).ColumnMeta.HasLength(2000);
            Meta.Property(ItemInf.DescriptionProperty).ColumnMeta.HasLength(2000);
            Meta.Property(ItemInf.EnglishDescriptionProperty).ColumnMeta.HasLength(2000);
            Meta.Property(ItemInf.SpecificationModelProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}