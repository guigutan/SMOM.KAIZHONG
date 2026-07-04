using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using System;

namespace SIE.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 班组借调
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig), "班组借调单号生成规则", "用于配置生成班组借调单号")]
    //[CriteriaQuery]
    [ConditionQueryType(typeof(WorkGroupOnLoanCriteria))]
    [Label("班组借调")]
    [DisplayMember(nameof(WorkGroupOnLoan.No))] 
    public class WorkGroupOnLoan : DataEntity
    {
        #region 借调单号 No
        /// <summary>
        /// 借调单号
        /// </summary>
        [Label("借调单号")]
        [Required]
        public static readonly Property<string> NoProperty = P<WorkGroupOnLoan>.Register(e => e.No);

        /// <summary>
        /// 借调单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 借入班组 GroupIn
        /// <summary>
        /// 借入班组Id
        /// </summary>
        [Label("借入班组")]
        public static readonly IRefIdProperty GroupInIdProperty =
            P<WorkGroupOnLoan>.RegisterRefId(e => e.GroupInId, ReferenceType.Normal);

        /// <summary>
        /// 借入班组Id
        /// </summary>
        public double GroupInId
        {
            get { return (double)this.GetRefId(GroupInIdProperty); }
            set { this.SetRefId(GroupInIdProperty, value); }
        }

        /// <summary>
        /// 借入班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> GroupInProperty =
            P<WorkGroupOnLoan>.RegisterRef(e => e.GroupIn, GroupInIdProperty);

        /// <summary>
        /// 借入班组
        /// </summary>
        public WorkGroup GroupIn
        {
            get { return this.GetRefEntity(GroupInProperty); }
            set { this.SetRefEntity(GroupInProperty, value); }
        }
        #endregion

        #region 借出班组 GroupOut
        /// <summary>
        /// 借出班组Id
        /// </summary>
        [Label("借出班组")]
        public static readonly IRefIdProperty GroupOutIdProperty =
            P<WorkGroupOnLoan>.RegisterRefId(e => e.GroupOutId, ReferenceType.Normal);

        /// <summary>
        /// 借出班组Id
        /// </summary>
        public double GroupOutId
        {
            get { return (double)this.GetRefId(GroupOutIdProperty); }
            set { this.SetRefId(GroupOutIdProperty, value); }
        }

        /// <summary>
        /// 借出班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> GroupOutProperty =
            P<WorkGroupOnLoan>.RegisterRef(e => e.GroupOut, GroupOutIdProperty);

        /// <summary>
        /// 借出班组
        /// </summary>
        public WorkGroup GroupOut
        {
            get { return this.GetRefEntity(GroupOutProperty); }
            set { this.SetRefEntity(GroupOutProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty =
            P<WorkGroupOnLoan>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)this.GetRefId(ShiftIdProperty); }
            set { this.SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty =
            P<WorkGroupOnLoan>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return this.GetRefEntity(ShiftProperty); }
            set { this.SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 发起时间 InitiateDate
        /// <summary>
        /// 发起时间
        /// </summary>
        [Label("发起时间")]
        public static readonly Property<DateTime> InitiateDateProperty = P<WorkGroupOnLoan>.Register(e => e.InitiateDate);

        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime InitiateDate
        {
            get { return this.GetProperty(InitiateDateProperty); }
            set { this.SetProperty(InitiateDateProperty, value); }
        }
        #endregion

        #region 需求人数 DemandQty
        /// <summary>
        /// 需求人数
        /// </summary>
        [Label("需求人数")]
        [MinValue(1)]
        public static readonly Property<decimal> DemandQtyProperty = P<WorkGroupOnLoan>.Register(e => e.DemandQty);

        /// <summary>
        /// 需求人数
        /// </summary>
        public decimal DemandQty
        {
            get { return this.GetProperty(DemandQtyProperty); }
            set { this.SetProperty(DemandQtyProperty, value); }
        }
        #endregion

        #region 借调开始时间 BeginDate
        /// <summary>
        /// 借调开始时间
        /// </summary>
        [Label("借调开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<WorkGroupOnLoan>.Register(e => e.BeginDate);

        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return this.GetProperty(BeginDateProperty); }
            set { this.SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 借调结束时间 EndDate
        /// <summary>
        /// 借调结束时间
        /// </summary>
        [Label("借调结束时间")]
        public static readonly Property<DateTime> EndDateProperty = P<WorkGroupOnLoan>.Register(e => e.EndDate);

        /// <summary>
        /// 借调结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return this.GetProperty(EndDateProperty); }
            set { this.SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 审核时间 ApprovalDate
        /// <summary>
        /// 审核
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime?> ApprovalDateProperty = P<WorkGroupOnLoan>.Register(e => e.ApprovalDate);

        /// <summary>
        /// 审核
        /// </summary>
        public DateTime? ApprovalDate
        {
            get { return this.GetProperty(ApprovalDateProperty); }
            set { this.SetProperty(ApprovalDateProperty, value); }
        }
        #endregion

        #region 审核人 Approver
        /// <summary>
        /// 审核人Id
        /// </summary>
        [Label("审核人")]
        public static readonly IRefIdProperty ApproverIdProperty =
            P<WorkGroupOnLoan>.RegisterRefId(e => e.ApproverId, ReferenceType.Normal);

        /// <summary>
        /// 审核人Id
        /// </summary>
        public double ApproverId
        {
            get { return (double)this.GetRefId(ApproverIdProperty); }
            set { this.SetRefId(ApproverIdProperty, value); }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApproverProperty =
            P<WorkGroupOnLoan>.RegisterRef(e => e.Approver, ApproverIdProperty);

        /// <summary>
        /// 审核人
        /// </summary>
        public Employee Approver
        {
            get { return this.GetRefEntity(ApproverProperty); }
            set { this.SetRefEntity(ApproverProperty, value); }
        }
        #endregion

        #region 发起人 Initiator
        /// <summary>
        /// 发起人Id
        /// </summary>
        [Label("发起人")]
        public static readonly IRefIdProperty InitiatorIdProperty =
            P<WorkGroupOnLoan>.RegisterRefId(e => e.InitiatorId, ReferenceType.Normal);

        /// <summary>
        /// 发起人Id
        /// </summary>
        public double InitiatorId
        {
            get { return (double)this.GetRefId(InitiatorIdProperty); }
            set { this.SetRefId(InitiatorIdProperty, value); }
        }

        /// <summary>
        /// 发起人
        /// </summary>
        public static readonly RefEntityProperty<Employee> InitiatorProperty =
            P<WorkGroupOnLoan>.RegisterRef(e => e.Initiator, InitiatorIdProperty);

        /// <summary>
        /// 发起人
        /// </summary>
        public Employee Initiator
        {
            get { return this.GetRefEntity(InitiatorProperty); }
            set { this.SetRefEntity(InitiatorProperty, value); }
        }
        #endregion 

        #region 借调单状态 State
        /// <summary>
        /// 借调单状态
        /// </summary>
        [Label("借调单状态")]
        public static readonly Property<OnLoanState> StateProperty = P<WorkGroupOnLoan>.Register(e => e.State);

        /// <summary>
        /// 借调单状态
        /// </summary>
        public OnLoanState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 借调明细列表 DetailList
        /// <summary>
        /// 借调明细列表
        /// </summary>
        [Label("借调明细列表")]
        public static readonly ListProperty<EntityList<OnLoanDetail>> DetailListProperty = P<WorkGroupOnLoan>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 借调明细列表
        /// </summary>
        public EntityList<OnLoanDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 员工列表 EmployeeList
        /// <summary>
        /// 员工列表
        /// </summary>
        public static readonly ListProperty<EntityList<OnLoanEmployee>> EmployeeListProperty = P<WorkGroupOnLoan>.RegisterList(e => e.EmployeeList);

        /// <summary>
        /// 员工列表
        /// </summary>
        public EntityList<OnLoanEmployee> EmployeeList
        {
            get { return this.GetLazyList(EmployeeListProperty); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<WorkGroupOnLoan>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 借调时长 LoanHour
        /// <summary>
        /// 借调时长（扣去休息时间）
        /// </summary>  
        [Label("借调时长")]
        public static readonly Property<decimal> LoanHourProperty = P<WorkGroupOnLoan>.Register(e => e.LoanHour);

        /// <summary>
        /// 借调时长
        /// </summary>
        public decimal LoanHour
        {
            get { return this.GetProperty(LoanHourProperty); }
            set { this.SetProperty(LoanHourProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 发起人编码 InitiatorCode
        /// <summary>
        /// 发起人编码
        /// </summary>
        [Label("发起人编码")]
        public static readonly Property<string> InitiatorCodeProperty = P<WorkGroupOnLoan>.RegisterView(e => e.InitiatorCode, p => p.Initiator.Code);

        /// <summary>
        /// 发起人编码
        /// </summary>
        public string InitiatorCode
        {
            get { return this.GetProperty(InitiatorCodeProperty); }
        }
        #endregion

        #region 发起人名称 InitiatorName
        /// <summary>
        /// 发起人名称
        /// </summary>
        [Label("发起人名称")]
        public static readonly Property<string> InitiatorNameProperty = P<WorkGroupOnLoan>.RegisterView(e => e.InitiatorName, p => p.Initiator.Name);

        /// <summary>
        /// 发起人名称
        /// </summary>
        public string InitiatorName
        {
            get { return this.GetProperty(InitiatorNameProperty); }
        }
        #endregion

        #region 借入班组编码 GroupInCode
        /// <summary>
        /// 借入班组编码
        /// </summary>
        [Label("借入班组编码")]
        public static readonly Property<string> GroupInCodeProperty = P<WorkGroupOnLoan>.RegisterView(e => e.GroupInCode, p => p.GroupIn.Code);

        /// <summary>
        /// 借入班组编码
        /// </summary>
        public string GroupInCode
        {
            get { return this.GetProperty(GroupInCodeProperty); }
        }
        #endregion

        #region 借入班组名称 GroupInName
        /// <summary>
        /// 借入班组名称
        /// </summary>
        [Label("借入班组名称")]
        public static readonly Property<string> GroupInNameProperty = P<WorkGroupOnLoan>.RegisterView(e => e.GroupInName, p => p.GroupIn.Name);

        /// <summary>
        /// 借入班组名称
        /// </summary>
        public string GroupInName
        {
            get { return this.GetProperty(GroupInNameProperty); }
        }
        #endregion

        #region 审核人编码 ApproverCode
        /// <summary>
        /// 审核人编码
        /// </summary>
        [Label("审核人编码")]
        public static readonly Property<string> ApproverCodeProperty = P<WorkGroupOnLoan>.RegisterView(e => e.ApproverCode, p => p.Approver.Code);

        /// <summary>
        /// 审核人编码
        /// </summary>
        public string ApproverCode
        {
            get { return this.GetProperty(ApproverCodeProperty); }
        }
        #endregion

        #region 审核人名称 ApproverName
        /// <summary>
        /// 审核人名称
        /// </summary>
        [Label("审核人名称")]
        public static readonly Property<string> ApproverNameProperty = P<WorkGroupOnLoan>.RegisterView(e => e.ApproverName, p => p.Approver.Name);

        /// <summary>
        /// 审核人名称
        /// </summary>
        public string ApproverName
        {
            get { return this.GetProperty(ApproverNameProperty); }
        }
        #endregion

        #region 借出班组编码 GroupOutCode
        /// <summary>
        /// 借出班组编码
        /// </summary>
        [Label("借出班组编码")]
        public static readonly Property<string> GroupOutCodeProperty = P<WorkGroupOnLoan>.RegisterView(e => e.GroupOutCode, p => p.GroupOut.Code);

        /// <summary>
        /// 借出班组编码
        /// </summary>
        public string GroupOutCode
        {
            get { return this.GetProperty(GroupOutCodeProperty); }
        }
        #endregion

        #region 借出班组名称 GroupOutName
        /// <summary>
        /// 借出班组名称
        /// </summary>
        [Label("借出班组名称")]
        public static readonly Property<string> GroupOutNameProperty = P<WorkGroupOnLoan>.RegisterView(e => e.GroupOutName, p => p.GroupOut.Name);

        /// <summary>
        /// 借出班组名称
        /// </summary>
        public string GroupOutName
        {
            get { return this.GetProperty(GroupOutNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 班组借调 实体配置
    /// </summary>
    internal class WorkGroupOnLoanConfig : EntityConfig<WorkGroupOnLoan>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_ON_LOAN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
