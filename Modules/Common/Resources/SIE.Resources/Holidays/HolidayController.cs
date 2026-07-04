using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.Holidays
{
    /// <summary>
    /// 法定假期控制器
    /// </summary>
    public class HolidayController : DomainController
    {
        /// <summary>
        /// 获取法定假期列表
        /// </summary>
        /// <returns>返回法定假期列表</returns>
        public virtual EntityList<Holiday> GetHoliday()
        {
            return Query<Holiday>().ToList();
        }

        /// <summary>
        /// 获取有交集的法定假期列表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回法定假期列表</returns>
        public virtual EntityList<Holiday> GetIntersectHoliday(DateTime beginDate, DateTime endDate)
        {
            return Query<Holiday>().Where(x => x.EndDate >= beginDate && x.BeginDate <= endDate).ToList();
        }

        /// <summary>
        /// 根据开始日期、结束日期获取法定假期时间
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回法定假期时间</returns>
        public virtual List<HolidayInfo> GetHolidayInfos(DateTime beginDate, DateTime endDate)
        {
            var dataList = Query<Holiday>().Where(x => x.EndDate >= beginDate && x.BeginDate <= endDate)
                .Select(p => new
                {
                    BeginDate = p.BeginDate,
                    EndDate = p.EndDate,
                    Remark = p.Remark
                }).ToList<HolidayInfo>().ToList();

            return dataList;
        }

        /// <summary>
        /// 通过日期范围查询法定假期列表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>法定假期列表</returns>
        public virtual EntityList<Holiday> GetHolidayList(DateTime beginDate, DateTime endDate)
        {
            beginDate = beginDate.AddMonths(-1);
            endDate = endDate.AddMonths(+1);
            return Query<Holiday>().Where(p => p.BeginDate >= beginDate && p.EndDate <= endDate).ToList();
        }
    }
}
