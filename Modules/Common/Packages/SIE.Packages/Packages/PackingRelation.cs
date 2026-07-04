using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using System;

namespace SIE.Packages
{
    /// <summary>
    /// 包装关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装关系")]
    [DisplayMember(nameof(PackageNo))]
    public partial class PackingRelation : DataEntity
    {
        #region 包装号 PackageNo
        /// <summary>
        /// 包装号
        /// </summary>
        [Label("包装号")]
        public static readonly Property<string> PackageNoProperty = P<PackingRelation>.Register(e => e.PackageNo);

        /// <summary>
        /// 包装号
        /// </summary>
        public string PackageNo
        {
            get { return GetProperty(PackageNoProperty); }
            set { SetProperty(PackageNoProperty, value); }
        }
        #endregion

        #region 父包装号 ParentNo
        /// <summary>
        /// 父包装号
        /// </summary>
        [Label("父包装号")]
        public static readonly Property<string> ParentNoProperty = P<PackingRelation>.Register(e => e.ParentNo);

        /// <summary>
        /// 父包装号
        /// </summary>
        public string ParentNo
        {
            get { return GetProperty(ParentNoProperty); }
            set { SetProperty(ParentNoProperty, value); }
        }
        #endregion

        #region 已加入包装数 PackedQty
        /// <summary>
        /// 已加入包装数
        /// </summary>
        [Label("已加入包装数")]
        public static readonly Property<decimal> PackedQtyProperty = P<PackingRelation>.Register(e => e.PackedQty);

        /// <summary>
        /// 已加入包装数
        /// </summary>
        public decimal PackedQty
        {
            get { return GetProperty(PackedQtyProperty); }
            set { SetProperty(PackedQtyProperty, value); }
        }
        #endregion

        #region 满包装包装数 FullPackedQty
        /// <summary>
        /// 满包装包装数
        /// </summary>
        [Label("满包装包装数")]
        public static readonly Property<decimal> FullPackedQtyProperty = P<PackingRelation>.Register(e => e.FullPackedQty);

        /// <summary>
        /// 满包装包装数
        /// </summary>
        public decimal FullPackedQty
        {
            get { return this.GetProperty(FullPackedQtyProperty); }
            set { this.SetProperty(FullPackedQtyProperty, value); }
        }
        #endregion 

        #region 物料数量
        /// <summary>
        /// 物料数量
        /// </summary>
        [Label("物料数量")]
        public static readonly Property<decimal> ItemQtyProperty = P<PackingRelation>.Register(e => e.ItemQty);

        /// <summary>
        /// 物料数量
        /// </summary>
        public decimal ItemQty
        {
            get { return this.GetProperty(ItemQtyProperty); }
            set { this.SetProperty(ItemQtyProperty, value); }
        }
        #endregion

        #region 包装人 PackingBy
        /// <summary>
        /// 包装人
        /// </summary>
        [Label("包装人")]
        public static readonly Property<double> PackingByProperty = P<PackingRelation>.Register(e => e.PackingBy);

        /// <summary>
        /// 包装人
        /// </summary>
        public double PackingBy
        {
            get { return GetProperty(PackingByProperty); }
            set { SetProperty(PackingByProperty, value); }
        }
        #endregion

        #region 包装时间 PackedDate
        /// <summary>
        /// 包装时间
        /// </summary>
        [Label("包装时间")]
        public static readonly Property<DateTime> PackedDateProperty = P<PackingRelation>.Register(e => e.PackedDate);

        /// <summary>
        /// 包装时间
        /// </summary>
        public DateTime PackedDate
        {
            get { return GetProperty(PackedDateProperty); }
            set { SetProperty(PackedDateProperty, value); }
        }
        #endregion

        #region 包装单位 PackageUnit
        /// <summary>
        /// 包装单位IDProperty
        /// </summary>
        [Label("包装单位")]
        public static readonly IRefIdProperty PackageUnitIdProperty = P<PackingRelation>.RegisterRefId(e => e.PackageUnitId, ReferenceType.Normal);

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId
        {
            get { return (double)this.GetRefId(PackageUnitIdProperty); }
            set { this.SetRefId(PackageUnitIdProperty, value); }
        }

        /// <summary>
        /// 包装单位Property
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackageUnitProperty = P<PackingRelation>.RegisterRef(e => e.PackageUnit, PackageUnitIdProperty);

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackageUnit
        {
            get { return this.GetRefEntity(PackageUnitProperty); }
            set { this.SetRefEntity(PackageUnitProperty, value); }
        }
        #endregion

        #region 根Id RootId
        /// <summary>
        /// 根Id
        /// </summary>
        [Label("根Id")]
        public static readonly Property<double> RootIdProperty = P<PackingRelation>.Register(e => e.RootId);

        /// <summary>
        /// 根Id
        /// </summary>
        public double RootId
        {
            get { return GetProperty(RootIdProperty); }
            set { SetProperty(RootIdProperty, value); }
        }
        #endregion

        #region 物流状态 State
        /// <summary>
        /// 物流状态
        /// </summary>
        [Label("物流状态")]
        public static readonly Property<LogisticState> StateProperty = P<PackingRelation>.Register(e => e.State);

        /// <summary>
        /// 物流状态
        /// </summary>
        public LogisticState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工序 ProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<PackingRelation>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工位 StationId
        /// <summary>
        /// 工位ID
        /// </summary>
        [Label("工位Id")]
        public static readonly Property<double> StationIdProperty = P<PackingRelation>.Register(e => e.StationId);

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId
        {
            get { return this.GetProperty(StationIdProperty); }
            set { this.SetProperty(StationIdProperty, value); }
        }
        #endregion

        #region 工序是否完成
        /// <summary>
        /// 工序是否完成
        /// </summary>
        [Label("工序是否完成")]
        public static readonly Property<bool> IsProcessFinishProperty = P<PackingRelation>.Register(e => e.IsProcessFinish);

        /// <summary>
        /// 工序是否完成
        /// </summary>
        public bool IsProcessFinish
        {
            get { return this.GetProperty(IsProcessFinishProperty); }
            set { this.SetProperty(IsProcessFinishProperty, value); }
        }
        #endregion

        #region 工单 WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WorkOrderIdProperty = P<PackingRelation>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 包装单位 PackageUnitName
        /// <summary>
        /// 包装单位
        /// </summary>
        [Label("包装单位")]
        public static readonly Property<string> PackageUnitNameProperty = P<PackingRelation>.RegisterView(e => e.PackageUnitName, p => p.PackageUnit.Name);

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackageUnitName
        {
            get { return this.GetProperty(PackageUnitNameProperty); }
        }
        #endregion

        #region 是否已打包 IsPacked
        /// <summary>
        /// 是否已打包
        /// </summary>
        [Label("已打包")]
        public static readonly Property<bool> IsPackedProperty = P<PackingRelation>.RegisterReadOnly(
            e => e.IsPacked, e => !e.PackageNo.IsNullOrEmpty());

        /// <summary>
        /// 是否已打包
        /// </summary> 
        public bool IsPacked
        {
            get { return this.GetProperty(IsPackedProperty); }
        }
        #endregion

        #region 是否满包装 IsFullPackage
        /// <summary>
        /// 是否满包装
        /// </summary>
        [Label("是否满包装")]
        public static readonly Property<bool> IsFullPackageProperty = P<PackingRelation>.RegisterReadOnly(
            e => e.IsFullPackage, e => e.FullPackedQty > 0 && e.PackedQty == e.FullPackedQty);

        /// <summary>
        /// 是否满包装
        /// </summary> 
        public bool IsFullPackage
        {
            get { return this.GetProperty(IsFullPackageProperty); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        public static readonly Property<string> CustomerProperty = P<PackingRelation>.Register(e => e.Customer);

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer
        {
            get { return this.GetProperty(CustomerProperty); }
            set { this.SetProperty(CustomerProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PackingRelation>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<PackingRelation>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 设备编码 EquipCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipCodeProperty = P<PackingRelation>.Register(e => e.EquipCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode
        {
            get { return this.GetProperty(EquipCodeProperty); }
            set { this.SetProperty(EquipCodeProperty, value); }
        }
        #endregion

        #region 信息ID MessageId
        /// <summary>
        /// 信息ID
        /// </summary>
        [Label("信息ID")]
        public static readonly Property<string> MessageIdProperty = P<PackingRelation>.Register(e => e.MessageId);

        /// <summary>
        /// 信息ID
        /// </summary>
        public string MessageId
        {
            get { return this.GetProperty(MessageIdProperty); }
            set { this.SetProperty(MessageIdProperty, value); }
        }
        #endregion

        #region 批次号 Batch
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchProperty = P<PackingRelation>.Register(e => e.Batch);

        /// <summary>
        /// 批次号
        /// </summary>
        public string Batch
        {
            get { return this.GetProperty(BatchProperty); }
            set { this.SetProperty(BatchProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 包装关系 实体配置
    /// </summary>    
    internal class PackingRelationConfig : EntityConfig<PackingRelation>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PKG_RELATION").MapAllProperties();
            Meta.Property(PackingRelation.PackageNoProperty).ColumnMeta.HasIndex();
            Meta.SupportTree();
            Meta.EnablePhantoms();
        }
    }
}