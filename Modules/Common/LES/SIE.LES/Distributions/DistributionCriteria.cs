using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.Distributions
{
    /// <summary>
    /// 配送单管理查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("配送单管理查询实体")]
    public class DistributionCriteria : Criteria
    {
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<DistributionCriteria>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 订单状态 OrderState
        /// <summary>
        /// 订单状态
        /// </summary>
        [Label("订单状态")]
        public static readonly Property<string> OrderStateProperty = P<DistributionCriteria>.Register(e => e.OrderState);

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderState
        {
            get { return this.GetProperty(OrderStateProperty); }
            set { this.SetProperty(OrderStateProperty, value); }
        }
        #endregion

        #region 来源订单号 SourceNo
        /// <summary>
        /// 来源订单号
        /// </summary>
        [Label("来源订单号")]
        public static readonly Property<string> SourceNoProperty = P<DistributionCriteria>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源订单号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 容器编码 Lpn
        /// <summary>
        /// 容器编码
        /// </summary>
        [Label("容器编码")]
        public static readonly Property<string> LpnProperty = P<DistributionCriteria>.Register(e => e.Lpn);

        /// <summary>
        /// 容器编码
        /// </summary>
        public string Lpn
        {
            get { return this.GetProperty(LpnProperty); }
            set { this.SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 发货仓库 Warehouse
        /// <summary>
        /// 发货仓库Id
        /// </summary>
        [Label("发货仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<DistributionCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发货仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<DistributionCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发货仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 物料 ItemCode
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly Property<string> ItemCodeProperty = P<DistributionCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<DistributionCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 目标产线 ProductLine
        /// <summary>
        /// 目标产线Id
        /// </summary>
        [Label("目标产线")]
        public static readonly IRefIdProperty ProductLineIdProperty =
            P<DistributionCriteria>.RegisterRefId(e => e.ProductLineId, ReferenceType.Normal);

        /// <summary>
        /// 目标产线Id
        /// </summary>
        public double? ProductLineId
        {
            get { return (double?)this.GetRefNullableId(ProductLineIdProperty); }
            set { this.SetRefNullableId(ProductLineIdProperty, value); }
        }

        /// <summary>
        /// 目标产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ProductLineProperty =
            P<DistributionCriteria>.RegisterRef(e => e.ProductLine, ProductLineIdProperty);

        /// <summary>
        /// 目标产线
        /// </summary>
        public WipResource ProductLine
        {
            get { return this.GetRefEntity(ProductLineProperty); }
            set { this.SetRefEntity(ProductLineProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DistributionController>().GetDistributions(this);
        }
    }
}
