using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Inventory.Piles.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Boxs;
using System;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 垛表
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig))]
    [EntityWithConfig(typeof(LpnRuleConfig))]
    [ConditionQueryType(typeof(PileCriteria))]
    [Label("垛表")]
    [DisplayMember(nameof(Code))]
    public partial class Pile : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Pile>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 型号 Model
        /// <summary>
        /// 型号
        /// </summary>        
        [MaxLength(240)]
        [Label("型号")]
        public static readonly Property<string> ModelProperty = P<Pile>.Register(e => e.Model);

        /// <summary>
        /// 型号
        /// </summary>
        public string Model
        {
            get { return GetProperty(ModelProperty); }
            set { SetProperty(ModelProperty, value); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>        
        [MaxLength(240)]
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<Pile>.Register(e => e.ModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return GetProperty(ModelNameProperty); }
            set { SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 周转容器 TurnoverContainer
        /// <summary>
        /// 周转容器
        /// </summary>
        [Label("周转容器")]
        public static readonly Property<bool> TurnoverContainerProperty = P<Pile>.Register(e => e.TurnoverContainer);

        /// <summary>
        /// 周转容器
        /// </summary>
        public bool TurnoverContainer
        {
            get { return GetProperty(TurnoverContainerProperty); }
            set { SetProperty(TurnoverContainerProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<Pile>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 业务类型 BusinessType
        /// <summary>
        /// 业务类型
        /// </summary>
        [Label("业务类型")]
        public static readonly Property<BusinessType?> BusinessTypeProperty = P<Pile>.Register(e => e.BusinessType);

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType? BusinessType
        {
            get { return GetProperty(BusinessTypeProperty); }
            set { SetProperty(BusinessTypeProperty, value); }
        }
        #endregion

        #region 当前位置 CurLocation
        /// <summary>
        /// 当前位置
        /// </summary>
        [MaxLength(80)]
        [Label("当前位置")]
        public static readonly Property<string> CurLocationProperty = P<Pile>.Register(e => e.CurLocation);

        /// <summary>
        /// 当前位置
        /// </summary>
        public string CurLocation
        {
            get { return GetProperty(CurLocationProperty); }
            set { SetProperty(CurLocationProperty, value); }
        }
        #endregion

        #region 重量(KG) Weight
        /// <summary>
        /// 重量(KG)
        /// </summary>
        [Label("重量(KG)")]
        public static readonly Property<decimal?> WeightProperty = P<Pile>.Register(e => e.Weight);

        /// <summary>
        /// 重量(KG)
        /// </summary>
        public decimal? Weight
        {
            get { return GetProperty(WeightProperty); }
            set { SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 长(CM) Length
        /// <summary>
        /// 长(CM)
        /// </summary>
        [Label("长(CM)")]
        public static readonly Property<decimal?> LengthProperty = P<Pile>.Register(e => e.Length);

        /// <summary>
        /// 长(CM)
        /// </summary>
        public decimal? Length
        {
            get { return GetProperty(LengthProperty); }
            set { SetProperty(LengthProperty, value); }
        }
        #endregion

        #region 宽(CM) Width
        /// <summary>
        /// 宽(CM)
        /// </summary>
        [Label("宽(CM)")]
        public static readonly Property<decimal?> WidthProperty = P<Pile>.Register(e => e.Width);

        /// <summary>
        /// 宽(CM)
        /// </summary>
        public decimal? Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 高(CM) Height
        /// <summary>
        /// 高(CM)
        /// </summary>
        [Label("高(CM)")]
        public static readonly Property<decimal?> HeightProperty = P<Pile>.Register(e => e.Height);

        /// <summary>
        /// 高(CM)
        /// </summary>
        public decimal? Height
        {
            get { return GetProperty(HeightProperty); }
            set { SetProperty(HeightProperty, value); }
        }
        #endregion

        #region 垛型号长(CM) ModelLength
        /// <summary>
        /// 垛型号长(CM)
        /// </summary>
        [Label("垛型号长(CM)")]
        public static readonly Property<decimal?> ModelLengthProperty = P<Pile>.Register(e => e.ModelLength);

        /// <summary>
        /// 垛型号长(CM)
        /// </summary>
        public decimal? ModelLength
        {
            get { return this.GetProperty(ModelLengthProperty); }
            set { this.SetProperty(ModelLengthProperty, value); }
        }
        #endregion

        #region 垛型号宽(CM) ModelWidth
        /// <summary>
        /// 垛型号宽(CM)
        /// </summary>
        [Label("垛型号宽(CM)")]
        public static readonly Property<decimal?> ModelWidthProperty = P<Pile>.Register(e => e.ModelWidth);

        /// <summary>
        /// 垛型号宽(CM)
        /// </summary>
        public decimal? ModelWidth
        {
            get { return this.GetProperty(ModelWidthProperty); }
            set { this.SetProperty(ModelWidthProperty, value); }
        }
        #endregion

        #region 垛型号高(CM) ModelHeight
        /// <summary>
        /// 垛型号高(CM)
        /// </summary>
        [Label("垛型号高(CM)")]
        public static readonly Property<decimal?> ModelHeightProperty = P<Pile>.Register(e => e.ModelHeight);

        /// <summary>
        /// 垛型号高(CM)
        /// </summary>
        public decimal? ModelHeight
        {
            get { return this.GetProperty(ModelHeightProperty); }
            set { this.SetProperty(ModelHeightProperty, value); }
        }
        #endregion

        #region 垛明细列表 PileDetailList
        /// <summary>
        /// 垛明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<PileDetail>> PileDetailListProperty = P<Pile>.RegisterList(e => e.PileDetailList);
        /// <summary>
        /// 垛明细列表
        /// </summary>
        public EntityList<PileDetail> PileDetailList
        {
            get { return this.GetLazyList(PileDetailListProperty); }
        }
        #endregion

        #region 物料状态 ItemState
        /// <summary>
        /// 物料状态
        /// </summary>
        [Label("物料状态")]
        public static readonly Property<ItemState?> ItemStateProperty = P<Pile>.Register(e => e.ItemState);

        /// <summary>
        /// 物料状态
        /// </summary>
        public ItemState? ItemState
        {
            get { return this.GetProperty(ItemStateProperty); }
            set { this.SetProperty(ItemStateProperty, value); }
        }
        #endregion

        #region 垛状态 PileState
        /// <summary>
        /// 垛状态
        /// </summary>
        [Label("垛状态")]
        public static readonly Property<BoxState> PileStateProperty = P<Pile>.Register(e => e.PileState);

        /// <summary>
        /// 垛状态
        /// </summary>
        public BoxState PileState
        {
            get { return this.GetProperty(PileStateProperty); }
            set { this.SetProperty(PileStateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 垛表 实体配置
    /// </summary>
    internal class PileConfig : EntityConfig<Pile>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PILE").MapAllProperties();
            Meta.Property(Pile.CodeProperty).ColumnMeta.HasLength(160);
            Meta.Property(Pile.ModelProperty).ColumnMeta.HasLength(480);
            Meta.Property(Pile.CurLocationProperty).ColumnMeta.HasLength(160);
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }
}