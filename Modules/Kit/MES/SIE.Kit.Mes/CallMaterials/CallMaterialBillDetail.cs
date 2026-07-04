using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料单明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("叫料单明细")]
    public partial class CallMaterialBillDetail : DataEntity
    {
        #region 叫料数量 CalledQty
        /// <summary>
        /// 叫料数量
        /// </summary>
        [Label("叫料数量")]
        public static readonly Property<decimal> CalledQtyProperty = P<CallMaterialBillDetail>.Register(e => e.CalledQty);

        /// <summary>
        /// 叫料数量
        /// </summary>
        public decimal CalledQty
        {
            get { return GetProperty(CalledQtyProperty); }
            set { SetProperty(CalledQtyProperty, value); }
        }
        #endregion

        #region 实际数量 ActualQty
        /// <summary>
        /// 实际数量
        /// </summary>
        [Label("实发数量")]
        public static readonly Property<decimal> ActualQtyProperty = P<CallMaterialBillDetail>.Register(e => e.ActualQty);

        /// <summary>
        /// 实际数量
        /// </summary>
        public decimal ActualQty
        {
            get { return GetProperty(ActualQtyProperty); }
            set { SetProperty(ActualQtyProperty, value); }
        }
        #endregion

        #region 已接收数量 ReceiveQty
        /// <summary>
        /// 已接收数量
        /// </summary>
        [Label("已接收数量")]
        public static readonly Property<decimal> ReceiveQtyProperty = P<CallMaterialBillDetail>.Register(e => e.ReceiveQty);

        /// <summary>
        /// 已接收数量
        /// </summary>
        public decimal ReceiveQty
        {
            get { return this.GetProperty(ReceiveQtyProperty); }
            set { this.SetProperty(ReceiveQtyProperty, value); }
        }
        #endregion

        #region 是否全部已接收 IsReceived
        /// <summary>
        /// 是否全部已接收
        /// </summary>
        [Label("是否全部已接收")]
        public static readonly Property<bool> IsReceivedProperty = P<CallMaterialBillDetail>.Register(e => e.IsReceived);

        /// <summary>
        /// 是否全部已接收
        /// </summary>
        public bool IsReceived
        {
            get { return this.GetProperty(IsReceivedProperty); }
            set { this.SetProperty(IsReceivedProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveDate
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiveDateProperty = P<CallMaterialBillDetail>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveDate
        {
            get { return GetProperty(ReceiveDateProperty); }
            set { SetProperty(ReceiveDateProperty, value); }
        }
        #endregion

        #region 接收人 ReceiveBy
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiveByIdProperty = P<CallMaterialBillDetail>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double? ReceiveById
        {
            get { return (double?)GetRefNullableId(ReceiveByIdProperty); }
            set { SetRefNullableId(ReceiveByIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        [Label("接收人")]
        public static readonly RefEntityProperty<Employee> ReceiveByProperty = P<CallMaterialBillDetail>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee ReceiveBy
        {
            get { return GetRefEntity(ReceiveByProperty); }
            set { SetRefEntity(ReceiveByProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<CallMaterialBillDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<CallMaterialBillDetail>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>      
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 叫料单 Bill
        /// <summary>
        /// 叫料单Id
        /// </summary>
        [Label("叫料单")]
        public static readonly IRefIdProperty BillIdProperty = P<CallMaterialBillDetail>.RegisterRefId(e => e.BillId, ReferenceType.Parent);

        /// <summary>
        /// 叫料单Id
        /// </summary>      
        public double BillId
        {
            get { return (double)GetRefId(BillIdProperty); }
            set { SetRefId(BillIdProperty, value); }
        }

        /// <summary>
        /// 叫料单
        /// </summary>
        [Label("叫料单")]
        public static readonly RefEntityProperty<CallMaterialBill> BillProperty = P<CallMaterialBillDetail>.RegisterRef(e => e.Bill, BillIdProperty);

        /// <summary>
        /// 叫料单
        /// </summary>        
        public CallMaterialBill Bill
        {
            get { return GetRefEntity(BillProperty); }
            set { SetRefEntity(BillProperty, value); }
        }
        #endregion

        #region 行号 RowNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> RowNoProperty = P<CallMaterialBillDetail>.Register(e => e.RowNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int RowNo
        {
            get { return this.GetProperty(RowNoProperty); }
            set { this.SetProperty(RowNoProperty, value); }
        }
        #endregion

        #region 已上料 IsLoaded
        /// <summary>
        /// 已上料
        /// </summary>
        [Label("已上料")]
        public static readonly Property<bool> IsLoadedProperty = P<CallMaterialBillDetail>.Register(e => e.IsLoaded);

        /// <summary>
        /// 已上料
        /// </summary>
        public bool IsLoaded
        {
            get { return this.GetProperty(IsLoadedProperty); }
            set { this.SetProperty(IsLoadedProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<CallMaterialBillDetail>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<CallMaterialBillDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 接收人 ReceiveByName
        /// <summary>
        /// 接收人
        /// </summary>
        [Label("接收人")]
        public static readonly Property<string> ReceiveByNameProperty = P<CallMaterialBillDetail>.RegisterView(e => e.ReceiveByName, p => p.ReceiveBy.Name);

        /// <summary>
        /// 接收人
        /// </summary>
        public string ReceiveByName
        {
            get { return this.GetProperty(ReceiveByNameProperty); }
        }
        #endregion

        #region 叫料单号 BillNo
        /// <summary>
        /// 叫料单号
        /// </summary>
        [Label("叫料单号")]
        public static readonly Property<string> BillNoProperty = P<CallMaterialBillDetail>.RegisterView(e => e.BillNo, p => p.Bill.No);

        /// <summary>
        /// 叫料单号
        /// </summary>
        public string BillNo
        {
            get { return this.GetProperty(BillNoProperty); }
        }
        #endregion

        #region 优先级 BillPriority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<Priority> BillPriorityProperty = P<CallMaterialBillDetail>.RegisterView(e => e.BillPriority, p => p.Bill.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public Priority BillPriority
        {
            get { return this.GetProperty(BillPriorityProperty); }
        }
        #endregion

        #region 状态 BillStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<CallMaterialStatus> BillStatusProperty = P<CallMaterialBillDetail>.RegisterView(e => e.BillStatus, p => p.Bill.Status);

        /// <summary>
        /// 状态
        /// </summary>
        public CallMaterialStatus BillStatus
        {
            get { return this.GetProperty(BillStatusProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 叫料单明细 实体配置
    /// </summary>
    internal class CallMaterialBillDetailConfig : EntityConfig<CallMaterialBillDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_BILL_DTL").MapAllProperties();
            Meta.Property(CallMaterialBillDetail.ItemIdProperty).ColumnMeta.HasIndex();
            Meta.IndexGroupOnProperties(CallMaterialBillDetail.BillIdProperty, CallMaterialBillDetail.ItemIdProperty);
            Meta.EnablePhantoms();
        }
    }
}