using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Outsourcing.Model
{
    /// <summary>
    ///添加模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("添加模型")]
    [DisplayMember(nameof(Product))]
    public class AddModel : ViewModel
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<AddModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<AddModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 产品名称 Product
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductProperty = P<AddModel>.RegisterView(e => e.Product, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Product
        {
            get { return this.GetProperty(ProductProperty); }
            set { this.SetProperty(ProductProperty, value); }
        }
        #endregion

        #region 委外需求数量 Qty
        /// <summary>
        /// 委外需求数量
        /// </summary>
        [Label("委外需求数量")]
        public static readonly Property<decimal> QtyProperty = P<AddModel>.Register(e => e.Qty);

        /// <summary>
        /// 委外需求数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty =
            P<AddModel>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)this.GetRefNullableId(SupplierIdProperty); }
            set { this.SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty =
            P<AddModel>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return this.GetRefEntity(SupplierProperty); }
            set { this.SetRefEntity(SupplierProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 临时添加委外
    /// </summary>
    [RootEntity, Serializable]
    [Label("添加模型")]
    [DisplayMember(nameof(Product))]
    public class TemporaryAddModel : AddModel
    {

        #region 开始工序 BeginRoutingProcess
        /// <summary>
        /// 开始工序Id
        /// </summary>
        [Label("开始工序")]
        public static readonly IRefIdProperty BeginRoutingProcessIdProperty =
            P<TemporaryAddModel>.RegisterRefId(e => e.BeginRoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 开始工序Id
        /// </summary>
        public double BeginRoutingProcessId
        {
            get { return (double)this.GetRefId(BeginRoutingProcessIdProperty); }
            set { this.SetRefId(BeginRoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 开始工序
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderRoutingProcess> BeginRoutingProcessProperty =
            P<TemporaryAddModel>.RegisterRef(e => e.BeginRoutingProcess, BeginRoutingProcessIdProperty);

        /// <summary>
        /// 开始工序
        /// </summary>
        public WorkOrderRoutingProcess BeginRoutingProcess
        {
            get { return this.GetRefEntity(BeginRoutingProcessProperty); }
            set { this.SetRefEntity(BeginRoutingProcessProperty, value); }
        }
        #endregion



        #region 结束工序 EndRoutingProcess
        /// <summary>
        /// 结束工序Id
        /// </summary>
        [Label("结束工序")]
        public static readonly IRefIdProperty EndRoutingProcessIdProperty =
            P<TemporaryAddModel>.RegisterRefId(e => e.EndRoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 结束工序Id
        /// </summary>
        public double EndRoutingProcessId
        {
            get { return (double)this.GetRefId(EndRoutingProcessIdProperty); }
            set { this.SetRefId(EndRoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 结束工序
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderRoutingProcess> EndRoutingProcessProperty =
            P<TemporaryAddModel>.RegisterRef(e => e.EndRoutingProcess, EndRoutingProcessIdProperty);

        /// <summary>
        /// 结束工序
        /// </summary>
        public WorkOrderRoutingProcess EndRoutingProcess
        {
            get { return this.GetRefEntity(EndRoutingProcessProperty); }
            set { this.SetRefEntity(EndRoutingProcessProperty, value); }
        }
        #endregion


    }
}
