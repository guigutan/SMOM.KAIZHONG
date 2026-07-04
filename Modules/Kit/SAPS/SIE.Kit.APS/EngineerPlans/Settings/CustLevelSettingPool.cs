using SIE.Domain;
using SIE.Kit.APS.EngineerPlan.Settings;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.EngineerPlans.Settings
{
    /// <summary>
    /// 客户等级配置池
    /// </summary>
    public class CustLevelSettingPool
    {
        #region 属性
        /// <summary>
        ///  key:等级, value:限制值
        /// </summary>
        public Dictionary<double, List<CustLevelSetting>> DicCustLevel { get; set; }

        /// <summary>
        /// 客户等级控制器
        /// </summary>
        protected CustLevelSettingController CustLevelSettingCtrl { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustLevelSettingPool()
        {
            CustLevelSettingCtrl = RT.Service.Resolve<CustLevelSettingController>();
        }

        /// <summary>
        /// 加载客户等级设置数据
        /// </summary>
        public virtual void Load()
        {
            EntityList<CustLevelSetting> custLevelList = CustLevelSettingCtrl.GetCustLevelSettingList();
            DicCustLevel = custLevelList.GroupBy(a => a.FactoryId ?? -1).ToDictionary(a => a.Key, a => a.ToList());
        }

        /// <summary>
        /// 根据等级获取数据
        /// </summary>
        /// <param name="FactoryId">工厂id</param>
        /// <returns>返回限制值</returns>
        public virtual List<CustLevelSetting> GetCustLevelSettingList(double FactoryId)
        {
            List<CustLevelSetting> CustLevelSettings = null;
            FactoryId = GetValidateFacotryId(FactoryId);
            if (DicCustLevel.TryGetValue(FactoryId, out CustLevelSettings))
            {
                return CustLevelSettings;
            }
            return null;
        }

        /// <summary>
        /// 获取优先的工厂Id，如果指定工厂、指定日期内有维护款数限制，则返回指定工厂。否则返回-1
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <returns>返回有效工厂Id</returns>
        public virtual double GetValidateFacotryId(double factoryId)
        {
            if (factoryId != -1 && (!DicCustLevel.ContainsKey(factoryId)))
                factoryId = -1;
            return factoryId;
        }
    }
}
