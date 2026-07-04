using SIE.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Workbench.KeyPerformances
{
    /// <summary>
    /// 目标值设置控制器
    /// </summary>
    public class TargetSettingController : DomainController
    {
        /// <summary>
        /// 获取车间目标值设置
        /// </summary>
        /// <param name="shopId">车间Id</param>
        /// <param name="type">目标值类型</param>
        /// <returns>车间目标设置列表</returns>
        public virtual EntityList<ShopTargetSetting> GetShopTargetSettings(double shopId, TargetSettingType type)
        {
            return Query<ShopTargetSetting>().Where(p => p.WorkShopId == shopId && p.TargetSettingType == type).ToList();
        }

        /// <summary>
        /// 获取产线目标值设置
        /// </summary>
        /// <param name="lineId">产线Id</param>
        /// <param name="type">目标值类型</param>
        /// <returns>产线目标设置列表</returns>
        public virtual EntityList<LineTargetSetting> GetLineTargetSettings(double lineId, TargetSettingType type)
        {
            return Query<LineTargetSetting>().Where(p => p.LineId == lineId && p.TargetSettingType == type).ToList();
        }

        /// <summary>
        /// 获取产线目标值设置
        /// </summary>
        /// <param name="lineId">产线Id</param>
        /// <param name="type">目标值类型</param>
        /// <returns>产线目标设置列表</returns>
        public virtual EntityList<LineTargetSetting> GetLineTargetSettings(List<double> lineIds, TargetSettingType type)
        {
            return Query<LineTargetSetting>().Where(p => lineIds.Contains(p.LineId) && p.TargetSettingType == type).ToList();
        }
    }
}
