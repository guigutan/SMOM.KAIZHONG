using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 拆分任务ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("拆分任务")]
    public class SplitTaskViewModel : ViewModel
    {
        #region 拆分数量 Qty
        /// <summary>
        /// 拆分数量
        /// </summary>
        [Label("拆分数量")]
        public static readonly Property<decimal> QtyProperty = P<SplitTaskViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 拆分数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 任务数量 DispatchQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> DispatchQtyProperty = P<SplitTaskViewModel>.Register(e => e.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return GetProperty(DispatchQtyProperty); }
            set { SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 已报工数量 ReportQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<SplitTaskViewModel>.Register(e => e.ReportQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return GetProperty(ReportQtyProperty); }
            set { SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> NgQtyProperty = P<SplitTaskViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 派工任务 Qty
        /// <summary>
        /// 派工任务
        /// </summary>
        [Label("派工任务")]
        public static readonly Property<double> DispatchTaskIdProperty = P<SplitTaskViewModel>.Register(e => e.DispatchTaskId);

        /// <summary>
        /// 派工任务
        /// </summary>
        public double DispatchTaskId
        {
            get { return GetProperty(DispatchTaskIdProperty); }
            set { SetProperty(DispatchTaskIdProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> SuspectQtyProperty = P<SplitTaskViewModel>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 生产资源 WipResource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<SplitTaskViewModel>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double WipResourceId
        {
            get { return (double)this.GetRefId(WipResourceIdProperty); }
            set { this.SetRefId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<SplitTaskViewModel>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

    }
}
