using SIE.Domain;
using SIE.Kit.APS.EngineerPlan.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.EngineerPlans.Settings
{
    /// <summary>
    /// 工程节假日维护控制器
    /// </summary>
    public class HolidaySettingController : DomainController
    {
        /// <summary>
        /// 获取工程节假日维护数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<HolidaySetting> GetHolidaySettingList(HolidaySettingCriteria criteria)
        {
            var query = Query<HolidaySetting>();
            if (criteria.FactoryId > 0)
            {
                query = query.Where(p => p.FactoryId == criteria.FactoryId);
            }

            //开始时间
            if (criteria.StartDate.BeginValue.HasValue)
            {
                query.Where(p => p.StartDate >= criteria.StartDate.BeginValue);
            }
            if (criteria.StartDate.EndValue.HasValue)
            {
                query.Where(p => p.StartDate <= criteria.StartDate.EndValue);
            }

            //结束时间
            if (criteria.EndDate.BeginValue.HasValue)
            {
                query.Where(p => p.EndDate >= criteria.EndDate.BeginValue);
            }
            if (criteria.EndDate.EndValue.HasValue)
            {
                query.Where(p => p.EndDate <= criteria.EndDate.EndValue);
            }

            if (criteria.Remerk.IsNotEmpty())
            {
                query = query.Where(p => p.Remerk.Contains(criteria.Remerk));
            }
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工厂获取工程节假日维护信息
        /// </summary>
        /// <param name="FactoryIds"></param>
        /// <returns></returns>
        public virtual EntityList<HolidaySetting> GetHolidaySettingList()
        {
            return Query<HolidaySetting>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
