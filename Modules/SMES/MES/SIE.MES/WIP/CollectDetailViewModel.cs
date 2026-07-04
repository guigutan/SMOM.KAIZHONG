using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Specialized;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 采集明细
    /// </summary>
    [RootEntity, Serializable]
    public class CollectDetailViewModel : ViewModel
    {
        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<CollectDetailViewModel>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<CollectDetailViewModel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 条码类型 BarcodeType
        /// <summary>
        /// 条码类型
        /// </summary>
        [Label("条码类型")]
        public static readonly Property<BarcodeType> BarcodeTypeProperty = P<CollectDetailViewModel>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType
        {
            get { return this.GetProperty(BarcodeTypeProperty); }
            set { this.SetProperty(BarcodeTypeProperty, value); }
        }
        #endregion

        #region 采集结果 Result
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        public static readonly Property<ResultType> ResultProperty = P<CollectDetailViewModel>.Register(e => e.Result);

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result
        {
            get { return this.GetProperty(ResultProperty); }
            set { this.SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 采集时间 CollectDate
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateTime> CollectDateProperty = P<CollectDetailViewModel>.Register(e => e.CollectDate);

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectDate
        {
            get { return this.GetProperty(CollectDateProperty); }
            set { this.SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 入站时间 InputDate
        /// <summary>
        /// 入站时间
        /// </summary>
        [Label("入站时间")]
        public static readonly Property<DateTime> InputDateProperty = P<CollectDetailViewModel>.Register(e => e.InputDate);

        /// <summary>
        /// 入站时间
        /// </summary>
        public DateTime InputDate
        {
            get { return this.GetProperty(InputDateProperty); }
            set { this.SetProperty(InputDateProperty, value); }
        }
        #endregion

        #region 出站时间 OutputDate
        /// <summary>
        /// 出站时间
        /// </summary>
        [Label("出站时间")]
        public static readonly Property<DateTime?> OutputDateProperty = P<CollectDetailViewModel>.Register(e => e.OutputDate);

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? OutputDate
        {
            get { return this.GetProperty(OutputDateProperty); }
            set { this.SetProperty(OutputDateProperty, value); }
        }
        #endregion

        #region 子批次号 SubBatchNo
        /// <summary>
        /// 子批次号
        /// </summary>
        [Label("子批次号")]
        public static readonly Property<string> ChildBarcodeProperty = P<CollectDetailViewModel>.Register(e => e.SubBatchNo);

        /// <summary>
        /// 子批次号
        /// </summary>
        public string SubBatchNo
        {
            get { return this.GetProperty(ChildBarcodeProperty); }
            set { this.SetProperty(ChildBarcodeProperty, value); }
        }
        #endregion

        #region 载具号 ContainerNo
        /// <summary>
        /// 载具号
        /// </summary>
        [Label("载具号")]
        public static readonly Property<string> ContainerNoProperty = P<CollectDetailViewModel>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion 

        #region 出入类型 PlugType
        /// <summary>
        /// 出入类型
        /// </summary>
        [Label("出入类型")]
        public static readonly Property<PlugType?> PlugTypeProperty = P<CollectDetailViewModel>.Register(e => e.PlugType);

        /// <summary>
        /// 出入类型
        /// </summary>
        public PlugType? PlugType
        {
            get { return this.GetProperty(PlugTypeProperty); }
            set { this.SetProperty(PlugTypeProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<CollectDetailViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal?> RemainQtyProperty = P<CollectDetailViewModel>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal? RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 报废总数 ScrapQty
        /// <summary>
        /// 报废总数
        /// </summary>
        [Label("报废总数")]
        public static readonly Property<decimal> ScrapQtyProperty = P<CollectDetailViewModel>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废总数
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 批次状态 BatchState
        /// <summary>
        /// 批次状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<BatchState> BatchStateProperty = P<CollectDetailViewModel>.Register(e => e.BatchState);

        /// <summary>
        /// 批次状态
        /// </summary>
        public BatchState BatchState
        {
            get { return this.GetProperty(BatchStateProperty); }
            set { this.SetProperty(BatchStateProperty, value); }
        }
        #endregion

        #region 拆分数量 SplitQty
        /// <summary>
        /// 拆分数量
        /// </summary>
        [Label("拆分数量")]
        public static readonly Property<decimal> SplitQtyProperty = P<CollectDetailViewModel>.Register(e => e.SplitQty);

        /// <summary>
        /// 拆分数量
        /// </summary>
        public decimal SplitQty
        {
            get { return this.GetProperty(SplitQtyProperty); }
            set { this.SetProperty(SplitQtyProperty, value); }
        }
        #endregion

        #region 操作时间 OptTme
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OptTmeProperty = P<CollectDetailViewModel>.Register(e => e.OptTme);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OptTme
        {
            get { return this.GetProperty(OptTmeProperty); }
            set { this.SetProperty(OptTmeProperty, value); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> NgQtyProperty = P<CollectDetailViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 采集明细集合
    /// </summary>
    public class CollectDetailViewModelList : EntityList<CollectDetailViewModel>
    {
        /// <summary>
        /// 集合变更通知
        /// </summary>
        /// <param name="e">集合变更参数</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

        /// <summary>
        /// 插入采集明细
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="item">采集明细对象</param>
        protected override void InsertItem(int index, object item)
        {
            base.InsertItem(0, item);
            if (Count > 20)
                RemoveAt(Count - 1);
        }
    }
}