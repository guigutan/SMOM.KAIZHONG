using SIE.Core.Common.Models;
using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.Resources.Employees;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 客制查询数据处理
    /// </summary>
    public class EmployeeAttentDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取评分记录数据
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="criter">查询实体</param>
        /// <returns>评分记录信息</returns>
        public ExportDataTable GetEmployeeAttentData(int year, int month, int day, EmployeeClockInAttentCriteria criter)
        {
            return RT.Service.Resolve<ClockInController>().GetExportEmployeeClockIns(year, month, day, criter);
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns>集合</returns>
        public List<EntityJson> GetYearAndMonth()
        {
            var now = RF.Find<Employee>().GetDbTime();
            List<EntityJson> res = new List<EntityJson>();
            EntityJson year = new EntityJson();
            year.SetProperty("Year", now.Year);
            List<EntityJson> month = new List<EntityJson>();
            for (int i = 1; i <= now.Month; i++)
            {
                EntityJson m = new EntityJson();
                m.SetProperty("month", i);
                month.Add(m);
            }

            year.SetProperty("Month", month);
            res.Add(year);
            return res;
        }

        /// <summary>
        /// 按月获取天数
        /// </summary>
        /// <param name="month">月</param>
        /// <returns>集合</returns>
        public List<EntityJson> GetDayByMonth(int month)
        {
            var now = RF.Find<Employee>().GetDbTime();
            List<EntityJson> res = new List<EntityJson>();
            DateTime begin = DateTime.Parse(now.Year + "-" + month + "-" + "01");
            DateTime end = begin.AddMonths(1).AddDays(-1);
            if (end > now) end = now.Date;
            for (int i = 1; i <= end.Day; i++)
            {
                EntityJson d = new EntityJson();
                d.SetProperty("day", i);
                res.Add(d);
            }

            return res;
        }
    }
}
