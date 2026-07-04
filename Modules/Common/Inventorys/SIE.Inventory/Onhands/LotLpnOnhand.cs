using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 批次和LPN库存，LPN可为空
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LpnOnhandCriteria))]
    [Label("批次和LPN库存")]
    [DisplayMember(nameof(Id))]
    public partial class LotLpnOnhand : BaseOnhand
    {
        #region 货主编码 StorerCode
        /// <summary>
        /// 货主编码
        /// </summary>
        [Label("货主编码")]
        public static readonly Property<string> StorerCodeProperty = P<LotLpnOnhand>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly IRefIdProperty LotIdProperty =
            P<LotLpnOnhand>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

        /// <summary>
        /// 批次
        /// </summary>
        public double LotId
        {
            get { return (double)this.GetRefId(LotIdProperty); }
            set { this.SetRefId(LotIdProperty, value); }
        }

        /// <summary>
        /// 批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> LotProperty =
            P<LotLpnOnhand>.RegisterRef(e => e.Lot, LotIdProperty);

        /// <summary>
        /// 批次
        /// </summary>
        public Lot Lot
        {
            get { return this.GetRefEntity(LotProperty); }
            set { this.SetRefEntity(LotProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<LotLpnOnhand>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return GetProperty(LotCodeProperty); }
            set { SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region LPN Lpn
        /// <summary>
        /// LPN
        /// </summary>
        [Label("LPN")]
        public static readonly Property<string> LpnProperty = P<LotLpnOnhand>.Register(e => e.Lpn);

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn
        {
            get { return GetProperty(LpnProperty); }
            set { SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LotLpnOnhand>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 任务号 TaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> TaskNoProperty = P<LotLpnOnhand>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 库存状态 State
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState> StateProperty = P<LotLpnOnhand>.Register(e => e.State);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 批次属性01 LotAtt01
        /// <summary>
        /// 批次属性01
        /// </summary>
        [Label("批次属性01")]
        public static readonly Property<DateTime?> LotAtt01Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt01, p => p.Lot.LotAtt01);

        /// <summary>
        /// 批次属性01
        /// </summary>
        public DateTime? LotAtt01
        {
            get { return this.GetProperty(LotAtt01Property); }
        }
        #endregion

        #region 批次属性02 LotAtt02
        /// <summary>
        /// 批次属性02
        /// </summary>
        [Label("批次属性02")]
        public static readonly Property<DateTime?> LotAtt02Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt02, p => p.Lot.LotAtt02);

        /// <summary>
        /// 批次属性02
        /// </summary>
        public DateTime? LotAtt02
        {
            get { return this.GetProperty(LotAtt02Property); }
        }
        #endregion

        #region 批次属性03 LotAtt03
        /// <summary>
        /// 批次属性03
        /// </summary>
        [Label("批次属性03")]
        public static readonly Property<DateTime?> LotAtt03Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt03, p => p.Lot.LotAtt03);

        /// <summary>
        /// 批次属性03
        /// </summary>
        public DateTime? LotAtt03
        {
            get { return this.GetProperty(LotAtt03Property); }
        }
        #endregion

        #region 批次属性04 LotAtt04
        /// <summary>
        /// 批次属性04
        /// </summary>
        [Label("批次属性04")]
        public static readonly Property<string> LotAtt04Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt04, p => p.Lot.LotAtt04);

        /// <summary>
        /// 批次属性04
        /// </summary>
        public string LotAtt04
        {
            get { return this.GetProperty(LotAtt04Property); }
        }
        #endregion

        #region 批次属性05 LotAtt05
        /// <summary>
        /// 批次属性05
        /// </summary>
        [Label("批次属性05")]
        public static readonly Property<decimal?> LotAtt05Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt05, p => p.Lot.LotAtt05);

        /// <summary>
        /// 批次属性05
        /// </summary>
        public decimal? LotAtt05
        {
            get { return this.GetProperty(LotAtt05Property); }
        }
        #endregion

        #region 批次属性06 LotAtt06
        /// <summary>
        /// 批次属性06
        /// </summary>
        [Label("批次属性06")]
        public static readonly Property<decimal?> LotAtt06Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt06, p => p.Lot.LotAtt06);

        /// <summary>
        /// 批次属性06
        /// </summary>
        public decimal? LotAtt06
        {
            get { return this.GetProperty(LotAtt06Property); }
        }
        #endregion

        #region 批次属性07 LotAtt07
        /// <summary>
        /// 批次属性07
        /// </summary>
        [Label("批次属性07")]
        public static readonly Property<bool?> LotAtt07Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt07, p => p.Lot.LotAtt07);

        /// <summary>
        /// 批次属性07
        /// </summary>
        public bool? LotAtt07
        {
            get { return this.GetProperty(LotAtt07Property); }
        }
        #endregion

        #region 批次属性08 LotAtt08
        /// <summary>
        /// 批次属性08
        /// </summary>
        [Label("批次属性08")]
        public static readonly Property<string> LotAtt08Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt08, p => p.Lot.LotAtt08);

        /// <summary>
        /// 批次属性08
        /// </summary>
        public string LotAtt08
        {
            get { return this.GetProperty(LotAtt08Property); }
        }
        #endregion

        #region 批次属性09 LotAtt09
        /// <summary>
        /// 批次属性09
        /// </summary>
        [Label("批次属性09")]
        public static readonly Property<string> LotAtt09Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt09, p => p.Lot.LotAtt09);

        /// <summary>
        /// 批次属性09
        /// </summary>
        public string LotAtt09
        {
            get { return this.GetProperty(LotAtt09Property); }
        }
        #endregion

        #region 批次属性10 LotAtt10
        /// <summary>
        /// 批次属性10
        /// </summary>
        [Label("批次属性10")]
        public static readonly Property<string> LotAtt10Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt10, p => p.Lot.LotAtt10);

        /// <summary>
        /// 批次属性10
        /// </summary>
        public string LotAtt10
        {
            get { return this.GetProperty(LotAtt10Property); }
        }
        #endregion

        #region 批次属性11 LotAtt11
        /// <summary>
        /// 批次属性11
        /// </summary>
        [Label("批次属性11")]
        public static readonly Property<DateTime?> LotAtt11Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt11, p => p.Lot.LotAtt11);

        /// <summary>
        /// 批次属性11
        /// </summary>
        public DateTime? LotAtt11
        {
            get { return this.GetProperty(LotAtt11Property); }
        }
        #endregion

        #region 批次属性12 LotAtt12
        /// <summary>
        /// 批次属性12
        /// </summary>
        [Label("批次属性12")]
        public static readonly Property<DateTime?> LotAtt12Property = P<LotLpnOnhand>.RegisterView(e => e.LotAtt12, p => p.Lot.LotAtt12);

        /// <summary>
        /// 批次属性12
        /// </summary>
        public DateTime? LotAtt12
        {
            get { return this.GetProperty(LotAtt12Property); }
        }
        #endregion        

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<LotLpnOnhand>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 不映射数据库栏位
        #region 现有量SET QtySet
        /// <summary>
        /// 现有量SET
        /// </summary>
        [Label("现有量SET")]
        public static readonly Property<decimal?> QtySetProperty = P<LotLpnOnhand>.Register(e => e.QtySet);

        /// <summary>
        /// 现有量SET
        /// </summary>
        public decimal? QtySet
        {
            get { return this.GetProperty(QtySetProperty); }
            set { this.SetProperty(QtySetProperty, value); }
        }
        #endregion

        #region 叉板数PCS PcsQty
        /// <summary>
        /// 叉板数PCS
        /// </summary>
        [Label("叉板数PCS")]
        public static readonly Property<decimal?> PcsQtyProperty = P<LotLpnOnhand>.Register(e => e.PcsQty);

        /// <summary>
        /// 叉板数PCS
        /// </summary>
        public decimal? PcsQty
        {
            get { return this.GetProperty(PcsQtyProperty); }
            set { this.SetProperty(PcsQtyProperty, value); }
        }
        #endregion

        #region 优先使用库存 IsFirstUse
        /// <summary>
        /// 优先使用库存 (分配的时候指定库存需要优先用掉)
        /// </summary>
        [Label("优先使用库存")]
        public static readonly Property<bool> IsFirstUseProperty = P<LotLpnOnhand>.Register(e => e.IsFirstUse);

        /// <summary>
        /// 优先使用库存
        /// </summary>
        public bool IsFirstUse
        {
            get { return this.GetProperty(IsFirstUseProperty); }
            set { this.SetProperty(IsFirstUseProperty, value); }
        }
        #endregion

        #region 转换率 ConvertFigre
        /// <summary>
        /// 转换率
        /// </summary>
        [Label("转换率")]
        public static readonly Property<decimal> ConvertFigreProperty = P<LotLpnOnhand>.Register(e => e.ConvertFigre);

        /// <summary>
        /// 转换率
        /// </summary>
        public decimal ConvertFigre
        {
            get { return this.GetProperty(ConvertFigreProperty); }
            set { this.SetProperty(ConvertFigreProperty, value); }
        }
        #endregion

        #region 对照后的批次号 RealLotCode
        /// <summary>
        /// 对照后的批次号
        /// </summary>
        [Label("对照后的批次号")]
        public static readonly Property<string> RealLotCodeProperty = P<LotLpnOnhand>.Register(e => e.RealLotCode);

        /// <summary>
        /// 对照后的批次号
        /// </summary>
        public string RealLotCode
        {
            get { return this.GetProperty(RealLotCodeProperty); }
            set { this.SetProperty(RealLotCodeProperty, value); }
        }
        #endregion

        #region 现有量(辅) SecondQty
        /// <summary>
        /// 现有量(辅)
        /// </summary>
        [Label("现有量(辅)")]
        public static readonly Property<decimal> SecondQtyProperty = P<LotLpnOnhand>.Register(e => e.SecondQty);

        /// <summary>
        /// 现有量(辅)
        /// </summary>
        public decimal SecondQty
        {
            get { return this.GetProperty(SecondQtyProperty); }
            set { this.SetProperty(SecondQtyProperty, value); }
        }
        #endregion

        #region 顺序号 SeqNo
        /// <summary>
        /// 顺序号
        /// </summary>
        [Label("顺序号")]
        public static readonly Property<int> SeqNoProperty = P<LotLpnOnhand>.Register(e => e.SeqNo);

        /// <summary>
        /// 顺序号
        /// </summary>
        public int SeqNo
        {
            get { return this.GetProperty(SeqNoProperty); }
            set { this.SetProperty(SeqNoProperty, value); }
        }
        #endregion

        #region 是否位置跟踪物料 是否位置跟踪物料
        /// <summary>
        /// 是否位置跟踪物料
        /// </summary>
        [Label("是否位置跟踪物料")]
        public static readonly Property<bool> IsSerItemProperty = P<LotLpnOnhand>.Register(e => e.IsSerItem);

        /// <summary>
        /// 是否位置跟踪物料
        /// </summary>
        public bool IsSerItem
        {
            get { return this.GetProperty(IsSerItemProperty); }
            set { this.SetProperty(IsSerItemProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 批次和LPN库存 实体配置
    /// </summary>
    internal class LotLpnOnhandConfig : EntityConfig<LotLpnOnhand>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INV_LOT_LPN_ONHAND").MapAllProperties();
            Meta.Property(LotLpnOnhand.SeqNoProperty).DontMapColumn();
            Meta.Property(LotLpnOnhand.QtySetProperty).DontMapColumn();
            Meta.Property(LotLpnOnhand.PcsQtyProperty).DontMapColumn();
            Meta.Property(LotLpnOnhand.ConvertFigreProperty).DontMapColumn();
            Meta.Property(LotLpnOnhand.SecondQtyProperty).DontMapColumn();
            Meta.Property(LotLpnOnhand.IsFirstUseProperty).DontMapColumn();
            Meta.Property(LotLpnOnhand.RealLotCodeProperty).DontMapColumn();
            Meta.Property(LotLpnOnhand.IsSerItemProperty).DontMapColumn();
            Meta.IndexGroupOnProperties(LotLpnOnhand.LotProperty, LotLpnOnhand.StorageLocationIdProperty, LotLpnOnhand.ItemIdProperty, LotLpnOnhand.LpnProperty, LotLpnOnhand.StorerCodeProperty);
            Meta.IndexGroupOnProperties(LotLpnOnhand.StorageLocationIdProperty, LotLpnOnhand.ItemIdProperty, LotLpnOnhand.LotCodeProperty);
            Meta.IndexGroupOnProperties(LotLpnOnhand.WarehouseIdProperty, LotLpnOnhand.StorageLocationIdProperty, LotLpnOnhand.ItemIdProperty);
            Meta.Property(LotLpnOnhand.LpnProperty).ColumnMeta.HasIndex();
            Meta.Property(LotLpnOnhand.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(LotLpnOnhand.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}