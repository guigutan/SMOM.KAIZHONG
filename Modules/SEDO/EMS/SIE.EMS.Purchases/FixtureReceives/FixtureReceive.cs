using SIE;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.FixtureReceives.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收
    /// </summary>
    [RootEntity, Serializable]
    [Label("工治具接收")]
    [ConditionQueryType(typeof(FixtureReceiveCriteria))]
    [EntityWithConfig(typeof(NoConfig), "工治具接收单号配置项", "工治具接收单号生成规则")]
    [EntityWithConfig(typeof(ReceiveSnNoConfig))]
    [EntityWithConfig(typeof(ReceiveSnPrintTempConfig))]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BussinessDepartmentAuth(nameof(DepartmentId), true)]
    [DisplayMember(nameof(ReceiveNo))]
    public partial class FixtureReceive : DataEntity
    {
        #region 接收单号 ReceiveNo
        /// <summary>
        /// 接收单号
        /// </summary>
        [Label("接收单号")]
        public static readonly Property<string> ReceiveNoProperty = P<FixtureReceive>.Register(e => e.ReceiveNo);

        /// <summary>
        /// 接收单号
        /// </summary>
        public string ReceiveNo
        {
            get { return GetProperty(ReceiveNoProperty); }
            set { SetProperty(ReceiveNoProperty, value); }
        }
        #endregion

        #region 品种数 VarietyQuantity
        /// <summary>
        /// 品种数
        /// </summary>
        [Label("品种数")]
        public static readonly Property<int> VarietyQuantityProperty = P<FixtureReceive>.Register(e => e.VarietyQuantity);

        /// <summary>
        /// 品种数
        /// </summary>
        public int VarietyQuantity
        {
            get { return GetProperty(VarietyQuantityProperty); }
            set { SetProperty(VarietyQuantityProperty, value); }
        }
        #endregion

        #region 总数量 TotalQty
        /// <summary>
        /// 总数量
        /// </summary>
        [Label("总数量")]
        public static readonly Property<int> TotalQtyProperty = P<FixtureReceive>.Register(e => e.TotalQty);

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalQty
        {
            get { return GetProperty(TotalQtyProperty); }
            set { SetProperty(TotalQtyProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveDateTime
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiveDateTimeProperty = P<FixtureReceive>.Register(e => e.ReceiveDateTime);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveDateTime
        {
            get { return GetProperty(ReceiveDateTimeProperty); }
            set { SetProperty(ReceiveDateTimeProperty, value); }
        }
        #endregion

        #region 工治具接收明细列表 FixtureReceiveDetailList
        /// <summary>
        /// 工治具接收明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureReceiveDetail>> FixtureReceiveDetailListProperty = P<FixtureReceive>.RegisterList(e => e.FixtureReceiveDetailList);
        /// <summary>
        /// 工治具接收明细列表
        /// </summary>
        public EntityList<FixtureReceiveDetail> FixtureReceiveDetailList
        {
            get { return this.GetLazyList(FixtureReceiveDetailListProperty); }
        }
        #endregion

        #region 接收人 Receiver
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiverIdProperty = P<FixtureReceive>.RegisterRefId(e => e.ReceiverId, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double? ReceiverId
        {
            get { return (double?)GetRefNullableId(ReceiverIdProperty); }
            set { SetRefNullableId(ReceiverIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiverProperty = P<FixtureReceive>.RegisterRef(e => e.Receiver, ReceiverIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee Receiver
        {
            get { return GetRefEntity(ReceiverProperty); }
            set { SetRefEntity(ReceiverProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<FixtureReceive>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<FixtureReceive>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<FixtureReceive>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double DepartmentId
        {
            get { return (double)GetRefId(DepartmentIdProperty); }
            set { SetRefId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<FixtureReceive>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 接收单状态 ReceiveBillStatus
        /// <summary>
        /// 接收单状态
        /// </summary>
        [Label("接收单状态")]
        public static readonly Property<ReceiveBillStatus> ReceiveBillStatusProperty = P<FixtureReceive>.Register(e => e.ReceiveBillStatus);

        /// <summary>
        /// 接收单状态
        /// </summary>
        public ReceiveBillStatus ReceiveBillStatus
        {
            get { return GetProperty(ReceiveBillStatusProperty); }
            set { SetProperty(ReceiveBillStatusProperty, value); }
        }
        #endregion

        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<FixtureReceive>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return GetProperty(ReceiveTypeProperty); }
            set { SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion


        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<FixtureReceive>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion


        #region 部门名称 DepartmentName
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> DepartmentNameProperty = P<FixtureReceive>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion


    }

    /// <summary>
    /// 工治具接收 实体配置
    /// </summary>
    internal class FixtureReceiveConfig : EntityConfig<FixtureReceive>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXT_RECV").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}