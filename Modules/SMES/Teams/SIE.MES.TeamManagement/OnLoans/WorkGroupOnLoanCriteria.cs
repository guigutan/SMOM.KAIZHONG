using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TeamManagement.OnLoans
{
    [QueryEntity, Serializable]
    public class WorkGroupOnLoanCriteria : Criteria
    {
        #region 借阅单号 No
        /// <summary>
        /// 借阅单号
        /// </summary>
        [Label("借阅单号")]
        public static readonly Property<string> NoProperty = P<WorkGroupOnLoanCriteria>.Register(e => e.No);

        /// <summary>
        /// 借阅单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 发起人 Initiator
        /// <summary>
        /// 发起人Id
        /// </summary>
        [Label("发起人")]
        public static readonly IRefIdProperty InitiatorIdProperty =
            P<WorkGroupOnLoanCriteria>.RegisterRefId(e => e.InitiatorId, ReferenceType.Normal);

        /// <summary>
        /// 发起人Id
        /// </summary>
        public double? InitiatorId
        {
            get { return (double?)this.GetRefNullableId(InitiatorIdProperty); }
            set { this.SetRefNullableId(InitiatorIdProperty, value); }
        }

        /// <summary>
        /// 发起人
        /// </summary>
        public static readonly RefEntityProperty<Employee> InitiatorProperty =
            P<WorkGroupOnLoanCriteria>.RegisterRef(e => e.Initiator, InitiatorIdProperty);

        /// <summary>
        /// 发起人
        /// </summary>
        public Employee Initiator
        {
            get { return this.GetRefEntity(InitiatorProperty); }
            set { this.SetRefEntity(InitiatorProperty, value); }
        }
        #endregion

        #region 借入班组 GroupIn
        /// <summary>
        /// 借入班组Id
        /// </summary>
        [Label("借入班组")]
        public static readonly IRefIdProperty GroupInIdProperty =
            P<WorkGroupOnLoanCriteria>.RegisterRefId(e => e.GroupInId, ReferenceType.Normal);

        /// <summary>
        /// 借入班组Id
        /// </summary>
        public double? GroupInId
        {
            get { return (double?)this.GetRefNullableId(GroupInIdProperty); }
            set { this.SetRefNullableId(GroupInIdProperty, value); }
        }

        /// <summary>
        /// 借入班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> GroupInProperty =
            P<WorkGroupOnLoanCriteria>.RegisterRef(e => e.GroupIn, GroupInIdProperty);

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
            P<WorkGroupOnLoanCriteria>.RegisterRefId(e => e.GroupOutId, ReferenceType.Normal);

        /// <summary>
        /// 借出班组Id
        /// </summary>
        public double? GroupOutId
        {
            get { return (double?)this.GetRefNullableId(GroupOutIdProperty); }
            set { this.SetRefNullableId(GroupOutIdProperty, value); }
        }

        /// <summary>
        /// 借出班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> GroupOutProperty =
            P<WorkGroupOnLoanCriteria>.RegisterRef(e => e.GroupOut, GroupOutIdProperty);

        /// <summary>
        /// 借出班组
        /// </summary>
        public WorkGroup GroupOut
        {
            get { return this.GetRefEntity(GroupOutProperty); }
            set { this.SetRefEntity(GroupOutProperty, value); }
        }
        #endregion

        #region 审核人 Approver
        /// <summary>
        /// 审核人Id
        /// </summary>
        [Label("审核人")]
        public static readonly IRefIdProperty ApproverIdProperty =
            P<WorkGroupOnLoanCriteria>.RegisterRefId(e => e.ApproverId, ReferenceType.Normal);

        /// <summary>
        /// 审核人Id
        /// </summary>
        public double? ApproverId
        {
            get { return (double?)this.GetRefNullableId(ApproverIdProperty); }
            set { this.SetRefNullableId(ApproverIdProperty, value); }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApproverProperty =
            P<WorkGroupOnLoanCriteria>.RegisterRef(e => e.Approver, ApproverIdProperty);

        /// <summary>
        /// 审核人
        /// </summary>
        public Employee Approver
        {
            get { return this.GetRefEntity(ApproverProperty); }
            set { this.SetRefEntity(ApproverProperty, value); }
        }
        #endregion

        #region 借调单状态 State
        /// <summary>
        /// 借调单状态
        /// </summary>
        [Label("借调单状态")]
        public static readonly Property<OnLoanState?> StateProperty = P<WorkGroupOnLoanCriteria>.Register(e => e.State);

        /// <summary>
        /// 借调单状态
        /// </summary>
        public OnLoanState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WorkGroupOnLoanController>().GetWorkGroupOnLoans(this);
        }

    }
}
