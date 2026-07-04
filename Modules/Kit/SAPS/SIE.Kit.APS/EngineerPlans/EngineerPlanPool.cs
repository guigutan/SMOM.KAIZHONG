using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.Kit.APS.EngineerPlans
{
    /// <summary>
    /// 工程计划配置池 计划产能占用情况
    /// </summary>
    public class EngineerPlanPool
    {
        #region 属性

        /// <summary>
        /// 面积产能 key0:工厂Id,key1:日期,value:已经使用的产能
        /// </summary>
        protected Dictionary<double, Dictionary<DateTime, decimal>> DicUsedDayAreaCapacity { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public EngineerPlanPool()
        {
            DicUsedDayAreaCapacity = new Dictionary<double, Dictionary<DateTime, decimal>>();
        }

        /// <summary>
        /// 获取工厂、指定日期已经占用的产能
        /// </summary>
        /// <param name="factoryId">工厂</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public virtual decimal GetUsedCapacity(double factoryId, DateTime date)
        {
            decimal result = 0;
            Dictionary<DateTime, decimal> dicArea = null;
            if (DicUsedDayAreaCapacity.TryGetValue(factoryId, out dicArea) &&
                dicArea.TryGetValue(date, out result))
            {
                return result;
            }
            return 0;
        }

        /// <summary>
        /// 占用指定日期、工厂的产能
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <param name="date">日期</param>
        /// <param name="capacity">已经被使用产能</param>

        public virtual void AddUsedCapacity(double factoryId, DateTime date, decimal capacity)
        {
            date = date.Date;
            Dictionary<DateTime, decimal> dicDayArea = null;
            if (!DicUsedDayAreaCapacity.TryGetValue(factoryId, out dicDayArea))
            {
                dicDayArea = new Dictionary<DateTime, decimal>();
                DicUsedDayAreaCapacity.Add(factoryId, dicDayArea);
            }
            if (!dicDayArea.ContainsKey(date))
            {
                dicDayArea.Add(date, capacity);
            }
            else
            {
                dicDayArea[date] += capacity;
            }
        }

        /// <summary>
        /// 获取优先的工厂Id，如果指定工厂、指定月份有维护产能，则返回指定工厂。否则返回-1
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <returns>返回有效工厂Id</returns>
        public virtual double GetValidateFacotryId(double factoryId)
        {
            // 如果指定工厂这个月没有维护产能，则取工厂为空的产能数据
            if (factoryId != -1 && (!DicUsedDayAreaCapacity.ContainsKey(factoryId) ))
                factoryId = -1;
            return factoryId;
        }

    }
}
