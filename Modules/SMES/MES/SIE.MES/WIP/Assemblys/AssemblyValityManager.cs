using SIE.Domain.Validation;
using SIE.MES.Validitys;
using SIE.MES.Validitys.Service;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WIP.Assemblys
{
    /// <summary>
    /// 上料采集有效期管理类
    /// </summary>
    public static class AssemblyValityManager
    {
        #region 方法
        /// <summary>
        /// 校验标签是否有效(true表示有效)
        /// </summary>
        /// <param name="itemLabel"></param>
        public static bool IsValidity(ItemLabel itemLabel)
        {
            if (itemLabel == null)
            {
                return true;
            }
            // 结束时间不为空，剩余可用时长大于0为有效，否则无效
            if (itemLabel.ValidityEnd.HasValue)
            {
                return itemLabel.RemainLongLived.Value > 0;
            }
            // 开始时间不为空，当前时间-开始时间 小于 剩余可用时长为有效，否则无效
            else
            {
                if (itemLabel.ValidityStart.HasValue)
                {
                    return (decimal)(DateTime.Now - itemLabel.ValidityStart.Value).TotalHours < itemLabel.RemainLongLived.Value;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 有效期开始计时
        /// 开始时间=当前上料时间
        /// 清空结束时间
        /// 剩余可用时长为空：可用时长寿命更新为标准，剩余可用时长更新为标准
        /// 剩余可用时长不为空，不变
        /// <param name="itemLabel">标签</param>
        /// <param name="firstLoad">首次提前上料</param>
        /// </summary>
        public static void ValidityStart(ItemLabel itemLabel, bool firstLoad)
        {
            if (itemLabel == null)
            {
                return;
            }
            ValidityStandard validityStandard = RT.Service.Resolve<ValidityService>().GetValidityStandard(itemLabel.ItemId, itemLabel.ItemExtProp);
            if (validityStandard == null)
            {
                return;
            }
            if ((itemLabel.ValidityStart == null && itemLabel.ValidityEnd == null) || (itemLabel.ValidityStart != null && itemLabel.ValidityEnd != null))
            {
                itemLabel.ValidityStart = DateTime.Now;
                itemLabel.ValidityEnd = null;
            }
            if (itemLabel.RemainLongLived == null)
            {
                itemLabel.LongLived = validityStandard.LongLived;
                itemLabel.RemainLongLived = itemLabel.LongLived;
            }
        }

        /// <summary>
        /// 有效期结束计时
        /// 结束时间=当前下料时间
        /// 可用时长寿命、开始时间不变
        /// 更新剩余可用时长-=结束时间-开始时间
        /// </summary>
        public static void ValidityEnd(ItemLabel itemLabel)
        {
            if (itemLabel == null || itemLabel.ValidityStart == null)
            {
                return;
            }
            itemLabel.ValidityEnd = DateTime.Now;
            itemLabel.RemainLongLived -= Math.Round((decimal)(itemLabel.ValidityEnd.Value - itemLabel.ValidityStart.Value).TotalHours, 3);
        }
        #endregion
    }
}
