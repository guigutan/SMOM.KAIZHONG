using SIE.Core.Equipments;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Fixtures.Warns
{
    /// <summary>
    /// 工治具保养预警
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureWarnCriteria))]
    [Label("工治具保养预警")]
    public partial class FixtureWarn : DataEntity
    {
        #region 剩余次数 RestNum
        /// <summary>
        /// 剩余次数
        /// </summary>
        [Label("剩余次数")]
        public static readonly Property<int> RestNumProperty = P<FixtureWarn>.Register(e => e.RestNum);

        /// <summary>
        /// 剩余次数
        /// </summary>
        public int RestNum
        {
            get { return GetProperty(RestNumProperty); }
            set { SetProperty(RestNumProperty, value); }
        }
        #endregion

        #region 剩余时长 RestHour
        /// <summary>
        /// 剩余时长
        /// </summary>
        [Label("剩余时长")]
        public static readonly Property<decimal> RestHourProperty = P<FixtureWarn>.Register(e => e.RestHour);

        /// <summary>
        /// 剩余时长
        /// </summary>
        public decimal RestHour
        {
            get { return GetProperty(RestHourProperty); }
            set { SetProperty(RestHourProperty, value); }
        }
        #endregion

        #region 保养后抛料总数 TotalThrowQty
        /// <summary>
        /// 保养后抛料总数
        /// </summary>
        [Label("保养后抛料总数")]
        public static readonly Property<int> TotalThrowQtyProperty = P<FixtureWarn>.Register(e => e.TotalThrowQty);

        /// <summary>
        /// 保养后抛料总数
        /// </summary>
        public int TotalThrowQty
        {
            get { return GetProperty(TotalThrowQtyProperty); }
            set { SetProperty(TotalThrowQtyProperty, value); }
        }
        #endregion

        #region 分区 Subarea
        /// <summary>
        /// 分区
        /// </summary>
        [Label("分区")]
        public static readonly Property<string> SubareaProperty = P<FixtureWarn>.Register(e => e.Subarea);

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
        public static readonly Property<string> StanceProperty = P<FixtureWarn>.Register(e => e.Stance);

        /// <summary>
        /// 站位
        /// </summary>
        public string Stance
        {
            get { return GetProperty(StanceProperty); }
            set { SetProperty(StanceProperty, value); }
        }
        #endregion

        #region 工治具治具台账 FixtureAccount
        /// <summary>
        /// 工治具治具台账Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<FixtureWarn>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

        /// <summary>
        /// 工治具治具台账Id
        /// </summary>
        public double FixtureAccountId
        {
            get { return (double)GetRefId(FixtureAccountIdProperty); }
            set { SetRefId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具治具台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<FixtureWarn>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具治具台账
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<FixtureWarn>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<FixtureWarn>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 状态 AccountState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<FixtureAccountState> AccountStateProperty = P<FixtureWarn>.Register(e => e.AccountState);

        /// <summary>
        /// 状态
        /// </summary>
        public FixtureAccountState AccountState
        {
            get { return GetProperty(AccountStateProperty); }
            set { SetProperty(AccountStateProperty, value); }
        }
        #endregion

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<FixtureWarn>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<FixtureWarn>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 生产资源 WipResource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty = P<FixtureWarn>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double WipResourceId
        {
            get { return (double)GetRefId(WipResourceIdProperty); }
            set { SetRefId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty = P<FixtureWarn>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipResource
        {
            get { return GetRefEntity(WipResourceProperty); }
            set { SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工治具ID AccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> AccountCodeProperty = P<FixtureWarn>.RegisterView(e => e.AccountCode, p => p.FixtureAccount.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string AccountCode
        {
            get { return this.GetProperty(AccountCodeProperty); }
            set { this.SetProperty(AccountCodeProperty, value); }
        }
        #endregion

        #region 工治具型号编码 ModelCode
        /// <summary>
        /// 工治具型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureWarn>.RegisterView(e => e.ModelCode, p => p.FixtureAccount.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 工治具型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 工治具型号名称 ModelName
        /// <summary>
        /// 工治具型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureWarn>.RegisterView(e => e.ModelName, p => p.FixtureAccount.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 工治具型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<FixtureWarn>.RegisterView(e => e.FixtureType, p => p.FixtureAccount.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
        }
        #endregion

        #region 预警值(次数) WarnNum
        /// <summary>
        /// 预警值(次数)
        /// </summary>
        [Label("预警值(次数)")]
        public static readonly Property<int> WarnNumProperty = P<FixtureWarn>.RegisterView(e => e.WarnNum, p => p.FixtureAccount.FixtureEncode.FixtureModel.WarnNum);

        /// <summary>
        /// 预警值(次数)
        /// </summary>
        public int WarnNum
        {
            get { return this.GetProperty(WarnNumProperty); }
        }
        #endregion

        #region 预警值(小时) WarnHour
        /// <summary>
        /// 预警值(小时)
        /// </summary>
        [Label("预警值(小时)")]
        public static readonly Property<decimal> WarnHourProperty = P<FixtureWarn>.RegisterView(e => e.WarnHour, p => p.FixtureAccount.FixtureEncode.FixtureModel.WarnHour);

        /// <summary>
        /// 预警值(小时)
        /// </summary>
        public decimal WarnHour
        {
            get { return this.GetProperty(WarnHourProperty); }
        }
        #endregion

        #region 保养后使用次数 MaintainedNum
        /// <summary>
        /// 保养后使用次数
        /// </summary>
        [Label("保养后使用次数")]
        public static readonly Property<int> MaintainedNumProperty = P<FixtureWarn>.RegisterView(e => e.MaintainedNum, p => p.FixtureAccount.MaintainedNum);

        /// <summary>
        /// 保养后使用次数
        /// </summary>
        public int MaintainedNum
        {
            get { return this.GetProperty(MaintainedNumProperty); }
        }
        #endregion

        #region 保养后使用时长 MaintainedHour
        /// <summary>
        /// 保养后使用时长
        /// </summary>
        [Label("保养后使用时长")]
        public static readonly Property<decimal> MaintainedHourProperty = P<FixtureWarn>.RegisterView(e => e.MaintainedHour, p => p.FixtureAccount.MaintainedHour);

        /// <summary>
        /// 保养后使用时长
        /// </summary>
        public decimal MaintainedHour
        {
            get { return this.GetProperty(MaintainedHourProperty); }
        }
        #endregion

        #region 总使用次数 TotalUseNum
        /// <summary>
        /// 总使用次数
        /// </summary>
        [Label("总使用次数")]
        public static readonly Property<int> TotalUseNumProperty = P<FixtureWarn>.RegisterView(e => e.TotalUseNum, p => p.FixtureAccount.TotalUseNum);

        /// <summary>
        /// 总使用次数
        /// </summary>
        public int TotalUseNum
        {
            get { return this.GetProperty(TotalUseNumProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工治具保养预警 实体配置
    /// </summary>
    internal class FixtureWarnConfig : EntityConfig<FixtureWarn>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_FIXTURE_WARN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
