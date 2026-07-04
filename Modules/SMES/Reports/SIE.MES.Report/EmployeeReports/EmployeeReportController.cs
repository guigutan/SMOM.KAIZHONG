using SIE.Common;
using SIE.Domain;
using SIE.MES.Report.EmployeeReports.ClockingIns;
using SIE.MES.Report.EmployeeReports.Vacancies;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Report.EmployeeReports
{
    /// <summary>
    /// 员工模块报表控制器
    /// </summary>
    public class EmployeeReportController : ClockInController
    {
        #region 员工出勤
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">实体</param>
        /// <returns>数据</returns>
        public virtual EntityList<EmployeeClockInReport> GetEmployeeClockIns(EmployeeClockInReportCriteria criteria)
        {
            var query = Query<EmployeeClockInReport>();
            if (criteria.ShiftRange.BeginValue.HasValue)
                query.Where(p => p.ShiftBegin >= criteria.ShiftRange.BeginValue.Value);
            if (criteria.ShiftRange.EndValue.HasValue)
                query.Where(p => p.ShiftBegin <= criteria.ShiftRange.EndValue.Value);
            if (!criteria.EmployeeCode.IsNullOrEmpty())
                query.Where(p => p.Employee.Code == criteria.EmployeeCode);
            if (!criteria.EmployeeName.IsNullOrEmpty())
                query.Where(p => p.Employee.Name == criteria.EmployeeName);
            if (criteria.WorkGroupId.HasValue)
                query.Where(p => p.WorkGroupId == criteria.WorkGroupId.Value);
            if (criteria.OnDutyState.HasValue)
                query.Where(p => p.OnDutyState == criteria.OnDutyState);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 人员工时统计
        /// <summary>
        /// 员工出勤统计查询
        /// </summary>
        /// <param name="criteria">实体</param>
        /// <returns>数据</returns>
        public virtual EntityList<EmployeeClockInAttentReport> GetEmployeeClockIns(EmployeeClockInAttentReportCriteria criteria)
        {
            var query = Query<EmployeeClockInAttentReport>().Where(p => p.WorkGroupId > 0);
            if (criteria.EmployeeDate.HasValue)
                query.Where(p => p.ClockInDate == criteria.EmployeeDate && (p.Employee.HireDate == null || p.Employee.HireDate <= criteria.EmployeeDate));
            if (!criteria.EmployeeCode.IsNullOrEmpty())
                query.Where(p => p.Employee.Code == criteria.EmployeeCode);
            if (!criteria.EmployeeName.IsNullOrEmpty())
                query.Where(p => p.Employee.Name == criteria.EmployeeName);
            if (criteria.WorkGroupId.HasValue)
                query.Where(p => p.WorkGroupId == criteria.WorkGroupId.Value);
            if (criteria.OnDutyState.HasValue)
                query.Where(p => p.OnDutyState == criteria.OnDutyState);
            var model = query.OrderBy(p => criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var emp = RF.GetById<Employee>(RT.IdentityId);
            if (emp != null)
            {
                model.ForEach(p => p.UserEmpType = emp.EmployeeType);
            }
            return model;
        }

        /// <summary>
        /// 获取班组借调By员工Id和日期
        /// </summary>
        /// <param name="empId">员工Id</param>
        /// <param name="date">日期</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>班组借调</returns>
        public virtual EntityList<WorkGroupOnLoanReport> GetWorkGroupOnLoanByEmp(double empId, DateTime date, PagingInfo pagingInfo)
        {
            return Query<WorkGroupOnLoanReport>().Join<OnLoanEmployeeReport>((x, y) => x.Id == y.OnLoanId && y.EmployeeId == empId)
                .Where(p => p.BeginDate >= date.Date && p.BeginDate < date.AddDays(1).Date).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 班组缺编统计
        /// <summary>
        /// 重写查询
        /// </summary>
        /// <param name="criteria">班组缺编统计查询实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>班组缺编集合</returns>
        public virtual EntityList<WorkGroupVacancyReport> GetWorkGroupVacancy(WorkGroupVacancyReportCriteria criteria, PagingInfo pagingInfo)
        {
            var query = Query<WorkGroupVacancyReport>().Where(p => p.VacancyDate == criteria.ClockInDate);
            if (criteria.WorkGroupId.HasValue && criteria.WorkGroupId > 0)
                query = query.Where(p => p.WorkGroupId == criteria.WorkGroupId);
            var vacancy = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            DateRange dr = new DateRange() { BeginValue = criteria.ClockInDate, EndValue = criteria.ClockInDate };
            var workGroupIds = vacancy.Select(p => p.WorkGroupId).ToList();
            var clockInList = RT.Service.Resolve<ClockInController>().GetEmployeeClockIns(dr, workGroupIds);
            vacancy.ForEach(p =>
            {
                var attEmp = clockInList?.Where(e => e.WorkGroupId == p.WorkGroupId && e.OnDutyState == OnDutyState.Normal).AsEntityList();
                p.EmployeeIds = attEmp?.Select(f => f.EmployeeId).ToList();
            });
            return vacancy.OrderBy(p => criteria.OrderInfoList).AsEntityList();
        }

        /// <summary>
        /// 获取出勤数据
        /// </summary>
        /// <param name="dr">时间范围</param>
        /// <param name="workGroupId">班组ID</param>
        /// <returns>出勤数据集合</returns>
        public virtual EntityList<EmployeeClockInReport> GetEmpClockInReports(DateRange dr, double? workGroupId = null)
        {
            var query = Query<EmployeeClockInReport>().Where(p => p.ClockInDate >= dr.BeginValue && p.ClockInDate <= dr.EndValue);
            if (workGroupId.HasValue)
                query = query.Where(p => p.WorkGroupId == workGroupId.Value);
            return query.ToList();
        }

        /// <summary>
        /// 获取班组缺编工单集合
        /// </summary>
        /// <param name="empIds">员工IDs</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="dr">班次时间范围</param>
        /// <param name="vacancyId">缺编ID</param>
        /// <returns>工单集合</returns>
        public virtual EntityList<WorkOrderViewModel> GetWoModel(List<double> empIds, double resourceId, DateRange dr, double vacancyId)
        {
            EntityList<WorkOrderViewModel> rst = new EntityList<WorkOrderViewModel>();
            var woList = GetWorkOrdersByResource(resourceId, dr);
            woList.ForEach(p =>
            {
                WorkOrderViewModel item = new WorkOrderViewModel();
                item.No = p.No;
                item.Id = p.Id.ToString();
                item.ProductCode = p.ProductCode;
                item.ProductName = p.ProductName;
                item.ProductModel = p.ProductModelName;
                item.ProductModelId = p.Product.ModelId;
                item.PlanBeginDate = p.PlanBeginDate;
                item.PlanEndDate = p.PlanEndDate;
                item.PlanQty = p.PlanQty;
                item.FinishQty = p.FinishQty;
                if (p.IsPause == YesNo.Yes && (p.State == Core.WorkOrders.WorkOrderState.Release || p.State == Core.WorkOrders.WorkOrderState.Producing))
                    item.StateName = EnumViewModel.EnumToLabel(p.State).L10N() + "暂停".L10N();
                else
                    item.StateName = EnumViewModel.EnumToLabel(p.State).L10N();
                item.WorkShopName = p.WorkShopName;
                item.ResourceName = p.ResourceName;
                string empId = string.Empty;
                empIds.ForEach(e => { empId += e + ","; });
                item.EmployeeIds = empId.TrimEnd(',');
                item.VacancyId = vacancyId;
                rst.Add(item);
            });
            rst.SetTotalCount(rst.Count);
            return rst;
        }

        /// <summary>
        /// 获取工单By资源
        /// </summary>
        /// <param name="resourceId">资源id</param>
        /// <param name="dr">计划时间区间</param>
        /// <returns>工单集合</returns>
        private EntityList<WorkOrderReport> GetWorkOrdersByResource(double resourceId, DateRange dr = null)
        {
            var query = Query<WorkOrderReport>().Where(p => p.ResourceId != null && resourceId == p.ResourceId);
            if (dr != null)
            {
                query.Where(p => (p.PlanBeginDate >= dr.BeginValue.Value && p.PlanBeginDate < dr.EndValue.Value.AddDays(1)) || (dr.BeginValue.Value >= p.PlanBeginDate && dr.BeginValue.Value <= p.PlanEndDate));
            }
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据时间和状态获取出勤员工信息
        /// </summary>
        /// <param name="workGroupId">班组ID</param>
        /// <param name="dr">时间范围</param>
        /// <param name="state">出勤状态</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>员工信息集合</returns>
        public virtual EntityList<EmployeeClockInReport> GetEmpClockReportByWorkGroup(double workGroupId, DateRange dr, OnDutyState state, PagingInfo pagingInfo = null)
        {
            return Query<EmployeeClockInReport>().Where(p => p.WorkGroupId == workGroupId && p.ClockInDate >= dr.BeginValue && p.ClockInDate <= dr.EndValue && p.OnDutyState == state).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion
    }
}
