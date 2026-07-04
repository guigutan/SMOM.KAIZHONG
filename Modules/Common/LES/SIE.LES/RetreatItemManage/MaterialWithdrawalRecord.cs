using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.RetreatItemManage
{
    /// <summary>
    /// 物料退料
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("物料退料")]
    [DisplayMember(nameof(ItemLabel))]
    public class MaterialWithdrawalRecord : DataEntity
    {
        #region 物料标签编号 ItemLabel
        /// <summary>
        /// 物料标签编号
        /// </summary>
        [Required]
        [Label("物料标签编号")]
        public static readonly Property<string> ItemLabelProperty = P<MaterialWithdrawalRecord>.Register(e => e.ItemLabel);

        /// <summary>
        /// 物料标签编号
        /// </summary>
        public string ItemLabel
        {
            get { return GetProperty(ItemLabelProperty); }
            set { SetProperty(ItemLabelProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<MaterialWithdrawalRecord>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get { return GetProperty(RemainQtyProperty); }
            set { SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 退料数量 WithdrawalQty
        /// <summary>
        /// 退料数量
        /// </summary>
        [Label("退料数量")]
        public static readonly Property<decimal> WithdrawalQtyProperty = P<MaterialWithdrawalRecord>.Register(e => e.WithdrawalQty);

        /// <summary>
        /// 退料数量
        /// </summary>
        public decimal WithdrawalQty
        {
            get { return GetProperty(WithdrawalQtyProperty); }
            set { SetProperty(WithdrawalQtyProperty, value); }
        }
        #endregion

        #region 物料批次号 BatchNo
        /// <summary>
        /// 物料批次号
        /// </summary>
        [Label("物料批次号")]
        public static readonly Property<string> BatchNoProperty = P<MaterialWithdrawalRecord>.Register(e => e.BatchNo);

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 退料时间 WithdrawalDate
        /// <summary>
        /// 退料时间
        /// </summary>
        [Label("退料时间")]
        public static readonly Property<DateTime> WithdrawalDateProperty = P<MaterialWithdrawalRecord>.Register(e => e.WithdrawalDate);

        /// <summary>
        /// 退料时间
        /// </summary>
        public DateTime WithdrawalDate
        {
            get { return GetProperty(WithdrawalDateProperty); }
            set { SetProperty(WithdrawalDateProperty, value); }
        }
        #endregion

        #region 退料人员 WithdrawalBy
        /// <summary>
        /// 退料人员Id
        /// </summary>
        public static readonly IRefIdProperty WithdrawalByIdProperty = P<MaterialWithdrawalRecord>.RegisterRefId(e => e.WithdrawalById, ReferenceType.Normal);

        /// <summary>
        /// 退料人员Id
        /// </summary>
        public double WithdrawalById
        {
            get { return (double)GetRefId(WithdrawalByIdProperty); }
            set { SetRefId(WithdrawalByIdProperty, value); }
        }

        /// <summary>
        /// 退料人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> WithdrawalByProperty = P<MaterialWithdrawalRecord>.RegisterRef(e => e.WithdrawalBy, WithdrawalByIdProperty);

        /// <summary>
        /// 退料人员
        /// </summary>
        public Employee WithdrawalBy
        {
            get { return GetRefEntity(WithdrawalByProperty); }
            set { SetRefEntity(WithdrawalByProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<MaterialWithdrawalRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<MaterialWithdrawalRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion



        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<MaterialWithdrawalRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<MaterialWithdrawalRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<MaterialWithdrawalRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<MaterialWithdrawalRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<MaterialWithdrawalRecord>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<MaterialWithdrawalRecord>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 退料人员 WithdrawalByName
        /// <summary>
        /// 退料人员
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> WithdrawalByNameProperty = P<MaterialWithdrawalRecord>.RegisterView(e => e.WithdrawalByName, p => p.WithdrawalBy.Name);

        /// <summary>
        /// 退料人员
        /// </summary>
        public string WithdrawalByName
        {
            get { return this.GetProperty(WithdrawalByNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialWithdrawalRecord>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialWithdrawalRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        
        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<MaterialWithdrawalRecord>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<MaterialWithdrawalRecord>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion


        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<MaterialWithdrawalRecord>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<MaterialWithdrawalRecord>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// 物料退料 实体配置
    /// </summary>
    internal class CallMatrialWithdrawalConfig : EntityConfig<MaterialWithdrawalRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("LES_WITHDRAWAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
