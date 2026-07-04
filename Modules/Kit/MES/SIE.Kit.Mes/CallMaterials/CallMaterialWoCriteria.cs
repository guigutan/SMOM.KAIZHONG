using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料工单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("叫料工单查询实体")]
    public partial class CallMaterialWoCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CallMaterialWoCriteria()
        {
            Resource = RT.Service.Resolve<CallMaterialController>().GetDefaultResource();
        }

        #region No 工单号
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<CallMaterialWoCriteria>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<CallMaterialWoCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<CallMaterialWoCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState?> WorkOrderStateProperty = P<CallMaterialWoCriteria>.Register(e => e.WorkOrderState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState? WorkOrderState
        {
            get { return GetProperty(WorkOrderStateProperty); }
            set { SetProperty(WorkOrderStateProperty, value); }
        }
        #endregion

        #region 当前资源 Resource
        /// <summary>
        /// 当前资源Id
        /// </summary>
        [Label("当前资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<CallMaterialWoCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 当前资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 当前资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<CallMaterialWoCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 当前资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region ResourceName

        /// <summary>
        /// 当前资源
        /// </summary>
        [Label("当前资源")]
        public static readonly Property<string> ResourceNameProperty = P<CallMaterialWoCriteria>.Register(e => e.ResourceName);

        /// <summary>
        /// 当前资源
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 叫料单集合
        /// </summary>
        /// <returns>叫料单</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CallMaterialController>().GetCallMaterialWo(this);
        }
    }
}
