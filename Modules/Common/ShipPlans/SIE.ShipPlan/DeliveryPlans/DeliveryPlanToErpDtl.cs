using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 每次发货到ERP明细记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("每次发货到ERP明细记录")]
    public class DeliveryPlanToErpDtl : DataEntity
    {
        #region 发货计划 DeliveryPlan
        /// <summary>
        /// 发货计划Id
        /// </summary>
        [Label("发货计划")]
        public static readonly IRefIdProperty DeliveryPlanIdProperty =
            P<DeliveryPlanToErpDtl>.RegisterRefId(e => e.DeliveryPlanId, ReferenceType.Normal);

        /// <summary>
        /// 发货计划Id
        /// </summary>
        public double DeliveryPlanId
        {
            get { return (double)this.GetRefId(DeliveryPlanIdProperty); }
            set { this.SetRefId(DeliveryPlanIdProperty, value); }
        }

        /// <summary>
        /// 发货计划
        /// </summary>
        public static readonly RefEntityProperty<DeliveryPlan> DeliveryPlanProperty =
            P<DeliveryPlanToErpDtl>.RegisterRef(e => e.DeliveryPlan, DeliveryPlanIdProperty);

        /// <summary>
        /// 发货计划
        /// </summary>
        public DeliveryPlan DeliveryPlan
        {
            get { return this.GetRefEntity(DeliveryPlanProperty); }
            set { this.SetRefEntity(DeliveryPlanProperty, value); }
        }
        #endregion

        #region ERP单据Id ErpOrderId
        /// <summary>
        /// ERP单据Id
        /// </summary>
        [Label("ERP单据Id")]
        public static readonly Property<double> ErpOrderIdProperty = P<DeliveryPlanToErpDtl>.Register(e => e.ErpOrderId);

        /// <summary>
        /// ERP单据Id
        /// </summary>
        public double ErpOrderId
        {
            get { return this.GetProperty(ErpOrderIdProperty); }
            set { this.SetProperty(ErpOrderIdProperty, value); }
        }
        #endregion

        #region ERP明细Id或主键值 ErpDetailId
        /// <summary>
        /// ERP明细Id或主键值
        /// </summary>
        [Label("ERP明细Id或主键值")]
        public static readonly Property<string> ErpDetailIdProperty = P<DeliveryPlanToErpDtl>.Register(e => e.ErpDetailId);

        /// <summary>
        /// ERP明细Id或主键值
        /// </summary>
        public string ErpDetailId
        {
            get { return this.GetProperty(ErpDetailIdProperty); }
            set { this.SetProperty(ErpDetailIdProperty, value); }
        }
        #endregion

        #region 发货数量 ShippingQty
        /// <summary>
        /// 发货数量
        /// </summary>
        [Label("属性名")]
        public static readonly Property<decimal> ShippingQtyProperty = P<DeliveryPlanToErpDtl>.Register(e => e.ShippingQty);

        /// <summary>
        /// 发货数量
        /// </summary>
        public decimal ShippingQty
        {
            get { return this.GetProperty(ShippingQtyProperty); }
            set { this.SetProperty(ShippingQtyProperty, value); }
        }
        #endregion

        #region 发货时间 ShippingDate
        /// <summary>
        /// 发货时间
        /// </summary>
        [Label("发货时间")]
        public static readonly Property<DateTime> ShippingDateProperty = P<DeliveryPlanToErpDtl>.Register(e => e.ShippingDate);

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime ShippingDate
        {
            get { return this.GetProperty(ShippingDateProperty); }
            set { this.SetProperty(ShippingDateProperty, value); }
        }
        #endregion

        #region 是否已传ERP IsUpload
        /// <summary>
        /// 是否已传ERP
        /// </summary>
        [Label("是否已传ERP")]
        public static readonly Property<bool> IsUploadProperty = P<DeliveryPlanToErpDtl>.Register(e => e.IsUpload);

        /// <summary>
        /// 是否已传ERP
        /// </summary>
        public bool IsUpload
        {
            get { return this.GetProperty(IsUploadProperty); }
            set { this.SetProperty(IsUploadProperty, value); }
        }
        #endregion

    }
    /// <summary>
    /// 发货计划 实体配置
    /// </summary>
    internal class DeliveryPlanToErpDtlConfig : EntityConfig<DeliveryPlanToErpDtl>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DELIVERY_PLAN_TO_ERP_DTL").MapAllProperties();           
            Meta.EnablePhantoms();
        }
    }

}
