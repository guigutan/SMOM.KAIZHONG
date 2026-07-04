using SIE.Common;
using SIE.Core.Common.Models;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.MES.TeamManagement.Vacancies
{
    /// <summary>
    /// 缺编控制器
    /// </summary>
    public partial class VacancyController : DomainController
    {
        /// <summary>
        /// 重写查询
        /// </summary>
        /// <param name="criteria">班组缺编统计查询实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>班组缺编集合</returns>
        public virtual EntityList<WorkGroupVacancy> GetWorkGroupVacancy(WorkGroupVacancyCriteria criteria, PagingInfo pagingInfo)
        {
            if (criteria.ClockInDate == null)
            {
                return new EntityList<WorkGroupVacancy>();
            }
            var query = Query<WorkGroupVacancy>().Where(p => p.VacancyDate == criteria.ClockInDate);
            if (criteria.WorkGroupId.HasValue && criteria.WorkGroupId > 0)
            {
                query = query.Where(p => p.WorkGroupId == criteria.WorkGroupId);
            }

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
        /// 重写查询（导出查询数据）
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <param name="dr">时间范围</param>
        /// <returns>班组缺编集合</returns>
        public virtual EntityList<WorkGroupVacancy> GetWorkGroupVacancy(WorkGroupVacancyCriteria criteria, DateRange dr)
        {
            if (criteria.ClockInDate == null)
            {
                return new EntityList<WorkGroupVacancy>();
            }
            var query = Query<WorkGroupVacancy>().Where(p => p.VacancyDate >= dr.BeginValue && p.VacancyDate <= dr.EndValue);
            if (criteria.WorkGroupId.HasValue && criteria.WorkGroupId > 0)
            {
                query = query.Where(p => p.WorkGroupId == criteria.WorkGroupId);
            }

            var vacancy = query.ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 }, new EagerLoadOptions().LoadWithViewProperty());

            var workGroupIds = vacancy.Select(p => p.WorkGroupId).ToList();
            var clockInList = RT.Service.Resolve<ClockInController>().GetEmployeeClockIns(dr, workGroupIds);
            vacancy.ForEach(p =>
            {
                var attEmp = clockInList?.Where(e => e.WorkGroupId == p.WorkGroupId && e.OnDutyState == OnDutyState.Normal).AsEntityList();
                p.EmployeeIds = attEmp?.Select(f => f.EmployeeId).ToList();
            });

            return vacancy;
        }

        /// <summary>
        /// 根据日期获取班组缺编数据
        /// </summary>     
        /// <param name="dr">日期范围</param>
        /// <returns>班组缺编数据</returns>
        public virtual EntityList<WorkGroupVacancy> GetWorkGroupVacancy(DateRange dr)
        {
            return Query<WorkGroupVacancy>().Where(p => p.VacancyDate >= dr.BeginValue && p.VacancyDate <= dr.EndValue).ToList();
        }

        /// <summary>
        /// 获取班组缺编导出数据
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="criter">人员出勤查询实体</param>
        /// <returns>导出数据表</returns>
        public virtual ExportDataTable GetExportVacancys(int year, int month, int day, WorkGroupVacancyCriteria criter)
        {
            string[] columns = new string[] { "日期", "编码", "班组", "在编人数", "当日出勤", "异常人数", "班次时间" };
            if (year == 0 || month == 0)
                throw new ValidationException("年月不能为空".L10N());
            ExportDataTable exportDataTable = new ExportDataTable();
            exportDataTable.Columns.Add(columns);
            List<DataTable> dataTables = new List<DataTable>();

            var begin = DateTime.Parse(year + "-" + month + "-01");
            var end = begin.AddMonths(1).AddDays(-1);
            if (day > 0)
            {
                ////只导出一天的数据
                begin = begin.AddDays(day - 1);
                end = begin;
            }
            var now = RF.Find<Employee>().GetDbTime();
            if (end > now)
                end = now.Date;
            ////结束日期比今天大则取今天为结束
            DateRange dr = new DateRange() { BeginValue = begin, EndValue = end };
            var attentList = RT.Service.Resolve<VacancyController>().GetWorkGroupVacancy(criter, dr);
            var shiftList = attentList.Where(p => p.ShiftId > 0).Select(p => p.Shift).AsEntityList();
            var shiftDic = RT.Service.Resolve<ClockInController>().GetShiftDic(shiftList);
            for (DateTime i = begin; i <= end;)
            {
                var data = i.ToShortDateString();
                DataTable dataTable = new DataTable();
                columns.ForEach(column => dataTable.Columns.Add(column));
                ////一天一个sheet表
                var daylist = attentList.Where(p => p.VacancyDate == i).AsEntityList();
                if (daylist.Count > 0)
                {
                    daylist.GroupBy(p => p.WorkGroupId).ForEach(f =>
                    {
                        ////以班组分组放数据
                        f.ForEach(p =>
                        {
                            string shifTime = string.Empty;
                            shiftDic.TryGetValue(p.ShiftId, out shifTime);
                            DataRow row = dataTable.NewRow();
                            row[0] = data;
                            row[1] = p.WorkGroupCode;
                            row[2] = p.WorkGroupName;
                            row[3] = p.ActualQty;
                            row[4] = p.ClockingInQty;
                            row[5] = p.AbnormalQty;
                            row[6] = shifTime;
                            dataTable.Rows.Add(row);
                        });
                    });
                }
                dataTables.Add(dataTable);
                exportDataTable.SheetNames.Add(i.ToString("D"));
                i = i.AddDays(1);
            }

            exportDataTable.Tables.AddRange(dataTables);
            return exportDataTable;
        }

        /// <summary>
        /// 每天凌晨同步生成当天的缺编初始数据
        /// </summary>
        /// <returns>msg</returns>
        public virtual string SyncWorkGroupVacancy()
        {
            try
            {
                var date = RF.Find<WorkGroupVacancy>().GetDbTime().Date;
                List<double> wrokGroupIds = new List<double>();
                DateRange dr = new DateRange() { BeginValue = date, EndValue = date };
                var vancyList = GetWorkGroupVacancy(dr);
                if (vancyList.Count > 0)
                {
                    wrokGroupIds = vancyList.Select(p => p.WorkGroupId).ToList();
                }
                var schedules = RT.Service.Resolve<ShiftScheduleController>().GetShiftSchedules(date, wrokGroupIds);
                if (schedules.Count == 0)
                {
                    return string.Empty; ////"没有需要同步的已排班班组";
                }
                EntityList<WorkGroupVacancy> rst = new EntityList<WorkGroupVacancy>();
                schedules.ForEach(p =>
                 {
                     WorkGroupVacancy item = new WorkGroupVacancy();
                     item.WorkGroupId = p.WorkGroupId;
                     item.VacancyDate = date;
                     item.ShiftId = p.ShiftId;
                     item.WipResourceId = p.WipResourceId;
                     item.WorkShopId = p.WorkShopId;
                     item.ActualQty = p.WorkGroup.ActualQty == null ? 0 : p.WorkGroup.ActualQty.Value;
                     item.ClockingInQty = 0;
                     item.AbnormalQty = 0;
                     item.ClockingInQty = 0;
                     rst.Add(item);
                 });
                RF.Save(rst);
                //// "成功同步" + rst.Count + "条数据";
                return string.Empty; 
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 更新缺编数据,当天的更新在编、出勤、异常，非当天的只更新出勤异常,只更新当天数据days=0
        /// </summary>
        /// <param name="days">天数</param>
        /// <param name="inculeToday">是否包含今天</param>
        /// <returns>msg</returns>
        public virtual string ExeWorkGroupVacancy(int days, bool inculeToday)
        {
            try
            {
                var date = RF.Find<WorkGroupVacancy>().GetDbTime().Date;
                DateRange dr = new DateRange() {
                    BeginValue = date.AddDays(-days), 
                    EndValue = date 
                };
                if (!inculeToday)
                {
                    dr.EndValue = date.AddDays(-1);
                }
                var vancyList = GetWorkGroupVacancy(dr);
                if (vancyList.Count == 0)
                {
                    return dr.BeginValue.Value + "至" + dr.EndValue.Value + "没有缺编数据";
                }
                var workGroupIds = vancyList.Select(p => p.WorkGroupId).ToList();
                var clockInList = RT.Service.Resolve<ClockInController>().GetEmployeeClockIns(dr, workGroupIds);
                vancyList.GroupBy(p => p.WorkGroupId).ForEach(e =>
                  {
                      var wgId = e.Key;
                      e.GroupBy(f => f.VacancyDate.Date).ForEach(f =>
                      {
                          var curDate = f.Key;
                          f.ToList().ForEach(g =>
                          {
                              if (curDate == date)
                                  g.ActualQty = g.WorkGroup.ActualQty == null ? 0 : g.WorkGroup.ActualQty.Value;
                              g.ClockingInQty = clockInList.Where(h => h.WorkGroupId == wgId && h.OnDutyState == OnDutyState.Normal && h.ClockInDate == curDate).ToList().Count;
                              g.AbnormalQty = clockInList.Where(h => h.WorkGroupId == wgId && h.OnDutyState == OnDutyState.Absence && h.ClockInDate == curDate).ToList().Count;
                          });
                      });
                  });
                RF.Save(vancyList);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
            var woList = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByResource(resourceId, dr);
            woList.ForEach(p =>
            {
                WorkOrderViewModel item = new WorkOrderViewModel();
                item.No = p.No;
                item.Id = p.Id.ToString();
                item.ProductCode = p.Product.Code;
                item.ProductName = p.Product.Name;
                item.ProductModel = p.Product.Model?.Name;
                item.ProductModelId = p.Product.ModelId;
                item.PlanBeginDate = p.PlanBeginDate;
                item.PlanEndDate = p.PlanEndDate;
                item.PlanQty = p.PlanQty;
                item.FinishQty = p.FinishQty;
                if (p.IsPause == YesNo.Yes && (p.State == Core.WorkOrders.WorkOrderState.Release || p.State == Core.WorkOrders.WorkOrderState.Producing))
                    item.StateName = EnumViewModel.EnumToLabel(p.State).L10N() + "暂停".L10N();
                else
                {
                    item.StateName = EnumViewModel.EnumToLabel(p.State).L10N();
                }

                item.WorkShopName = p.WorkShop?.Name;
                item.ResourceName = p.Resource?.Name;
                string empId = string.Empty;
                empIds.ForEach(e =>
                {
                    empId += e + ",";
                });
                item.EmployeeIds = empId.TrimEnd(',');
                item.VacancyId = vacancyId;
                rst.Add(item);
            });
            rst.SetTotalCount(rst.Count);
            return rst;
        }
    }
}