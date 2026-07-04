using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SuspectProductLabels.ViewModels
{
    /// <summary>
    /// 可疑品处理ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("可疑品处理ViewModel")]
    public class ProcessingViewModel : ViewModel
    {
        #region 可疑品标签Id SuspectProductLabelId
        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        [Label("可疑品标签Id")]
        public static readonly Property<double> SuspectProductLabelIdProperty = P<ProcessingViewModel>.Register(e => e.SuspectProductLabelId);

        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        public double SuspectProductLabelId
        {
            get { return this.GetProperty(SuspectProductLabelIdProperty); }
            set { this.SetProperty(SuspectProductLabelIdProperty, value); }
        }
        #endregion

        #region 可疑品标签 SuspectProductLabel
        /// <summary>
        /// 可疑品标签
        /// </summary>
        [Label("可疑品标签")]
        public static readonly Property<string> SuspectProductLabelProperty = P<ProcessingViewModel>.Register(e => e.SuspectProductLabel);

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public string SuspectProductLabel
        {
            get { return this.GetProperty(SuspectProductLabelProperty); }
            set { this.SetProperty(SuspectProductLabelProperty, value); }
        }
        #endregion

        #region 物料描述 ItemDesc
        /// <summary>
        /// 物料描述
        /// </summary>
        [Label("物料描述")]
        public static readonly Property<string> ItemDescProperty = P<ProcessingViewModel>.Register(e => e.ItemDesc);

        /// <summary>
        /// 物料描述
        /// </summary>
        public string ItemDesc
        {
            get { return this.GetProperty(ItemDescProperty); }
            set { this.SetProperty(ItemDescProperty, value); }
        }
        #endregion

        #region 可疑品数量 Qty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> QtyProperty = P<ProcessingViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 良品数量 GoodQty
        /// <summary>
        /// 良品数量
        /// </summary>
        [Label("良品数量")]
        public static readonly Property<decimal?> GoodQtyProperty = P<ProcessingViewModel>.Register(e => e.GoodQty);

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal? GoodQty
        {
            get { return this.GetProperty(GoodQtyProperty); }
            set { this.SetProperty(GoodQtyProperty, value); }
        }
        #endregion

    }
}
