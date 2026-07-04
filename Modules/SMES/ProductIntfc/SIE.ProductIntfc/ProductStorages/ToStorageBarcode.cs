using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 待入库条码
    /// </summary>
    [RootEntity, Serializable]
    [Label("待入库条码")]
    public partial class ToStorageBarcode : DataEntity
    {
        #region 入库条码 Barcode
        /// <summary>
        /// 入库条码
        /// </summary>
        [Label("入库条码")]
        public static readonly Property<string> BarcodeProperty = P<ToStorageBarcode>.Register(e => e.Barcode);

        /// <summary>
        /// 入库条码
        /// </summary>
        public string Barcode
        {
            get { return GetProperty(BarcodeProperty); }
            set { SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 产品数量 Qty
        /// <summary>
        /// 产品数量
        /// </summary>
        [Label("产品数量")]
        public static readonly Property<decimal> QtyProperty = P<ToStorageBarcode>.Register(e => e.Qty);

        /// <summary>
        /// 产品数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 入库层级 Level
        /// <summary>
        /// 入库层级
        /// </summary>
        [Label("入库层级")]
        public static readonly Property<string> LevelProperty = P<ToStorageBarcode>.Register(e => e.Level);

        /// <summary>
        /// 入库层级
        /// </summary>
        public string Level
        {
            get { return GetProperty(LevelProperty); }
            set { SetProperty(LevelProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult> InspectionResultProperty = P<ToStorageBarcode>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 待入库条码明细列表 ToStorageBarcodeDetailList
        /// <summary>
        /// 待入库条码明细列表
        /// </summary>
        [Label("条码明细")]
        public static readonly ListProperty<EntityList<ToStorageBarcodeDetail>> ToStorageBarcodeDetailListProperty = P<ToStorageBarcode>.RegisterList(e => e.ToStorageBarcodeDetailList);

        /// <summary>
        /// 待入库条码明细列表
        /// </summary>
        public EntityList<ToStorageBarcodeDetail> ToStorageBarcodeDetailList
        {
            get { return this.GetLazyList(ToStorageBarcodeDetailListProperty); }
        }
        #endregion

        #region 入库工单 StorageWorkOrder
        /// <summary>
        /// 入库工单Id
        /// </summary>
        public static readonly IRefIdProperty StorageWorkOrderIdProperty = P<ToStorageBarcode>.RegisterRefId(e => e.StorageWorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 入库工单Id
        /// </summary>
        public double StorageWorkOrderId
        {
            get { return (double)GetRefId(StorageWorkOrderIdProperty); }
            set { SetRefId(StorageWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 入库工单
        /// </summary>
        public static readonly RefEntityProperty<StorageWorkOrder> StorageWorkOrderProperty = P<ToStorageBarcode>.RegisterRef(e => e.StorageWorkOrder, StorageWorkOrderIdProperty);

        /// <summary>
        /// 入库工单
        /// </summary>
        public StorageWorkOrder StorageWorkOrder
        {
            get { return GetRefEntity(StorageWorkOrderProperty); }
            set { SetRefEntity(StorageWorkOrderProperty, value); }
        }
        #endregion

        #region 任务单ID DispatchTaskId
        /// <summary>
        /// 任务单ID
        /// </summary>
        [Label("任务单ID")]
        public static readonly Property<double?> DispatchTaskIdProperty = P<ToStorageBarcode>.Register(e => e.DispatchTaskId);

        /// <summary>
        /// 任务单ID
        /// </summary>
        public double? DispatchTaskId
        {
            get { return this.GetProperty(DispatchTaskIdProperty); }
            set { this.SetProperty(DispatchTaskIdProperty, value); }
        }
        #endregion

        #region 是否已入库 IsStored
        /// <summary>
        /// 是否已入库
        /// </summary>
        [Label("已入库")]
        public static readonly Property<bool> IsStoredProperty = P<ToStorageBarcode>.Register(e => e.IsStored);

        /// <summary>
        /// 是否已入库
        /// </summary>
        public bool IsStored
        {
            get { return this.GetProperty(IsStoredProperty); }
            set { this.SetProperty(IsStoredProperty, value); }
        }
        #endregion

        #region 包装规则名称 PackageRuleName
        /// <summary>
        /// 包装规则名称
        /// </summary>
        [Label("包装规则名称")]
        public static readonly Property<string> PackageRuleNameProperty = P<ToStorageBarcode>.Register(e => e.PackageRuleName);

        /// <summary>
        /// 包装规则名称
        /// </summary>
        public string PackageRuleName
        {
            get { return this.GetProperty(PackageRuleNameProperty); }
            set { this.SetProperty(PackageRuleNameProperty, value); }
        }
        #endregion

        #region 生产批次 BatchBarcode
        /// <summary>
        /// 入库条码
        /// </summary>
        [Label("生产批号")]
        public static readonly Property<string> BatchBarcodeProperty = P<ToStorageBarcode>.Register(e => e.BatchBarcode);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string BatchBarcode
        {
            get { return GetProperty(BatchBarcodeProperty); }
            set { SetProperty(BatchBarcodeProperty, value); }
        }
        #endregion

        #region 是主单位 IsMasterUnit
        /// <summary>
        /// 是主单位
        /// </summary>
        [Label("是主单位")]
        public static readonly Property<bool?> IsMasterUnitProperty = P<ToStorageBarcode>.Register(e => e.IsMasterUnit);

        /// <summary>
        /// 是主单位
        /// </summary>
        public bool? IsMasterUnit
        {
            get { return this.GetProperty(IsMasterUnitProperty); }
            set { this.SetProperty(IsMasterUnitProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 待入库条码 实体配置
    /// </summary>
    internal class ToStorageBarcodeConfig : EntityConfig<ToStorageBarcode>
    {
        /// <summary>
        /// 数据表Config
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("INF_TO_STO_BAR").MapAllProperties();
            Meta.Property(ToStorageBarcode.BarcodeProperty).ColumnMeta.HasIndex(IndexTypeMeta.Indexed);
            Meta.EnablePhantoms();
        }
    }
}