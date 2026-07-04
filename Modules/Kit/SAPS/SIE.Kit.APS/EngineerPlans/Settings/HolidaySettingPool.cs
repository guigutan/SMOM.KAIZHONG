using SIE.Domain;
using SIE.Kit.APS.EngineerPlan.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SIE.Kit.APS.EngineerPlans.Settings
{
    /// <summary>
    /// 工程节假日配置池
    /// </summary>
    public class HolidaySettingPool
    {
        #region 属性
        /// <summary>
        ///  key:等级, value:限制值
        /// </summary>
        public Dictionary<double, List<HolidaySetting>> DicHolidaySetting { get; set; }

        /// <summary>
        /// 工程节假日维护控制器
        /// </summary>
        protected HolidaySettingController HolidaySettingCtrl { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public HolidaySettingPool()
        {
            HolidaySettingCtrl = RT.Service.Resolve<HolidaySettingController>();
        }

        /// <summary>
        /// 加载工程节假日维护数据
        /// </summary>
        public virtual void Load()
        {
            EntityList<HolidaySetting> HolidaySettingList = HolidaySettingCtrl.GetHolidaySettingList();
            // 如果工厂为空，默认为-1
            DicHolidaySetting = HolidaySettingList.GroupBy(a => a.FactoryId ?? -1).ToDictionary(a => a.Key, a => a.ToList());
        }

        /// <summary>
        /// 根据工厂获取数据
        /// </summary>
        /// <param name="FactoryId">工厂ID</param>
        /// <returns>返回限制值</returns>
        public virtual List<HolidaySetting> GetHolidaySettingList(double FactoryId)
        {
            List<HolidaySetting> HolidaySettingList = null;
            FactoryId = GetValidateFacotryId(FactoryId);
            if (DicHolidaySetting.TryGetValue(FactoryId, out HolidaySettingList))
            {
                return HolidaySettingList;
            }
            return HolidaySettingList;
        }

        /// <summary>
        /// 获取优先的工厂Id，如果指定工厂、指定日期内有维护款数限制，则返回指定工厂。否则返回-1
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <returns>返回有效工厂Id</returns>
        public virtual double GetValidateFacotryId(double factoryId)
        {
            if (factoryId != -1 && (!DicHolidaySetting.ContainsKey(factoryId)))
                factoryId = -1;
            return factoryId;
        }

    }
}
