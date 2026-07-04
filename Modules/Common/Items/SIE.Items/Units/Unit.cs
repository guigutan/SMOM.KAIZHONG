using SIE.Domain;
using SIE.Items.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 单位
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("单位")]
    [DisplayMember(nameof(Name))]
    public partial class Unit : DataEntity
    {
        /// <summary>
        /// 快码类型
        /// </summary>
        public const string CatalogType = "UNIT_TYPE";

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Unit>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<Unit>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<string> TypeProperty = P<Unit>.Register(e => e.Type, new PropertyMetadata<string>
        {
            PropertyChangedCallBack = (o, e) => (o as Unit).OnUnitTypeChanged(e)
        });

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }

        /// <summary>
        /// 类型属性变更后调用的方法
        /// </summary>
        /// <param name="e">属性变更事件</param>
        private void OnUnitTypeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (!Type.IsNullOrEmpty() && !Precision.HasValue)
            {
                Precision = 0;
            }
        }
        #endregion

        #region 单位精度 Precision
        /// <summary>
        /// 单位精度
        /// </summary>
        [MinValue(0)]
        [MaxValue(8)]
        [Label("单位精度")]
        public static readonly Property<int?> PrecisionProperty = P<Unit>.Register(e => e.Precision);

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? Precision
        {
            get { return this.GetProperty(PrecisionProperty); }
            set { this.SetProperty(PrecisionProperty, value); }
        }
        #endregion

        #region 单位来源 UnitSource
        /// <summary>
        /// 单位来源
        /// </summary>
        [Label("单位来源")]
        [Required]
        public static readonly Property<UnitSource> UnitSourceProperty = P<Unit>.Register(e => e.UnitSource);

        /// <summary>
        /// 单位来源
        /// </summary>
        public UnitSource UnitSource
        {
            get { return GetProperty(UnitSourceProperty); }
            set { SetProperty(UnitSourceProperty, value); }
        }
        #endregion

        #region 取舍类型 TradeType
        /// <summary>
        /// 取舍类型
        /// </summary>
        [Label("取舍类型")]
        [Required]
        public static readonly Property<TradeType> TradeTypeProperty = P<Unit>.Register(e => e.TradeType);

        /// <summary>
        /// 取舍类型
        /// </summary>
        public TradeType TradeType
        {
            get { return GetProperty(TradeTypeProperty); }
            set { SetProperty(TradeTypeProperty, value); }
        }
        #endregion

        #region 是否初始化 IsInit
        /// <summary>
        /// 是否初始化
        /// </summary>
        [Label("是否初始化")]
        public static readonly Property<bool> IsInitProperty = P<Unit>.Register(e => e.IsInit);

        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool IsInit
        {
            get { return GetProperty(IsInitProperty); }
            set { SetProperty(IsInitProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 单位 实体配置
    /// </summary>
    internal class UnitConfig : EntityConfig<Unit>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BD_UNIT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}