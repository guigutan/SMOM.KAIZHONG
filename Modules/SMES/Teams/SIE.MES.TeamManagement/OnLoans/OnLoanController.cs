using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ApiInterfaces;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 借调控制器
    /// </summary>
    public partial class OnLoanController : DomainController
    {
        /// <summary>
        /// 获取班组借调By员工Id和日期
        /// </summary>
        /// <param name="empId">员工Id</param>
        /// <param name="date">日期</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>班组借调</returns>
        public virtual EntityList<WorkGroupOnLoan> GetWorkGroupOnLoanByEmp(double empId, DateTime date, PagingInfo pagingInfo)
        {
            return Query<WorkGroupOnLoan>().Join<OnLoanEmployee>((x, y) => x.Id == y.OnLoanId && y.EmployeeId == empId)
                .Where(p => p.BeginDate >= date.Date && p.BeginDate < date.AddDays(1).Date).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 判断是否存在借调
        /// </summary>
        /// <param name="empId">员工Id</param>
        /// <param name="date">日期</param>
        /// <returns>bool</returns>
        public virtual bool IsExistOnLoan(double empId, DateTime date)
        {
            return Query<WorkGroupOnLoan>().Join<OnLoanEmployee>((x, y) => x.Id == y.OnLoanId && y.EmployeeId == empId)
               .Where(p => p.BeginDate >= date.Date && p.BeginDate < date.AddDays(1).Date).ToList().Count > 0;
        }

        /// <summary>
        /// 获取班组缺编信息集合
        /// </summary>
        /// <param name="vacancyDate">日期</param>
        /// <param name="workGroupId">班组Id</param>
        /// <param name="shiftId">班次Id</param>
        /// <param name="wipResourceId">资源Id</param>
        /// <param name="workShopId">车间Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>班组缺编信息集合</returns>
        public virtual EntityList<WorkGroupVacancy> GetWorkGroupVacancys(DateTime? vacancyDate = null, double? workGroupId = null,
            double? shiftId = null, double? wipResourceId = null, double? workShopId = null, PagingInfo pagingInfo = null)
        {
            var querys = Query<WorkGroupVacancy>();
            if (vacancyDate.HasValue)
                querys.Where(x => x.VacancyDate == vacancyDate.Value);
            if (workGroupId.HasValue)
                querys.Where(x => x.WorkGroupId == workGroupId.Value);
            if (shiftId.HasValue)
                querys.Where(x => x.ShiftId == shiftId.Value);
            if (wipResourceId.HasValue)
                querys.Where(x => x.WipResourceId == wipResourceId);
            if (workShopId.HasValue)
                querys.Where(x => x.WorkGroupId == workShopId);
            var workGroupVacancys = querys.ToList(pagingInfo);
            return workGroupVacancys;
        }

        /// <summary>
        /// 过滤当日无班次的班组及指定班组的班组缺编集合
        /// </summary>
        /// <param name="vacancyDate">考勤日期</param>
        /// <param name="notWorkGroupId">班组Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>班组缺编统计集合</returns>
        public virtual EntityList<WorkGroupVacancy> GetWorkGroupVacancys(DateTime vacancyDate, double notWorkGroupId, PagingInfo pagingInfo = null)
        {
            var workGroupVacancyList = Query<WorkGroupVacancy>().Where(p => p.VacancyDate == vacancyDate
                    && p.WorkGroupId != notWorkGroupId).ToList(pagingInfo);
            return workGroupVacancyList;
        }

        /// <summary>
        /// 获取班组缺编信息
        /// </summary>
        /// <param name="vacancyDate">日期</param>
        /// <param name="workGroupId">班组Id</param>
        /// <param name="shiftId">班次Id</param>
        /// <returns>班组缺编信息</returns>
        public virtual WorkGroupVacancy GetDefaultWorkGroupVacancy(DateTime vacancyDate, double workGroupId, double shiftId)
        {
            var curVacancys = GetWorkGroupVacancys(vacancyDate, workGroupId, shiftId, null, null);
            var defaultVacancy = curVacancys.FirstOrDefault();
            return defaultVacancy;
        }

        /// <summary>
        /// 获取班组借调集合
        /// </summary>
        /// <param name="onLoanStates">借调单状态集合</param>
        /// <param name="initiatorId">发起人Id</param>
        /// <param name="approverId">审核人Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>班组借调集合</returns>
        public virtual EntityList<WorkGroupOnLoan> GetWorkGroupOnLoans(List<int> onLoanStates, double? initiatorId = null, double? approverId = null, PagingInfo pagingInfo = null)
        {
            var querys = Query<WorkGroupOnLoan>();
            if (onLoanStates != null && onLoanStates.Any())
                querys.Where(p => onLoanStates.Contains((int)p.State));
            if (initiatorId.HasValue)
                querys.Where(p => p.InitiatorId == initiatorId.Value);
            if (approverId.HasValue)
                querys.Where(p => p.ApproverId == approverId.Value);
            var workGroupOnLoans = querys.OrderBy(p => (int)p.State).ToList(pagingInfo);
            return workGroupOnLoans;
        }

        /// <summary>
        /// 获取借调单的单号
        /// </summary>
        /// <returns>借调单单号</returns>
        public virtual string GetWorkGroupOnLoanNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(WorkGroupOnLoan));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到借调单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule, 1).FirstOrDefault();
        }

        #region 人员借调API私有方法
        /// <summary>
        /// 创建分页信息对象
        /// </summary>
        /// <param name="pageNumber">页号</param>
        /// /// <param name="pageSize">页码</param>
        /// <param name="isNeedCount">是否需要总数</param>
        /// <returns>分页信息对象</returns>
        private PagingInfo CreatePagingInfo(int pageNumber, int pageSize, bool isNeedCount = true)
        {
            PagingInfo pagingInfo = null;
            if (pageNumber > 0 && pageSize > 0)
                pagingInfo = new PagingInfo(pageNumber, pageSize, isNeedCount);
            return pagingInfo;
        }

        /// <summary>
        /// Check班组人员信息查询参数
        /// </summary>
        /// <param name="queryInfo">班组考勤查询条件API信息</param>
        /// <returns>员工</returns>
        private Employee CheckWorkGroupQueryInfo(WorkGroupQueryInfo queryInfo)
        {
            if (queryInfo.PageNumber <= 0 || queryInfo.PageSize <= 0)
                throw new ValidationException("筛选条件中的页号及页码不能<=0".L10N());
            var curEmployee = RF.GetById<Employee>(queryInfo.EmployeeId);
            if (curEmployee == null)
            {
                throw new ValidationException("未查询到员工ID[{0}]!".L10nFormat(queryInfo.EmployeeId));
            }

            if (!curEmployee.WorkGroupId.HasValue)
                throw new ValidationException("员工[{0}]未配置班组信息!".L10nFormat(curEmployee.Code));
            return curEmployee;
        }

        /// <summary>
        /// 班组人员信息API信息
        /// </summary>
        /// <param name="curVacancy">班组缺编统计实体</param>
        /// <returns>班组人员API信息</returns>
        private WorkGroupInfo CreateWorkGroupInfo(WorkGroupVacancy curVacancy)
        {
            var curWorkGroupInfo = new WorkGroupInfo();

            var curMonitor = RT.Service.Resolve<EmployeeController>().GetMonitor(curVacancy.WorkGroupId);
            curWorkGroupInfo.WorkGroupId = curVacancy.WorkGroupId;
            curWorkGroupInfo.WorkGroupCode = curVacancy.WorkGroup.Code;
            curWorkGroupInfo.WorkGroupName = curVacancy.WorkGroup.Name;
            if (curMonitor != null)
            {
                curWorkGroupInfo.EmployeeId = curMonitor.Id;
                curWorkGroupInfo.EmployeeName = curMonitor.Name;
            }
            else
            {
                curWorkGroupInfo.EmployeeId = 0;
                curWorkGroupInfo.EmployeeName = string.Empty;
            }

            curWorkGroupInfo.ResourceCode = curVacancy.WipResource.Code;
            curWorkGroupInfo.ResourceName = curVacancy.WipResource.Name;
            curWorkGroupInfo.ShiftId = curVacancy.ShiftId;
            curWorkGroupInfo.BeginDate = curVacancy.Shift.BeginTime;
            curWorkGroupInfo.EndDate = curVacancy.Shift.EndTime;
            curWorkGroupInfo.ActualQty = curVacancy.ActualQty;
            curWorkGroupInfo.ClockingInQty = curVacancy.ClockingInQty;

            return curWorkGroupInfo;
        }

        /// <summary>
        /// Check借调单查询条件
        /// </summary>
        /// <param name="queryInfo">借调单查询API信息</param>
        private void CheckOnLoanQueryInfo(OnLoanQueryInfo queryInfo)
        {
            if (queryInfo.QueryMode != 0 && queryInfo.QueryMode != 1)
                throw new ValidationException("查询条件中的查询方式值不合法!".L10N());
            if (queryInfo.PageNumber <= 0 || queryInfo.PageSize <= 0)
                throw new ValidationException("查询条件中的页号及页码不能<=0".L10N());
        }

        /// <summary>
        /// 审核人查询
        /// </summary>
        /// <param name="onLoanInfoList">借调单列表</param>
        /// <param name="curPagingInfo">分页信息</param>
        private void QueryByShr(OnLoanInfoList onLoanInfoList, PagingInfo curPagingInfo)
        {
            List<int> onLoanStates = new List<int>() { (int)OnLoanState.ToBeApproved };
            var workGroupOnLoans = GetWorkGroupOnLoans(onLoanStates, null, RT.IdentityId, curPagingInfo);
            var totalCount = curPagingInfo.TotalCount;
            SetOnLoanInfoList(onLoanInfoList, workGroupOnLoans, totalCount);
        }

        /// <summary>
        /// 发起人查询
        /// </summary>
        /// <param name="onLoanInfoList">借调单列表</param>
        /// <param name="curPagingInfo">分页信息</param>
        private void QueryByFqr(OnLoanInfoList onLoanInfoList, PagingInfo curPagingInfo)
        {
            List<int> onLoanStates = new List<int>() { (int)OnLoanState.Refuse, (int)OnLoanState.ToBeApproved };
            var workGroupOnLoans = GetWorkGroupOnLoans(onLoanStates, RT.IdentityId, null, curPagingInfo);
            var totalCount = curPagingInfo.TotalCount;
            SetOnLoanInfoList(onLoanInfoList, workGroupOnLoans, totalCount);
        }

        /// <summary>
        /// 设置借调单API信息集合
        /// </summary>
        /// <param name="onLoanInfoList">借调单API信息集合</param>
        /// <param name="workGroupOnLoans">班组借调集合</param>
        /// <param name="totalCount">借调单总条数</param>
        private void SetOnLoanInfoList(OnLoanInfoList onLoanInfoList, EntityList<WorkGroupOnLoan> workGroupOnLoans, int totalCount)
        {
            var onLoanInfos = CreateOnLoanInfos(workGroupOnLoans);
            if (onLoanInfos != null)
            {
                onLoanInfoList.OnLoanInfos.AddRange(onLoanInfos);
                onLoanInfoList.TotalCount = totalCount;
            }
        }

        /// <summary>
        /// 创建借调单API信息集合
        /// </summary>
        /// <param name="workGroupOnLoans">班组借调集合</param>
        /// <returns>借调单API信息集合</returns>
        private List<OnLoanInfo> CreateOnLoanInfos(EntityList<WorkGroupOnLoan> workGroupOnLoans)
        {
            List<OnLoanInfo> onLoanInfos = null;
            if (workGroupOnLoans != null && workGroupOnLoans.Any())
            {
                onLoanInfos = new List<OnLoanInfo>();
                foreach (var curWorkGroupOnLoan in workGroupOnLoans)
                {
                    var defaultVacancy = GetDefaultWorkGroupVacancy(curWorkGroupOnLoan.BeginDate.Date, curWorkGroupOnLoan.GroupOutId, curWorkGroupOnLoan.ShiftId);
                    var curOnLoanInfo = CreateOnLoanInfo(curWorkGroupOnLoan, defaultVacancy);
                    onLoanInfos.Add(curOnLoanInfo);
                }
            }

            return onLoanInfos;
        }

        /// <summary>
        /// 创建借调单API信息
        /// </summary>
        /// <param name="curWorkGroupOnLoan">班组借调实体</param>
        /// <param name="curWorkGroupVacancy">班组缺编统计实体</param>
        /// <returns>借调单API信息</returns>
        private OnLoanInfo CreateOnLoanInfo(WorkGroupOnLoan curWorkGroupOnLoan, WorkGroupVacancy curWorkGroupVacancy)
        {
            OnLoanInfo curOnLoanInfo = new OnLoanInfo();
            curOnLoanInfo.Id = curWorkGroupOnLoan.Id;
            curOnLoanInfo.No = curWorkGroupOnLoan.No;
            curOnLoanInfo.BeginDate = curWorkGroupOnLoan.BeginDate;
            curOnLoanInfo.EndDate = curWorkGroupOnLoan.EndDate;
            curOnLoanInfo.DemandQty = curWorkGroupOnLoan.DemandQty;
            curOnLoanInfo.GroupOutId = curWorkGroupOnLoan.GroupOutId;
            curOnLoanInfo.GroupOutName = curWorkGroupOnLoan.GroupOut.Name;
            curOnLoanInfo.GroupInId = curWorkGroupOnLoan.GroupInId;
            curOnLoanInfo.GroupInName = curWorkGroupOnLoan.GroupIn.Name;
            curOnLoanInfo.InitiateDate = curWorkGroupOnLoan.InitiateDate;
            curOnLoanInfo.InitiateName = curWorkGroupOnLoan.Initiator.Name;
            curOnLoanInfo.InitiatorId = curWorkGroupOnLoan.Initiator.Id;
            curOnLoanInfo.State = curWorkGroupOnLoan.State.ToLabel();
            curOnLoanInfo.Remark = curWorkGroupOnLoan.Remark;
            curOnLoanInfo.ShiftId = curWorkGroupOnLoan.ShiftId;
            curOnLoanInfo.ShiftBeginTime = curWorkGroupOnLoan.Shift.BeginTime;
            curOnLoanInfo.ShiftEndTime = curWorkGroupOnLoan.Shift.EndTime;
            if (curWorkGroupVacancy != null)
            {
                var curMonitor = RT.Service.Resolve<EmployeeController>().GetMonitor(curWorkGroupOnLoan.GroupOutId);
                if (curMonitor != null)
                {
                    curOnLoanInfo.EmployeeId = curMonitor.Id;
                    curOnLoanInfo.EmployeeName = curMonitor.Name;
                }
                else
                {
                    curOnLoanInfo.EmployeeId = 0;
                    curOnLoanInfo.EmployeeName = string.Empty;
                }

                curOnLoanInfo.ResourceCode = curWorkGroupVacancy.WipResource.Code;
                curOnLoanInfo.ResourceName = curWorkGroupVacancy.WipResource.Name;
                curOnLoanInfo.ActualQty = curWorkGroupVacancy.ActualQty;
                curOnLoanInfo.ClockingInQty = curWorkGroupVacancy.ClockingInQty;
            }

            var orderOnDetails = curWorkGroupOnLoan.DetailList.OrderBy(x => x.RowIndex).ToList();
            curOnLoanInfo.DetailList = CreateCreateOnLoanDetailInfos(orderOnDetails);

            return curOnLoanInfo;
        }

        /// <summary>
        /// 创建借调单明细API信息集合
        /// </summary>
        /// <param name="onLoanDetails">班组借调明细节后</param>
        /// <returns>借调单明细API信息集合</returns>
        private List<OnLoanDetailInfo> CreateCreateOnLoanDetailInfos(List<OnLoanDetail> onLoanDetails)
        {
            List<OnLoanDetailInfo> detailInfoList = null;
            if (onLoanDetails != null && onLoanDetails.Any())
            {
                var detailCount = onLoanDetails.Count;
                detailInfoList = new List<OnLoanDetailInfo>();
                var curRowIndex = 1;
                foreach (var curOnLoanDetail in onLoanDetails)
                {
                    if (CheckShowDetail(curOnLoanDetail, detailCount))
                    {
                        var curDetailInfo = CreateOnLoanDetailInfo(curOnLoanDetail, curRowIndex);
                        detailInfoList.Add(curDetailInfo);
                        curRowIndex++;
                    }
                }
            }

            return detailInfoList;
        }

        /// <summary>
        /// 判断借调流程明细是否显示
        /// </summary>
        /// <param name="curOnLoanDetail">借调流程明细对象</param>
        /// <param name="detailCount">借调流程明细总数</param>
        /// <returns>true:显示; false:不显示</returns>
        private bool CheckShowDetail(OnLoanDetail curOnLoanDetail, int detailCount)
        {
            var check = false;
            if (curOnLoanDetail.RowIndex == detailCount && (curOnLoanDetail.State == ApprovalState.InReview || curOnLoanDetail.State == ApprovalState.Updating))
            {
                check = true;
            }
            else if (curOnLoanDetail.State != ApprovalState.InReview && curOnLoanDetail.State != ApprovalState.Updating)
            {
                check = true;
            }
            else
            {
                //
            }
            return check;
        }

        /// <summary>
        /// 创建借调单明细API信息
        /// </summary>
        /// <param name="curOnLoanDetail">借调明细实体</param>
        /// <param name="curRowIndex">当前行号</param>
        /// <returns>借调单明细API信息</returns>
        private OnLoanDetailInfo CreateOnLoanDetailInfo(OnLoanDetail curOnLoanDetail, int curRowIndex)
        {
            OnLoanDetailInfo curDetailInfo = new OnLoanDetailInfo();
            curDetailInfo.RowIndex = curRowIndex; ////(int)curOnLoanDetail.RowIndex;
            curDetailInfo.Operator = curOnLoanDetail.Operator.Name;
            curDetailInfo.OperateDate = curOnLoanDetail.OperateDate;
            curDetailInfo.Remark = curOnLoanDetail.Remark;
            curDetailInfo.ApprovalState = curOnLoanDetail.State.ToLabel();
            curDetailInfo.ApprovalStateValue = curOnLoanDetail.State;
            return curDetailInfo;
        }

        /// <summary>
        /// 检验借调单输入信息是否合法
        /// </summary>
        /// <param name="onLoanInfo">借调单API信息</param>
        private void CheckOnLoanInfoBase(OnLoanInfo onLoanInfo)
        {
            if (onLoanInfo == null)
                throw new ValidationException("请输入借调单信息!".L10N());
            /*if (onLoanInfo.Remark.IsNullOrWhiteSpace()) //非必填
                throw new ValidationException("备注不能为空!".L10N());*/
            if (onLoanInfo.DemandQty <= 0)
                throw new ValidationException("需求人数不能 <= 0".L10N());
            if (TimeToInt(onLoanInfo.BeginDate) >= TimeToInt(onLoanInfo.EndDate))
                throw new ValidationException("借调开始时间不能晚于借调结束时间".L10N());
        }

        /// <summary>
        /// 检查借调单输入信息是否合法
        /// </summary>
        /// <param name="onLoanInfo">借调单输入信息</param>
        private void CheckOnLoanInfo(OnLoanInfo onLoanInfo)
        {
            CheckOnLoanInfoBase(onLoanInfo);
            if (onLoanInfo.No.IsNullOrEmpty())
                throw new ValidationException("借调单号为空".L10N());
            if (onLoanInfo.ShiftId <= 0)
                throw new ValidationException("班次ID不能<=0".L10N());
            var shift = RF.GetById<Shift>(onLoanInfo.ShiftId);
            if (shift == null)
                throw new ValidationException("班次Id[{0}]对应的班次实体未找到!".L10nFormat(onLoanInfo.ShiftId));

            if (TimeToInt(shift.BeginTime) < TimeToInt(shift.EndTime))
            {//当跨天的班次，暂时前端判断
                if (TimeToInt(onLoanInfo.BeginDate) < TimeToInt(shift.BeginTime))
                    throw new ValidationException("借调开始时间不能早于班次开始时间!".L10N());
                if (TimeToInt(onLoanInfo.EndDate) > TimeToInt(shift.EndTime))
                    throw new ValidationException("借调结束时间不能晚于班次结束时间!".L10N());
            }

            if (onLoanInfo.GroupOutId <= 0)
                throw new ValidationException("借出班组Id不能 <= 0".L10N());
            var workgroupOut = RF.GetById<WorkGroup>(onLoanInfo.GroupOutId);
            if (workgroupOut == null)
                throw new ValidationException("借出班组ID[{0}]对应的班组实体不存在!".L10nFormat(onLoanInfo.GroupOutId));
            if (workgroupOut.ActualQty < onLoanInfo.DemandQty)
                throw new ValidationException("需求人数不能大于借出班组在编人数".L10N());
            var curMonitor = RT.Service.Resolve<EmployeeController>().GetMonitor(workgroupOut.Id);
            if (curMonitor == null)
                throw new ValidationException("借出班组无班组长类型员工!".L10N());
            if (onLoanInfo.GroupOutId == onLoanInfo.GroupInId)
                throw new ValidationException("借入班组和借出班组不能相同".L10N());
        }

        /// <summary>
        /// 创建班组借调单
        /// </summary>
        /// <param name="onLoanInfo">借调单信息API信息</param>
        /// <returns>班组借调单</returns>
        private WorkGroupOnLoan CreateWorkGroupOnLoan(OnLoanInfo onLoanInfo)
        {
            var curMonitor = RT.Service.Resolve<EmployeeController>().GetMonitor(onLoanInfo.GroupOutId);
            WorkGroupOnLoan onLoanMain = new WorkGroupOnLoan();
            var curDateTime = RF.Find<WorkGroupOnLoan>().GetDbTime();
            onLoanMain.GenerateId();
            onLoanMain.No = onLoanInfo.No;
            onLoanMain.InitiateDate = curDateTime;
            onLoanMain.DemandQty = onLoanInfo.DemandQty;
            onLoanMain.BeginDate = onLoanInfo.BeginDate;
            onLoanMain.EndDate = onLoanInfo.EndDate;
            onLoanMain.State = OnLoanState.ToBeApproved;
            onLoanMain.GroupInId = GetWorkGroupId(RT.IdentityId).Value;
            onLoanMain.GroupOutId = onLoanInfo.GroupOutId;
            onLoanMain.ShiftId = onLoanInfo.ShiftId;
            onLoanMain.InitiatorId = RT.IdentityId;
            onLoanMain.ApproverId = curMonitor != null ? curMonitor.Id : 0;
            onLoanMain.Remark = onLoanInfo.Remark;
            onLoanMain.LoanHour = GetLoanHour(onLoanInfo);
            return onLoanMain;
        }

        /// <summary>
        /// 获取员工的班组Id
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <returns>班组Id</returns>
        private double? GetWorkGroupId(double employeeId)
        {
            double? workGroupId = 0;
            var employee = RF.GetById<Employee>(employeeId);
            if (employee != null)
                workGroupId = employee.WorkGroupId;
            return workGroupId;
        }

        /// <summary>
        /// 修改班组借调单
        /// </summary>
        /// <param name="onLoan">班组借调单</param>
        /// <param name="onLoanInfo">借调单API信息</param>
        private void UpdateWorkGroupOnLoan(WorkGroupOnLoan onLoan, OnLoanInfo onLoanInfo)
        {
            onLoan.DemandQty = onLoanInfo.DemandQty;
            onLoan.BeginDate = onLoanInfo.BeginDate;
            onLoan.EndDate = onLoanInfo.EndDate;
            onLoan.Remark = onLoanInfo.Remark;
            if (onLoan.State == OnLoanState.Refuse) //被拒绝后修改借调单--再次发起
            {
                onLoan.State = OnLoanState.ToBeApproved;
            }
        }

        /// <summary>
        /// 更新班组借调实体
        /// </summary>
        /// <param name="onLoan">班组借调实体</param>
        /// <param name="approveInfo">借调单审核API信息</param>
        private void UpdateWorkGroupOnLoan(WorkGroupOnLoan onLoan, ApproveInfo approveInfo)
        {
            var curDbTime = RF.Find<WorkGroupOnLoan>().GetDbTime();
            onLoan.ApprovalDate = curDbTime;
            if (approveInfo.ApproveResult == 0)
            {
                onLoan.State = OnLoanState.Agree;
            }
            else if (approveInfo.ApproveResult == 1)
            {
                onLoan.State = OnLoanState.Refuse;
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 获取当前评分记录的申诉记录的RowIndex
        /// </summary>
        /// <param name="workGroupOnLoan">当前评分记录实体</param>
        /// <returns>申诉记录的RowIndex</returns>
        private int GetRowIndex(WorkGroupOnLoan workGroupOnLoan)
        {
            var rowIndex = 0;
            if (workGroupOnLoan.DetailList != null && workGroupOnLoan.DetailList.Count > 0)
            {
                rowIndex = workGroupOnLoan.DetailList.Count + 1;
            }
            else
            {
                rowIndex = 1;
            }
            return rowIndex;
        }

        /// <summary>
        /// 创建借调单明细
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="onLoanId">借调单Id</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="state">审核状态</param>
        /// <param name="remark">备注</param>
        /// <returns>借调单明细</returns>
        private OnLoanDetail CreateOnLoanDetail(decimal rowIndex, double onLoanId, double operatorId, ApprovalState state, string remark)
        {
            OnLoanDetail item = new OnLoanDetail();
            item.RowIndex = rowIndex;
            item.OnLoanId = onLoanId;
            item.OperateDate = RF.Find<WorkGroupOnLoan>().GetDbTime();
            item.OperatorId = operatorId;
            item.State = state;
            item.Remark = remark;
            return item;
        }

        /// <summary>
        /// 获取借调时长
        /// </summary>
        /// <param name="onLoanInfo">借调信息</param>
        /// <returns>借调时长</returns>
        private decimal GetLoanHour(OnLoanInfo onLoanInfo)
        {
            var curShift = RF.GetById<Shift>(onLoanInfo.ShiftId);
            decimal hour = 0;
            if (curShift != null)
            {
                curShift.ShiftRestList.ForEach(p =>
                {
                    p.BeginTime = onLoanInfo.BeginDate.Date + p.BeginTime.TimeOfDay;
                    if (curShift.IsOverDay)
                    {
                        p.EndTime = onLoanInfo.BeginDate.AddDays(1).Date + p.EndTime.TimeOfDay;
                    }
                    else
                    {
                        p.EndTime = onLoanInfo.BeginDate.Date + p.EndTime.TimeOfDay;
                    }
                    ////刚好借调时间结束，在休息时间的区间,实际用于计算的结束时间就是休息的开始时间
                    if (onLoanInfo.EndDate >= p.BeginTime && onLoanInfo.EndDate <= p.EndTime)
                    {
                        onLoanInfo.EndDate = p.BeginTime;
                    }
                    ////刚好借调时间开始，在休息时间的区间,实际用于计算的开始时间就是休息的结束时间
                    if (onLoanInfo.BeginDate >= p.BeginTime && onLoanInfo.BeginDate <= p.EndTime)
                    {
                        onLoanInfo.BeginDate = p.EndTime;
                    }
                });
                var totalMins = (onLoanInfo.EndDate - onLoanInfo.BeginDate).TotalMinutes;
                if (totalMins > 0)
                {
                    var restMins = curShift.ShiftRestList.Where(p => p.BeginTime >= onLoanInfo.BeginDate && p.EndTime <= onLoanInfo.EndDate).Sum(e => (e.EndTime - e.BeginTime).TotalMinutes);
                    hour = decimal.Parse(((totalMins - restMins) / 60).ToString("F1"));
                }
            }

            return hour;
        }

        /// <summary>
        /// 借调单修改的参数验证
        /// </summary>
        /// <param name="onLoanInfo">借调单API信息</param>
        /// <returns>班组借调实体</returns>
        private WorkGroupOnLoan CheckUpdateOnLoan(OnLoanInfo onLoanInfo)
        {
            CheckOnLoanInfoBase(onLoanInfo);
            if (onLoanInfo.Id == null)
                throw new ValidationException("请传入要修改的借调单ID".L10N());
            var onLoan = RF.GetById<WorkGroupOnLoan>(onLoanInfo.Id);
            if (onLoan == null)
                throw new ValidationException("借调单Id[{0}]对应的实体不存在!".L10nFormat(onLoanInfo.Id));
            if (onLoan.State == OnLoanState.Agree)
                throw new ValidationException("借调单状态已审核,不可修改!".L10N());
            if (onLoan.State == OnLoanState.Cancel)
                throw new ValidationException("借调单状态已撤销,不可修改".L10N());
            var workgroupOut = RF.GetById<WorkGroup>(onLoan.GroupOutId);
            if (workgroupOut == null)
                throw new ValidationException("借出班组ID[{0}]对应的班组实体不存在!".L10nFormat(onLoanInfo.GroupOutId));
            if (workgroupOut.ActualQty < onLoanInfo.DemandQty)
                throw new ValidationException("需求人数不能大于借出班组在编人数".L10N());
            var shift = RF.GetById<Shift>(onLoan.ShiftId);

            if (TimeToInt(shift.BeginTime) < TimeToInt(shift.EndTime))
            {//当跨天的班次，暂时前端判断
                if (TimeToInt(onLoanInfo.BeginDate) < TimeToInt(shift.BeginTime))
                {
                    throw new ValidationException("借调开始时间不能早于班次开始时间!".L10N());
                }
                if (TimeToInt(onLoanInfo.EndDate) > TimeToInt(shift.EndTime))
                {
                    throw new ValidationException("借调结束时间不能晚于班次结束时间!".L10N());
                }
            }
            return onLoan;
        }

        /// <summary>
        /// 检查撤销内容
        /// </summary>
        /// <param name="onLoanId">借调单ID</param>
        /// <returns>班组借调实体</returns>
        private WorkGroupOnLoan CheckCancel(double onLoanId)
        {
            var onLoan = RF.GetById<WorkGroupOnLoan>(onLoanId);
            if (onLoan == null)
            {
                throw new ValidationException("借调单Id[{0}]对应的实体不存在!".L10nFormat(onLoanId));
            }
            if (onLoan.State != OnLoanState.Refuse && onLoan.State != OnLoanState.ToBeApproved) //onLoan.State != OnLoanState.LoseEfficacy &&
            {
                throw new ValidationException("借调单当前状态为[{0}], 不能撤销!".L10nFormat(onLoan.State.ToLabel()));
            }
            return onLoan;
        }

        /// <summary>
        /// 检查审核内容
        /// </summary>
        /// <param name="approveInfo">借调单审核API信息</param>
        /// <returns>班组借调实体</returns>
        private WorkGroupOnLoan CheckApproveInfo(ApproveInfo approveInfo)
        {
            WorkGroupOnLoan onLoan = RF.GetById<WorkGroupOnLoan>(approveInfo.OnLoanId);
            if (onLoan == null)
                throw new ValidationException("借调单Id[{0}]对应的实体不存在!".L10nFormat(approveInfo.OnLoanId));
            if (onLoan.State != OnLoanState.ToBeApproved)
                throw new ValidationException("借调单当前状态是[{0}], 不能进行审核操作!".L10nFormat(onLoan.State));
            if (approveInfo.ApproveResult == 0 && !approveInfo.EmployeeList.Any())
                throw new ValidationException("请选择借调员工!".L10N());
            foreach (var curEmployeeId in approveInfo.EmployeeList)
            {
                var employee = RF.GetById<Employee>(double.Parse(curEmployeeId));
                if (employee == null)
                {
                    throw new ValidationException("借调员工Id[{0}]对应的实体不存在!".L10nFormat(curEmployeeId));
                }
                if (employee.WorkGroupId != onLoan.GroupOutId)
                {
                    throw new ValidationException("借调员工[{0}]不属于借调单中的借出班组[{1}]".L10nFormat(employee.Name, onLoan.GroupOut.Name));
                }
            }

            return onLoan;
        }

        /// <summary>
        /// 获取审核状态
        /// </summary>
        /// <param name="approveInfo">借调单审核API信息</param>
        /// <returns>审核状态</returns>
        private ApprovalState GetApprovalState(ApproveInfo approveInfo)
        {
            ApprovalState detailItemState = ApprovalState.Agree;
            if (approveInfo.ApproveResult == 0)
            {
                detailItemState = ApprovalState.Agree;
            }
            else if (approveInfo.ApproveResult == 1)
            {
                detailItemState = ApprovalState.Refuse;
            }
            else
            {
                //
            }
            return detailItemState;
        }

        /// <summary>
        /// 创建借调员工集合
        /// </summary>
        /// <param name="onLoanId">班组借调Id</param>
        /// <param name="employeeIds">员工Id集合</param>
        /// <returns>借调员工集合</returns>
        private EntityList<OnLoanEmployee> CreateOnLoanEmployees(double onLoanId, string[] employeeIds)
        {
            var onLoanEmps = new EntityList<OnLoanEmployee>();
            foreach (var strEmpId in employeeIds)
            {
                var curOnLoanEmployee = CreateOnLoanEmployee(onLoanId, double.Parse(strEmpId));
                onLoanEmps.Add(curOnLoanEmployee);
            }

            return onLoanEmps;
        }

        /// <summary>
        /// 创建借调员工
        /// </summary>
        /// <param name="onLoanId">班组借调Id</param>
        /// <param name="employeeId">员工Id</param>
        /// <returns>借调员工</returns>
        private OnLoanEmployee CreateOnLoanEmployee(double onLoanId, double employeeId)
        {
            var curOnLoanEmployee = new OnLoanEmployee();
            curOnLoanEmployee.EmployeeId = employeeId;
            curOnLoanEmployee.OnLoanId = onLoanId;
            return curOnLoanEmployee;
        }

        /// <summary>
        /// 时间的时分秒转换为整形
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>时分秒的整型</returns>
        private int TimeToInt(DateTime dt)
        {
            return int.Parse("1" + dt.ToString("HHmmss"));
        }

        /// <summary>
        /// 获取班组出勤的异常信息集合
        /// </summary>
        /// <param name="workGroupId">班组Id</param>
        /// <returns>班组出勤的异常信息集合</returns>
        private EntityList<EmployeeClockIn> GetAbnormalEmployeeClockIns(double workGroupId)
        {
            List<int> onDutyStates = new List<int>() { (int)OnDutyState.Absence, (int)OnDutyState.Rest };
            var curDate = RF.Find<EmployeeClockIn>().GetDbTime().Date;
            var abnormalEmps = RT.Service.Resolve<ClockInController>().GetEmployeeClockIns(workGroupId, curDate, onDutyStates);
            return abnormalEmps;
        }

        /// <summary>
        /// 获取班组借调集合
        /// </summary>
        /// <param name="workGroupId">班组Id</param>
        /// <param name="state">借调单状态</param>
        /// <param name="beginDTime">借调开始时间</param>
        /// <param name="endDTime">借调结束时间</param>
        /// <returns>班组借调集合</returns>
        private EntityList<WorkGroupOnLoan> GetWorkGroupOnLoans(double workGroupId, OnLoanState state, DateTime beginDTime, DateTime endDTime)
        {
            var onLoanList = Query<WorkGroupOnLoan>().Where(p => p.GroupOutId == workGroupId && p.State == state
                                                    && ((p.BeginDate >= beginDTime && p.BeginDate <= endDTime && p.EndDate >= endDTime)
                                                        || (p.BeginDate <= beginDTime && p.EndDate >= beginDTime && p.EndDate <= endDTime)
                                                        || (p.BeginDate >= beginDTime && p.EndDate <= endDTime)
                                                        || (p.BeginDate <= beginDTime && p.EndDate >= endDTime))).ToList();
            return onLoanList;
        }

        /// <summary>
        /// 获取可借调人员集合
        /// </summary>
        /// <param name="abnormalEmpClocks">员工异常出勤集合</param>
        /// <param name="onLoanedList">已借调集合</param>
        /// <param name="workGroupId">班组Id</param>
        /// <returns>可借调人员集合</returns>
        private EntityList<Employee> GetOnLoanAbleEmployees(EntityList<EmployeeClockIn> abnormalEmpClocks, EntityList<WorkGroupOnLoan> onLoanedList, double workGroupId)
        {
            List<double> notOnLoanEmpIds = new List<double>();
            if (abnormalEmpClocks != null && abnormalEmpClocks.Any())
            {
                var exEmpIds = abnormalEmpClocks.Select(x => x.EmployeeId).ToList();
                notOnLoanEmpIds.AddRange(exEmpIds);
            }

            if (onLoanedList != null && onLoanedList.Any())
            {
                var onLoanEmpIds = onLoanedList.SelectMany(x => x.EmployeeList).Select(x => x.EmployeeId).ToList();
                notOnLoanEmpIds.AddRange(onLoanEmpIds);
                notOnLoanEmpIds = notOnLoanEmpIds.Distinct().ToList();
            }

            var query = Query<Employee>().Where(p => p.WorkGroupId == workGroupId && !notOnLoanEmpIds.Contains(p.Id));
            var employees = query.ToList();
            return employees;
        }
        #endregion 人员借调API私有方法
    }
}