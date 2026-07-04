using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 报检记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("首件报检查询实体")]
    public class FirstInspCriteria : Criteria
    {
        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty =
            P<FirstInspCriteria>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

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
            P<FirstInspCriteria>.RegisterRef(e => e.Shop, ShopIdProperty);

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
            P<FirstInspCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<FirstInspCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
        public static readonly Property<string> WorkOrderProperty = P<FirstInspCriteria>.Register(e => e.WorkOrder);

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
        public static readonly Property<string> BarcodeProperty = P<FirstInspCriteria>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 报检单号 InspNo
        /// <summary>
        /// 报检单号
        /// </summary>
        [Label("报检单号")]
        public static readonly Property<string> InspNoProperty = P<FirstInspCriteria>.Register(e => e.InspNo);

        /// <summary>
        /// 报检单号
        /// </summary>
        public string InspNo
        {
            get { return GetProperty(InspNoProperty); }
            set { SetProperty(InspNoProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取报检记录列表
        /// </summary>
        /// <returns>报检记录列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InspLogs.InspLogController>().GetFirstInsps(this);
        }
    }
}