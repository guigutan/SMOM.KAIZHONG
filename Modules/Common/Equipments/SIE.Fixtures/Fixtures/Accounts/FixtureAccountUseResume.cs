using SIE.Core.Equipments;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
	/// 工治具台账-使用履历
	/// </summary>
	[ChildEntity, Serializable]
    [Label("工治具台账-使用履历")]
    public partial class FixtureAccountUseResume : DataEntity
    {
        #region 分区 Subarea
        /// <summary>
        /// 分区
        /// </summary>
        [Label("分区")]
        public static readonly Property<string> SubareaProperty = P<FixtureAccountUseResume>.Register(e => e.Subarea);

        /// <summary>
        /// 分区
        /// </summary>
        public string Subarea
        {
            get { return GetProperty(SubareaProperty); }
            set { SetProperty(SubareaProperty, value); }
        }
        #endregion

        #region 站位 Stance
        /// <summary>
        /// 站位
        /// </summary>
        [Label("站位")]
        public static readonly Property<string> StanceProperty = P<FixtureAccountUseResume>.Register(e => e.Stance);

        /// <summary>
        /// 站位
        /// </summary>
        public string Stance
        {
            get { return GetProperty(StanceProperty); }
            set { SetProperty(StanceProperty, value); }
        }
        #endregion

        #region 使用次数 UseNum
        /// <summary>
        /// 使用次数
        /// </summary>
        [Label("使用次数")]
        public static readonly Property<decimal> UseNumProperty = P<FixtureAccountUseResume>.Register(e => e.UseNum);

        /// <summary>
        /// 使用次数
        /// </summary>
        public decimal UseNum
        {
            get { return GetProperty(UseNumProperty); }
            set { SetProperty(UseNumProperty, value); }
        }
        #endregion

        #region 吸取数 DrawQty
        /// <summary>
        /// 吸取数
        /// </summary>
        [Label("吸取数")]
        public static readonly Property<int> DrawQtyProperty = P<FixtureAccountUseResume>.Register(e => e.DrawQty);

        /// <summary>
        /// 吸取数
        /// </summary>
        public int DrawQty
        {
            get { return GetProperty(DrawQtyProperty); }
            set { SetProperty(DrawQtyProperty, value); }
        }
        #endregion

        #region 抛料数 ThrowQty
        /// <summary>
        /// 抛料数
        /// </summary>
        [Label("抛料数")]
        public static readonly Property<int> ThrowQtyProperty = P<FixtureAccountUseResume>.Register(e => e.ThrowQty);

        /// <summary>
        /// 抛料数
        /// </summary>
        public int ThrowQty
        {
            get { return GetProperty(ThrowQtyProperty); }
            set { SetProperty(ThrowQtyProperty, value); }
        }
        #endregion

        #region 操作时间 OperationTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OperationTimeProperty = P<FixtureAccountUseResume>.Register(e => e.OperationTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime
        {
            get { return this.GetProperty(OperationTimeProperty); }
            set { this.SetProperty(OperationTimeProperty, value); }
        }
        #endregion

        #region 操作人 OperationBy
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperationByIdProperty =
            P<FixtureAccountUseResume>.RegisterRefId(e => e.OperationById, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperationById
        {
            get { return (double)this.GetRefId(OperationByIdProperty); }
            set { this.SetRefId(OperationByIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperationByProperty =
            P<FixtureAccountUseResume>.RegisterRef(e => e.OperationBy, OperationByIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee OperationBy
        {
            get { return this.GetRefEntity(OperationByProperty); }
            set { this.SetRefEntity(OperationByProperty, value); }
        }
        #endregion

        #region 操作类型 OperationType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<UseResumeType> OperationTypeProperty = P<FixtureAccountUseResume>.Register(e => e.OperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public UseResumeType OperationType
        {
            get { return this.GetProperty(OperationTypeProperty); }
            set { this.SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 操作数量 OperationQty
        /// <summary>
        /// 操作数量
        /// </summary>
        [Label("操作数量")]
        public static readonly Property<int> OperationQtyProperty = P<FixtureAccountUseResume>.Register(e => e.OperationQty);

        /// <summary>
        /// 操作数量
        /// </summary>
        public int OperationQty
        {
            get { return this.GetProperty(OperationQtyProperty); }
            set { this.SetProperty(OperationQtyProperty, value); }
        }
        #endregion

        #region 上线时间 OnlineDate
        /// <summary>
        /// 上线时间
        /// </summary>
        [Label("上线时间")]
        public static readonly Property<DateTime?> OnlineDateProperty = P<FixtureAccountUseResume>.Register(e => e.OnlineDate);

        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime? OnlineDate
        {
            get { return GetProperty(OnlineDateProperty); }
            set { SetProperty(OnlineDateProperty, value); }
        }
        #endregion

        #region 下线时间 OfflineDate
        /// <summary>
        /// 下线时间
        /// </summary>
        [Label("下线时间")]
        public static readonly Property<DateTime?> OfflineDateProperty = P<FixtureAccountUseResume>.Register(e => e.OfflineDate);

        /// <summary>
        /// 下线时间
        /// </summary>
        public DateTime? OfflineDate
        {
            get { return GetProperty(OfflineDateProperty); }
            set { SetProperty(OfflineDateProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<FixtureAccountUseResume>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<FixtureAccountUseResume>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<FixtureAccountUseResume>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<FixtureAccountUseResume>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<FixtureAccountUseResume>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<FixtureAccountUseResume>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<FixtureAccountUseResume>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<FixtureAccountUseResume>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工治具台账 FixtureAccount
        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<FixtureAccountUseResume>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Parent);

        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public double FixtureAccountId
        {
            get { return (double)GetRefId(FixtureAccountIdProperty); }
            set { SetRefId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<FixtureAccountUseResume>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具台账
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工治具台账-使用履历 实体配置
    /// </summary>
    internal class FixtureAccountUseResumeConfig : EntityConfig<FixtureAccountUseResume>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ACC_USE_RES").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
