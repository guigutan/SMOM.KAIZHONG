using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Boxs
{
    /// <summary>
    /// 周转箱
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("周转箱")]
    [DisplayMember(nameof(Code))]
    public partial class TurnoverBox : DataEntity
    {
        /// <summary>
        /// 周转箱类型常量
        /// </summary>
        public static readonly string BoxTypeCatalog = "BOX_TYPE";

        /// <summary>
        /// 构造函数
        /// </summary>
        public TurnoverBox()
        {
            State = BoxState.Unused;
            Capacity = 1;
        }

        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("条码")]
        public static readonly Property<string> CodeProperty = P<TurnoverBox>.Register(e => e.Code);

        /// <summary>
        /// 条码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [Label("类型")]
        public static readonly Property<string> TypeProperty = P<TurnoverBox>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Required]
        [Label("状态")]
        public static readonly Property<BoxState?> StateProperty = P<TurnoverBox>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public BoxState? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 默认容量 Capacity
        /// <summary>
        /// 默认容量
        /// </summary>
        [Label("默认容量")]
        [MinValue(1)]
        [MaxValue(1000000)]
        [Required]
        public static readonly Property<decimal> CapacityProperty = P<TurnoverBox>.Register(e => e.Capacity);

        /// <summary>
        /// 默认容量
        /// </summary>
        public decimal Capacity
        {
            get { return GetProperty(CapacityProperty); }
            set { SetProperty(CapacityProperty, value); }
        }
        #endregion

        #region 周转箱型号 TrunoverBoxModel
        /// <summary>
        /// 周转箱型号Id
        /// </summary>
        [Label("周转箱型号")]
        public static readonly IRefIdProperty TrunoverBoxModelIdProperty =
            P<TurnoverBox>.RegisterRefId(e => e.TrunoverBoxModelId, ReferenceType.Normal);

        /// <summary>
        /// 周转箱型号Id
        /// </summary>
        public double TrunoverBoxModelId
        {
            get { return (double)this.GetRefId(TrunoverBoxModelIdProperty); }
            set { this.SetRefId(TrunoverBoxModelIdProperty, value); }
        }

        /// <summary>
        /// 周转箱型号
        /// </summary>
        public static readonly RefEntityProperty<TurnoverBoxModel> TrunoverBoxModelProperty =
            P<TurnoverBox>.RegisterRef(e => e.TrunoverBoxModel, TrunoverBoxModelIdProperty);

        /// <summary>
        /// 周转箱型号
        /// </summary>
        public TurnoverBoxModel TrunoverBoxModel
        {
            get { return this.GetRefEntity(TrunoverBoxModelProperty); }
            set { this.SetRefEntity(TrunoverBoxModelProperty, value); }
        }
        #endregion

        #region 产品容量列表 CapacityList
        /// <summary>
        /// 产品容量列表
        /// </summary>
        public static readonly ListProperty<EntityList<ProductCapacity>> CapacityListProperty = P<TurnoverBox>.RegisterList(e => e.CapacityList);

        /// <summary>
        /// 产品容量列表
        /// </summary>
        public EntityList<ProductCapacity> CapacityList
        {
            get { return this.GetLazyList(CapacityListProperty); }
        }
        #endregion

        #region 周转箱型号编码 TurnoverBoxModelCode
        /// <summary>
        /// 周转箱型号编码
        /// </summary>
        [Label("周转箱型号编码")]
        public static readonly Property<string> TurnoverBoxModelCodeProperty = P<TurnoverBox>.RegisterView(e => e.TurnoverBoxModelCode, p => p.TrunoverBoxModel.Code);

        /// <summary>
        /// 周转箱型号编码
        /// </summary>
        public string TurnoverBoxModelCode
        {
            get { return this.GetProperty(TurnoverBoxModelCodeProperty); }
        }
        #endregion

        #region 周转箱型号名称 TurnoverBoxModelName
        /// <summary>
        /// 周转箱型号名称
        /// </summary>
        [Label("周转箱型号名称")]
        public static readonly Property<string> TurnoverBoxModelNameProperty = P<TurnoverBox>.RegisterView(e => e.TurnoverBoxModelName, p => p.TrunoverBoxModel.Name);

        /// <summary>
        /// 周转箱型号名称
        /// </summary>
        public string TurnoverBoxModelName
        {
            get { return this.GetProperty(TurnoverBoxModelNameProperty); }
        }
        #endregion

        #region 长 Length
        /// <summary>
        /// 长
        /// </summary>
        [Label("长")]
        public static readonly Property<decimal?> LengthProperty = P<TurnoverBox>.RegisterView(e => e.Length, p => p.TrunoverBoxModel.Length);

        /// <summary>
        /// 长
        /// </summary>
        public decimal? Length
        {
            get { return this.GetProperty(LengthProperty); }
        }
        #endregion

        #region 宽 Width
        /// <summary>
        /// 宽
        /// </summary>
        [Label("宽")]
        public static readonly Property<decimal?> WidthProperty = P<TurnoverBox>.RegisterView(e => e.Width, p => p.TrunoverBoxModel.Width);

        /// <summary>
        /// 宽
        /// </summary>
        public decimal? Width
        {
            get { return this.GetProperty(WidthProperty); }
        }
        #endregion

        #region 高 Height
        /// <summary>
        /// 高
        /// </summary>
        [Label("高")]
        public static readonly Property<decimal?> HeightProperty = P<TurnoverBox>.RegisterView(e => e.Height, p => p.TrunoverBoxModel.Height);

        /// <summary>
        /// 高
        /// </summary>
        public decimal? Height
        {
            get { return this.GetProperty(HeightProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 周转箱 实体配置
    /// </summary>
    internal class TurnoverBoxConfig : EntityConfig<TurnoverBox>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("CNT_TURNOVER_BOX").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}