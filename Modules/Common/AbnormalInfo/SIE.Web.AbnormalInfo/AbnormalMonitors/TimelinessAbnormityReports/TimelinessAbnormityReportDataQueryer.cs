using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.Service;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports
{
    /// <summary>
    /// 供应商不良率报表数据查询
    /// </summary>
    public class TimelinessAbnormityReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取来料批次合格率
        /// </summary>
        /// <param name="timevalue">输入的时间</param>
        /// <param name="buttonType">按钮的类型</param>
        /// <returns>评分记录信息</returns>
        public object GetChartStore(int timevalue, int buttonType)
        {
            var dataList = RT.Service.Resolve<TimelinessAbnormityReportService>().GetChartData(timevalue, buttonType);
            if (dataList == null)
            {
                return null;
            }
            //饼图的数据
            var PieChartStore = new
            {
                todoCount = dataList.Count(item => item.TaskState == SIE.AbnormalInfo.Common.TaskStateEnum.ToDo),
                doingCount = dataList.Count(item => item.TaskState == SIE.AbnormalInfo.Common.TaskStateEnum.Doing),
                upGradeCount = 0,//dataList.Count(item => item.TaskState == SIE.AbnormalInfo.Common.TaskStateEnum.Upgrade),
                doneCount = dataList.Count(item => item.TaskState == SIE.AbnormalInfo.Common.TaskStateEnum.Done),
                cancelCount = dataList.Count(item => item.TaskState == SIE.AbnormalInfo.Common.TaskStateEnum.Cancel),
            };
            //帕累托图数据
            var paretoChartDataList = dataList.GroupBy(p => p.AbnormalDefineName).ToList();
            List<object> paretoChartStores = new List<object>();
            foreach (var p in paretoChartDataList)
            {
                var paretoChartStore = new { name = p.Key, count = p.Count() };
                paretoChartStores.Add(paretoChartStore);
            }
            //折线图
            const int xAxisMax = 11;
            int[] xAxis = new int[xAxisMax];//坐标轴数量
            var lineDataList = dataList.Where(p => p.TaskState == TaskStateEnum.Done).ToList();//折线图数据，过滤状态不为完成状态的数据
            foreach (var data in lineDataList)
            {
                int daysPassed = (int)(data.UpdateDate - data.CreateDate).TotalDays;
                if (daysPassed < 10)
                {
                    xAxis[daysPassed]++;
                }
                else
                {
                    xAxis[xAxisMax-1]++;
                }
            }
            //异常任务分布图、异常统计列表数据
            var gridPanelStores = GetGridPanelStores(timevalue, buttonType, dataList);
            var storeList = new
            {
                PieChartStore = PieChartStore,
                paretoChartStores = paretoChartStores,
                lineChartChartStores = xAxis,
                gridPanelStores = gridPanelStores,
            };
            return storeList;
        }

        /// <summary>
        /// 对Double值取N位小数（四舍五入）
        /// </summary>
        /// <param name="val"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public double ToFixed(double val, int digits)
        {
            //MidpointRounding.AwayFromZero 解决判断位为5，前一位为偶数时，不进一的问题
            return Math.Round(val, digits, MidpointRounding.AwayFromZero);
        }


        /// <summary>
        /// 获取异常任务分布图、异常统计列表数据
        /// </summary>
        /// <param name="timevalue"></param>
        /// <param name="buttonType"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public List<object> GetGridPanelStores(int timevalue, int buttonType, EntityList<AbnormalMonitorTask> dataList)
        {
            //异常任务分布图、异常统计列表
            List<object> gridPanelStores = new List<object>();
            const string dateType = "yyyy-MM-dd";
            for (int i = 0; i < timevalue; i++)
            {
                DateTime startDateTime, endDateTime;
                var date = "";
                if (buttonType == 0)
                {
                    startDateTime = DateTime.Now.Date.AddDays(-i);  // 获取当前日期的日期部分
                    endDateTime = DateTime.Now.Date.AddDays(-i + 1);  // 获取当前日期的日期部分
                    date = startDateTime.ToString(dateType);
                }
                else if (buttonType == 1)
                {
                    (DateTime previousStartOfWeek, DateTime previousEndOfWeek) = GetPreviousWeekRange(i);  // 获取前X周
                    startDateTime = previousStartOfWeek;
                    endDateTime = previousEndOfWeek;
                    date = startDateTime.ToString(dateType);
                }
                else if (buttonType == 2)
                {
                    (DateTime previousStartOfMonths, DateTime previousEndOfMonths) = GetPreviousMonthsRange(i);  // 获取前X月
                    startDateTime = previousStartOfMonths;
                    endDateTime = previousEndOfMonths;
                    date = startDateTime.ToString("yyyy-MM");
                }
                else
                {
                    (DateTime previousStartOfYear, DateTime previousEndOfYear) = GetPreviousYearRange(i);  // 获取前X年
                    startDateTime = previousStartOfYear;
                    endDateTime = previousEndOfYear;
                    date = startDateTime.ToString("yyyy");
                }
                var todoCount = dataList.Count(item => item.TaskState == TaskStateEnum.ToDo && (item.UpdateDate >= startDateTime && item.UpdateDate < endDateTime));
                var doingCount = dataList.Count(item => item.TaskState == TaskStateEnum.Doing && (item.UpdateDate >= startDateTime && item.UpdateDate < endDateTime));
                var doneCount = dataList.Count(item => item.TaskState == TaskStateEnum.Done && (item.UpdateDate >= startDateTime && item.UpdateDate < endDateTime));
                var cancelCount = dataList.Count(item => item.TaskState == TaskStateEnum.Cancel && (item.UpdateDate >= startDateTime && item.UpdateDate < endDateTime));
                var sumCount = dataList.Count(item => (item.UpdateDate >= startDateTime && item.UpdateDate < endDateTime));
                gridPanelStores.Add(new
                {
                    date = date,
                    sumCount = sumCount == 0 ? 0: sumCount,
                    todoCount = todoCount,
                    doingCount = doingCount,
                    doneCount = doneCount,
                    cancelCount = cancelCount,
                    todoCountRatio = todoCount + "(" + (sumCount == 0 ? "0%" : (ToFixed(((double)todoCount / sumCount) * 100, 2) + "%")) + ")",
                    doingCountRatio = doingCount + "(" + (sumCount == 0 ? "0%" : (ToFixed(((double)doingCount / sumCount) * 100, 2) + "%")) + ")",
                    doneCountRatio = doneCount + "(" + (sumCount == 0 ? "0%" : (ToFixed(((double)doneCount / sumCount) * 100, 2) + "%")) + ")",
                    cancelCountRatio = cancelCount + "(" + (sumCount == 0 ? "0%" : (ToFixed(((double)cancelCount / sumCount) * 100, 2) + "%")) + ")",
                });

            }
            return gridPanelStores;
        }

        /// <summary>
        /// 获取前X周
        /// </summary>
        /// <param name="x"></param>
        /// <returns>起始日期和结束日期</returns>
        public (DateTime startDate, DateTime endDate) GetPreviousWeekRange(int x)
        {
            DateTime currentDate = DateTime.Now;
            int daysInWeek = (int)currentDate.DayOfWeek;

            // 计算当前日期所在周的起始日期和结束日期
            DateTime startOfWeek = currentDate.AddDays(-daysInWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // 计算前 x 周的起始日期和结束日期
            DateTime previousStartOfWeek = startOfWeek.AddDays(-7 * x);
            DateTime previousEndOfWeek = endOfWeek.AddDays(-7 * x);

            return (previousStartOfWeek, previousEndOfWeek);
        }


        /// <summary>
        /// 获取前X年
        /// </summary>
        /// <param name="x"></param>
        /// <returns>起始日期和结束日期</returns>
        public (DateTime startDate, DateTime endDate) GetPreviousYearRange(int x)
        {
            DateTime currentDate = DateTime.Now;

            // 计算当前年份和前 x 年的年份
            int currentYear = currentDate.Year;
            int previousYear = currentYear - x;

            // 构造前 x 年的起始日期和结束日期
            DateTime previousStartOfYear = new DateTime(previousYear, 1, 1);
            DateTime previousEndOfYear = new DateTime(previousYear, 12, 31);

            return (previousStartOfYear, previousEndOfYear);
        }

        /// <summary>
        /// 获取前X月
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public (DateTime startDate, DateTime endDate) GetPreviousMonthsRange(int x)
        {
            DateTime currentDate = DateTime.Now;

            // 获取当前日期的年份和月份
            int currentYear = currentDate.Year;
            int currentMonth = currentDate.Month;

            // 计算目标月份的年份和月份
            int targetYear = currentYear;
            int targetMonth = currentMonth - x;

            // 处理月份超出范围的情况
            while (targetMonth <= 0)
            {
                targetMonth += 12;
                targetYear--;
            }

            // 计算目标月份的起始日期和结束日期
            DateTime startOfMonth = new DateTime(targetYear, targetMonth, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return (startOfMonth, endOfMonth);
        }

    }
}
