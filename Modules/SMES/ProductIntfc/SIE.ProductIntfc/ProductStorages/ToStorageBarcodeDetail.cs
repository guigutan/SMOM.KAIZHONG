using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 待入库条码明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("待入库条码明细")]
    public partial class ToStorageBarcodeDetail : DataEntity
    {
        #region 生产条码 Barcode
        /// <summary>
        /// 生产条码
        /// </summary>
        [Label("生产条码")]
        public static readonly Property<string> BarcodeProperty = P<ToStorageBarcodeDetail>.Register(e => e.Barcode);

        /// <summary>
        /// 生产条码
        /// </summary>
        public string Barcode
        {
            get { return GetProperty(BarcodeProperty); }
            set { SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 批次条码 Batch
        /// <summary>
        /// 批次条码
        /// </summary>
        [Label("批次条码")]
        public static readonly Property<string> BatchProperty = P<ToStorageBarcodeDetail>.Register(e => e.Batch);

        /// <summary>
        /// 批次条码
        /// </summary>
        public string Batch
        {
            get { return GetProperty(BatchProperty); }
            set { SetProperty(BatchProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<ToStorageBarcodeDetail>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return GetProperty(FinishQtyProperty); }
            set { SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult> InspectionResultProperty = P<ToStorageBarcodeDetail>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 采集时间 CollectDate
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateTime> CollectDateProperty = P<ToStorageBarcodeDetail>.Register(e => e.CollectDate);

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectDate
        {
            get { return GetProperty(CollectDateProperty); }
            set { SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ToStorageBarcodeDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 待入库条码 ToStorageBarcode
        /// <summary>
        /// 待入库条码Id
        /// </summary>
        public static readonly IRefIdProperty ToStorageBarcodeIdProperty = P<ToStorageBarcodeDetail>.RegisterRefId(e => e.ToStorageBarcodeId, ReferenceType.Parent);

        /// <summary>
        /// 待入库条码Id
        /// </summary>
        public double ToStorageBarcodeId
        {
            get { return (double)GetRefId(ToStorageBarcodeIdProperty); }
            set { SetRefId(ToStorageBarcodeIdProperty, value); }
        }

        /// <summary>
        /// 待入库条码
        /// </summary>
        public static readonly RefEntityProperty<ToStorageBarcode> ToStorageBarcodeProperty = P<ToStorageBarcodeDetail>.RegisterRef(e => e.ToStorageBarcode, ToStorageBarcodeIdProperty);

        /// <summary>
        /// 待入库条码
        /// </summary>
        public ToStorageBarcode ToStorageBarcode
        {
            get { return GetRefEntity(ToStorageBarcodeProperty); }
            set { SetRefEntity(ToStorageBarcodeProperty, value); }
        }
        #endregion        
    }

    /// <summary>
    /// 待入库条码明细 实体配置
    /// </summary>
    internal class ToStorageBarcodeDetailConfig : EntityConfig<ToStorageBarcodeDetail>
    {
        /// <summary>
        /// 数据表Config
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_TO_STO_BAR_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}