using SIE.Core.Items;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.WorkOrders
{
    /// <summary>
    /// 工单BOM
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单BOM")]
    public class WorkOrderBom : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderBom()
        {
            IsByBill = false;
            IsRecoilItem = false;
            IsVritualItem = false;
        }

        #region 需求量 RequireQty
        /// <summary>
        /// 需求量
        /// </summary>
        [Label("需求量")]
        public static readonly Property<decimal> RequireQtyProperty = P<WorkOrderBom>.Register(e => e.RequireQty);

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal RequireQty
        {
            get { return GetProperty(RequireQtyProperty); }
            set { SetProperty(RequireQtyProperty, value); }
        }
        #endregion

        #region 单位耗用量 SingleQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量")]
        public static readonly Property<decimal> SingleQtyProperty = P<WorkOrderBom>.Register(e => e.SingleQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty
        {
            get { return GetProperty(SingleQtyProperty); }
            set { SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 是否反冲物料 IsRecoilItem
        /// <summary>
        /// 是否反冲物料
        /// </summary>
        [Label("是否反冲物料")]
        public static readonly Property<bool> IsRecoilItemProperty = P<WorkOrderBom>.Register(e => e.IsRecoilItem);

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool IsRecoilItem
        {
            get { return GetProperty(IsRecoilItemProperty); }
            set { SetProperty(IsRecoilItemProperty, value); }
        }
        #endregion

        #region 是否虚拟物料 IsVritualItem
        /// <summary>
        /// 是否虚拟物料
        /// </summary>
        [Label("是否虚拟物料")]
        public static readonly Property<bool> IsVritualItemProperty = P<WorkOrderBom>.Register(e => e.IsVritualItem);

        /// <summary>
        /// 是否虚拟物料
        /// </summary>
        public bool IsVritualItem
        {
            get { return GetProperty(IsVritualItemProperty); }
            set { SetProperty(IsVritualItemProperty, value); }
        }
        #endregion

        #region 是否按单标识 IsByBill
        /// <summary>
        /// 是否按单标识
        /// </summary>
        [Label("是否按单标识")]
        public static readonly Property<bool> IsByBillProperty = P<WorkOrderBom>.Register(e => e.IsByBill);

        /// <summary>
        /// 是否按单标识
        /// </summary>
        public bool IsByBill
        {
            get { return GetProperty(IsByBillProperty); }
            set { SetProperty(IsByBillProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<WorkOrderBom>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion 

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<WorkOrderBom>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<WorkOrderBom>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工单与工单BOM关系 WorkOrder
        /// <summary>
        /// 工单与工单BOM关系Id
        /// </summary>
        [Label("工单与工单BOM关系")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WorkOrderBom>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 工单与工单BOM关系Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WorkOrderBom>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WorkOrderBom>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtPropName
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrderBom>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 是替代料 IsAlternative
        /// <summary>
        /// 是替代料
        /// </summary>
        [Label("是替代料")]
        public static readonly Property<bool> IsAlternativeProperty = P<WorkOrderBom>.Register(e => e.IsAlternative);

        /// <summary>
        /// 是替代料
        /// </summary>
        public bool IsAlternative
        {
            get { return this.GetProperty(IsAlternativeProperty); }
            set { this.SetProperty(IsAlternativeProperty, value); }
        }
        #endregion

        #region 是否允许编辑物料扩展属性 IsAllowEdit
        /// <summary>
        /// 是否允许编辑物料扩展属性
        /// </summary>
        [Label("是否允许编辑物料扩展属性")]
        public static readonly Property<bool> IsAllowEditProperty = P<WorkOrderBom>.Register(e => e.IsAllowEdit);

        /// <summary>
        /// 是否允许编辑物料扩展属性
        /// </summary>
        public bool IsAllowEdit
        {
            get { return this.GetProperty(IsAllowEditProperty); }
            set { this.SetProperty(IsAllowEditProperty, value); }
        }
        #endregion

        #region Erp工单
        #region Erp主键 ErpKey
        /// <summary>
        /// Erp主键
        /// </summary>
        [Label("Erp主键")]
        public static readonly Property<string> ErpKeyProperty = P<WorkOrderBom>.Register(e => e.ErpKey);

        /// <summary>
        /// Erp主键
        /// </summary>
        public string ErpKey
        {
            get { return this.GetProperty(ErpKeyProperty); }
            set { this.SetProperty(ErpKeyProperty, value); }
        }
        #endregion

        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<WorkOrderBom>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 工单领耗退数量统计

        #region 初始已发料数 InitIssuedQty
        /// <summary>
        /// 初始已发料数
        /// </summary>
        [Label("初始已发料数")]
        public static readonly Property<decimal> InitIssuedQtyProperty = P<WorkOrderBom>.Register(e => e.InitIssuedQty);

        /// <summary>
        /// 初始已发料数
        /// </summary>
        public decimal InitIssuedQty
        {
            get { return this.GetProperty(InitIssuedQtyProperty); }
            set { this.SetProperty(InitIssuedQtyProperty, value); }
        }
        #endregion

        #region 初始耗料数 InitConsumedQty
        /// <summary>
        /// 初始耗料数
        /// </summary>
        [Label("初始耗料数")]
        public static readonly Property<decimal> InitConsumedQtyProperty = P<WorkOrderBom>.Register(e => e.InitConsumedQty);

        /// <summary>
        /// 初始耗料数
        /// </summary>
        public decimal InitConsumedQty
        {
            get { return this.GetProperty(InitConsumedQtyProperty); }
            set { this.SetProperty(InitConsumedQtyProperty, value); }
        }
        #endregion

        #region 申请领料数 ApplyIssueQty
        /// <summary>
        /// 申请领料数
        /// </summary>
        [Label("申请领料数")]
        public static readonly Property<decimal> ApplyIssueQtyProperty = P<WorkOrderBom>.Register(e => e.ApplyIssueQty);

        /// <summary>
        /// 申请领料数
        /// </summary>
        public decimal ApplyIssueQty
        {
            get { return this.GetProperty(ApplyIssueQtyProperty); }
            set { this.SetProperty(ApplyIssueQtyProperty, value); }
        }
        #endregion

        #region 发料数 IssuedQty
        /// <summary>
        /// 发料数
        /// </summary>
        [Label("发料数")]
        public static readonly Property<decimal> IssuedQtyProperty = P<WorkOrderBom>.Register(e => e.IssuedQty);

        /// <summary>
        /// 发料数
        /// </summary>
        public decimal IssuedQty
        {
            get { return this.GetProperty(IssuedQtyProperty); }
            set { this.SetProperty(IssuedQtyProperty, value); }
        }
        #endregion

        #region 待接收数 TobeReceiveQty
        /// <summary>
        /// 待接收数
        /// </summary>
        [Label("待接收数")]
        public static readonly Property<decimal> TobeReceiveQtyProperty = P<WorkOrderBom>.Register(e => e.TobeReceiveQty);

        /// <summary>
        /// 待接收数
        /// </summary>
        public decimal TobeReceiveQty
        {
            get { return this.GetProperty(TobeReceiveQtyProperty); }
            set { this.SetProperty(TobeReceiveQtyProperty, value); }
        }
        #endregion

        #region 申请耗料数 ApplyConsumeQty
        /// <summary>
        /// 申请耗料数
        /// </summary>
        [Label("申请耗料数")]
        public static readonly Property<decimal> ApplyConsumeQtyProperty = P<WorkOrderBom>.Register(e => e.ApplyConsumeQty);

        /// <summary>
        /// 申请耗料数
        /// </summary>
        public decimal ApplyConsumeQty
        {
            get { return this.GetProperty(ApplyConsumeQtyProperty); }
            set { this.SetProperty(ApplyConsumeQtyProperty, value); }
        }
        #endregion

        #region 耗料数 ConsumedQty
        /// <summary>
        /// 耗料数
        /// </summary>
        [Label("耗料数")]
        public static readonly Property<decimal> ConsumedQtyProperty = P<WorkOrderBom>.Register(e => e.ConsumedQty);

        /// <summary>
        /// 耗料数
        /// </summary>
        public decimal ConsumedQty
        {
            get { return this.GetProperty(ConsumedQtyProperty); }
            set { this.SetProperty(ConsumedQtyProperty, value); }
        }
        #endregion

        #region 申请退料数 ApplyReturnQty
        /// <summary>
        /// 申请退料数
        /// </summary>
        [Label("申请退料数")]
        public static readonly Property<decimal> ApplyReturnQtyProperty = P<WorkOrderBom>.Register(e => e.ApplyReturnQty);

        /// <summary>
        /// 申请退料数
        /// </summary>
        public decimal ApplyReturnQty
        {
            get { return this.GetProperty(ApplyReturnQtyProperty); }
            set { this.SetProperty(ApplyReturnQtyProperty, value); }
        }
        #endregion

        #region 退料数 ReturnedQty
        /// <summary>
        /// 退料数
        /// </summary>
        [Label("退料数")]
        public static readonly Property<decimal> ReturnedQtyProperty = P<WorkOrderBom>.Register(e => e.ReturnedQty);

        /// <summary>
        /// 退料数
        /// </summary>
        public decimal ReturnedQty
        {
            get { return this.GetProperty(ReturnedQtyProperty); }
            set { this.SetProperty(ReturnedQtyProperty, value); }
        }
        #endregion

        #region 挪出数 MoveOutQty
        /// <summary>
        /// 挪出数
        /// </summary>
        [Label("挪出数")]
        public static readonly Property<decimal> MoveOutQtyProperty = P<WorkOrderBom>.Register(e => e.MoveOutQty);

        /// <summary>
        /// 挪出数
        /// </summary>
        public decimal MoveOutQty
        {
            get { return this.GetProperty(MoveOutQtyProperty); }
            set { this.SetProperty(MoveOutQtyProperty, value); }
        }
        #endregion

        #region 挪入数 MoveInQty
        /// <summary>
        /// 挪入数
        /// </summary>
        [Label("挪入数")]
        public static readonly Property<decimal> MoveInQtyProperty = P<WorkOrderBom>.Register(e => e.MoveInQty);

        /// <summary>
        /// 挪入数
        /// </summary>
        public decimal MoveInQty
        {
            get { return this.GetProperty(MoveInQtyProperty); }
            set { this.SetProperty(MoveInQtyProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
	/// 工单Bom 实体配置
	/// </summary>
	internal class WorkOrderBomConfig : EntityConfig<WorkOrderBom>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_BOM").MapAllProperties();
            Meta.Property(WorkOrderBom.IsAllowEditProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}