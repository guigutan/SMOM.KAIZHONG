using SIE.Domain;
using SIE.Kit.APS.EngineerPlan.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.APS.EngineerPlans.Settings
{
    public class CustLevelController : DomainController
    {
        /// <summary>
        /// 获取客户优先级数据
        /// </summary>
        public virtual EntityList<CustLevel> GetCustLevelList()
        {
            return Query<CustLevel>().ToList();
        }
    }
}
