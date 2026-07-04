using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.LES.RetreatItemManage.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.RetreatItemManage.MaterialReturns
{
    /// <summary>
    /// 退料实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产退料")]
    [ConditionQueryType(typeof(MaterialReturnCriteria))]
    [EntityWithConfig(typeof(ReturnMaterialConfig))]
    [DisplayMember(nameof(NO))]
    [SIE.DataAuth.EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class MaterialReturn : DataEntity
    {
        /// <summary>
        /// 生产退料原因快码
        /// </summary>
        public const string ReasonMaterialReturn = "Reason_Material_Return";

        #region  NO
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NOProperty = P<MaterialReturn>.Register(e => e.NO);

        /// <summary>
        /// 
        /// </summary>
        public string NO
        {
            get { return GetProperty(NOProperty); }
            set { SetProperty(NOProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<MaterialReturn>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)GetRefId(FactoryIdProperty); }
            set { SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<MaterialReturn>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 状态 ReturnState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReturnStates> ReturnStateProperty = P<MaterialReturn>.Register(e => e.ReturnState);

        /// <summary>
        /// 状态
        /// </summary>
        public ReturnStates ReturnState
        {
            get { return GetProperty(ReturnStateProperty); }
            set { SetProperty(ReturnStateProperty, value); }
        }
        #endregion

        #region 退料类型 ReturnType
        /// <summary>
        /// 退料类型
        /// </summary>
        [Label("退料类型")]
        public static readonly Property<ReturnTypes> ReturnTypeProperty = P<MaterialReturn>.Register(e => e.ReturnType);

        /// <summary>
        /// 退料类型
        /// </summary>
        public ReturnTypes ReturnType
        {
            get { return GetProperty(ReturnTypeProperty); }
            set { SetProperty(ReturnTypeProperty, value); }
        }
        #endregion

        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelProperty = P<MaterialReturn>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return GetProperty(LabelProperty); }
            set { SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 标签号 LabelId
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<double> LabelIdProperty = P<MaterialReturn>.Register(e => e.LabelId);

        /// <summary>
        /// 标签号
        /// </summary>
        public double LabelId
        {
            get { return GetProperty(LabelIdProperty); }
            set { SetProperty(LabelIdProperty, value); }
        }
        #endregion

        #region 退料数量 Qty
        /// <summary>
        /// 退料数量
        /// </summary>
        [Label("退料数量")]
        public static readonly Property<decimal> QtyProperty = P<MaterialReturn>.Register(e => e.Qty);

        /// <summary>
        /// 退料数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 不良数量 BadQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> BadQtyProperty = P<MaterialReturn>.Register(e => e.BadQty);

        /// <summary>
        /// 退料数量
        /// </summary>
        public decimal BadQty
        {
            get { return GetProperty(BadQtyProperty); }
            set { SetProperty(BadQtyProperty, value); }
        }
        #endregion

        #region 批次号 BatchNO
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNOProperty = P<MaterialReturn>.Register(e => e.BatchNO);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNO
        {
            get { return GetProperty(BatchNOProperty); }
            set { SetProperty(BatchNOProperty, value); }
        }
        #endregion

        #region 批次Id BatchId
        /// <summary>
        /// 批次Id
        /// </summary>
        [Label("批次Id")]
        public static readonly Property<double?> BatchIdProperty = P<MaterialReturn>.Register(e => e.BatchId);

        /// <summary>
        /// 批次Id
        /// </summary>
        public double? BatchId
        {
            get { return GetProperty(BatchIdProperty); }
            set { SetProperty(BatchIdProperty, value); }
        }
        #endregion

        #region  提交时间
        /// <summary>
        /// 提交时间
        /// </summary>
        [Label("提交时间")]
        public static readonly Property<DateTime?> SubmitDateProperty = P<MaterialReturn>.Register(e => e.SubmitDate);

        /// <summary>
        /// 
        /// </summary>
        public DateTime? SubmitDate
        {
            get { return GetProperty(SubmitDateProperty); }
            set { SetProperty(SubmitDateProperty, value); }
        }
        #endregion

        #region 关联工单 WorkOrder
        /// <summary>
        /// 关联工单Id
        /// </summary>
        [Label("关联工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<MaterialReturn>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 关联工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 关联工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<MaterialReturn>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 关联工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<MaterialReturn>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<MaterialReturn>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 提交人 Employee
        /// <summary>
        /// 提交人Id
        /// </summary>
        [Label("提交人")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<MaterialReturn>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 提交人Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 提交人
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<MaterialReturn>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 提交人
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 生产资源 WipResource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty WipResourceIdProperty = P<MaterialReturn>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)GetRefNullableId(WipResourceIdProperty); }
            set { SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty = P<MaterialReturn>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipResource
        {
            get { return GetRefEntity(WipResourceProperty); }
            set { SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 退料仓库 ReturnWarehouse
        /// <summary>
        /// 退料仓库Id
        /// </summary>
        [Label("退料仓库")]
        public static readonly IRefIdProperty ReturnWarehouseIdProperty =
            P<MaterialReturn>.RegisterRefId(e => e.ReturnWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 退料仓库Id
        /// </summary>
        public double? ReturnWarehouseId
        {
            get { return (double?)this.GetRefNullableId(ReturnWarehouseIdProperty); }
            set { this.SetRefNullableId(ReturnWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 退料仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> ReturnWarehouseProperty =
            P<MaterialReturn>.RegisterRef(e => e.ReturnWarehouse, ReturnWarehouseIdProperty);

        /// <summary>
        /// 退料仓库
        /// </summary>
        public Warehouse ReturnWarehouse
        {
            get { return this.GetRefEntity(ReturnWarehouseProperty); }
            set { this.SetRefEntity(ReturnWarehouseProperty, value); }
        }
        #endregion

        #region 退料原因 ReturnReason
        /// <summary>
        /// 退料原因
        /// </summary>
        [Label("退料原因")]
        public static readonly Property<string> ReturnReasonProperty = P<MaterialReturn>.Register(e => e.ReturnReason);

        /// <summary>
        /// 退料原因
        /// </summary>
        public string ReturnReason
        {
            get { return this.GetProperty(ReturnReasonProperty); }
            set { this.SetProperty(ReturnReasonProperty, value); }
        }
        #endregion

        #region 退料描述 ReturnReasonDesc
        /// <summary>
        /// 退料描述
        /// </summary>
        [Label("退料描述")]
        [MaxLength(4000)]
        public static readonly Property<string> ReturnReasonDescProperty = P<MaterialReturn>.Register(e => e.ReturnReasonDesc);

        /// <summary>
        /// 退料描述
        /// </summary>
        public string ReturnReasonDesc
        {
            get { return this.GetProperty(ReturnReasonDescProperty); }
            set { this.SetProperty(ReturnReasonDescProperty, value); }
        }
        #endregion

        #region 退料库位 ReturnWarehouseLocation
        /// <summary>
        /// 退料库位Id
        /// </summary>
        [Label("退料库位")]
        public static readonly IRefIdProperty ReturnWarehouseLocationIdProperty =
            P<MaterialReturn>.RegisterRefId(e => e.ReturnWarehouseLocationId, ReferenceType.Normal);

        /// <summary>
        /// 退料库位Id
        /// </summary>
        public double? ReturnWarehouseLocationId
        {
            get { return (double?)this.GetRefNullableId(ReturnWarehouseLocationIdProperty); }
            set { this.SetRefNullableId(ReturnWarehouseLocationIdProperty, value); }
        }

        /// <summary>
        /// 退料库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> ReturnWarehouseLocationProperty =
            P<MaterialReturn>.RegisterRef(e => e.ReturnWarehouseLocation, ReturnWarehouseLocationIdProperty);

        /// <summary>
        /// 退料库位
        /// </summary>
        public StorageLocation ReturnWarehouseLocation
        {
            get { return this.GetRefEntity(ReturnWarehouseLocationProperty); }
            set { this.SetRefEntity(ReturnWarehouseLocationProperty, value); }
        }
        #endregion

        #region 现有数量 AlreadyQty
        /// <summary>
        /// 现有数量
        /// </summary>
        [Label("现有数量")]
        public static readonly Property<decimal> AlreadyQtyProperty = P<MaterialReturn>.Register(e => e.AlreadyQty);

        /// <summary>
        /// 现有数量
        /// </summary>
        public decimal AlreadyQty
        {
            get { return this.GetProperty(AlreadyQtyProperty); }
            set { this.SetProperty(AlreadyQtyProperty, value); }
        }
        #endregion

        #region 物料编码	 ItemCode	
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialReturn>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称	 ItemName	
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialReturn>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<MaterialReturn>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProName
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MaterialReturn>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 生产资源名称 WipResourceName
        /// <summary>
        /// 生产资源名称
        /// </summary>
        [Label("生产资源名称")]
        public static readonly Property<string> WipResourceNameProperty = P<MaterialReturn>.RegisterView(e => e.WipResourceName, p => p.WipResource.Name);

        /// <summary>
        /// 生产资源名称
        /// </summary>
        public string WipResourceName
        {
            get { return this.GetProperty(WipResourceNameProperty); }
        }
        #endregion

        #endregion

        #region 扫描条码（不映射数据库） Sn
        /// <summary>
        /// 扫描条码（不映射数据库）
        /// </summary>
        [Label("标签/批次号/物料号")]
        public static readonly Property<string> SnProperty = P<MaterialReturn>.Register(e => e.Sn);

        /// <summary>
        /// 扫描条码（不映射数据库）
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 是否序列号 IsSerial
        /// <summary>
        /// 是否序列号
        /// </summary>
        [Label("是否序列号管控")]
        public static readonly Property<bool?> IsSerialProperty = P<MaterialReturn>.Register(e => e.IsSerial);

        /// <summary>
        /// 是否序列号
        /// </summary>
        public bool? IsSerial
        {
            get { return this.GetProperty(IsSerialProperty); }
            set { this.SetProperty(IsSerialProperty, value); }
        }
        #endregion

        #region 注释 WoNo
        /// <summary>
        /// 工单号 
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<MaterialReturn>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 注释
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 退料实体 实体配置
    /// </summary>
    internal class MaterialReturnConfig : EntityConfig<MaterialReturn>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("MATERIA_RETURN").MapAllPropertiesExcept(MaterialReturn.SnProperty);
            Meta.Property(MaterialReturn.ReturnReasonDescProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
		}
	}
}