using SIE.Domain;
using SIE.ObjectModel;
using SIE.ProductIntfc.InspSettings;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("报检单查询实体")]
    public class InspLogCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InspLogCriteria()
        {
            InspDate = new DateRange();
            InspDate.DateTimePart = DateTimePart.Date;
            InspDate.DateRangeType = DateRangeType.All;
        }

        #region 报检单号 InspNo
        /// <summary>
        /// 报检单号
        /// </summary>
        [Label("报检单号")]
        public static readonly Property<string> InspNoProperty = P<InspLogCriteria>.Register(e => e.InspNo);

        /// <summary>
        /// 报检单号
        /// </summary>
        public string InspNo
        {
            get { return this.GetProperty(InspNoProperty); }
            set { this.SetProperty(InspNoProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<InspLogCriteria>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 报检类型 InspType
        /// <summary>
        /// 报检类型
        /// </summary>
        [Label("报检类型")]
        public static readonly Property<InspType?> InspTypeProperty = P<InspLogCriteria>.Register(e => e.InspType);

        /// <summary>
        /// 报检类型
        /// </summary>
        public InspType? InspType
        {
            get { return this.GetProperty(InspTypeProperty); }
            set { this.SetProperty(InspTypeProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<InspLogCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<InspLogCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty =
            P<InspLogCriteria>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

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
            P<InspLogCriteria>.RegisterRef(e => e.Shop, ShopIdProperty);

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
            P<InspLogCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<InspLogCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion 

        #region 班组 WorkGroup
        /// <summary>
        /// 班组
        /// </summary>
        [Label("班组")]
        public static readonly Property<string> WorkGroupProperty = P<InspLogCriteria>.Register(e => e.WorkGroup);

        /// <summary>
        /// 班组
        /// </summary>
        public string WorkGroup
        {
            get { return this.GetProperty(WorkGroupProperty); }
            set { this.SetProperty(WorkGroupProperty, value); }
        }
        #endregion

        #region 报检人 InspUser
        /// <summary>
        /// 报检人
        /// </summary>
        [Label("报检人")]
        public static readonly Property<string> InspUserProperty = P<InspLogCriteria>.Register(e => e.InspUser);

        /// <summary>
        /// 报检人
        /// </summary>
        public string InspUser
        {
            get { return this.GetProperty(InspUserProperty); }
            set { this.SetProperty(InspUserProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<InspLogCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 报检时间 InspDate
        /// <summary>
        /// 报检时间
        /// </summary>
        [Label("报检时间")]
        public static readonly Property<DateRange> InspDateProperty = P<InspLogCriteria>.Register(e => e.InspDate);

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateRange InspDate
        {
            get { return this.GetProperty(InspDateProperty); }
            set { this.SetProperty(InspDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取报检单
        /// </summary>
        /// <returns>报检单列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InspLogController>().GetInspLogs(this);
        }
    }
}