using SIE.AbnormalInfo.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常任务Dao
    /// </summary>
    public class AbnormalMonitorTaskDao : BaseDao<AbnormalMonitorTask>
    {
        /// <summary>
        /// 根据异常时效看板获取异常任务数据
        /// </summary>
        /// <param name="timevalue"></param>
        /// <param name="buttonType"></param>
        /// <returns></returns>
        public EntityList<AbnormalMonitorTask> GetChartData(int timevalue, int buttonType)
        {
            var query = DB.Query<AbnormalMonitorTask>();
            if (buttonType == 0)//日报
            {
                DateTime previousDate = DateTime.Now.AddDays(-timevalue);
                query.Where(p => p.UpdateDate >= previousDate && p.UpdateDate < DateTime.Now);
            }
            else if (buttonType == 1)//周报
            {
                DateTime previousStartOfWeek = GetPreviousWeekRange(timevalue-1);
                query.Where(p => p.UpdateDate >= previousStartOfWeek && p.UpdateDate < DateTime.Now);
            }
            else if (buttonType == 2)//月报
            {
                DateTime previousStartOfMonths = GetPreviousMonthsRange(timevalue - 1);
                query.Where(p => p.UpdateDate >= previousStartOfMonths && p.UpdateDate < DateTime.Now);
            }
            else//年报
            {
                DateTime previousStartOfYear = GetPreviousYearRange(timevalue-1);
                query.Where(p => p.UpdateDate >= previousStartOfYear && p.UpdateDate < DateTime.Now);
            }
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取前X周
        /// </summary>
        /// <param name="x"></param>
        /// <returns>起始日期和结束日期</returns>
        public DateTime GetPreviousWeekRange(int x)
        {
            DateTime currentDate = DateTime.Now;
            int daysInWeek = (int)currentDate.DayOfWeek;
            // 计算当前日期所在周的起始日期和结束日期
            DateTime startOfWeek = currentDate.AddDays(-daysInWeek);
            // 计算前 x 周的起始日期和结束日期
            DateTime previousStartOfWeek = startOfWeek.AddDays(-7 * x);
            return previousStartOfWeek;
        }


        /// <summary>
        /// 获取前X年
        /// </summary>
        /// <param name="x"></param>
        /// <returns>起始日期和结束日期</returns>
        public DateTime  GetPreviousYearRange(int x)
        {
            DateTime currentDate = DateTime.Now;
            // 计算当前年份和前 x 年的年份
            int currentYear = currentDate.Year;
            int previousYear = currentYear - x;
            // 构造前 x 年的起始日期和结束日期
            DateTime previousStartOfYear = new DateTime(previousYear, 1, 1);
            return previousStartOfYear;
        }

        /// <summary>
        /// 获取前X月
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public DateTime GetPreviousMonthsRange(int x)
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
            return startOfMonth;
        }

        /// <summary>
        /// 获取异常任务列表
        /// </summary>
        /// <param name="criteria">异常任务查询实体</param>
        /// <returns>异常任务实体列表</returns>
        public virtual EntityList<AbnormalMonitorTask> GetAbnormalTasks(AbnormalMonitorTaskCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria), "查询实体参数不能为空".L10N());
            var query = Query();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.AbnormalName.IsNotEmpty())
                query.Where(p => p.AbnormalName.Contains(criteria.AbnormalName));
            if (criteria.ProblemDescription.IsNotEmpty())
                query.Where(p => p.ProblemDescription.Contains(criteria.ProblemDescription));
            if (criteria.AbnormalDefineId.HasValue)
                query.Where(p => p.AbnormalDefineId==criteria.AbnormalDefineId);
            if (criteria.WorkShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            if (criteria.TaskState.HasValue)
                query.Where(p => p.TaskState == criteria.TaskState);
            if (criteria.LineId.HasValue)
                query.Where(p => p.LineId == criteria.LineId);
            if (criteria.TaskType.HasValue)
                query.Where(p => p.TaskType == criteria.TaskType);
            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            if (criteria.OrderInfoList != null && criteria.OrderInfoList.Count > 0)
                query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 不能生成任务
        /// </summary>
        /// <param name="inventorys"></param>
        /// <returns></returns>
        public List<AbnormalMonitorInventory> CannotMonitorTask(List<AbnormalMonitorInventory> inventorys)
        {
            var list= inventorys.Where(inventory => DB.Query<AbnormalMonitorTask>()
                .Where(c => c.AbnormalDefineId == inventory.AbnormalDefineId && c.ProblemDescription == inventory.ProblemDescription
                      && c.AbnormalName== inventory.AbnormalName && c.TaskType == TaskType.Auto && (c.TaskState == TaskStateEnum.Doing || c.TaskState == TaskStateEnum.ToDo)).Count() > 0).ToList();   
            return list;
        }
    }
}
