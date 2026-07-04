using SIE.Core.WorkOrders;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.InspSettings;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.ProductIntfc.InspRecords
{
    /// <summary>
    /// 报检记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("报检记录")]
    public partial class InspRecord : DataEntity
    {
        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<InspRecord>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 报检数量 InspectionQty
        /// <summary>
        /// 报检数量
        /// </summary>
        [Label("报检数量")]
        public static readonly Property<decimal> InspectionQtyProperty = P<InspRecord>.Register(e => e.InspectionQty);

        /// <summary>
        /// 报检数量
        /// </summary>
        public decimal InspectionQty
        {
            get { return GetProperty(InspectionQtyProperty); }
            set { SetProperty(InspectionQtyProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<InspRecord>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty =
            P<InspRecord>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)this.GetRefNullableId(CustomerIdProperty); }
            set { this.SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty =
            P<InspRecord>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return this.GetRefEntity(CustomerProperty); }
            set { this.SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 报检参数 InspParameter
        /// <summary>
        /// 报检参数Id
        /// </summary>
        [Label("报检参数")]
        public static readonly IRefIdProperty InspParameterIdProperty = P<InspRecord>.RegisterRefId(e => e.InspParameterId, ReferenceType.Normal);

        /// <summary>
        /// 报检参数Id
        /// </summary>
        [Label("报检参数")]
        public double InspParameterId
        {
            get { return (double)GetRefId(InspParameterIdProperty); }
            set { SetRefId(InspParameterIdProperty, value); }
        }

        /// <summary>
        /// 报检参数
        /// </summary>
        public static readonly RefEntityProperty<InspParameter> InspParameterProperty = P<InspRecord>.RegisterRef(e => e.InspParameter, InspParameterIdProperty);

        /// <summary>
        /// 报检参数
        /// </summary>
        public InspParameter InspParameter
        {
            get { return GetRefEntity(InspParameterProperty); }
            set { SetRefEntity(InspParameterProperty, value); }
        }
        #endregion

        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty = P<InspRecord>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public double ShopId
        {
            get { return (double)GetRefId(ShopIdProperty); }
            set { SetRefId(ShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ShopProperty = P<InspRecord>.RegisterRef(e => e.Shop, ShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Shop
        {
            get { return GetRefEntity(ShopProperty); }
            set { SetRefEntity(ShopProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<InspRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        [NotDuplicate]
        [Label("工单")]
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<InspRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<InspRecord>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 产品编码 WorkOrderProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> WorkOrderProductCodeProperty = P<InspRecord>.RegisterView(e => e.WorkOrderProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string WorkOrderProductCode
        {
            get { return this.GetProperty(WorkOrderProductCodeProperty); }
        }
        #endregion

        #region 产品名称 WorkOrderProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> WorkOrderProductNameProperty = P<InspRecord>.RegisterView(e => e.WorkOrderProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string WorkOrderProductName
        {
            get { return this.GetProperty(WorkOrderProductNameProperty); }
        }
        #endregion

        #region 工单类型 WorkOrderType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> WorkOrderTypeProperty = P<InspRecord>.RegisterView(e => e.WorkOrderType, p => p.WorkOrder.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType WorkOrderType
        {
            get { return this.GetProperty(WorkOrderTypeProperty); }
        }
        #endregion

        #region 计划数量 WorkOrderPlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> WorkOrderPlanQtyProperty = P<InspRecord>.RegisterView(e => e.WorkOrderPlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal WorkOrderPlanQty
        {
            get { return this.GetProperty(WorkOrderPlanQtyProperty); }
        }
        #endregion

        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> WorkOrderStateProperty = P<InspRecord>.RegisterView(e => e.WorkOrderState, p => p.WorkOrder.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState WorkOrderState
        {
            get { return this.GetProperty(WorkOrderStateProperty); }
        }
        #endregion

        #region 报检类型 InspType
        /// <summary>
        /// 报检类型
        /// </summary>
        [Label("报检类型")]
        public static readonly Property<InspType> InspTypeProperty = P<InspRecord>.Register(e => e.InspType);

        /// <summary>
        /// 报检类型
        /// </summary>
        public InspType InspType
        {
            get { return GetProperty(InspTypeProperty); }
            set { SetProperty(InspTypeProperty, value); }
        }
        #endregion

        #region 报检条码列表 InspBarcodeList
        /// <summary>
        /// 报检条码列表
        /// </summary>
        public static readonly ListProperty<EntityList<InspBarcode>> InspBarcodeListProperty = P<InspRecord>.RegisterList(e => e.InspBarcodeList);

        /// <summary>
		/// 报检条码列表
		/// </summary>
		public EntityList<InspBarcode> InspBarcodeList
        {
            get { return this.GetLazyList(InspBarcodeListProperty); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<InspRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<InspRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region BS
        #region 车间名称 ShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> ShopNameProperty = P<InspRecord>.RegisterView(e => e.ShopName, e => e.Shop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string ShopName
        {
            get { return GetProperty(ShopNameProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<InspRecord>.RegisterView(e => e.ResourceName, e => e.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 报检记录 实体配置
    /// </summary>
    internal class InspRecordConfig : EntityConfig<InspRecord>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_INSP_RECORD").MapAllProperties();
        }
    }
}