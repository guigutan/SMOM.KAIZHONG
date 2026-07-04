using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ApiInterfaces;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.Resources.Employees;
using System;
using System.Linq;

namespace SIE.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 人员借调API接口控制器
    /// </summary>
    public partial class OnLoanController : DomainController
    {
        /// <summary>
        /// 班组人员信息查询
        /// </summary>
        /// <param name="queryInfo">班组考勤查询条件API</param>
        /// <returns>班组人员信息列表</returns>
        [ApiService("班组人员信息查询")]
        [return: ApiReturn("班组人员信息. 参数类型: List<WorkGroupInfo>")]
        public virtual WorkGroupInfoList GetWorkGroupInfos(WorkGroupQueryInfo queryInfo)
        {
            var workGroupInfoList = new WorkGroupInfoList();
            if(queryInfo == null)
            {
                return workGroupInfoList;
            }    
            queryInfo.EmployeeId = RT.IdentityId;
            var curEmployee = CheckWorkGroupQueryInfo(queryInfo);
            var myWorkGroupId = curEmployee.WorkGroupId.Value; //获取登录人所在班组Id
            var curDate = RF.Find<WorkGroupOnLoan>().GetDbTime().Date;
            var curPagingInfo = CreatePagingInfo(queryInfo.PageNumber, queryInfo.PageSize, true);
            var workGroupVacancyList = GetWorkGroupVacancys(curDate, myWorkGroupId, curPagingInfo);
            if (workGroupVacancyList != null && workGroupVacancyList.Any())
            {
                workGroupInfoList.TotalCount = curPagingInfo.TotalCount;
                foreach (var vacancyitem in workGroupVacancyList)
                {
                    var curWorkGroupInfo = CreateWorkGroupInfo(vacancyitem);
                    workGroupInfoList.WorkGroupInfos.Add(curWorkGroupInfo);
                }
            }

            return workGroupInfoList;
        }

        /// <summary>
        /// 借调单查询
        /// </summary>
        /// <param name="queryInfo">借调单查询API信息</param>
        /// <returns>借调单查询API信息集合</returns>
        [ApiService("借调单查询")]
        [return: ApiReturn("借调单列表. 参数类型: OnLoanInfo")]
        public virtual OnLoanInfoList GetOnLoanInfos([ApiParameter("借调单查询API信息")] OnLoanQueryInfo queryInfo)
        {
            var onLoanInfoList = new OnLoanInfoList();
            if (queryInfo == null)
            {
                return onLoanInfoList;
            }
            CheckOnLoanQueryInfo(queryInfo);
            var curPagingInfo = CreatePagingInfo(queryInfo.PageNumber, queryInfo.PageSize, true);
            switch (queryInfo.QueryMode)
            {
                case 0: //发起人查询
                    QueryByFqr(onLoanInfoList, curPagingInfo);
                    break;
                case 1: //审核人查询
                    QueryByShr(onLoanInfoList, curPagingInfo);
                    break;
                default:
                    break;
            }

            return onLoanInfoList;
        }

        /// <summary>
        /// 获取借调单号
        /// </summary>
        /// <returns>借调单号</returns>
        [ApiService("获取借调单号")]
        [return: ApiReturn("借调单号. 参数类型: string")]
        public virtual string GetOnLoanNo()
        {
            var no = GetWorkGroupOnLoanNo();
            return no;
        }

        /// <summary>
        /// 创建借调单
        /// </summary>
        /// <param name="onLoanInfo">借调单API信息</param>
        [ApiService("创建借调单")]
        [return: ApiReturn("无返回结果")]
        public virtual void CreateOnLoan([ApiParameter("借调单API信息")] OnLoanInfo onLoanInfo)
        {
            if(onLoanInfo == null)
            {
                throw new ValidationException("创建借调单提交失败, 异常信息:借调单API信息不能为空".L10N());
            }
            CheckOnLoanInfo(onLoanInfo);
            try
            {
                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    WorkGroupOnLoan onLoanMain = CreateWorkGroupOnLoan(onLoanInfo); //创建借调单
                    OnLoanDetail detailItem = CreateOnLoanDetail(GetRowIndex(onLoanMain), onLoanMain.Id, RT.IdentityId, ApprovalState.Launch, onLoanInfo.Remark); //新增审核流程--发起
                    var detailInReview = CreateOnLoanDetail(GetRowIndex(onLoanMain) + 1, onLoanMain.Id, onLoanMain.ApproverId, ApprovalState.InReview, string.Empty); //新增审核流程--审核中
                    RF.Save(onLoanMain);
                    RF.Save(detailItem);
                    RF.Save(detailInReview);
                    tran.Complete();
                }
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("创建借调单提交失败, 异常信息:[{0}]".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 借调单修改
        /// </summary>
        /// <param name="onLoanInfo">借调单API信息</param>
        [ApiService("借调单修改")]
        [return: ApiReturn("无返回结果")]
        public virtual void UpdateOnLoan([ApiParameter("借调单API信息")] OnLoanInfo onLoanInfo)
        {
            var onLoan = CheckUpdateOnLoan(onLoanInfo); //检查修改内容
            try
            {
                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    UpdateWorkGroupOnLoan(onLoan, onLoanInfo);  //修改借调单
                    OnLoanDetail detailItem = CreateOnLoanDetail(GetRowIndex(onLoan), onLoan.Id, RT.IdentityId, ApprovalState.Update, onLoanInfo.Remark); //新增审核流程--修改
                    var detailInReview = CreateOnLoanDetail(GetRowIndex(onLoan) + 1, onLoan.Id, onLoan.ApproverId, ApprovalState.InReview, string.Empty); //新增审核流程--审核中
                    RF.Save(onLoan);
                    RF.Save(detailItem);
                    RF.Save(detailInReview);
                    tran.Complete();
                }
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("借调单修改提交失败, 异常信息:[{0}]".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 借调单撤销
        /// </summary>
        /// <param name="onLoanId">班组借调Id</param>
        [ApiService("借调单撤销")]
        [return: ApiReturn("无返回结果")]
        public virtual void CancelOnLoan([ApiParameter("借调单Id")] double onLoanId)
        {
            WorkGroupOnLoan onLoan = CheckCancel(onLoanId);
            try
            {
                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    onLoan.State = OnLoanState.Cancel; //撤销借调单 //新增借调明细撤销记录
                    OnLoanDetail detailItem = CreateOnLoanDetail(GetRowIndex(onLoan), onLoanId, RT.IdentityId, ApprovalState.Repeal, string.Empty);
                    RF.Save(onLoan);
                    RF.Save(detailItem);
                    tran.Complete();
                }
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("借调单撤销提交失败, 异常信息:[{0}]".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 借调单审核
        /// </summary>
        /// <param name="approveInfo">借调单审核API信息</param>
        [ApiService("借调单审核")]
        [return: ApiReturn("无返回结果")]
        public virtual void ApproveOnLoan([ApiParameter("借调单审核API信息")] ApproveInfo approveInfo)
        {
            var onLoan = CheckApproveInfo(approveInfo); //检查审核内容
            try
            {
                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    UpdateWorkGroupOnLoan(onLoan, approveInfo);
                    if (approveInfo.ApproveResult == 0 && approveInfo.EmployeeList.Length > 0)
                    {
                        var onLoanEmps = CreateOnLoanEmployees(onLoan.Id, approveInfo.EmployeeList);
                        RF.Save(onLoanEmps);
                        var employeeIds = approveInfo.EmployeeList.ToList().ConvertAll(d => Convert.ToDouble(d)).ToList();
                        RT.Service.Resolve<ClockInController>().UpdateEmployeeClockInLoan(employeeIds);
                    }

                    var detailItemState = GetApprovalState(approveInfo); //新增审核或拒绝记录
                    OnLoanDetail detailItem = CreateOnLoanDetail(GetRowIndex(onLoan), onLoan.Id, RT.IdentityId, detailItemState, approveInfo.Opinion);
                    OnLoanDetail detailUpdating = null;
                    if (approveInfo.ApproveResult == 1)
                    {
                        detailUpdating = CreateOnLoanDetail(GetRowIndex(onLoan) + 1, onLoan.Id, onLoan.InitiatorId, ApprovalState.Updating, string.Empty);
                    }

                    RF.Save(onLoan);
                    RF.Save(detailItem);
                    if (detailUpdating != null)
                        RF.Save(detailUpdating);

                    tran.Complete();
                }
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("借调单审核提交失败, 异常信息:[{0}]".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 获取可借调人员
        /// </summary>
        /// <param name="workGroupId">班组Id</param>
        /// <param name="onLoanId">班组借调Id</param>
        /// <returns>可借调人员列表</returns>
        [ApiService("获取可借调人员")]
        [return: ApiReturn("借调人员信息. 参数类型: EntityList<Employee>")]
        public virtual EntityList<Employee> GetOnLoanAbleEmployees([ApiParameter("班组Id")] double workGroupId, [ApiParameter("借调单Id")] double onLoanId)
        {
            var onLoan = Query<WorkGroupOnLoan>().Where(p => p.Id == onLoanId).FirstOrDefault();
            if (onLoan == null) { throw new ValidationException("借调单ID[{0}]错误".L10nFormat(onLoanId)); }
            var abnormalEmpClocks = GetAbnormalEmployeeClockIns(workGroupId); //异常出勤名单
            var onLoanedList = GetWorkGroupOnLoans(workGroupId, OnLoanState.Agree, onLoan.BeginDate, onLoan.EndDate); //已借调的班组借调集合
            var employees = GetOnLoanAbleEmployees(abnormalEmpClocks, onLoanedList, workGroupId);

            return employees;
        }
    }
}