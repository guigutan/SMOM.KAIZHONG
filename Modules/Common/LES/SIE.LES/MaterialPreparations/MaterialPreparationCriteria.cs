using SIE.Domain;
using SIE.LES.MaterialPreparations.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备料需求单查询实体")]
    public class MaterialPreparationCriteria : Criteria
    {
        #region 备料单号 No
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> NoProperty = P<MaterialPreparationCriteria>.Register(e => e.No);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 工单 Wo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoProperty = P<MaterialPreparationCriteria>.Register(e => e.Wo);

        /// <summary>
        /// 工单
        /// </summary>
        public string Wo
        {
            get { return this.GetProperty(WoProperty); }
            set { this.SetProperty(WoProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<MaterialPreparationCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<MaterialPreparationCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<MaterialPreparationCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<MaterialPreparationCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 状态 PrepareStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<PrepareStatus?> PrepareStatusProperty = P<MaterialPreparationCriteria>.Register(e => e.PrepareStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public PrepareStatus? PrepareStatus
        {
            get { return this.GetProperty(PrepareStatusProperty); }
            set { this.SetProperty(PrepareStatusProperty, value); }
        }
        #endregion

        #region 备料类型 PrepareType
        /// <summary>
        /// 备料类型
        /// </summary>
        [Label("备料类型")]
        public static readonly Property<PrepareType?> PrepareTypeProperty = P<MaterialPreparationCriteria>.Register(e => e.PrepareType);

        /// <summary>
        /// 备料类型
        /// </summary>
        public PrepareType? PrepareType
        {
            get { return this.GetProperty(PrepareTypeProperty); }
            set { this.SetProperty(PrepareTypeProperty, value); }
        }
        #endregion

        #region 备料原因 PrepareReason
        /// <summary>
        /// 备料原因
        /// </summary>
        [Label("备料原因")]
        public static readonly Property<string> PrepareReasonProperty = P<MaterialPreparationCriteria>.Register(e => e.PrepareReason);

        /// <summary>
        /// 备料原因
        /// </summary>
        public string PrepareReason
        {
            get { return this.GetProperty(PrepareReasonProperty); }
            set { this.SetProperty(PrepareReasonProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<MaterialPreparationCriteria>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return this.GetProperty(SaleOrderNoProperty); }
            set { this.SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 客户订单号 CustomerOrderNo
        /// <summary>
        /// 客户订单号
        /// </summary>
        [Label("客户订单号")]
        public static readonly Property<string> CustomerOrderNoProperty = P<MaterialPreparationCriteria>.Register(e => e.CustomerOrderNo);

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerOrderNo
        {
            get { return this.GetProperty(CustomerOrderNoProperty); }
            set { this.SetProperty(CustomerOrderNoProperty, value); }
        }
        #endregion

        #region 发运单号 ShippingOrderNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> ShippingOrderNoProperty = P<MaterialPreparationCriteria>.Register(e => e.ShippingOrderNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippingOrderNo
        {
            get { return this.GetProperty(ShippingOrderNoProperty); }
            set { this.SetProperty(ShippingOrderNoProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<MaterialPreparationCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaterialPreparationController>().MaterialPreparationQuery(this);
        }
    }
}
