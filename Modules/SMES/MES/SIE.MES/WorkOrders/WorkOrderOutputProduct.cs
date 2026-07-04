using SIE.Domain;
using SIE.Items;
using SIE.LES.LesStockCounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 联/副产品
    /// </summary>
    [ChildEntity, Serializable]
    [Label("联/副产品")]

    public class WorkOrderOutputProduct:DataEntity
    {
        #region 行号 RowNumber
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        [Required]
        public static readonly Property<string> RowNumberProperty = P<WorkOrderOutputProduct>.Register(e => e.RowNumber);

        /// <summary>
        /// 行号
        /// </summary>
        public string RowNumber
        {
            get { return this.GetProperty(RowNumberProperty); }
            set { this.SetProperty(RowNumberProperty, value); }
        }
        #endregion

        #region 产出类型 OutputListType
        /// <summary>
        /// 产出类型
        /// </summary>
        [Label("产出类型")]
        public static readonly Property<OutputListType> OutputListTypeProperty = P<WorkOrderOutputProduct>.Register(e => e.OutputListType);

        /// <summary>
        /// 产出类型
        /// </summary>
        public OutputListType OutputListType
        {
            get { return this.GetProperty(OutputListTypeProperty); }
            set { this.SetProperty(OutputListTypeProperty, value); }
        }
        #endregion


        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<WorkOrderOutputProduct>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<WorkOrderOutputProduct>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<WorkOrderOutputProduct>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtProp
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropProperty = P<WorkOrderOutputProduct>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 扩展属性名称 ItemExtPropName
        /// <summary>
        /// 扩展属性名称
        /// </summary>
        [Label("扩展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrderOutputProduct>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<WorkOrderOutputProduct>.RegisterView(e => e.Unit, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { this.SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 启用物料扩展属性 EnableExtPro
        /// <summary>
        /// 启用物料扩展属性
        /// </summary>
        [Label("启用物料扩展属性")]
        public static readonly Property<bool> EnableExtProProperty = P<WorkOrderOutputProduct>.RegisterView(e => e.EnableExtPro, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 启用物料扩展属性
        /// </summary>
        public bool EnableExtPro
        {
            get { return this.GetProperty(EnableExtProProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<WorkOrderOutputProduct>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static  readonly IRefIdProperty WorkOrderIdProperty = P<WorkOrderOutputProduct>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 工单
        /// </summary>
        public  double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static  readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WorkOrderOutputProduct>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public  WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion


        #region 已提交数量(后台数量，不显示) SubmitQty
        /// <summary>
        /// 已提交数量(后台数量，不显示)
        /// </summary>
        [Label("已提交数量")]
        public static readonly Property<decimal> SubmitQtyProperty = P<WorkOrderOutputProduct>.Register(e => e.SubmitQty);

        /// <summary>
        /// 已提交数量(后台数量，不显示)
        /// </summary>
        public decimal SubmitQty
        {
            get { return this.GetProperty(SubmitQtyProperty); }
            set { this.SetProperty(SubmitQtyProperty, value); }
        }
        #endregion


        #region 是批次管理 IsBatchCtrl
        /// <summary>
        /// 是批次管理
        /// </summary>
        [Label("属性名")]
        public static readonly Property<bool?> IsBatchCtrlProperty = P<WorkOrderOutputProduct>.Register(e => e.IsBatchCtrl);

        /// <summary>
        /// 是批次管理
        /// </summary>
        public bool? IsBatchCtrl
        {
            get { return this.GetProperty(IsBatchCtrlProperty); }
            set { this.SetProperty(IsBatchCtrlProperty, value); }
        }
        #endregion


        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WorkOrderOutputProduct>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<WorkOrderOutputProduct>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }

        #endregion


        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WorkOrderOutputProduct>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

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
        public static readonly Property<string> ProductCodeProperty = P<WorkOrderOutputProduct>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<WorkOrderOutputProduct>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }

        #endregion

    }
    #region 实体配置
    /// <summary>
    ///  实体配置
    /// </summary>
    internal class WorkOrderOutputProductConfig : EntityConfig<WorkOrderOutputProduct>
    {
        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_OUT_PROC").MapAllPropertiesExcept(WorkOrderOutputProduct.EnableExtProProperty, WorkOrderOutputProduct.IsBatchCtrlProperty);
            Meta.EnablePhantoms();
        }
    }
    #endregion
}
