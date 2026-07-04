using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ScoreRecords;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.TeamManagement
{
    /// <summary>
    /// 视图控制器
    /// </summary>
    public class ScoreRecordVMController : DomainController
    {
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns>评分统计表数据</returns>
        public virtual EntityList<ScoreRecordViewModel> GetScoreRecordsVM(ScoreRecordVMCriteria criteria)
        {
            var list = Query<ScoreRecord>().Where(p => p.IsEffective);
            if (criteria.OccurDate.BeginValue.HasValue && criteria.OccurDate.EndValue.HasValue)
            {
                var now = RF.Find<ScoreRecord>().GetDbTime();
                if (criteria.OccurDate.BeginValue.Value.Date > now.Date)
                {
                    throw new ValidationException("该日期范围还没产生评分，请重新选择".L10N());
                }

                list = list.Where(p => p.OccurDate >= criteria.OccurDate.BeginValue && p.OccurDate < criteria.OccurDate.EndValue.Value.AddDays(1).Date);
            }
            else
            {
                throw new ValidationException("请选择日期范围查询".L10N());
            }
            if (criteria.EmployeeId.HasValue)
            {
                list = list.Where(p => p.EmployeeId == criteria.EmployeeId);
                criteria.WorkGroup = criteria.Employee.WorkGroup;
            }
            else if (criteria.WorkGroup != null && criteria.WorkGroupId > 0)
            {
                var empList = RT.Service.Resolve<Resources.Employees.EmployeeController>().GetEmployeeByWorkGroupId(criteria.WorkGroupId);
                var empIds = empList.Select(p => p.Id).ToList();
                list = list.Where(p => empIds.Contains(p.EmployeeId));
            }
            else
            {
                throw new ValidationException("请选择员工或班组查询".L10N());
            }

            return DataToScoreRecord(criteria, list.ToList());
        }

        /// <summary>
        /// 根据时间区间拆分成集合,确保查询每一天月都有数据，没有会补0
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="type">类型</param>
        /// <returns>集合</returns>
        private List<DateTime> GetDateTimeArr(DateTime start, DateTime end, DateType type)
        {
            List<DateTime> dt = new List<DateTime>();
            if (type == DateType.Day)
            {
                for (; start <= end; start = start.AddDays(1))
                {
                    dt.Add(start);
                }
            }
            else if (type == DateType.Month)
            {
                for (DateTime i = start.Date; i <= end.Date;)
                {
                    if (i.Year == end.Year && i.Month > end.Month)
                    {
                        break;
                    }

                    dt.Add(i);
                    i = i.AddMonths(1);
                    if (i.Year == end.Year && i.Month == end.Month && i > end.Date)
                    {
                        i = end.Date;
                    }
                }
            }
            else
            {
                //
            }

            return dt;
        }

        /// <summary>
        /// 获取当天月分数，没有补0
        /// </summary>
        /// <param name="emp">员工</param>
        /// <param name="dt">事件</param>
        /// <param name="criteria">查询</param>
        /// <param name="score">分数</param>
        /// <returns>分数</returns>
        private ScoreRecordViewModel GetScoreRecordViewModel(Employee emp, DateTime dt, ScoreRecordVMCriteria criteria, decimal score)
        {
            ScoreRecordViewModel item = new ScoreRecordViewModel()
            {
                EmpId = emp.Id,
                EmpName = emp.Name,
                ActualDate = dt,
                WorkgroupId = criteria.WorkGroupId,
                WorkGroupName = criteria.WorkGroup.Name,
                Score = score
            };
            item.OccurDate = criteria.DateType == DateType.Day ? 
                dt.Year + "" + dt.Month.ToString("00") + "" + dt.Day.ToString("00") : 
                dt.Year + "" + dt.Month.ToString("00");
            return item;
        }

        /// <summary>
        /// 数据处理，没有数据的日期月份需要补0或100，然后转换成图表的数据实体
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <param name="itemList">评分记录</param>
        /// <returns>评分统计表实体</returns>
        private EntityList<ScoreRecordViewModel> DataToScoreRecord(ScoreRecordVMCriteria criteria, EntityList<ScoreRecord> itemList)
        {
            EntityList<ScoreRecordViewModel> rst = new EntityList<ScoreRecordViewModel>();
            var dtarr = GetDateTimeArr(criteria.OccurDate.BeginValue.Value.Date, criteria.OccurDate.EndValue.Value.Date, criteria.DateType);
            itemList.GroupBy(p => p.EmployeeId).ForEach(p =>
            {
                var emp = itemList.Where(f => f.EmployeeId == p.Key).Select(f => f.Employee).FirstOrDefault();
                var scroeList = p.OrderBy(c => c.OccurDate);
                if (criteria.DateType == DateType.Month)
                {
                    scroeList.GroupBy(e => e.OccurDate.ToString("yyyyMM")).ForEach(e =>
                    {
                        var first = e.FirstOrDefault();
                        var score = e.Sum(c => c.Score);
                        rst.Add(GetScoreRecordViewModel(emp, first.OccurDate.Date, criteria, score));
                    });
                    //没有评分的月份分数补100
                    var monthArr = scroeList.Select(e => e.OccurDate.Month).Distinct().ToList();
                    dtarr.Where(e => !monthArr.Contains(e.Month)).ForEach(e =>
                    {
                        int score = 100;
                        if (emp.HireDate.HasValue)
                        {
                            var hire = emp.HireDate.Value;
                            //当月还没入职分数=0
                            if (hire.Year > e.Year || hire.Year == e.Year && hire.Month > e.Month)
                            {
                                score = 0;
                            }
                        }
                        rst.Add(GetScoreRecordViewModel(emp, e, criteria, score));
                    });
                }
                else if (criteria.DateType == DateType.Day)
                {
                    scroeList.GroupBy(e => e.OccurDate.Date).ForEach(e =>
                    {
                        var score = e.Sum(c => c.Score);
                        rst.Add(GetScoreRecordViewModel(emp, e.Key, criteria, score));
                    });
                    //没有评分的日期分数补0
                    var dayArr = scroeList.Select(e => e.OccurDate.Date).Distinct().ToList();
                    dtarr.Where(e => !dayArr.Contains(e)).ForEach(e =>
                    {
                        rst.Add(GetScoreRecordViewModel(emp, e, criteria, 0));
                    });
                }
                else
                {
                    //
                }
            });
            return rst;
        }
    }
}
