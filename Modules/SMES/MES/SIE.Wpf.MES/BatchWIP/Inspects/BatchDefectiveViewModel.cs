using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 批次检验不良记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次检验不良记录")]
    public class BatchDefectiveViewModel : ViewModel
    {

        #region 生产批次 Barcode
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> BarcodeProperty = P<BatchDefectiveViewModel>.Register(e => e.Barcode);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 子批次号 ChildBarcode
        /// <summary>
        /// 子批次号
        /// </summary>
        [Label("子批次号")]
        public static readonly Property<string> ChildBarcodeProperty = P<BatchDefectiveViewModel>.Register(e => e.ChildBarcode);

        /// <summary>
        /// 子批次号
        /// </summary>
        public string ChildBarcode
        {
            get { return this.GetProperty(ChildBarcodeProperty); }
            set { this.SetProperty(ChildBarcodeProperty, value); }
        }
        #endregion

        #region 缺陷代码串 Defects
        /// <summary>
        /// 缺陷代码串
        /// </summary>
        [Label("缺陷代码串")]
        public static readonly Property<string> DefectsProperty = P<BatchDefectiveViewModel>.Register(e => e.Defects);

        /// <summary>
        /// 缺陷代码串
        /// </summary>
        public string Defects
        {
            get { return this.GetProperty(DefectsProperty); }
            set { this.SetProperty(DefectsProperty, value); }
        }
        #endregion

        #region 缺陷代码描述串 Descriptions
        /// <summary>
        /// 缺陷代码描述串
        /// </summary>
        [Label("缺陷代码描述串")]
        public static readonly Property<string> DescriptionsProperty = P<BatchDefectiveViewModel>.Register(e => e.Descriptions);

        /// <summary>
        /// 缺陷代码描述串
        /// </summary>
        public string Descriptions
        {
            get { return this.GetProperty(DescriptionsProperty); }
            set { this.SetProperty(DescriptionsProperty, value); }
        }
        #endregion


        #region 产品缺陷记录明细列表
        /// <summary>
        /// 产品缺陷记录明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductDefectDetail>> BatchWipPrdDrtDetailsProperty = P<BatchDefectiveViewModel>.RegisterList(e => e.BatchWipPrdDrtDetails, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<BatchWipProductDefectDetail>()
        });

        /// <summary>
        /// 产品缺陷记录明细列表
        /// </summary>
        public EntityList<BatchWipProductDefectDetail> BatchWipPrdDrtDetails
        {
            get { return this.GetLazyList(BatchWipPrdDrtDetailsProperty); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> NgQtyProperty = P<BatchDefectiveViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 检验时间 InspectDate
        /// <summary>
        /// 检验时间
        /// </summary>
        [Label("检验时间")]
        public static readonly Property<DateTime> InspectDateProperty = P<BatchDefectiveViewModel>.Register(e => e.InspectDate);

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime InspectDate
        {
            get { return this.GetProperty(InspectDateProperty); }
            set { this.SetProperty(InspectDateProperty, value); }
        }
        #endregion

        #region 批次检验不良集合 BatchDefectiveSetViewModel
        /// <summary>
        /// 批次检验不良集合Id
        /// </summary>
        [Label("批次检验不良集合")]
        public static readonly IRefIdProperty BatchDefectiveSetViewModelIdProperty =
            P<BatchDefectiveViewModel>.RegisterRefId(e => e.BatchDefectiveSetViewModelId, ReferenceType.Normal);

        /// <summary>
        /// 批次检验不良集合Id
        /// </summary>
        public string BatchDefectiveSetViewModelId
        {
            get { return (string)this.GetRefId(BatchDefectiveSetViewModelIdProperty); }
            set { this.SetRefId(BatchDefectiveSetViewModelIdProperty, value); }
        }

        /// <summary>
        /// 批次检验不良集合
        /// </summary>
        public static readonly RefEntityProperty<BatchDefectiveSetViewModel> BatchDefectiveSetViewModelProperty =
            P<BatchDefectiveViewModel>.RegisterRef(e => e.BatchDefectiveSetViewModel, BatchDefectiveSetViewModelIdProperty);

        /// <summary>
        /// 批次检验不良集合
        /// </summary>
        public BatchDefectiveSetViewModel BatchDefectiveSetViewModel
        {
            get { return this.GetRefEntity(BatchDefectiveSetViewModelProperty); }
            set { this.SetRefEntity(BatchDefectiveSetViewModelProperty, value); }
        }
        #endregion
    }
}
