using SIE.Domain;
using SIE.MES.BatchWIP.Products.SplitAndMerge.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BatchWIP.Products.SplitAndMerge
{
    /// <summary>
    /// 批次采集拆分记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次采集拆分记录")]
    public class BatchWipSplitViewModel : ViewModel
    {
        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipSplitViewModel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 批次来源 BatchSource
        /// <summary>
        /// 批次来源
        /// </summary>
        [Label("批次来源")]
        public static readonly Property<BatchSource?> BatchSourceProperty = P<BatchWipSplitViewModel>.Register(e => e.BatchSource);

        /// <summary>
        /// 批次来源
        /// </summary>
        public BatchSource? BatchSource
        {
            get { return this.GetProperty(BatchSourceProperty); }
            set { this.SetProperty(BatchSourceProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchWipSplitViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 是否不良工序 IsDefect
        /// <summary>
        /// 是否不良工序
        /// </summary>
        [Label("是否不良工序")]
        public static readonly Property<YesNo> IsDefectProperty = P<BatchWipSplitViewModel>.Register(e => e.IsDefect);

        /// <summary>
        /// 是否不良工序
        /// </summary>
        public YesNo IsDefect
        {
            get { return this.GetProperty(IsDefectProperty); }
            set { this.SetProperty(IsDefectProperty, value); }
        }
        #endregion

    }
}
