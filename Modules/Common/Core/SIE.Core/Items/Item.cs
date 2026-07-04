using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Items
{
    /// <summary>
    /// 物料
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("物料")]
    [DisplayMember(nameof(Code))]
    public partial class Item : DataEntity, IStateEntity
    {
        /// <summary>
        /// 快码类型
        /// </summary>
        public const string CatalogType = "ITEM_STATUS";

        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public Item()
        {
            State = State.Enable;
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Item>.Register(e => e.Code);

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
        [MaxLength(240)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Item>.Register(e => e.Name);

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
        [Label("描述")]
        [MaxLength(4000)]
        public static readonly Property<string> DescriptionProperty = P<Item>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Item>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 物料简称 ShortDescription
        /// <summary>
        /// 物料简称
        /// </summary>
        [Label("旧料号)")]
        public static readonly Property<string> ShortDescriptionProperty = P<Item>.Register(e => e.ShortDescription);

        /// <summary>
        /// 物料简称
        /// </summary>
        public string ShortDescription
        {
            get { return GetProperty(ShortDescriptionProperty); }
            set { SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion


        #region 标签模板设置 Template
        /// <summary>
        /// 标签模板设置Id
        /// </summary>
        [Label("标签模板设置")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<Item>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 标签模板设置Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double?)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 标签模板设置
        /// </summary>
        public static readonly RefEntityProperty<LabelPrintTemplate> LabelTemplateProperty =
            P<Item>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 标签模板设置
        /// </summary>
        public LabelPrintTemplate Template
        {
            get { return this.GetRefEntity(LabelTemplateProperty); }
            set { this.SetRefEntity(LabelTemplateProperty, value); }
        }
        #endregion

        #region 名称 Zmc
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> ZmcProperty = P<Item>.Register(e => e.Zmc);

        /// <summary>
        /// 名称
        /// </summary>
        public string Zmc
        {
            get { return this.GetProperty(ZmcProperty); }
            set { this.SetProperty(ZmcProperty, value); }
        }
        #endregion

        #region 颜色 Normt
        /// <summary>
        /// 颜色
        /// </summary>
        [Label("颜色")]
        public static readonly Property<string> NormtProperty = P<Item>.Register(e => e.Normt);

        /// <summary>
        /// 颜色
        /// </summary>
        public string Normt
        {
            get { return this.GetProperty(NormtProperty); }
            set { this.SetProperty(NormtProperty, value); }
        }
        #endregion

        #region 可焊性 Zkhxhgy
        /// <summary>
        /// 可焊性
        /// </summary>
        [Label("可焊性")]
        public static readonly Property<string> ZkhxhgyProperty = P<Item>.Register(e => e.Zkhxhgy);

        /// <summary>
        /// 可焊性
        /// </summary>
        public string Zkhxhgy
        {
            get { return this.GetProperty(ZkhxhgyProperty); }
            set { this.SetProperty(ZkhxhgyProperty, value); }
        }
        #endregion

        #region 胶带类型 Zjdlx
        /// <summary>
        /// 胶带类型
        /// </summary>
        [Label("胶带类型")]
        public static readonly Property<string> ZjdlxProperty = P<Item>.Register(e => e.Zjdlx);

        /// <summary>
        /// 胶带类型
        /// </summary>
        public string Zjdlx
        {
            get { return this.GetProperty(ZjdlxProperty); }
            set { this.SetProperty(ZjdlxProperty, value); }
        }
        #endregion

        #region 型号 Zmodel
        /// <summary>
        /// 型号
        /// </summary>
        [Label("型号")]
        public static readonly Property<string> ZmodelProperty = P<Item>.Register(e => e.Zmodel);

        /// <summary>
        /// 型号
        /// </summary>
        public string Zmodel
        {
            get { return this.GetProperty(ZmodelProperty); }
            set { this.SetProperty(ZmodelProperty, value); }
        }
        #endregion

        #region 规格 Zgg
        /// <summary>
        /// 规格
        /// </summary>
        [Label("规格")]
        public static readonly Property<string> ZggProperty = P<Item>.Register(e => e.Zgg);

        /// <summary>
        /// 规格
        /// </summary>
        public string Zgg
        {
            get { return this.GetProperty(ZggProperty); }
            set { this.SetProperty(ZggProperty, value); }
        }
        #endregion

        #region 重量 Weight
        /// <summary>
        /// 重量
        /// </summary>
        [Label("单位净重")]
        public static readonly Property<decimal?> WeightProperty = P<Item>.Register(e => e.Weight);

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight
        {
            get { return GetProperty(WeightProperty); }
            set { SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 净重单位 WeightUnit
        /// <summary>
        /// 净重单位
        /// </summary>
        [Label("净重单位")]
        public static readonly Property<string> WeightUnitProperty = P<Item>.Register(e => e.WeightUnit);

        /// <summary>
        /// 净重单位
        /// </summary>
        public string WeightUnit
        {
            get { return this.GetProperty(WeightUnitProperty); }
            set { this.SetProperty(WeightUnitProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 物料 实体配置
    /// </summary>
    internal class ItemConfig : EntityConfig<Item>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM").MapAllProperties();
            Meta.Property(Item.NameProperty).ColumnMeta.HasLength(240);
            Meta.Property(Item.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(Item.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(Item.NameProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }

    internal class ItemWebViewConfig : WebViewConfig<Item>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(SIE.Core.Items.Item.CodeProperty);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            base.ConfigSelectionView();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
                View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
            }
        }
    }


    /// <summary>
    /// 实体页面配置
    /// </summary>
    internal class ItemViewConfig : WPFViewConfig<Item>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(SIE.Core.Items.Item.CodeProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
                View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            base.ConfigSelectionView();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
                View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
                ////View.Property(p => p.Description).HasLabel("描述").Show(ShowInWhere.List);
                ////View.Property(p => p.State).HasLabel("状态").Show(ShowInWhere.List);
            }
        }
    }
}