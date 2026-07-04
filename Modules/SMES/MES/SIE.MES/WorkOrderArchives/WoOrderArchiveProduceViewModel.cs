using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案工单产出
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单制造档案工单产出")]
    public class WoOrderArchiveProduceViewModel : ViewModel
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<WoOrderArchiveProduceViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<WoOrderArchiveProduceViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 条码 BarCode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarCodeProperty = P<WoOrderArchiveProduceViewModel>.Register(e => e.BarCode);

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode
        {
            get { return this.GetProperty(BarCodeProperty); }
            set { this.SetProperty(BarCodeProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<WoOrderArchiveProduceViewModel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<WoOrderArchiveProduceViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 报废数量 DrawQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> DrawQtyProperty = P<WoOrderArchiveProduceViewModel>.Register(e => e.DrawQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal DrawQty
        {
            get { return this.GetProperty(DrawQtyProperty); }
            set { this.SetProperty(DrawQtyProperty, value); }
        }
        #endregion

        #region 是否已完工下线 IsOver
        /// <summary>
        /// 是否已完工下线
        /// </summary>
        [Label("是否已完工下线")]
        public static readonly Property<bool> IsOverProperty = P<WoOrderArchiveProduceViewModel>.Register(e => e.IsOver);

        /// <summary>
        /// 是否已完工下线
        /// </summary>
        public bool IsOver
        {
            get { return this.GetProperty(IsOverProperty); }
            set { this.SetProperty(IsOverProperty, value); }
        }
        #endregion

        #region 产品编码 ProCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProCodeProperty = P<WoOrderArchiveProduceViewModel>.RegisterView(e => e.ProCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProCode
        {
            get { return this.GetProperty(ProCodeProperty); }
            set { this.SetProperty(ProCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProNameProperty = P<WoOrderArchiveProduceViewModel>.RegisterView(e => e.ProName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
            set { this.SetProperty(ProNameProperty, value); }
        }
        #endregion
    }
}
