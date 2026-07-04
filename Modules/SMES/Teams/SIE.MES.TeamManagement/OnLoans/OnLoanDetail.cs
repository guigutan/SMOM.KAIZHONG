using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 借调明细
    /// </summary>
    [ChildEntity, Serializable]
    ////[CriteriaQuery]
    [Label("借调明细")]
    [DisplayMember(nameof(OnLoanDetail.RowIndex))]
    public class OnLoanDetail : DataEntity
    {
        #region 班组借调 OnLoan
        /// <summary>
        /// 班组借调Id
        /// </summary>
        [Label("班组借调")]
        public static readonly IRefIdProperty OnLoanIdProperty =
            P<OnLoanDetail>.RegisterRefId(e => e.OnLoanId, ReferenceType.Parent);

        /// <summary>
        /// 班组借调Id
        /// </summary>
        public double OnLoanId
        {
            get { return (double)this.GetRefId(OnLoanIdProperty); }
            set { this.SetRefId(OnLoanIdProperty, value); }
        }

        /// <summary>
        /// 班组借调
        /// </summary>
        public static readonly RefEntityProperty<WorkGroupOnLoan> OnLoanProperty =
            P<OnLoanDetail>.RegisterRef(e => e.OnLoan, OnLoanIdProperty);

        /// <summary>
        /// 班组借调
        /// </summary>
        public WorkGroupOnLoan OnLoan
        {
            get { return this.GetRefEntity(OnLoanProperty); }
            set { this.SetRefEntity(OnLoanProperty, value); }
        }
        #endregion

        #region 行号 RowIndex
        /// <summary>
        /// 行号
        /// </summary>
        [Label("属性名")]
        public static readonly Property<decimal> RowIndexProperty = P<OnLoanDetail>.Register(e => e.RowIndex);

        /// <summary>
        /// 行号
        /// </summary>
        public decimal RowIndex
        {
            get { return this.GetProperty(RowIndexProperty); }
            set { this.SetProperty(RowIndexProperty, value); }
        }
        #endregion

        #region 操作时间 OperateDate
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("属性名")]
        public static readonly Property<DateTime> OperateDateProperty = P<OnLoanDetail>.Register(e => e.OperateDate);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateDate
        {
            get { return this.GetProperty(OperateDateProperty); }
            set { this.SetProperty(OperateDateProperty, value); }
        }
        #endregion

        #region 操作人 Operator
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperatorIdProperty =
            P<OnLoanDetail>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperatorId
        {
            get { return (double)this.GetRefId(OperatorIdProperty); }
            set { this.SetRefId(OperatorIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperatorProperty =
            P<OnLoanDetail>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operator
        {
            get { return this.GetRefEntity(OperatorProperty); }
            set { this.SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 审核状态 State
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalState> StateProperty = P<OnLoanDetail>.Register(e => e.State);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<OnLoanDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 借调单号 OnLoanNo
        /// <summary>
        /// 借调单号
        /// </summary>
        [Label("借调单号")]
        public static readonly Property<string> OnLoanNoProperty = P<OnLoanDetail>.RegisterView(e => e.OnLoanNo, p => p.OnLoan.No);

        /// <summary>
        /// 借调单号
        /// </summary>
        public string OnLoanNo
        {
            get { return this.GetProperty(OnLoanNoProperty); }
        }
        #endregion

        #region 操作人编码 OperatorCode
        /// <summary>
        /// 操作人编码
        /// </summary>
        [Label("操作人编码")]
        public static readonly Property<string> OperatorCodeProperty = P<OnLoanDetail>.RegisterView(e => e.OperatorCode, p => p.Operator.Code);

        /// <summary>
        /// 操作人编码
        /// </summary>
        public string OperatorCode
        {
            get { return this.GetProperty(OperatorCodeProperty); }
        }
        #endregion

        #region 操作人名称 OperatorName
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Label("操作人名称")]
        public static readonly Property<string> OperatorNameProperty = P<OnLoanDetail>.RegisterView(e => e.OperatorName, p => p.Operator.Name);

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName
        {
            get { return this.GetProperty(OperatorNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 借调明细 实体配置
    /// </summary>
    internal class OnLoanDetailConfig : EntityConfig<OnLoanDetail>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_ON_LOAN_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
