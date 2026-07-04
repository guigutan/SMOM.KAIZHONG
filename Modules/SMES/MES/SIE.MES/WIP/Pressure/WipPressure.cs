using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.ItemEquipAccount;
using SIE.MES.WIP.Pressure.Configs;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压测试批次数据
    /// </summary>
    [RootEntity, Serializable]
    [Label("耐压测试批次数据")]
    [ConditionQueryType(typeof(WipPressureCriteria))]
    [EntityWithConfig(typeof(WipPressureVerifyCodeConfig))]
    [DisplayMember(nameof(BatchNo))]
    public partial class WipPressure : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipPressure()
        {
        }

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Required]
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<WipPressure>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WipPressure>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WipPressure>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<WipPressure>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<WipPressure>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<WipPressure>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<WipPressure>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<WipPressure>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 原始数量 OriginalQty
        /// <summary>
        /// 原始数量
        /// </summary>
        [Label("原始数量")]
        public static readonly Property<decimal> OriginalQtyProperty = P<WipPressure>.Register(e => e.OriginalQty);

        /// <summary>
        /// 原始数量
        /// </summary>
        public decimal OriginalQty
        {
            get { return this.GetProperty(OriginalQtyProperty); }
            set { this.SetProperty(OriginalQtyProperty, value); }
        }
        #endregion

        #region 是否允许超打 IsAllowOver
        /// <summary>
        /// 是否允许超打
        /// </summary>
        [Label("是否允许超打")]
        public static readonly Property<bool> IsAllowOverProperty = P<WipPressure>.Register(e => e.IsAllowOver);

        /// <summary>
        /// 是否允许超打
        /// </summary>
        public bool IsAllowOver
        {
            get { return this.GetProperty(IsAllowOverProperty); }
            set { this.SetProperty(IsAllowOverProperty, value); }
        }
        #endregion

        #region 测试开始时间 BeginTime
        /// <summary>
        /// 测试开始时间
        /// </summary>
        [Label("测试开始时间")]
        public static readonly Property<DateTime?> BeginTimeProperty = P<WipPressure>.Register(e => e.BeginTime);

        /// <summary>
        /// 测试开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            get { return this.GetProperty(BeginTimeProperty); }
            set { this.SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 测试结束时间 EndTime
        /// <summary>
        /// 测试结束时间
        /// </summary>
        [Label("测试结束时间")]
        public static readonly Property<DateTime?> EndTimeProperty = P<WipPressure>.Register(e => e.EndTime);

        /// <summary>
        /// 测试结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region SN明细列表 WipPressureSnList
        /// <summary>
        /// SN明细列表
        /// </summary>
        [Label("SN明细列表")]
        public static readonly ListProperty<EntityList<WipPressureSn>> WipPressureSnListProperty = P<WipPressure>.RegisterList(e => e.WipPressureSnList);

        /// <summary>
        /// SN明细列表
        /// </summary>
        public EntityList<WipPressureSn> WipPressureSnList
        {
            get { return this.GetLazyList(WipPressureSnListProperty); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipPressure>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WipPressure>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WipPressure>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<WipPressure>.RegisterView(e => e.ResourceCode, e => e.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return GetProperty(ResourceCodeProperty); }
        }
        #endregion   

        #region 资源 ResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WipPressure>.RegisterView(e => e.ResourceName, e => e.Resource.Name);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 生产批次 实体配置
    /// </summary>
    internal class WipBatchEntityConfig : EntityConfig<WipPressure>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PRESSURE").MapAllProperties();
            Meta.Property(WipPressure.BatchNoProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}