using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("成品入库查询实体")]
    public class ProductStorageCriteria : Criteria
    {
        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty =
            P<ProductStorageCriteria>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? ShopId
        {
            get { return (double?)this.GetRefNullableId(ShopIdProperty); }
            set { this.SetRefNullableId(ShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ShopProperty =
            P<ProductStorageCriteria>.RegisterRef(e => e.Shop, ShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Shop
        {
            get { return this.GetRefEntity(ShopProperty); }
            set { this.SetRefEntity(ShopProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<ProductStorageCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<ProductStorageCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderProperty = P<ProductStorageCriteria>.Register(e => e.WorkOrder);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder
        {
            get { return this.GetProperty(WorkOrderProperty); }
            set { this.SetProperty(WorkOrderProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<ProductStorageCriteria>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState?> StateProperty = P<ProductStorageCriteria>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取报检记录列表
        /// </summary>
        /// <returns>报检记录列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProductStorageController>().GetProductStorages(this);
        }
    }
}