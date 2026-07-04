using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.FixtureTypes;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.MaintainTasks
{
    /// <summary>
	/// 工治具保养任务
	/// </summary>
	[RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig), "工治具保养任务编号配置项", "工治具保养任务编号配置规则")]
    [ConditionQueryType(typeof(MaintainTaskCriteria))]
    [Label("工治具保养任务")]
    [DisplayMember(nameof(No))]
    public partial class MaintainTask : DataEntity
    {
        #region 保养任务编号 No
        /// <summary>
        /// 保养任务编号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("保养任务编号")]
        public static readonly Property<string> NoProperty = P<MaintainTask>.Register(e => e.No);

        /// <summary>
        /// 保养任务编号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 关联单号 RelatedNo
        /// <summary>
        /// 关联单号
        /// </summary>
        [Label("关联单号")]
        public static readonly Property<string> RelatedNoProperty = P<MaintainTask>.Register(e => e.RelatedNo);

        /// <summary>
        /// 关联单号
        /// </summary>
        public string RelatedNo
        {
            get { return GetProperty(RelatedNoProperty); }
            set { SetProperty(RelatedNoProperty, value); }
        }
        #endregion

        #region 治具数量 Qty
        /// <summary>
        /// 治具数量
        /// </summary>
        [Label("治具数量")]
        [MinValue(0)]
        public static readonly Property<int> QtyProperty = P<MaintainTask>.Register(e => e.Qty);

        /// <summary>
        /// 治具数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 合格数量 PassQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        [MinValue(0)]
        public static readonly Property<int?> PassQtyProperty = P<MaintainTask>.Register(e => e.PassQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public int? PassQty
        {
            get { return GetProperty(PassQtyProperty); }
            set { SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        [MinValue(0)]
        public static readonly Property<int?> NgQtyProperty = P<MaintainTask>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int? NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 保养完成时间 FinishDate
        /// <summary>
        /// 保养完成时间
        /// </summary>
        [Label("保养完成时间")]
        public static readonly Property<DateTime?> FinishDateProperty = P<MaintainTask>.Register(e => e.FinishDate);

        /// <summary>
        /// 保养完成时间
        /// </summary>
        public DateTime? FinishDate
        {
            get { return GetProperty(FinishDateProperty); }
            set { SetProperty(FinishDateProperty, value); }
        }
        #endregion

        #region 保养单创建时间 ApplyDate
        /// <summary>
        /// 保养单创建时间
        /// </summary>
        [Label("保养单创建时间")]
        public static readonly Property<DateTime> ApplyDateProperty = P<MaintainTask>.Register(e => e.ApplyDate);

        /// <summary>
        /// 保养单创建时间
        /// </summary>
        public DateTime ApplyDate
        {
            get { return GetProperty(ApplyDateProperty); }
            set { SetProperty(ApplyDateProperty, value); }
        }
        #endregion

        #region 保养状态 State
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<MaintainState> StateProperty = P<MaintainTask>.Register(e => e.State);

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintainState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工治具台账 FixtureAccount
        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<MaintainTask>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<MaintainTask>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具台账
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 保养执行详情 Details
        /// <summary>
        /// 保养执行详情
        /// </summary>
        public static readonly ListProperty<EntityList<MaintainTaskDetail>> DetailsProperty = P<MaintainTask>.RegisterList(e => e.Details);
        /// <summary>
        /// 保养执行详情
        /// </summary>
        public EntityList<MaintainTaskDetail> Details
        {
            get { return this.GetLazyList(DetailsProperty); }
        }
        #endregion

        #region 更换记录 Records
        /// <summary>
        /// 更换记录
        /// </summary>
        public static readonly ListProperty<EntityList<MaintainChangeRecord>> RecordsProperty = P<MaintainTask>.RegisterList(e => e.Records);
        /// <summary>
        /// 更换记录
        /// </summary>
        public EntityList<MaintainChangeRecord> Records
        {
            get { return this.GetLazyList(RecordsProperty); }
        }
        #endregion

        #region 保养触发条件 MaintainType
        /// <summary>
        /// 保养触发条件
        /// </summary>
        [Label("保养触发条件")]
        public static readonly Property<MaintainType> MaintainTypeProperty = P<MaintainTask>.Register(e => e.MaintainType);

        /// <summary>
        /// 保养触发条件
        /// </summary>
        public MaintainType MaintainType
        {
            get { return GetProperty(MaintainTypeProperty); }
            set { SetProperty(MaintainTypeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工治具ID AccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> AccountCodeProperty = P<MaintainTask>.RegisterView(e => e.AccountCode, p => p.FixtureAccount.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string AccountCode
        {
            get { return this.GetProperty(AccountCodeProperty); }
        }
        #endregion

        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<MaintainTask>.RegisterView(e => e.EncodeCode, p => p.FixtureAccount.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<MaintainTask>.RegisterView(e => e.ModelCode, p => p.FixtureAccount.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<MaintainTask>.RegisterView(e => e.ModelName, p => p.FixtureAccount.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
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
        public static readonly Property<string> FixtureTypeProperty = P<MaintainTask>.RegisterView(e => e.FixtureType, p => p.FixtureAccount.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库字段
        #region 存在合格结果 HasPass
        /// <summary>
        /// 存在合格结果
        /// </summary>
        [Label("存在合格结果")]
        public static readonly Property<bool> HasPassProperty = P<MaintainTask>.Register(e => e.HasPass);

        /// <summary>
        /// 存在合格结果
        /// </summary>
        public bool HasPass
        {
            get { return this.GetProperty(HasPassProperty); }
            set { this.SetProperty(HasPassProperty, value); }
        }
        #endregion

        #region 存在不合格结果 HasNg
        /// <summary>
        /// 存在不合格结果
        /// </summary>
        [Label("存在不合格结果")]
        public static readonly Property<bool> HasNgProperty = P<MaintainTask>.Register(e => e.HasNg);

        /// <summary>
        /// 存在不合格结果
        /// </summary>
        public bool HasNg
        {
            get { return this.GetProperty(HasNgProperty); }
            set { this.SetProperty(HasNgProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 保养任务 实体配置
    /// </summary>
    internal class MaintainTaskConfig : EntityConfig<MaintainTask>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_TASK").MapAllProperties();
            Meta.Property(MaintainTask.HasPassProperty).DontMapColumn();
            Meta.Property(MaintainTask.HasNgProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 工治具台帐保养履历扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public static class MaintainTaskDetailProperty
    {
        /// <summary>
        /// 工治具台帐保养履历属性
        /// </summary>
        public static readonly ListProperty<EntityList<MaintainTask>> MaintainTaskListProperty =
            P<FixtureIDAccount>.RegisterExtensionList<EntityList<MaintainTask>>("MaintainTaskList", typeof(MaintainTaskDetailProperty));

        /// <summary>
        /// 获取工治具台帐保养履历对象
        /// </summary>
        /// <param name="me">工治具台帐对象</param>
        /// <returns>返回工治具台帐保养履历对象</returns>
        public static EntityList<MaintainTask> GetMaintainTaskList(FixtureIDAccount me)
        {
            return me.GetProperty(MaintainTaskListProperty);
        }

        /// <summary>
        /// 设置工治具台帐保养履历对象
        /// </summary>
        /// <param name="me">工治具台帐对象</param>
        /// <param name="value">需要设置的工治具台帐保养履历对象</param>
        public static void SetMaintainTaskList(FixtureIDAccount me, EntityList<MaintainTask> value)
        {
            me.SetProperty(MaintainTaskListProperty, value);
        }
    }

    /// <summary>
    /// 工治具台帐保养履历 实体配置
    /// </summary>
    internal class MaintainTaskDetailPropertyConfig : EntityConfig<FixtureIDAccount>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(MaintainTaskDetailProperty.MaintainTaskListProperty).DontMapColumn();
        }
    }

    /// <summary>
    /// 工治具台帐保养履历扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public static class TaskDetailProperty
    {
        /// <summary>
        /// 工治具台帐保养履历属性
        /// </summary>
        public static readonly ListProperty<EntityList<MaintainTask>> MaintainTaskListProperty =
            P<FixtureCodeAccount>.RegisterExtensionList<EntityList<MaintainTask>>("MaintainTaskList", typeof(TaskDetailProperty));

        /// <summary>
        /// 获取工治具台帐保养履历对象
        /// </summary>
        /// <param name="me">工治具台帐对象</param>
        /// <returns>返回工治具台帐保养履历对象</returns>
        public static EntityList<MaintainTask> GetMaintainTaskList(FixtureCodeAccount me)
        {
            return me.GetProperty(MaintainTaskListProperty);
        }

        /// <summary>
        /// 设置工治具台帐保养履历对象
        /// </summary>
        /// <param name="me">物料</param>
        /// <param name="value">需要设置的工治具台帐保养履历对象</param>
        public static void SetMaintainTaskList(FixtureCodeAccount me, EntityList<MaintainTask> value)
        {
            me.SetProperty(MaintainTaskListProperty, value);
        }
    }

    /// <summary>
    /// 工治具台帐保养履历 实体配置
    /// </summary>
    internal class TaskDetailPropertyConfig : EntityConfig<FixtureCodeAccount>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(TaskDetailProperty.MaintainTaskListProperty).DontMapColumn();
        }
    }
}
