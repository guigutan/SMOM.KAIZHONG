using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 特殊工艺
    /// </summary>
    [ChildEntity, Serializable]
    [Label("特殊工艺")]
    public partial class SpecialProcess : DataEntity
    {
        #region 值 Value
        /// <summary>
        /// 值
        /// </summary>
        [Label("值")]
        public static readonly Property<decimal> ValueProperty = P<SpecialProcess>.Register(e => e.Value);

        /// <summary>
        /// 值
        /// </summary>
        public decimal Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 工艺 Process
        /// <summary>
        /// 工艺
        /// </summary>
        [Label("工艺")]
        public static readonly Property<Process> ProcessProperty = P<SpecialProcess>.Register(e => e.Process);

        /// <summary>
        /// 工艺
        /// </summary>
        public Process Process
        {
            get { return GetProperty(ProcessProperty); }
            set { SetProperty(ProcessProperty, value); }
        }
        #endregion

        #region 销售订单明细 SaleOrderDetail
        /// <summary>
        /// 销售订单明细Id
        /// </summary>
        public static readonly IRefIdProperty SaleOrderDetailIdProperty = P<SpecialProcess>.RegisterRefId(e => e.SaleOrderDetailId, ReferenceType.Parent);

        /// <summary>
        /// 销售订单明细Id
        /// </summary>
        public double SaleOrderDetailId
        {
            get { return (double)GetRefId(SaleOrderDetailIdProperty); }
            set { SetRefId(SaleOrderDetailIdProperty, value); }
        }

        /// <summary>
        /// 销售订单明细
        /// </summary>
        public static readonly RefEntityProperty<SaleOrderDetail> SaleOrderDetailProperty = P<SpecialProcess>.RegisterRef(e => e.SaleOrderDetail, SaleOrderDetailIdProperty);

        /// <summary>
        /// 销售订单明细
        /// </summary>
        public SaleOrderDetail SaleOrderDetail
        {
            get { return GetRefEntity(SaleOrderDetailProperty); }
            set { SetRefEntity(SaleOrderDetailProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 特殊工艺 实体配置
    /// </summary>
    internal class SpecialProcessConfig : EntityConfig<SpecialProcess>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("SALE_SPECIAL_PROCESS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}